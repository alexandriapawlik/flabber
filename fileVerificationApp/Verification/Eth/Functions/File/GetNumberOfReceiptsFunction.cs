using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Verification.Eth.Functions
{
    // function GetNumberOfReceipts() external view returns(uint)
    [Function("GetNumberOfReceipts", "uint")]
    public class GetNumberOfReceiptsFunction : FunctionMessage
    {}
}
