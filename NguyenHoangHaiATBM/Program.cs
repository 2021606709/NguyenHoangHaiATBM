using System;

namespace NguyenHoangHaiATBM
{
    class Program
    {
        //Ham nghich dao cua a modulo m
        static int modInverse(int a, int m)
        {
            for (int x = 1; x < m; x++)
            {
                if (((a % m) * (x % m)) % m == 1)
                    return x;
            }
            return -1;
        }

        static void Main(string[] args)
        {
            int a = 550;
            int b = 1759;
            Console.WriteLine(modInverse(a, b));
        }
    }
}
