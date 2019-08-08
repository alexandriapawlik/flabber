File Verification
=================

<br />

A .NET Core 2.1 console application that accesses private Ethereum blockchain contracts using the Nethereum Web3 API and retrieves file information from OneDrive using the Microsoft Graph API

<br />
<br />

Overview
--------

<br />

- **EthInteraction** manages calls to the Ethereum consortium blockchain using smart contracts' encoded function definitions.
- **DriveInteraction** manages calls to the Microsoft Graph API using **TokenProvider** to get an access token and **ProtectedApiCallHelper** to make calls to the Microsoft Graph API and process the JSON results.
- **VerificationManager** contains an EthInteraction instance and a DriveInteraction instance and combines the methods into 5 basic functions that can be called by the console.

<br />

#### Functions Available to the Console
1. VerifyFile: generate a new receipt for a specific file and view its verification state
2. GetHistory: view the history of all verifications of a specific file
3. GetFileList: view all files available for verification
4. GetFilesToRegister: view all files that have not yet been added to the registry
5. RegisterFile: add a specific file to the registry

<br />

The key value used to track files is the file ID, assigned by OneDrive to a file upon upload. This means that the only way a file can be editted but retain its identity is if it's editted within the OneDrive environment. 
The key value used to identify changes in a file is the xor hash, which changes each time an edit is made to a file's content.

<br />
<br />

Smart Contracts
----------------

<br />

File data is stored on the blockchain using 3 smart contracts:
1. **File Registry**: contains a set of files
2. **File**: stores file-specific data and contains a set of receipts
3. **Receipt**: stores verification-instance-specific file data

<br />
<br />

About the Code
--------------

<br />

To call the Microsoft Graph API:
1. Console fires a device code callback
2. User signs in online
3. Console polls server for a successful login and aquires an access token from Azure Active Directory
4. Console uses access token to make calls to the Microsoft Graph API as user 

<br />
<br />

To call Ethereum's Web3 API:
1. Console connects to Ethereum consortium blockchain's RPC endpoint using a URL
2. Console creates an admin Ethereum account using private key
3. Console queries data as admin without submitting a transaction
4. Console queries data that changes blockchain state by signing transactions with private key
