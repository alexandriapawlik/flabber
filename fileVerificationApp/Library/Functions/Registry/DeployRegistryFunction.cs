﻿using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

using FV.Infrastructure;

namespace FV.Library.Functions
{
    // constructor (address libraryAddress, string name, string description) 
    public class DeployRegistryFunction : ContractDeploymentMessage
    {
        public static string BYTECODE = Constants.RegistryBytecode;

        public DeployRegistryFunction() : base(BYTECODE) { }

        [Parameter("address", "libraryAddress")]
        public string LibraryAddress { get; set; }

        [Parameter("string", "name")]
        public string Name { get; set; }

        [Parameter("string", "description")]
        public string Description { get; set; }
    }
}
