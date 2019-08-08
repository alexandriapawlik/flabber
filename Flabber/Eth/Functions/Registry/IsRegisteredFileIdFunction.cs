using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Registry
{
    // function IsRegisteredFileId(string FileId) external view returns(bool)
    [Function("IsRegisteredFileId", "bool")]
    public class IsRegisteredFileIdFunction : FunctionMessage
    {
        [Parameter("string", "FileId", 1)]
        public string FileId { get; set; }
    }
}
