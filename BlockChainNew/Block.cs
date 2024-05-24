using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace BlockChainExample
{
    public class Block
    {
        public readonly DateTime _timeStamp;
        private long _nonce;
        public string PreviousHash { get; set; }
        public List<Transaction> Transactions { get; set; }

    public string Hash { get; private set; }

    public Block(DateTime timeStamp, List<Transaction> transactions, string previousHash)
    {
        _timeStamp = timeStamp;
        _nonce = 0;
        Transactions = transactions;
        PreviousHash = previousHash;
        Hash = CreateHash();
    }


    public void MineBlock(int proofOfWorkDifficulty)
    {
        string hashValidationTemplate = new String('0', proofOfWorkDifficulty);

        Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();

        var endMemory = 0.0;
        var startMemory = 0.0;

        startMemory = GC.GetTotalMemory(true);

        while (Hash.Substring(0, proofOfWorkDifficulty) != hashValidationTemplate)
        {
            _nonce++;
            Hash = CreateHash();


            if ((Hash.Substring(0, proofOfWorkDifficulty) == hashValidationTemplate)) {
                endMemory = GC.GetTotalMemory(true);          
            }

        }

        //endMemory = GC.GetTotalMemory(false);
        var memory = endMemory - startMemory;
        Console.WriteLine("Finding a Hash (MEMORY): " + memory);          //**********//


        sw.Stop();
        long elapsedMs = sw.ElapsedMilliseconds;
        Console.WriteLine("Finding a Hash (TIME): " + elapsedMs);


        string tempHash = CreateHash();
    }


    public void CollisionTest(int proofOfWorkDifficulty, string hashToMatch)
    {
        string hashValidationTemplate = new String('0', proofOfWorkDifficulty);
        Stopwatch sw = System.Diagnostics.Stopwatch.StartNew(); //**********//
        while ((Hash.Substring(0, proofOfWorkDifficulty) != 
            hashValidationTemplate)&& Hash.Equals(hashToMatch))
        {
            _nonce++;
            Hash = CreateHash();
        }
        sw.Stop(); 
        long elapsedMs = sw.ElapsedMilliseconds; 
        Console.WriteLine("Time taken to find hash: " + elapsedMs); 
        string tempHash = CreateHash();
    }


    public string CreateHash()
    {


            //using (MD5 md5 = MD5.Create())
            //{
            //    string rawData = PreviousHash + _timeStamp + _nonce; //data is constructed of these values, and also the transaction list (below)

            //    var binFormatter = new BinaryFormatter();
            //    var mStream = new MemoryStream();
            //    binFormatter.Serialize(mStream, Transactions);
            //    List<byte> toProcess = new List<byte>();
            //    toProcess.AddRange(Encoding.UTF8.GetBytes(rawData));
            //    toProcess.AddRange(mStream.ToArray());

            //    byte[] bytes = md5.ComputeHash(toProcess.ToArray());
            //    return Encoding.Default.GetString(bytes);
            //}

            using (SHA256 sha256 = SHA256.Create())
            {
                string rawData = PreviousHash + _timeStamp + _nonce; //data is constructed of these values, and also the transaction list (below)

                var binFormatter = new BinaryFormatter();
                var mStream = new MemoryStream();
                binFormatter.Serialize(mStream, Transactions);
                List<byte> toProcess = new List<byte>();
                toProcess.AddRange(Encoding.UTF8.GetBytes(rawData));
                toProcess.AddRange(mStream.ToArray());

                byte[] bytes = sha256.ComputeHash(toProcess.ToArray());
                return Encoding.Default.GetString(bytes);
            }


            //using (SHA512 sha512 = SHA512.Create())
            //{
            //    string rawData = PreviousHash + _timeStamp + _nonce; //data is constructed of these values, and also the transaction list (below)

            //    var binFormatter = new BinaryFormatter();
            //    var mStream = new MemoryStream();
            //    binFormatter.Serialize(mStream, Transactions);
            //    List<byte> toProcess = new List<byte>();
            //    toProcess.AddRange(Encoding.UTF8.GetBytes(rawData));
            //    toProcess.AddRange(mStream.ToArray());

            //    byte[] bytes = sha512.ComputeHash(toProcess.ToArray());
            //    return Encoding.Default.GetString(bytes);
            //}


        }


    }
}

