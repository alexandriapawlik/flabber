
// Alexandria Pawlik, August 2019


pragma solidity ^0.4.24;


import "./FileRegistry.sol";
import "./File.sol";


contract Receipt
{
  ///////////////// public data


	enum StateType { Stopped, Success, Failure}
	StateType public State;

	string public FileName; // name of the file that this receipt is for
	string public VerificationDateTime; // MM/DD/YY HH:MM:SS
	string public VerifiedBy;  // username of the user that creates the receipt


  ///////////////// internal data


  uint internal Index;  // index of receipt in file's history array
  string internal FileHash; // sha256 hash of instant file content
	string internal MetadataHash; // sha256 hash of instant file metadata

  File internal MyFile;
	address internal FileAddress;


  /////////////////


	constructor (address registryAddress, string fileId, string fileHash, string metadataHash, string user)
	public
	{
		// for debugging, not permanent
		State = StateType.Stopped;

		// verification data
		FileHash = fileHash;
		MetadataHash = metadataHash;
		VerifiedBy = user;

    // registry
		FileRegistry MyFileRegistry = FileRegistry(registryAddress);

    // find file id in this registry
    FileAddress = MyFileRegistry.GetFileAddressGivenId(fileId);

		// file
		MyFile = File(FileAddress);
		FileName = MyFile.FileName();

		// add verification receipt to history, makes sure that file has not been deleted
		VerificationDateTime = MyFile.AddReceipt(address(this));
    Index = MyFile.GetNumberOfReceipts() - 1;

		// verify with original
		VerifyWithOriginal();
	}

	// get function for internal variable
	function GetFileHash()
	external
	view
	returns(string)
	{
		return FileHash;
	}

	// get function for internal variable
	function GetMetadataHash()
	external
	view
	returns(string)
	{
		return MetadataHash;
	}

	// returns 0, 1 or 2 representing
  // the state of the receipt as indexed in the enumeration {Stopped, Success, Failure}
	function GetStateInt()
	external
	view
	returns(uint)
	{
		return uint(State);
	}


  ///////////////// helper functions


  function VerifyWithOriginal()
  internal
  {
    // first check that metadata hashes (contenttype, etag, id) match
    if ( !compareStrings(MetadataHash, MyFile.GetMetadataHash()) ) 
    {
      State = StateType.Failure;
    }
    // then check that content hashes match
    else if ( !compareStrings(FileHash, MyFile.GetFileHash()) )
    {
      State = StateType.Failure;
    }
    // unchanged
    else
    {
      State = StateType.Success;
    }
  }

	function stringToAddress(string _a) 
  internal 
  pure 
  returns(address)
  {
    bytes memory tmp = bytes(_a);
    uint160 iaddr = 0;
    uint160 b1;
    uint160 b2;
    
    for (uint i=2; i<2+2*20; i+=2)
    {
      iaddr *= 256;
      b1 = uint160(tmp[i]);
      b2 = uint160(tmp[i+1]);
      if ((b1 >= 97)&&(b1 <= 102)) b1 -= 87;
      else if ((b1 >= 48)&&(b1 <= 57)) b1 -= 48;
      if ((b2 >= 97)&&(b2 <= 102)) b2 -= 87;
      else if ((b2 >= 48)&&(b2 <= 57)) b2 -= 48;
      iaddr += (b1*16+b2);
    }

    return address(iaddr);
  }

	function stringToBytes32(string memory source) 
  internal 
  pure 
  returns(bytes32 result) 
  {
    bytes memory tempEmptyStringTest = bytes(source);
    if (tempEmptyStringTest.length == 0) 
    {
      return 0x0;
    }

    assembly 
    {
      result := mload(add(source, 32))
    }
  }

	function compareStrings(string a, string b) 
  internal 
  pure 
  returns(bool)
  {
    return keccak256(bytes(a)) == keccak256(bytes(b));
  }
}