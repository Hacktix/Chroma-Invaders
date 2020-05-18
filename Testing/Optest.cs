using Chroma_Invaders.Opcodes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chroma_Invaders.Testing
{
    class Optest
    {
        private static int Tested = 0;
        private static int Passed = 0;

        static void Main(string[] args)
        {
            Machine testMachine = new Machine();

            #region "Increment Register"
            testMachine.Registers[Register.B] = 0;
            new IncrementOperation(testMachine, 0x04).Execute();
            if (testMachine.Registers[Register.B] != 1) LogResult("Increment Register - Any", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b00000010) LogResult("Increment Register - Any", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Register - Any", true);

            testMachine.Registers[Register.B] = 0b1111;
            new IncrementOperation(testMachine, 0x04).Execute();
            if (testMachine.Registers[Register.B] != 0b10000) LogResult("Increment Register - Auxiliary Carry", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b00010110) LogResult("Increment Register - Auxiliary Carry", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Register - Auxiliary Carry", true);

            testMachine.Registers[Register.B] = 0b1111111;
            new IncrementOperation(testMachine, 0x04).Execute();
            if (testMachine.Registers[Register.B] != 0b10000000) LogResult("Increment Register - Sign", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b10010110) LogResult("Increment Register - Sign", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Register - Sign", true);

            testMachine.Registers[Register.B] = 0b11111111;
            new IncrementOperation(testMachine, 0x04).Execute();
            if (testMachine.Registers[Register.B] != 0) LogResult("Increment Register - Zero", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b01010110) LogResult("Increment Register - Zero", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Register - Zero", true);
            #endregion

            #region "Increment Memory"
            testMachine.Memory[0] = 0;
            new IncrementOperation(testMachine, 0x34).Execute();
            if (testMachine.Memory[0] != 1) LogResult("Increment Memory - Any", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b00000010) LogResult("Increment Memory - Any", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Memory - Any", true);

            testMachine.Memory[0] = 0b1111;
            new IncrementOperation(testMachine, 0x34).Execute();
            if (testMachine.Memory[0] != 0b10000) LogResult("Increment Memory - Auxiliary Carry", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b00010110) LogResult("Increment Memory - Auxiliary Carry", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Memory - Auxiliary Carry", true);

            testMachine.Memory[0] = 0b1111111;
            new IncrementOperation(testMachine, 0x34).Execute();
            if (testMachine.Memory[0] != 0b10000000) LogResult("Increment Memory - Sign", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b10010110) LogResult("Increment Memory - Sign", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Memory - Sign", true);

            testMachine.Memory[0] = 0b11111111;
            new IncrementOperation(testMachine, 0x34).Execute();
            if (testMachine.Memory[0] != 0) LogResult("Increment Memory - Zero", false, "Register did not increment.");
            else if (testMachine.Registers[Register.F] != 0b01010110) LogResult("Increment Memory - Zero", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Increment Memory - Zero", true);
            #endregion

            #region "Decrement Register"
            testMachine.Registers[Register.B] = 6;
            new DecrementOperation(testMachine, 0x05).Execute();
            if (testMachine.Registers[Register.B] != 5) LogResult("Decrement Register - Any", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b00000010) LogResult("Decrement Register - Any", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Register - Any", true);

            testMachine.Registers[Register.B] = 0b10000;
            new DecrementOperation(testMachine, 0x05).Execute();
            if (testMachine.Registers[Register.B] != 0b1111) LogResult("Decrement Register - Auxiliary Carry", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b00010010) LogResult("Decrement Register - Auxiliary Carry", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Register - Auxiliary Carry", true);

            testMachine.Registers[Register.B] = 0;
            new DecrementOperation(testMachine, 0x05).Execute();
            if (testMachine.Registers[Register.B] != 0b11111111) LogResult("Decrement Register - Sign", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b10010010) LogResult("Decrement Register - Sign", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Register - Sign", true);

            testMachine.Registers[Register.B] = 1;
            new DecrementOperation(testMachine, 0x05).Execute();
            if (testMachine.Registers[Register.B] != 0) LogResult("Decrement Register - Zero", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b01000110) LogResult("Decrement Register - Zero", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Register - Zero", true);
            #endregion

            #region "Decrement Memory"
            testMachine.Memory[0] = 6;
            new DecrementOperation(testMachine, 0x35).Execute();
            if (testMachine.Memory[0] != 5) LogResult("Decrement Memory - Any", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b00000010) LogResult("Decrement Memory - Any", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Memory - Any", true);

            testMachine.Memory[0] = 0b10000;
            new DecrementOperation(testMachine, 0x35).Execute();
            if (testMachine.Memory[0] != 0b1111) LogResult("Decrement Memory - Auxiliary Carry", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b00010010) LogResult("Decrement Memory - Auxiliary Carry", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Memory - Auxiliary Carry", true);

            testMachine.Memory[0] = 0;
            new DecrementOperation(testMachine, 0x35).Execute();
            if (testMachine.Memory[0] != 0b11111111) LogResult("Decrement Memory - Sign", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b10010010) LogResult("Decrement Memory - Sign", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Memory - Sign", true);

            testMachine.Memory[0] = 1;
            new DecrementOperation(testMachine, 0x35).Execute();
            if (testMachine.Memory[0] != 0) LogResult("Decrement Memory - Zero", false, "Register did not decrement.");
            else if (testMachine.Registers[Register.F] != 0b01000110) LogResult("Decrement Memory - Zero", false, "Incorrect flags set. [" + Convert.ToString(testMachine.Registers[Register.F], 2) + "]");
            else LogResult("Decrement Memory - Zero", true);
            #endregion

            LogTotal();
        }

        static void LogTotal()
        {
            double percent = ((int)(((double)Passed / Tested) * 10000)) / 100.0;
            Console.WriteLine("\nTotal: Passed " + Passed + " out of " + Tested + " (" + percent + "%)");
        }

        static void LogResult(string testName, bool passed, string err = "Test failed.")
        {
            Console.ForegroundColor = passed ? ConsoleColor.Green : ConsoleColor.Red;
            Console.Write(passed ? " [GOOD] " : " [FAIL] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(testName + (passed ? "" : ": " + err));

            Tested++;
            if (passed) Passed++;
        }
    }
}
