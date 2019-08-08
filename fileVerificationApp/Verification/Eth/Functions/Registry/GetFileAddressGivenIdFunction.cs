using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace FV.Verification.Eth.Functions
{
    // function GetFileAddressGivenId(string FileId) external view returns(address)
    [Function("GetFileAddressGivenId", "address")]
    public class GetFileAddressGivenIdFunction : FunctionMessage
    {
        [Parameter("string", "FileId", 1)]
        public string FileId { get; set; }
    }
}
