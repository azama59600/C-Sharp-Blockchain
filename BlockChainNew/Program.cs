using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime;
using System.Runtime.CompilerServices;

namespace BlockChainExample
{
    class Program
    {
        static void Main()
        {

            for (int k = 1; k<2; k++)
            {
                Console.WriteLine("\nProof of Work = " + k);

                for (int i = 1000; i <= 10000; i += 1000)
                {

                    Console.WriteLine("\n" + i);
                    //create the miner and two user addresses
                    const string minerAddress = "miner1";
                    const string user1Address = "A";
                    const string user2Address = "B";
                    BlockChain blockChain = new BlockChain(proofOfWorkDifficulty: k, miningReward: 10); //set the mining reward and difficulty



                    Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();              //**********//

                    var startMemory = GC.GetTotalMemory(true);

                    //var startMemoryPM = 0.0;
                    //var startMemoryVM = 0.0;
                    //var startWorkingSet = 0.0;
                    //var startWorkingSetPeak = 0.0;
                    //var startMemoryVMPeak = 0.0;

                    //using (Process proc = Process.GetCurrentProcess())
                    //{
                    //    startMemoryPM = proc.PrivateMemorySize64;
                    //    startMemoryVM = proc.VirtualMemorySize64;
                    //    startWorkingSet = proc.WorkingSet64;
                    //    startWorkingSetPeak = proc.PeakWorkingSet64;
                    //    startMemoryVMPeak = proc.PeakVirtualMemorySize64;

                    //}

                    //Console.WriteLine("Adding Transactions (STARTMEMORYPM): " + startMemoryPM);          //**********//
                    //Console.WriteLine("Adding Transactions (STARTMEMORYVM): " + startMemoryVM);          //**********//
                    //Console.WriteLine("Adding Transactions (STARTWORKINGSET): " + startWorkingSet);          //**********//
                    //Console.WriteLine("Adding Transactions (STARTWORKINGSETPEAK): " + startWorkingSetPeak);          //**********//
                    //Console.WriteLine("Adding Transactions (STARTMEMORYVMPEAK): " + startMemoryVMPeak);          //**********//


                    for (int j = 0; j < i; j++) // create the transactions (up to the limit of i)
                    {
                        blockChain.CreateTransaction(new Transaction(user1Address, user2Address, j + 1));                                                                      // is the chain valid

                    }

                    //var endMemoryPM = 0.0;
                    //var endMemoryVM = 0.0;
                    //var endWorkingSet = 0.0;
                    //var endWorkingSetPeak = 0.0;
                    //var endMemoryVMPeak = 0.0;

                    //using (Process proc = Process.GetCurrentProcess())
                    //{
                    //    endMemoryPM = proc.PrivateMemorySize64;
                    //    endMemoryVM = proc.VirtualMemorySize64;
                    //    endWorkingSet = proc.WorkingSet64;
                    //    endWorkingSetPeak = proc.PeakWorkingSet64;
                    //    endMemoryVMPeak = proc.PeakVirtualMemorySize64;
                    //}

                    //Console.WriteLine("Adding Transactions (ENDMEMORYPM): " + endMemoryPM);          //**********//
                    //Console.WriteLine("Adding Transactions (ENDMEMORYVM): " + endMemoryVM);          //**********//
                    //Console.WriteLine("Adding Transactions (ENDWORKINGSET): " + endWorkingSet);          //**********//
                    //Console.WriteLine("Adding Transactions (ENDWORKINGSETPEAK): " + endWorkingSetPeak);          //**********//
                    //Console.WriteLine("Adding Transactions (ENDMEMORYVMPEAK): " + endMemoryVMPeak);          //**********//


                    var endMemory = GC.GetTotalMemory(true);
                    var memory = endMemory - startMemory;
                    //var memoryPM = endMemoryPM - startMemoryPM;
                    //var memoryVM = endMemoryVM - startMemoryVM;
                    //var workingSet = endWorkingSet - startWorkingSet;


                    Console.WriteLine("Adding Transactions (MEMORY): " + memory);          //**********//
                    //Console.WriteLine("Adding Transactions (MEMORYPM): " + memoryPM);          //**********//
                    //Console.WriteLine("Adding Transactions (MEMORYVM): " + memoryVM);          //**********//
                    //Console.WriteLine("Adding Transactions (WORKINGSET): " + workingSet);          //**********//


                    sw.Stop();                                                              //**********//
                    long elapsedMs = sw.ElapsedMilliseconds;                                //**********//
                    Console.WriteLine("Adding Transactions (TIME): " + elapsedMs);          //**********//
                    //Console.WriteLine(i + " " + elapsedMs);


                    Stopwatch sw2 = System.Diagnostics.Stopwatch.StartNew();                //**********//

                    var endMemory2 = 0.0;
                    var startMemory2 = 0.0;

                    startMemory2 = GC.GetTotalMemory(true);

                    blockChain.MineBlock(minerAddress);

                    endMemory2 = GC.GetTotalMemory(true);
                    var memory2 = endMemory2 - startMemory2;
                    Console.WriteLine("Mining (MEMORY): " + memory2);          //**********//

                    sw2.Stop();                                                             //**********//
                    long elapsedMs2 = sw2.ElapsedMilliseconds;                              //**********//
                    Console.WriteLine("Mining (TIME): " + elapsedMs2);                      //**********//
                    //Console.WriteLine(i + " " + elapsedMs2);

                    // Console.WriteLine("Is valid: {0}", blockChain.IsValidChain());
                    // Console.WriteLine();
                    //Console.WriteLine("--------- Start mining ---------");
                    // blockChain.MineBlock(minerAddress);

                    // blockChain.HashCollision(blockChain.Chain[0]);


                    //is the chain valid?
                    //Console.WriteLine("Is valid: {0}", blockChain.IsValidChain());


                    blockChain.IsValidChain();

                }
            }

            Console.ReadKey();
        }


        private static void PrintChain(BlockChain blockChain)
        {
            Console.WriteLine("----------------- Start Blockchain -----------------");
            foreach (Block block in blockChain.Chain)
            {
                Console.WriteLine();
                Console.WriteLine("------ Start Block ------");
                Console.WriteLine("Hash: {0}", block.Hash);
                Console.WriteLine("Previous Hash: {0}", block.PreviousHash);
                Console.WriteLine("--- Start Transactions ---");
                foreach (Transaction transaction in block.Transactions)
                {
                    Console.WriteLine("From: {0} To {1} Amount {2}", transaction.From, transaction.To, transaction.Amount);
                }
                Console.WriteLine("--- End Transactions ---");
                Console.WriteLine("------ End Block ------");
            }
            Console.WriteLine("----------------- End Blockchain -----------------");
        }
    }
}
