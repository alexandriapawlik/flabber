//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		driver
// Purpose:		Initiates VerificationManager object and provides options to user

//////////////////////////////////////////////


using System;
using System.Threading.Tasks;

using FV.Drive.Objects;
using FV.Eth.Objects;
using FV.Infrastructure;

namespace FV
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Enter the address of the registry you want to add a receipt to: ");
            //string address = Console.ReadLine();
            string address = Constants.TestRegistry;

            Console.WriteLine("Logging in...");
            Console.WriteLine();

            VerificationManager manager = new VerificationManager(address);

            // loop until user selects exit
            bool again = true;
            while (again)
            {
                try
                {
                    int option = GetOption();
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
            Console.WriteLine("Exiting...");
        }

        private static void AddingSync(VerificationManager manager)
        {
            // list unregistered files
            Console.WriteLine("Unregistered files:");
            FileList list = manager.GetFilesToRegister().GetAwaiter().GetResult();
            list.PrintFileList();

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
            FileList list = manager.GetFileList().GetAwaiter().GetResult();
            list.PrintFileList();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Enter the ID of the file you want to verify:");
            Console.ResetColor();
            string fileId = Console.ReadLine();

            // verify file
            string state = manager.VerifyFile(fileId).GetAwaiter().GetResult();

            // success
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Status: {state}");
            Console.ResetColor();
            Console.WriteLine();

            // get history
            Console.WriteLine("Verification history:");
            History history = manager.GetHistory(fileId).GetAwaiter().GetResult();
            history.PrintHistory();
        }

        private static int GetOption()
        {
            while (true)
            {
                Console.WriteLine("1. Add a new file to the registry");
                Console.WriteLine("2. Verify an existing file");
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
    }
}
