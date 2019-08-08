//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		RegistryList class
// Purpose:		Object used to store data about a file's verification history

//////////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace FV.Library.Objects
{
    public class RegistryList
    {
        public int NumRegistries;
        public List<string> Names;
        public List<string> Addresses;

        public RegistryList()
        {
            NumRegistries = 0;
            Names = new List<string>();
            Addresses = new List<string>();
        }

        public void Print()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < NumRegistries; ++i)
            {
                Console.WriteLine($"{Names[i]}:\t{Addresses[i]}");
            }
            Console.ResetColor();
            Console.WriteLine();
        }

        public void AddLine(string name, string address)
        {
            Names.Add(name);
            Addresses.Add(address);
            ++NumRegistries;
        }
    }
}
