using System;
using System.Numerics;
using System.Collections.Generic;
namespace Elgamal
{
    class Program
    {
        static readonly Random random = new Random();

        static BigInteger GCD(BigInteger a, BigInteger b)
        {
            while (b != 0)
            {
                BigInteger temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }

        static BigInteger GenKey(BigInteger q)
        {
            BigInteger maxInt32Value = new BigInteger(int.MaxValue);
            BigInteger key = RandomBigInteger(BigInteger.Pow(10, 20), maxInt32Value);
            while (GCD(q, key) != 1)
            {
                key = RandomBigInteger(BigInteger.Pow(10, 20), maxInt32Value);
            }
            return key;
        }

        static BigInteger Power(BigInteger a, BigInteger b, BigInteger c)
        {
            BigInteger x = 1;
            BigInteger y = a;

            while (b > 0)
            {
                if (b % 2 != 0)
                    x = (x * y) % c;
                y = (y * y) % c;
                b /= 2;
            }

            return x % c;
        }

        static Tuple<List<BigInteger>, BigInteger> Encrypt(string msg, BigInteger q, BigInteger h, BigInteger g)
        {
            List<BigInteger> enMsg = new List<BigInteger>();

            BigInteger k = GenKey(q);
            BigInteger s = Power(h, k, q);
            BigInteger p = Power(g, k, q);

            foreach (char c in msg)
            {
                enMsg.Add(s * c);
            }

            return Tuple.Create(enMsg, p);
        }

        static List<char> Decrypt(List<BigInteger> enMsg, BigInteger p, BigInteger key, BigInteger q)
        {
            List<char> drMsg = new List<char>();

            BigInteger h = Power(p, key, q);
            foreach (BigInteger c in enMsg)
            {
                drMsg.Add((char)(c / h));
            }

            return drMsg;
        }

        static BigInteger RandomBigInteger(BigInteger min, BigInteger max)
        {
            byte[] bytes = new byte[max.ToByteArray().Length];
            random.NextBytes(bytes);
            BigInteger result = new BigInteger(bytes);
            return result % (max - min) + min;
        }

        static void Main(string[] args)
        {
            string msg = "Truong Dai Hoc Cong Nghiep 2 HsnS";
            Console.WriteLine("Original Message: " + msg);

            BigInteger q = RandomBigInteger(BigInteger.Pow(10, 20), BigInteger.Pow(10, 50));
            BigInteger g = RandomBigInteger(2, q);

            BigInteger key = GenKey(q);
            BigInteger h = Power(g, key, q);
            Console.WriteLine("g used: " + g);
            Console.WriteLine("g^a used: " + h);

            var encryptionResult = Encrypt(msg, q, h, g);
            List<BigInteger> enMsg = encryptionResult.Item1;
            BigInteger p = encryptionResult.Item2;

            List<char> drMsg = Decrypt(enMsg, p, key, q);
            string dmsg = new string(drMsg.ToArray());
            Console.WriteLine("Decrypted Message: " + dmsg);
        }
    }
}
