using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Verification.Eth.Functions
{
    // function GetNumberOfFiles() external view returns(uint)
    [Function("GetNumberOfFiles", "uint")]
    public class GetNumberOfFilesFunction : FunctionMessage
    { }
}