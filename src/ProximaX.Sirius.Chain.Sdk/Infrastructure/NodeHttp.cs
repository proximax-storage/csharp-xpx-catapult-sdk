using Flurl.Http;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Node;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;


namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    /// NodeHttp class
    /// </summary>
    public class NodeHttp: BaseHttp
    {
        /// <summary>
        /// NodeHttp constructor
        /// </summary>
        /// <param name="host">The node host</param>
        public NodeHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        public NodeHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        /// GetNodeTime
        /// </summary>
        /// <returns>IObservable&ltNodeTime&gt;</returns>
        public IObservable<NodeTime> GetNodeTime()
        {
            var route = $"{BasePath}/node/time";

            return Observable.FromAsync(async res => await route.GetJsonAsync<NodeTimeDTO>())
                .Select(r => NodeTime.FromDto(r));
        }

        /// <summary>
        /// GetNodeInfo
        /// </summary>
        /// <returns>IObservableNodeInfo&gt;</returns>
        public IObservable<NodeInfo> GetNodeInfo()
        {
            var route = $"{BasePath}/node/info";

            return Observable.FromAsync(async res => await route.GetJsonAsync<NodeInfoDTO>())
                .Select(r => NodeInfo.FromDto(r));
        }
    }
}
