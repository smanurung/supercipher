﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace SuperCipher
{
    public class Enkripsi
    {
        private byte[][] internalKey; //digunakan pada generateInternalKey, addRoundKey

        public void generateAllInternalKey(string key) //men-generate seluruh (10) internal key dengan pseudo random
        {
            byte[] byteKey = Encoding.ASCII.GetBytes(key);
            byte[] firstHalfInternalKey = new byte[key.Length / 2];
            byte[] secondHalfInternalKey = new byte[key.Length / 2];
            
            //inisialisasi nilai internalKey
            internalKey = new byte[10][];
            for (int i = 0; i < 10; i++ )
            {
                internalKey[i] = new byte[key.Length/2];
            }

            //menjumlahkan seluruh elemen key untuk menjadi integer seed
            int sumKey = 0;
            for (int i = 0; i < key.Length; i++)
                sumKey += (int)key[i];

            //generate seluruh (i = 10) internal key
            Random rnd = new Random(sumKey);
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < key.Length / 2; j++)
                {
                    firstHalfInternalKey[j] = (byte)rnd.Next(255); //random max value = 255 (ASCII)
                    secondHalfInternalKey[j] = (byte)rnd.Next(255);
                    secondHalfInternalKey[j] = (byte)rnd.Next(255);
                    internalKey[i][j] = (byte)(firstHalfInternalKey[j] ^ secondHalfInternalKey[j]);
                }
            }
        }

        public byte[] generateNewVector(byte[] input) //men-generate satu vector terutama IV secara pseudo random --versi sementara, ada kemungkinan di non-random-kan
        {
            int sum = 0;
            byte[] result = new byte[input.Length];
            for (int i = 0; i < 10; i++)
                sum += internalKey[i][1];

            Random rnd = new Random(sum);
            for (int i = 0; i < input.Length; i++)
            {
                result[i] = (byte)(rnd.Next() % 255); //random max value = 255 (ASCII)
                result[i] = (byte)(rnd.Next() % 255); //random max value = 255 (ASCII)
            }
            return result;
        }

        public byte[] addRoundKey(byte[] b) //mengenkripsi b (hasil feistel) dengan menggunakan suatu vector dan internal key --versi sementara, kurang rumit
        {
            byte[] roundKey = generateNewVector(b);
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)((b[i] ^ roundKey[i]) ^ internalKey[i % 10][i % 4]);

            }
            return b;
        }

        //tabel lookup substitusi
        private static readonly byte[] TABLE = new byte[] { 0x26, 0xdc, 0xff, 0x00, 0xad, 0xed, 0x7a, 0xee, 0xc5, 0xfe, 0x07, 0xaf, 0x4d, 0x08, 0x22, 0x3c };
        public static byte[] Substitusi(byte[] b, string key)
        {
            MemoryStream memoryStream;
            CryptoStream cryptoStream;
            //generate rijndael
            Rijndael rijndael = Rijndael.Create();
            //menggunakan class Rfc2898DeriveBytes untuk generate IV dan Key sementara untuk substitusi
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(key, TABLE);
            rijndael.Key = pdb.GetBytes(32);
            rijndael.IV = pdb.GetBytes(16);
            memoryStream = new MemoryStream();
            cryptoStream = new CryptoStream(memoryStream, rijndael.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(b, 0, b.Length);
            cryptoStream.Close();
            return memoryStream.ToArray();
        }

        public byte[] transpose(byte[] b)
        {
            UInt32 length = (UInt32)b.Length;

            Fibonacci fib = new Fibonacci();
            fib.start();

            //initialize value
            UInt32 i = fib.next(), j = fib.next();

            //from-start tranposition
            while(j<length)
            {
                Byte temp = b[i];
                b[i] = b[j];
                b[j] = temp;

                i = j; j = fib.next();
            }

            //from-end transposition
            fib.start();
            i = fib.next(); j = fib.next();

            while(j<length)
            {
                Byte temp = b[length-i];
                b[length-i] = b[length-j];
                b[length-j] = temp;

                i = j; j = fib.next();
            }

            return b;
        }

        public byte[] feistel(byte[] plain, byte[][] b)
        {
            Console.WriteLine("masuk feistel");

            int totalRound = 9;

            //temp var
            byte[] L = new byte[b[0].Length];
            byte[] R = new byte[b[0].Length];

            //res var
            byte[] resL = new byte[b[0].Length];
            byte[] resR = new byte[b[0].Length];
            byte[] res = new byte[plain.Length];

            Console.WriteLine("L, R=" + L.Length + "," + R.Length);
            Console.WriteLine("resL, resR=" + resL.Length + "," + resR.Length);

            //inisialisasi bagian kanan dan kiri, disamakan dengan bentuk bytes plainteks
            for (int left = 0; left < plain.Length / 2; left++)
            {
                resL[left] = plain[left];
                L[left] = plain[left];
            }
            for (int right = plain.Length / 2; right < plain.Length; right++)
            {
                resR[right - plain.Length / 2] = plain[right];
                R[right - plain.Length / 2] = plain[right];
            }

            Console.WriteLine("Nilai awal L dan R: ");
            for (int xx = 0; xx < resL.Length; xx++)
            {
                Console.Write(resL[xx].ToString());
            }
            Console.Write(" ");
            for (int xx = 0; xx < resL.Length; xx++)
            {
                Console.Write(resR[xx].ToString());
            }
            Console.WriteLine("");

            //feistel
            int j = 0;
            do
            {
                //1 ronde feistel
                for (int i = 0; i < R.Length; i++)
                {
                    R[i] = resR[i];
                }
                for (int i = 0; i < L.Length; i++)
                {
                    L[i] = (byte)(resL[i] ^ (R[i] ^ b[j][i]));
                }
                for (int x = 0; x < L.Length; x++)
                {
                    resR[x] = L[x];
                }
                for (int y = 0; y < R.Length; y++)
                {
                    resL[y] = R[y];
                }
                Console.WriteLine("iterasi #" + (j + 1));
                for (int xx = 0; xx < resL.Length; xx++)
                {
                    Console.Write(resL[xx].ToString() + " ");
                }
                Console.Write("|");
                for (int xx = 0; xx < resL.Length; xx++)
                {
                    Console.Write(resR[xx].ToString() + " ");
                }
                Console.WriteLine("");

                j++;
            } while (j < totalRound);

            //ronde terakhir feistel
            for (int i = 0; i < R.Length; i++)
            {
                R[i] = resR[i];
            }
            for (int i = 0; i < L.Length; i++)
            {
                L[i] = (byte)(resL[i] ^ (R[i] ^ b[9][i]));
            }
            for (int x = 0; x < L.Length; x++)
            {
                resR[x] = R[x];
            }
            for (int y = 0; y < R.Length; y++)
            {
                resL[y] = L[y];
            }

            Console.WriteLine("iterasi #10");
            for (int xx = 0; xx < resL.Length; xx++)
            {
                Console.Write(resL[xx].ToString() + " ");
            }
            Console.Write("|");
            for (int xx = 0; xx < resL.Length; xx++)
            {
                Console.Write(resR[xx].ToString() + " ");
            }
            Console.WriteLine("");

            for (int i = 0; i < resL.Length; i++)
            {
                res[i] = resL[i];
            }
            for (int i = 0; i < resR.Length; i++)
            {
                res[i + resR.Length] = resR[i];
            }
            Console.Write("feistel result = ");
            for (int xx = 0; xx < res.Length; xx++)
            {
                Console.Write(res[xx]);
            }
            return res;
        }

        internal byte[] encrypt(byte[] blokPlain, byte[] key)
        {
            generateAllInternalKey(Encoding.ASCII.GetString(key));
            byte[] result = blokPlain;
            result = addRoundKey(result);
            int loop = 1;
            for (int i = 0; i < loop; i++)
            {
                result = Substitusi(result, Encoding.ASCII.GetString(key));
                result = transpose(result);
                result = feistel(result, internalKey);
                result = addRoundKey(result);
            }

            result = Substitusi(result, Encoding.ASCII.GetString(key));
            result = transpose(result);
            result = addRoundKey(result);
            return result;
        }
    }
}
