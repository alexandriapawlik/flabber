﻿//////////////////////////////////////////////

// Project:		File Verification Web App
// Author:		Alexandria Pawlik

// File:		History class
// Purpose:		Object used to store data about a file's verification history

//////////////////////////////////////////////


using System;
using System.Collections.Generic;

namespace FV.Eth.Objects
{
    public class History
    {
        public int NumReceipts;
        public List<string> DateTimes;
        public List<string> States;

        public History()
        {
            NumReceipts = 0;
            DateTimes = new List<string>();
            States = new List<string>();
        }

        public void PrintHistory()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < NumReceipts; ++i)
            {
                Console.WriteLine($"{DateTimes[i]}:\t{States[i]}");
            }
            Console.ResetColor();
        }

        public void AddLine(string dateTime, string state)
        {
            DateTimes.Add(dateTime);
            States.Add(state);
            ++NumReceipts;
        }
    }
}
