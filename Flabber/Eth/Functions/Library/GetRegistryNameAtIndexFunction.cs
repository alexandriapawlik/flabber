using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Library
{
    // function GetRegistryNameAtIndex(uint Index) external view returns(string at)
    [Function("GetRegistryNameAtIndex", "string")]
    public class GetRegistryNameAtIndexFunction : FunctionMessage
    {
        [Parameter("uint", "Index", 1)]
        public int Index { get; set; }
    }
}
