using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

using FV.Infrastructure;

namespace FV.Verification.Eth.Functions
{
    // constructor (address registryAddress, string filename, string fileId, string fileHash, 
    // string metadataHash, string contentType, string etag) 
    public class DeployFileFunction : ContractDeploymentMessage
    {
        public static string BYTECODE = Constants.FileBytecode;

        public DeployFileFunction() : base(BYTECODE) { }

        [Parameter("address", "registryAddress")]
        public string RegistryAddress { get; set; }

        [Parameter("string", "filename")]
        public string Name { get; set; }

        [Parameter("string", "fileId")]
        public string FileId { get; set; }

        [Parameter("string", "fileHash")]
        public string FileHash { get; set; }

        [Parameter("string", "metadataHash")]
        public string MetadataHash { get; set; }

        [Parameter("string", "contentType")]
        public string Type { get; set; }

        [Parameter("string", "etag")]
        public string Etag { get; set; }
    }
}
