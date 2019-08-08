using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Receipt
{
    // function GetStateInt() external view returns(uint)
    [Function("GetStateInt", "uint")]
    public class GetStateFunction : FunctionMessage
    { }
}
