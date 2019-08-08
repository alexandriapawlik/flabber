using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Library
{
    // function IsRegistryActiveAtIndex(uint Index) external view returns(bool)
    [Function("IsRegistryActiveAtIndex", "bool")]
    public class IsRegistryActiveAtIndexFunction : FunctionMessage
    {
        [Parameter("uint", "Index", 1)]
        public int Index { get; set; }
    }
}
