using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Receipt
{
    // string public VerifiedBy;  // username of the user that creates the receipt
    [Function("VerifiedBy", "string")]
    public class GetVerifiedByFunction : FunctionMessage
    { }
}