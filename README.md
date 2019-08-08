File Verification
=================

<br />

A .NET Core 2.1 console application that accesses private Ethereum blockchain contracts using the Nethereum Web3 API and retrieves file information from OneDrive using the Microsoft Graph API

<br />
<br />

Smart Contracts
----------------

<br />

File data is stored on the blockchain using 3 smart contracts:
1. **Library**: contains a set of registries
2. **File Registry**: stores a name and contains a set of files
3. **File**: stores file-specific data and contains a set of receipts
4. **Receipt**: stores verification-instance-specific file data

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
