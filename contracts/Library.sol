
// Alexandria Pawlik, August 2019


pragma solidity ^0.4.24;


contract Library
{
	///////////////// internal data


	struct RegistryStruct
	{ 
		address Address;
		string Name; 
		uint Index;
		bool Active;
	}

	// registry tracking
  mapping(address => RegistryStruct) internal RegistryContractAddressLookup;
	address[] internal RegistryAddressIndex;


	// adds registry to library
  function AddRegistry(address ContractAddress, string Name) 
	external
	{
		require(!ExistsContractAddress(ContractAddress), "This contract address cannot be registered to a second file registry.");

		// add lookup by address
		RegistryContractAddressLookup[ContractAddress].Address = ContractAddress;
		RegistryContractAddressLookup[ContractAddress].Name = Name;
		RegistryContractAddressLookup[ContractAddress].Index = RegistryAddressIndex.push(ContractAddress)-1;
		RegistryContractAddressLookup[ContractAddress].Active = true;
	}

	function DeactivateRegistry(address ContractAddress)
	external
	{
		RegistryContractAddressLookup[ContractAddress].Active = false;
	}

	function GetNumberOfRegistries()
	external
	view
	returns(uint)
	{
		return RegistryAddressIndex.length;
	}

	function GetRegistryAddressAtIndex(uint Index)
  external
  view
  returns(address at)
  {
		// check bounds
		require(Index < RegistryAddressIndex.length, "Index passed to GetRegistryAddressAtIndex is too large.");

    at =  RegistryAddressIndex[Index];
  }

	function GetRegistryNameAtIndex(uint Index)
  external
  view
  returns(string at)
  {
		// check bounds
		require(Index < RegistryAddressIndex.length, "Index passed to GetRegistryNameAtIndex is too large.");

    at =  RegistryContractAddressLookup[RegistryAddressIndex[Index]].Name;
  }

	function IsRegistryActiveAtIndex(uint Index)
  external
  view
  returns(bool active)
  {
		// check bounds
		require(Index < RegistryAddressIndex.length, "Index passed to IsRegistryActiveAtIndex is too large.");

    active = RegistryContractAddressLookup[RegistryAddressIndex[Index]].Active;
  }


	///////////////// helper functions


	// lookup to see if a contract address for a Registry contract is already registered
	function ExistsContractAddress(address ContractAddress)
	internal
	view
	returns(bool) 
	{
		if(RegistryAddressIndex.length == 0) return false;
			
		return (RegistryAddressIndex[RegistryContractAddressLookup[ContractAddress].Index] == ContractAddress);
	}

	function compareStrings(string a, string b) 
	internal 
	pure 
	returns(bool)
	{
		return keccak256(bytes(a)) == keccak256(bytes(b));
	}
}