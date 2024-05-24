using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainExample
{
    public class BlockChain
    {
        private readonly int _proofOfWorkDifficulty;
        private readonly double _miningReward;
        private List<Transaction> _pendingTransactions;
        public List<Block> Chain { get; set; }
        public object Transactions { get; private set; }

        public BlockChain(int proofOfWorkDifficulty, int miningReward)
        {
            _proofOfWorkDifficulty = proofOfWorkDifficulty;
            _miningReward = miningReward;
            _pendingTransactions = new List<Transaction>();
            Chain = new List<Block> { CreateGenesisBlock() };
        }
        public void CreateTransaction(Transaction transaction)
        {
            _pendingTransactions.Add(transaction);
        }
        public void MineBlock(string minerAddress)
        {

            // ========== Outer ========== //
            //Stopwatch swOuter = System.Diagnostics.Stopwatch.StartNew();

            Transaction minerRewardTransaction = new Transaction(null, minerAddress, _miningReward);
            _pendingTransactions.Add(minerRewardTransaction);

            // ==================== Inner ==================== //
            Stopwatch swInner = System.Diagnostics.Stopwatch.StartNew();

            var endMemory = 0.0;
            var startMemory = 0.0;
            startMemory = GC.GetTotalMemory(true);

            Block block = new Block(DateTime.Now, _pendingTransactions, Chain.Last().Hash);

            endMemory = GC.GetTotalMemory(true);
            var memory = endMemory - startMemory;
            Console.WriteLine("Create Block (MEMORY): " + memory);          //**********//

            swInner.Stop();
            long elapsedMsInner = swInner.ElapsedMilliseconds;
            Console.WriteLine("Create Block (TIME): " + elapsedMsInner);
            // ==================== Inner ==================== //

            block.MineBlock(_proofOfWorkDifficulty);
            Chain.Add(block);
            _pendingTransactions = new List<Transaction>();


            //swOuter.Stop();
            //long elapsedMsOuter = swOuter.ElapsedMilliseconds;
            //Console.WriteLine("Mine Block (TIME): " + elapsedMsOuter);

        }


        public void HashCollision(Block block)
        {
            Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            int _nonce = 0;
            string newHash = "";
            while (true)
            {
                _nonce++;
                using (MD5 md5 = MD5.Create())
                {
                    string rawData = block.PreviousHash + block._timeStamp + _nonce;

                    var binFormatter = new BinaryFormatter();
                    var mStream = new MemoryStream();
                    binFormatter.Serialize(mStream, block.Transactions);
                    List<byte> toProcess = new List<byte>();
                    toProcess.AddRange(Encoding.UTF8.GetBytes(rawData));
                    toProcess.AddRange(mStream.ToArray());

                    byte[] bytes = md5.ComputeHash(toProcess.ToArray());
                     newHash = Encoding.Default.GetString(bytes);
                }
                if (newHash.Equals(block.Hash))
                {
                    break;
                }
                Console.WriteLine(_nonce);
            }

            sw.Stop();
            long elapsedMs = sw.ElapsedMilliseconds;
            Console.WriteLine("Hash Collision (TIME): " + elapsedMs);
        }

        public bool IsValidChain()
        {
            Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

            var endMemory = 0.0;
            var startMemory = 0.0;
            startMemory = GC.GetTotalMemory(true);

            for (int i = 1; i < Chain.Count; i++)
            {
                Block previousBlock = Chain[i - 1];
                Block currentBlock = Chain[i];
                string newHash = currentBlock.CreateHash();
                //is anything wrong with the hash relationship? either current block hash is wrong, or the previous block hash does not match what it should be
                if ((currentBlock.Hash != currentBlock.CreateHash()) || (currentBlock.PreviousHash != previousBlock.Hash))
                {
                    return false;
                }
            }

            endMemory = GC.GetTotalMemory(true);
            var memory = endMemory - startMemory;
            Console.WriteLine("Chain Validity Check (MEMORY): " + memory);          //**********//

            sw.Stop();
            long elapsedMs = sw.ElapsedMilliseconds;
            Console.WriteLine("Chain Validity Check (TIME): " + elapsedMs);

            return true;
        }
        
        public double GetBalance(string address)
        {
            double balance = 0;
            foreach (Block block in Chain)
            {
                foreach (Transaction transaction in block.Transactions)
                {
                    if (transaction.From == address)
                    {
                        balance -= transaction.Amount;
                    }
                    if (transaction.To == address)
                    {
                        balance += transaction.Amount;
                    }
                }
            }
            return balance;
        }
        
        private Block CreateGenesisBlock()
        {
            List<Transaction> transactions = new List<Transaction> { new Transaction("", "", 0) };
            return new Block(DateTime.Now, transactions, "0");
        }
    }
}
