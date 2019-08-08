using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Library.Functions
{
    // function GetNumberOfRegistries() external view returns(uint)
    [Function("GetNumberOfRegistries", "uint")]
    public class GetNumberOfRegistriesFunction : FunctionMessage
    { }
}
