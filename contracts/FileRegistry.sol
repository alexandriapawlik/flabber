
// Alexandria Pawlik, August 2019


pragma solidity ^0.4.24;

import "./Library.sol";


contract FileRegistry
{
	///////////////// public data


	enum StateType { Open, Closed }
	StateType public State;

  string public Name;
	string public Description;


  ///////////////// internal data


	struct FileStruct 
	{ 
		address FileContractAddress;
		string FileId; 
		uint Index;
	}

	// file tracking
	mapping(string => FileStruct) internal FileIdLookup;
  mapping(address => FileStruct) internal FileContractAddressLookup;
	address[] internal FileAddressIndex;
	string[] internal FileIdIndex;


  /////////////////


	constructor (address libraryAddress, string name, string description) 
	public
	{
		// registry data
		Name = name;
		Description = description;

		// add to library
		Library MyLibrary = Library(libraryAddress);
		MyLibrary.AddRegistry(address(this), name);

		// update state
		State = StateType.Open;
	}

	// stop new files from being registered
	function CloseRegistry() 
	external 
	{
		State = StateType.Closed;
	}

	// adds file to registry
  function RegisterFile(address FileContractAddress, string FileId) 
	external
	{
		require(State == StateType.Open, "File cannot be added to a registry that is not open.");
		require(!IsRegisteredFileContractAddress(FileContractAddress), "This contract address cannot be registered to a second file.");

		// add lookup by address
		FileContractAddressLookup[FileContractAddress].FileContractAddress = FileContractAddress;
		FileContractAddressLookup[FileContractAddress].FileId = FileId;
		FileContractAddressLookup[FileContractAddress].Index = FileAddressIndex.push(FileContractAddress)-1;

		// add look up by reg number
		FileIdLookup[FileId].FileContractAddress = FileContractAddress;
		FileIdLookup[FileId].FileId = FileId;
		FileIdLookup[FileId].Index = FileIdIndex.push(FileId)-1;
	}

  // lookup to see if this file is registered
	function IsRegisteredFileId(string FileId)
	external 
	view
	returns(bool) 
	{
		if (FileIdIndex.length == 0) return false;

		string memory FileIdString = FileId;
		string memory FileIdInternalString = FileIdIndex[FileIdLookup[FileIdString].Index];
		return (compareStrings(FileIdInternalString, FileIdString));
	}

	function GetNumberOfFiles()
	external
	view
	returns(uint)
	{
		return FileIdIndex.length;
	}

  function GetFileAddressGivenId(string FileId)
  external
  view
  returns(address)
  {
    return FileIdLookup[FileId].FileContractAddress;
  }


  ///////////////// helper functions


	// lookup to see if a contract address for a File contract is already registered
	function IsRegisteredFileContractAddress(address FileContractAddress)
	internal
	view
	returns(bool isRegistered) 
	{
		if(FileAddressIndex.length == 0) return false;
			
		return (FileAddressIndex[FileContractAddressLookup[FileContractAddress].Index] == FileContractAddress);
	}

	function bytes32ToString(bytes32 x)  
	internal 
	pure 
	returns(string) 
	{
		bytes memory bytesString = new bytes(32);
		uint charCount = 0;
		for (uint j = 0; j < 32; j++) 
		{
			byte char = byte(bytes32(uint(x) * 2 ** (8 * j)));
			if (char != 0) 
			{
				bytesString[charCount] = char;
				charCount++;
			}
		}

		bytes memory bytesStringTrimmed = new bytes(charCount);
		for (j = 0; j < charCount; j++) 
		{
			bytesStringTrimmed[j] = bytesString[j];
		}

		return string(bytesStringTrimmed);  
	}

	function compareStrings(string a, string b) 
	internal 
	pure 
	returns(bool)
	{
		return keccak256(bytes(a)) == keccak256(bytes(b));
	}
}