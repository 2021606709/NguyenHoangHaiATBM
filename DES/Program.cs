using System;
using System.Collections.Generic;
using System.Text;

namespace DES
{
    class Program
    {
        private static string Hex2Bin(string s)
        {
            Dictionary<char, string> mp = new Dictionary<char, string>
            {
                {'0', "0000"},
                {'1', "0001"},
                {'2', "0010"},
                {'3', "0011"},
                {'4', "0100"},
                {'5', "0101"},
                {'6', "0110"},
                {'7', "0111"},
                {'8', "1000"},
                {'9', "1001"},
                {'A', "1010"},
                {'B', "1011"},
                {'C', "1100"},
                {'D', "1101"},
                {'E', "1110"},
                {'F', "1111"}
            };
            StringBuilder bin = new StringBuilder();
            foreach (char c in s)
            {
                bin.Append(mp[c]);
            }
            return bin.ToString();
        }

        private static string Bin2Hex(string s)
        {
            Dictionary<string, char> mp = new Dictionary<string, char>
            {
                {"0000", '0'},
                {"0001", '1'},
                {"0010", '2'},
                {"0011", '3'},
                {"0100", '4'},
                {"0101", '5'},
                {"0110", '6'},
                {"0111", '7'},
                {"1000", '8'},
                {"1001", '9'},
                {"1010", 'A'},
                {"1011", 'B'},
                {"1100", 'C'},
                {"1101", 'D'},
                {"1110", 'E'},
                {"1111", 'F'}
            };
            StringBuilder hex = new StringBuilder();
            for (int i = 0; i < s.Length; i += 4)
            {
                string ch = "";
                ch += s[i];
                ch += s[i + 1];
                ch += s[i + 2];
                ch += s[i + 3];
                hex.Append(mp[ch]);
            }
            return hex.ToString();
        }

        private static int Bin2Dec(string binary)
        {
            int decimalNum = 0, power = 0;
            for (int i = binary.Length - 1; i >= 0; i--)
            {
                decimalNum += (int)(int.Parse(binary[i].ToString()) * Math.Pow(2, power));
                power++;
            }
            return decimalNum;
        }

        private static string Dec2Bin(int num)
        {
            string binary = Convert.ToString(num, 2);
            int len = binary.Length;
            if (len % 4 != 0)
            {
                int div = len / 4;
                int counter = (4 * (div + 1)) - len;
                binary = binary.PadLeft(binary.Length + counter, '0');
            }
            return binary;
        }

        private static string Permute(string k, List<int> arr, int n)
        {
            StringBuilder permutation = new StringBuilder();
            for (int i = 0; i < n; i++)
            {
                permutation.Append(k[arr[i] - 1]);
            }
            return permutation.ToString();
        }

        private static string ShiftLeft(string k, int nthShifts)
        {
            string s = "";
            for (int i = 0; i < nthShifts; i++)
            {
                for (int j = 1; j < k.Length; j++)
                {
                    s += k[j];
                }
                s += k[0];
                k = s;
                s = "";
            }
            return k;
        }

        private static string Xor(string a, string b)
        {
            StringBuilder ans = new StringBuilder();
            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] == b[i])
                {
                    ans.Append('0');
                }
                else
                {
                    ans.Append('1');
                }
            }
            return ans.ToString();
        }

        private static string Encrypt(string pt, List<string> rkb, List<string> rk)
        {
            string ptBin = Hex2Bin(pt);
            List<int> initialPerm = new List<int>
            {
                58, 50, 42, 34, 26, 18, 10, 2,
                60, 52, 44, 36, 28, 20, 12, 4,
                62, 54, 46, 38, 30, 22, 14, 6,
                64, 56, 48, 40, 32, 24, 16, 8,
                57, 49, 41, 33, 25, 17, 9, 1,
                59, 51, 43, 35, 27, 19, 11, 3,
                61, 53, 45, 37, 29, 21, 13, 5,
                63, 55, 47, 39, 31, 23, 15, 7
            };
            string ip = Permute(ptBin, initialPerm, 64);
            Console.WriteLine("After initial permutation: " + Bin2Hex(ip));
            string left = ip.Substring(0, 32);
            string right = ip.Substring(32);

            for (int i = 0; i < 16; i++)
            {
                string rightExpanded = Permute(right, new List<int>
                {
                    32, 1, 2, 3, 4, 5,
                    4, 5, 6, 7, 8, 9,
                    8, 9, 10, 11, 12, 13,
                    12, 13, 14, 15, 16, 17,
                    16, 17, 18, 19, 20, 21,
                    20, 21, 22, 23, 24, 25,
                    24, 25, 26, 27, 28, 29,
                    28, 29, 30, 31, 32, 1
                }, 48);
                string xorX = Xor(rightExpanded, rkb[i]);
                StringBuilder sBoxStr = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    int row = Bin2Dec(xorX[j * 6].ToString() + xorX[j * 6 + 5].ToString());
                    int col = Bin2Dec(xorX[j * 6 + 1].ToString() + xorX[j * 6 + 2].ToString() + xorX[j * 6 + 3].ToString() + xorX[j * 6 + 4].ToString());
                    int val = sbox[j][row][col];
                    sBoxStr.Append(Dec2Bin(val));
                }
                string sBoxPermute = Permute(sBoxStr.ToString(), new List<int>
                {
                    16, 7, 20, 21,
                    29, 12, 28, 17,
                    1, 15, 23, 26,
                    5, 18, 31, 10,
                    2, 8, 24, 14,
                    32, 27, 3, 9,
                    19, 13, 30, 6,
                    22, 11, 4, 25
                }, 32);
                string result = Xor(left, sBoxPermute);
                left = result;
                if (i != 15)
                {
                    string temp = right;
                    right = left;
                    left = temp;
                }
                Console.WriteLine("Round " + (i + 1) + " " + Bin2Hex(left) + " " + Bin2Hex(right) + " " + rk[i]);
            }
            string combine = left + right;
            string cipherText = Permute(combine, new List<int>
            {
                40, 8, 48, 16, 56, 24, 64, 32,
                39, 7, 47, 15, 55, 23, 63, 31,
                38, 6, 46, 14, 54, 22, 62, 30,
                37, 5, 45, 13, 53, 21, 61, 29,
                36, 4, 44, 12, 52, 20, 60, 28,
                35, 3, 43, 11, 51, 19, 59, 27,
                34, 2, 42, 10, 50, 18, 58, 26,
                33, 1, 41, 9, 49, 17, 57, 25
            }, 64);
            return cipherText;
        }

        private static string Decrypt(string ct, List<string> rkb, List<string> rk)
        {
            string ctBin = Hex2Bin(ct);
            List<int> initialPerm = new List<int>
            {
                40, 8, 48, 16, 56, 24, 64, 32,
                39, 7, 47, 15, 55, 23, 63, 31,
                38, 6, 46, 14, 54, 22, 62, 30,
                37, 5, 45, 13, 53, 21, 61, 29,
                36, 4, 44, 12, 52, 20, 60, 28,
                35, 3, 43, 11, 51, 19, 59, 27,
                34, 2, 42, 10, 50, 18, 58, 26,
                33, 1, 41, 9, 49, 17, 57, 25
            };
            string ip = Permute(ctBin, initialPerm, 64);
            string left = ip.Substring(0, 32);
            string right = ip.Substring(32);
            for (int i = 15; i >= 0; i--)
            {
                string rightExpanded = Permute(right, new List<int>
                {
                    32, 1, 2, 3, 4, 5,
                    4, 5, 6, 7, 8, 9,
                    8, 9, 10, 11, 12, 13,
                    12, 13, 14, 15, 16, 17,
                    16, 17, 18, 19, 20, 21,
                    20, 21, 22, 23, 24, 25,
                    24, 25, 26, 27, 28, 29,
                    28, 29, 30, 31, 32, 1
                }, 48);
                string xorX = Xor(rightExpanded, rkb[i]);
                StringBuilder sBoxStr = new StringBuilder();
                for (int j = 0; j < 8; j++)
                {
                    int row = Bin2Dec(xorX[j * 6].ToString() + xorX[j * 6 + 5].ToString());
                    int col = Bin2Dec(xorX[j * 6 + 1].ToString() + xorX[j * 6 + 2].ToString() + xorX[j * 6 + 3].ToString() + xorX[j * 6 + 4].ToString());
                    int val = sbox[j][row][col];
                    sBoxStr.Append(Dec2Bin(val));
                }
                string sBoxPermute = Permute(sBoxStr.ToString(), new List<int>
                {
                    16, 7, 20, 21,
                    29, 12, 28, 17,
                    1, 15, 23, 26,
                    5, 18, 31, 10,
                    2, 8, 24, 14,
                    32, 27, 3, 9,
                    19, 13, 30, 6,
                    22, 11, 4, 25
                }, 32);
                string result = Xor(left, sBoxPermute);
                left = result;
                if (i != 0)
                {
                    string temp = right;
                    right = left;
                    left = temp;
                }
            }
            string combine = left + right;
            string ptBin = Permute(combine, new List<int>
            {
                8, 40, 16, 48, 24, 56, 32, 64,
                7, 39, 15, 47, 23, 55, 31, 63,
                6, 38, 14, 46, 22, 54, 30, 62,
                5, 37, 13, 45, 21, 53, 29, 61,
                4, 36, 12, 44, 20, 52, 28, 60,
                3, 35, 11, 43, 19, 51, 27, 59,
                 2, 34, 10, 42, 18, 50, 26, 58,
                 1, 33, 9, 41, 17, 49, 25, 57
            }, 64);
            return Bin2Hex(ptBin);
        }

        private static List<List<List<int>>> sbox = new List<List<List<int>>>
        {
            new List<List<int>>
            {
                new List<int> {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7},
                new List<int> {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8},
                new List<int> {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0},
                new List<int> {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13}
            },
            new List<List<int>>
            {
                new List<int> {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10},
                new List<int> {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5},
                new List<int> {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15},
                new List<int> {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9}
            },
            new List<List<int>>
            {
                new List<int> {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8},
                new List<int> {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1},
                new List<int> {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7},
                new List<int> {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12}
            },
            new List<List<int>>
            {
                new List<int> {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15},
                new List<int> {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9},
                new List<int> {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4},
                new List<int> {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14}
            },
            new List<List<int>>
            {
                new List<int> {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9},
                new List<int> {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6},
                new List<int> {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14},
                new List<int> {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3}
            },
            new List<List<int>>
            {
                new List<int> {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11},
                new List<int> {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8},
                new List<int> {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6},
                new List<int> {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13}
            },
            new List<List<int>>
            {
                new List<int> {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1},
                new List<int> {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6},
                new List<int> {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2},
                new List<int> {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12}
            },
            new List<List<int>>
            {
                new List<int> {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7},
                new List<int> {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2},
                new List<int> {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8},
                new List<int> {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11}
            }
        };

        static void Main(string[] args)
        {
            string plainText = "123456ABCD132536";
            string key = "AABB09182736CCDD";
            List<string> rkb = new List<string>();
            List<string> rk = new List<string>();
            Console.WriteLine("Plain Text: " + plainText);
            Console.WriteLine("Key: " + key);
            Console.WriteLine();
            string keyBin = Hex2Bin(key);
            Console.WriteLine("Key in binary: " + keyBin);
            Console.WriteLine();
            int[] shiftBits = {
                1, 1, 2, 2, 2, 2, 2, 2,
                1, 2, 2, 2, 2, 2, 2, 1
            };
            string keyPerm = Permute(keyBin, new List<int>
            {
                57, 49, 41, 33, 25, 17, 9,
                1, 58, 50, 42, 34, 26, 18,
                10, 2, 59, 51, 43, 35, 27,
                19, 11, 3, 60, 52, 44, 36,
                63, 55, 47, 39, 31, 23, 15,
                7, 62, 54, 46, 38, 30, 22,
                14, 6, 61, 53, 45, 37, 29,
                21, 13, 5, 28, 20, 12, 4
            }, 56);
            Console.WriteLine("Key after initial permutation: " + Bin2Hex(keyPerm));
            Console.WriteLine();
            string left = keyPerm.Substring(0, 28);
            string right = keyPerm.Substring(28);
            for (int i = 0; i < 16; i++)
            {
                left = ShiftLeft(left, shiftBits[i]);
                right = ShiftLeft(right, shiftBits[i]);
                string combinedKey = left + right;
                string roundKey = Permute(combinedKey, new List<int>
                {
                    14, 17, 11, 24, 1, 5,
                    3, 28, 15, 6, 21, 10,
                    23, 19, 12, 4, 26, 8,
                    16, 7, 27, 20, 13, 2,
                    41, 52, 31, 37, 47, 55,
                    30, 40, 51, 45, 33, 48,
                    44, 49, 39, 56, 34, 53,
                    46, 42, 50, 36, 29, 32
                }, 48);
                rkb.Add(roundKey);
                rk.Add(Bin2Hex(roundKey));
            }
            Console.WriteLine();
            string cipherText = Encrypt(plainText, rkb, rk);
            Console.WriteLine("Cipher Text: " + cipherText);
            Console.WriteLine();
            string decryptText = Decrypt(cipherText, rkb, rk);
            Console.WriteLine("Plain Text: " + decryptText);
            Console.ReadLine();
        }
    }
}