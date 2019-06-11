using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Model.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Sdk.Tests.E2E
{
    [Collection("E2ETestFixtureCollection")]
    public class E2ENamespaceTests
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;


        public E2ENamespaceTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;


        }

        [Fact]
        public async Task Should_Create_RootNamespace()
        {
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var bobAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            var rootNamespaceInfo = await GenerateNamespace(bobAccount, namespaceName, null);

            rootNamespaceInfo.Should().NotBeNull();
            rootNamespaceInfo.ParentId.Should().BeNull();
            rootNamespaceInfo.IsRoot.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Create_SubNamespace()
        {
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var parentId = new NamespaceId(namespaceName);
            var bobAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            var rootNamespaceInfo = await GenerateNamespace(bobAccount, namespaceName, null);
            _output.WriteLine($"Generated namespace {rootNamespaceInfo}");

            var subNs = "subns" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            var subNamespaceInfo = await GenerateNamespace(bobAccount, subNs, parentId);
            _output.WriteLine($"Generated sub namespace {subNamespaceInfo}");


            subNamespaceInfo.Should().NotBeNull();
            subNamespaceInfo.ParentId.Should().NotBeNull();
            subNamespaceInfo.IsSubNamespace.Should().BeTrue();
        }

        [Fact]
        public async Task Should_Create_Aggregate_Root_And_SubNamespace()
        {
            var bobAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);
            var networkType = _fixture.NetworkHttp.GetNetworkType().Wait();

            var rootNs = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var subNs = "subnp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);

            var rootId = new NamespaceId(rootNs);
            var childId = new NamespaceId(rootNs + "." + subNs);
            _output.WriteLine($"Going to create aggregate root {rootId} and child namespace {childId}");

            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                rootNs,
                100,
                networkType
            );

            var registerChildTransaction = RegisterNamespaceTransaction.CreateSubNamespace(
                Deadline.Create(),
                subNs,
                rootId,
                networkType
            );

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    registerRootTransaction.ToAggregate(bobAccount.PublicAccount),
                    registerChildTransaction.ToAggregate(bobAccount.PublicAccount)
                }, networkType);

            var signedTransaction = bobAccount.Sign(aggregateTransaction);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.Listener.ConfirmedTransactionsGiven(bobAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            result.TransactionType.Should().BeEquivalentTo(TransactionType.AGGREGATE_COMPLETE);

            /*
            var rootNsInfo = await _fixture.NamespaceHttp.GetNamespace(rootId).Timeout(_fixture.DefaultTimeout);
            rootNsInfo.Should().NotBeNull();
            rootNsInfo.IsRoot.Should().BeTrue();

            var subNsInfo = await _fixture.NamespaceHttp.GetNamespace(childId).Timeout(_fixture.DefaultTimeout);
            subNsInfo.Should().NotBeNull();
            subNsInfo.IsSubNamespace.Should().BeTrue();
            */

            //Verify the root namespace and sub namespace owned by the account
            var nsInfos = await _fixture.NamespaceHttp.GetNamespacesFromAccount(bobAccount.Address, null);
            nsInfos.Should().HaveCount(2);
            nsInfos.Single(ns => ns.Id.HexId == rootId.HexId).Should().NotBeNull();
            nsInfos.Single(ns => ns.Id.HexId == childId.HexId).Should().NotBeNull();

            //Verify the name of the namespaces
            var nsNames = await _fixture.NamespaceHttp.GetNamespacesNames(new List<NamespaceId>
            {
                rootId,
                childId
            });

            nsNames.Should().HaveCount(3);
            nsNames.Select(ns => ns.Name == rootId.Name).Should().NotBeNull();
            nsNames.Select(ns => ns.Name == childId.Name).Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Return_NamespacesInfo_For_Given_Addresses()
        {
            var bobNsName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var bobAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            var bobNsInfo = await GenerateNamespace(bobAccount, bobNsName, null);
            _output.WriteLine($"Generated namespace { bobNsInfo }for account {bobAccount}");


            var aliceNsName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var aliceAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            var aliceNsInfo = await GenerateNamespace(aliceAccount, aliceNsName, null);
            _output.WriteLine($"Generated namespace { aliceNsInfo }for account {aliceAccount}");


            _output.WriteLine("Getting namespaces info from accounts");

            var nsInfos = await _fixture.NamespaceHttp.GetNamespacesFromAccount(new List<Address>
            {
                bobAccount.Address,
                aliceAccount.Address
            }, null);

            nsInfos.Should().HaveCount(2);
            nsInfos.Select(ns => ns.Id.HexId == bobNsInfo.Id.HexId).Should().NotBeNull();
            nsInfos.Select(ns => ns.Id.HexId == aliceNsInfo.Id.HexId).Should().NotBeNull();
        }


        private async Task<NamespaceInfo> GenerateNamespace(Account account, string namespaceName, NamespaceId parentId)
        {
            RegisterNamespaceTransaction registerNamespaceTransaction;
            var networkType = _fixture.NetworkHttp.GetNetworkType().Wait();
            if (parentId == null)
            {
                registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                    Deadline.Create(),
                    namespaceName,
                    100,
                    networkType
                );
            }
            else
            {
                registerNamespaceTransaction = RegisterNamespaceTransaction.CreateSubNamespace(
                    Deadline.Create(),
                    namespaceName,
                    parentId,
                    networkType
                );


            }


            var signedTransaction = account.Sign(registerNamespaceTransaction);

            var tx = _fixture.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.TransactionHttp.Announce(signedTransaction);

            _output.WriteLine(
                $"Registered namespace {namespaceName} for account {account.Address.Plain} with transaction {signedTransaction.Hash}");

            var result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = parentId != null ? NamespaceId.CreateFromParent(namespaceName, parentId) : new NamespaceId(namespaceName);

            var namespaceInfo = await _fixture.NamespaceHttp.GetNamespace(expectedId).Timeout(_fixture.DefaultTimeout);

            _output.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {namespaceInfo.Id.HexId}");

            return namespaceInfo;
        }


    }
}
