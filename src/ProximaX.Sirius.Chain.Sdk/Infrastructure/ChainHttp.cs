using Flurl.Http;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Reactive.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    public class ChainHttp: BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ChainHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public ChainHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChainHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public ChainHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        public IObservable<ulong> GetBlockScore()
        {
            var route = $"{BasePath}/chain/score";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainScoreDTO>())
                .Select(i => new ulong[]
                {
                   i.ScoreHigh.ToUInt64(),
                   i.ScoreLow.ToUInt64()
                }.FromUInt64Array());
        }

        public IObservable<ulong> GetBlockHeight()
        {
            var route = $"{BasePath}/chain/height";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<HeightDTO>())
                .Select(i => i.Height.ToUInt64());
        }

    }
}
