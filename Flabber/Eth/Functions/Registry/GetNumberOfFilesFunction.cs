using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Registry
{
    // function GetNumberOfFiles() external view returns(uint)
    [Function("GetNumberOfFiles", "uint")]
    public class GetNumberOfFilesFunction : FunctionMessage
    { }
}