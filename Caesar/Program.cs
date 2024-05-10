using System;

namespace Caesar
{
    class Program
    {
        static string CeasarEncrypt(String text, int s)
        {
            string result = "";

            //Duyet ky tu trong van ban
            foreach (char c in text)
            {
                if (char.IsUpper(c))
                    result += (char)((c + s - 65) % 26 + 65);
                else if (char.IsLower(c))
                    result += (char)((c + s - 97) % 26 + 97);
                else
                    result += c;
            }
            return result;
        }
        static string CeasarDecrypt(String text, int s)
        {
            string result = "";

            //Duyet ky tu trong van ban
            foreach (char c in text)
            {
                if (char.IsUpper(c))
                    result += (char)((c - s - 65) % 26 + 65);
                else if (char.IsLower(c))
                    result += (char)((c - s - 97) % 26 + 97);
                else
                    result += c;
            }
            return result;
        }
        static void Main(string[] args)
        {
            Console.Write("Nhap vao mot chuoi: ");
            string text = Convert.ToString(Console.ReadLine());
            int s = 4;

            Console.WriteLine("Van ban : " + text);
            Console.WriteLine("Dich chuyen : " + s);
            Console.WriteLine("Ma hoa: " + CeasarEncrypt(text, s));
            Console.WriteLine("Giai ma: " + CeasarDecrypt(CeasarEncrypt(text, s), s));
        }
    }
}
