using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Library
{
    // function GetNumberOfRegistries() external view returns(uint)
    [Function("GetNumberOfRegistries", "uint")]
    public class GetNumberOfRegistriesFunction : FunctionMessage
    { }
}
