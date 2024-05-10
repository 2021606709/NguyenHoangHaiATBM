using System;
using System.Numerics;

namespace RSA
{
    class Program
    {
        // Hàm tính ước chung lớn nhất (GCD) của hai số
        static int GCD(int a, int h)
        {
            int temp;
            while (true)
            {
                temp = a % h;
                if (temp == 0)
                    return h;
                a = h;
                h = temp;
            }
        }

        // Mã hóa c = (msg ^ e) % n
        //double c = Math.Pow(msg, e);
        //c = c % n;
        static string RSAEncrypt(String msg, BigInteger n, BigInteger e)
        {
            string result = "";

            //Duyet ky tu trong van ban
            foreach (char c in msg)
            {
                if (char.IsUpper(c))
                    result += (char)(BigInteger.ModPow(c - 65, e, n)  + 65);
                else if (char.IsLower(c))
                    result += (char)(BigInteger.ModPow(c - 97, e, n) + 97);
                else
                    result += c;
            }
            return result;
        }

        // Giải mã m = (c ^ d) % n
        //double m = Math.Pow(c, d);
        //m = m % n;
        static string RSADecrypt(String msg, BigInteger n, BigInteger d)
        {
            string result = "";

            //Duyet ky tu trong van ban
            foreach (char c in msg)
            {
                if (char.IsUpper(c))
                    result += (char)(BigInteger.ModPow(c - 65, d, n) + 65);
                else if (char.IsLower(c))
                    result += (char)(BigInteger.ModPow(c - 97, d, n) + 97);
                else
                    result += c;
            }
            return result;
        }

        static void Main(string[] args)
        {
            int p = 3;
            int q = 7;
            int n = p * q;
            int e = 2;
            int phi = (p - 1) * (q - 1);

            while (e < phi)
            {
                // e phải là số nguyên tố cùng nhau với phi và nhỏ hơn phi
                if (GCD(e, phi) == 1)
                    break;
                else
                    e++;
            }

            // Khóa riêng tư (d là decrypt)
            // chọn d sao cho nó thỏa mãn
            // d*e = 1 + k * phi

            int k = 2;
            BigInteger d = (1 + (k * phi)) / (BigInteger)e;

            // Tin nhắn cần được mã hóa
            string msg = "sgfcnqre67KJSIUhsSAHN";

            Console.WriteLine("Du lieu tin nhan = " + msg);
            string encryptText = RSAEncrypt(msg, n, e);           
            Console.WriteLine("Du lieu da ma hoa = " + encryptText);
            string decryptText = RSADecrypt(encryptText, n, d);
            Console.WriteLine("Tin nhan goc = " + decryptText);
        }
    }
}
