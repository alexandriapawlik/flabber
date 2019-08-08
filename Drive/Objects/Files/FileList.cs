//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		FileList class
// Purpose:		Object used to store array of File objects

//////////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace FV.Drive.Objects
{
    public class FileList
    {
        public int NumFiles { get; set; }
        public List<File> Files { get; set; }

        public FileList()
        {
            Files = new List<File>();
        }

        public void PrintFileList()
        {
            Console.WriteLine();
            for (int i = 0; i < NumFiles; ++i)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"{Files[i].FileId}: {Files[i].Name}");
                Console.ResetColor();
                Console.WriteLine($"{Files[i].Size} bytes");
                Console.WriteLine($"Type: {Files[i].Type}\t\t{Files[i].Etag}");
                Console.WriteLine();
            }
        }

        public void AddLine(string name, string fileId, string type, string etag, long size)
        {
            Files.Add(new File(name, fileId, type, etag, size));
            ++NumFiles;
        }
    }
}
