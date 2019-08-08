using System.Numerics;

using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Library
{
    // function GetRegistryAddressAtIndex(uint Index) external view returns(address at)
    [Function("GetRegistryAddressAtIndex", "address")]
    public class GetRegistryAddressAtIndexFunction : FunctionMessage
    {
        [Parameter("uint", "Index", 1)]
        public int Index { get; set; }
    }
}
