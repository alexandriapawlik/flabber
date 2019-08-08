using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.Library
{
    // function DeactivateRegistry(address ContractAddress) external
    [Function("DeactivateRegistry")]
    public class DeactivateRegistryFunction : FunctionMessage
    {
        [Parameter("address", "ContractAddress", 1)]
        public string RegistryAddress { get; set; }
    }
}
