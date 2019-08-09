Flabber: File Verification
=================

<br />

A .NET Core 2.1 console application that accesses private Ethereum blockchain contracts using the Nethereum Web3 API and retrieves file information from OneDrive using the Microsoft Graph API.

Tracks files using OneDrive's file IDs and defines file edits as a change in a file's Xor content hash.

<br />
<br />

About the Code
--------------

<br />

The **FlabberManager** class encapsulates a DriveManager, EthManager, and LoginManager, which together provide all functionality required for user interaction. 
- **DriveManager** makes all required calls to the Microsoft Graph API
- **EthManager** makes all required calls to the Ethereum blockchain's JSON-RPC API
- **LoginManager** aquires an access token and manages the app's allowed users

<br />
<br />

To call the Microsoft Graph API:
1. Console fires a device code callback
2. User signs in online
3. Console polls server for a successful login and aquires an access token from Azure Active Directory
4. Console uses access token to make calls to the Microsoft Graph API as user 

<br />
<br />

To call Ethereum's JSON-RPC API:
1. Console connects to Ethereum consortium blockchain's RPC endpoint using a URL
2. Console creates an admin Ethereum account using private key
3. Console queries data as admin without submitting a transaction
4. Console queries data that changes blockchain state by signing transactions with private key

<br />
<br />

Smart Contracts
----------------

<br />

File data is stored on the blockchain using 4 types of smart contract:
1. **Library** contains a set of registries
2. **File Registry** stores a name and contains a set of files
3. **File** stores file-specific data and contains a set of receipts
4. **Receipt** stores verification-instance-specific file data
