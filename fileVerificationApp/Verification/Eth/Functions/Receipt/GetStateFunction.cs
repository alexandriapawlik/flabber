using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Verification.Eth.Functions
{
    // function GetStateInt() external view returns(uint)
    [Function("GetStateInt", "uint")]
    public class GetStateFunction : FunctionMessage
    { }
}
