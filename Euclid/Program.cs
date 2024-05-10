using System;

namespace Euclid
{
    class Program
    {
        static int x, y; // Bien toan cuc luu gia tri x, y

        static int GcdExtended(int a, int b)
        {
            // Truong hop co ban
            if (a == 0)
            {
                x = 0;
                y = 1;
                return b;
            }

            // Luu ket qua cua goi de quy
            int gcd = GcdExtended(b % a, a);
            int x1 = x;
            int y1 = y;

            // Cap nhat x,y bang ket qua cua de quy
            x = y1 - (b / a) * x1;
            y = x1;

            return gcd;
        }

        //Ham tim nghich dao modulo cua A trong modulo M
        static void ModInverse(int A, int M)
        {
            int g = GcdExtended(A, M);
            if (g != 1)
            {
                Console.WriteLine("Nghich dao khong ton tai");
            }
            else
            {
                // Xử lý trường hợp x âm
                int res = (x % M + M) % M;
                Console.WriteLine("Nghich dao cua phep nhan modulo la: " + res);
            }
        }

        static void Main(string[] args)
        {
            int A = 550;
            int M = 1759;
            //Console.OutputEncoding = System.Text.Encoding.UTF8;

            ModInverse(A, M);
        }
    }
}
