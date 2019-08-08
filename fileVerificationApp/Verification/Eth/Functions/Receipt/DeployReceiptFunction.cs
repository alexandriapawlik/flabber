using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

using FV.Infrastructure;

namespace FV.Verification.Eth.Functions
{
    // constructor (address registryAddress, string fileId, string fileHash, string metadataHash, string user)
    public class DeployReceiptFunction : ContractDeploymentMessage
    {
        public static string BYTECODE = Constants.ReceiptBytecode;

        public DeployReceiptFunction() : base(BYTECODE) { }

        [Parameter("address", "registryAddress")]
        public string RegistryAddress { get; set; }

        [Parameter("string", "fileId")]
        public string FileId { get; set; }

        [Parameter("string", "fileHash")]
        public string FileHash { get; set; }

        [Parameter("string", "metadataHash")]
        public string MetadataHash { get; set; }

        [Parameter("string", "user")]
        public string VerifiedBy { get; set; }
    }
}
