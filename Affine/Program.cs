using System;

namespace Affine
{
    class Program
    {
        static Tuple<int, int, int> egcd(int a, int b)
        {
            int x = 0, y = 1, u = 1, v = 0;
            while (a != 0)
            {
                int q = b / a;
                int r = b % a;
                int m = x - u * q;
                int n = y - u * q;
                b = a;
                a = r;
                x = u;
                y = v;
                u = m;
                v = n;
            }
            return Tuple.Create(b, x, y);
        }

        static int modinv(int a, int m)
        {
            Tuple<int, int, int> results = egcd(a, m);
            int gcd = results.Item1;
            int x = results.Item2;
            int y = results.Item3;
            if(gcd != 1)
            {
                return -1;
            }
            else
            {
                return (x % m + m) % m;
            }
        }

        static string AffineEncrypt(string text, int[] key)
        {
            //E = aP+b % 26
            string encryptText = "";
            foreach(char c in text)
            {
                if (char.IsUpper(c))
                {
                    int p = c - 'A';
                    int encryptChar = (key[0] * p + key[1]) % 26 + 'A';
                    encryptText += (Char)encryptChar;
                }else if (char.IsLower(c))
                {
                    int p = c - 'a';
                    int encryptChar = (key[0] * p + key[1]) % 26 + 'a';
                    encryptText += (Char)encryptChar;
                }
                else
                {
                    encryptText += c;
                }
                
            }
            return encryptText;
        }

        static string AffineDecrypt(string cipher,int[] key)
        {
            //D = (a^-1 * (E-b)) % 26
            string decryptText = "";
            int modInv = modinv(key[0], 26);
            if(modInv == -1)
            {
                Console.WriteLine("Khong ton tai modulo nghich dao cho a va 26");
                return "";
            }
            foreach(char c in cipher)
            {
                if (char.IsUpper(c))
                {
                    int decryptChar = (modInv * (c - 'A' - key[1] + 26)) % 26 + 'A';
                    decryptText += (Char)decryptChar;
                }
                else if (char.IsLower(c))
                {    
                    int decryptChar = (modInv * (c - 'a' - key[1] + 26)) % 26 + 'a';
                    decryptText += (Char)decryptChar;
                }
                else
                {
                    decryptText += c;
                }
            }
            return decryptText;
        }
        static void Main(string[] args)
        {
            string text = "AFFIn2G CIPHER";
            int[] key = { 17, 20 };

            string encryptedText = AffineEncrypt(text, key);
            Console.WriteLine("Encrypted Text: " + encryptedText);

            string decryptedText = AffineDecrypt(encryptedText, key);
            Console.WriteLine("Decrypted Text: " + decryptedText);
        }
    }
}
