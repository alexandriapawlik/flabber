using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.File
{
    // function GetNumberOfReceipts() external view returns(uint)
    [Function("GetNumberOfReceipts", "uint")]
    public class GetNumberOfReceiptsFunction : FunctionMessage
    { }
}
