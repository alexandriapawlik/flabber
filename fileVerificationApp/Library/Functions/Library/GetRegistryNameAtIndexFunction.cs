using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Library.Functions
{
    // function GetRegistryNameAtIndex(uint Index) external view returns(string at)
    [Function("GetRegistryNameAtIndex", "string")]
    public class GetRegistryNameAtIndexFunction : FunctionMessage
    {
        [Parameter("uint", "Index", 1)]
        public int Index { get; set; }
    }
}
