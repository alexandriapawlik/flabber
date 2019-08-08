//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		File class
// Purpose:		Object used to store data about a file existing on OneDrive

//////////////////////////////////////////////


namespace Flabber.Objects
{
    public class File
    {
        public string Name { get; }
        public string FileId { get; }
        public string Type { get; }
        public string Etag { get; }
        public long Size { get; }

        // default ctor
        public File() { }

        // constructor (address registryAddress, string filename, string fileId, string fileHash, 
        // string metadataHash, string contentType, string etag) 
        public File(string name, string fileId, string type, string etag, long size)
        {
            Name = name;
            FileId = fileId;
            Type = type;
            Etag = etag;
            Size = size;
        }
    }
}