using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Receipt
{
    // string public VerificationDateTime; // MM/DD/YY HH:MM:SS
    [Function("VerificationDateTime", "string")]
    public class GetVerificationDateTimeFunction : FunctionMessage
    { }
}