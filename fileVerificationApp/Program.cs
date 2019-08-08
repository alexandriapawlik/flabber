//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		driver
// Purpose:		Initiates VerificationManager object and Library Manager object
//              and provides options to user

//////////////////////////////////////////////


using System;

using FV.Verification.Drive.Objects;
using FV.Verification.Eth.Objects;
using FV.Verification;
using FV.Library;
using FV.Library.Objects;

namespace FV
{
    public class Program
    {
        static void Main(string[] args)
        {
            LibraryManager library = new LibraryManager();

            // loop until user selects exit
            bool again = true;
            while (again)
            {
                try
                {
                    Console.WriteLine("Existing file registries:");
                    RegistryList list = library.GetList().GetAwaiter().GetResult();
                    list.Print();

                    int option = GetOption1();
                    if (option == 1)
                    {
                        NewRegistry(library);
                        again = false;
                    }
                    else if (option == 2)
                    {
                        ExistingRegistry();
                    }
                    else
                    {
                        again = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    again = true;
                }
                Console.WriteLine();
            }
            Console.WriteLine("Exiting...");
        }

        private static int GetOption1()
        {
            while (true)
            {
                Console.WriteLine("1. Add a new file registry");
                Console.WriteLine("2. Work in an existing file registry");
                Console.WriteLine("3. Exit");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter choice:");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.WriteLine();

                if (Int32.TryParse(input, out int number))
                {
                    // input can be parsed
                    if ((number != 1) && (number != 2) && (number != 3))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Choose 1, 2, or 3");
                        Console.ResetColor();
                    }
                    else
                    {
                        return number;
                    }
                }
                else
                {
                    // input cannot be parsed
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not an integer");
                    Console.ResetColor();
                }
            }
        }

        private static int GetOption2()
        {
            while (true)
            {
                Console.WriteLine("1. Add a new file to the registry");
                Console.WriteLine("2. Verify an existing file");
                Console.WriteLine("3. Back");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Enter choice:");
                Console.ResetColor();

                string input = Console.ReadLine();
                Console.WriteLine();
                
                if (Int32.TryParse(input, out int number))
                {
                    // input can be parsed
                    if ((number != 1) && (number != 2) && (number != 3))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Choose 1, 2, or 3");
                        Console.ResetColor();
                    }
                    else
                    {
                        return number;
                    }
                }
                else
                {
                    // input cannot be parsed
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Not an integer");
                    Console.ResetColor();
                }
            }
        }

        private static void NewRegistry(LibraryManager library)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Name for the new file registry:");
            Console.ResetColor();

            string name = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Description of the new file registry:");
            Console.ResetColor();

            string description = Console.ReadLine();
            Console.WriteLine();

            string address = library.NewRegistry(name, description).GetAwaiter().GetResult();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"The new registry's smart contract address is: {address}");
            Console.WriteLine("COPY THIS VALUE");
            Console.WriteLine("Follow the help guide's instructions to add this registry to the app before continuing");
            Console.ResetColor();
            Console.WriteLine();
        }

        private static void ExistingRegistry()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Address of the registry you'd like to work in:");
            Console.ResetColor();

            string address = Console.ReadLine();
            Console.WriteLine();

            Console.WriteLine("Logging in to OneDrive...");
            Console.WriteLine();

            VerificationManager manager = new VerificationManager(address);

            // loop until user selects exit
            bool again = true;
            while (again)
            {
                try
                {
                    int option = GetOption2();
                    if (option == 1)
                    {
                        AddingSync(manager);
                    }
                    else if (option == 2)
                    {
                        VerifyingSync(manager);
                    }
                    else
                    {
                        again = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ResetColor();
                    again = true;
                }
                Console.WriteLine();
            }
        }

        private static void AddingSync(VerificationManager manager)
        {
            // list unregistered files
            Console.WriteLine("Unregistered files:");
            FileList list = manager.GetFilesToRegister().GetAwaiter().GetResult();
            list.Print();

            // register new file
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the ID of the file you want to add to the registry:");
            Console.ResetColor();
            string fileId = Console.ReadLine();
            string name = manager.RegisterFile(fileId).GetAwaiter().GetResult();

            // success
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{name} was added to registry");
            Console.ResetColor();
        }

        private static void VerifyingSync(VerificationManager manager)
        {
            // list registered files
            Console.WriteLine("Registered files:");
            FileList list = manager.GetFilesToVerify().GetAwaiter().GetResult();
            list.Print();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the ID of the file you want to verify:");
            Console.ResetColor();
            string fileId = Console.ReadLine();

            // verify file
            string state = manager.VerifyFile(fileId).GetAwaiter().GetResult();

            // get history
            History history = manager.GetHistory(fileId).GetAwaiter().GetResult();

            // print
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Receipt #{history.NumReceipts} added");
            Console.WriteLine($"Status: {state}");
            Console.ResetColor();
            Console.WriteLine();

            // print history
            Console.WriteLine("Verification history:");
            history.Print();
        }
    }
}
