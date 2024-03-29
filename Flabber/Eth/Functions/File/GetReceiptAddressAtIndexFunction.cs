﻿using System.Numerics;

using Nethereum.Contracts;
using Nethereum.ABI.FunctionEncoding.Attributes;

namespace Flabber.Eth.Functions.File
{
    // function GetReceiptAddressAtIndex(uint idx) public view returns(address at)
    [Function("GetReceiptAddressAtIndex", "address")]
    public class GetReceiptAddressAtIndexFunction : FunctionMessage
    {
        [Parameter("uint", "idx", 1)]
        public BigInteger Index { get; set; }
    }
}
