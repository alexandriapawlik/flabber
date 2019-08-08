using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Verification.Eth.Functions
{
    // string public VerificationDateTime; // MM/DD/YY HH:MM:SS
    [Function("VerificationDateTime", "string")]
    public class GetVerificationDateTimeFunction : FunctionMessage
    { }
}