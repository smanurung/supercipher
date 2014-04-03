using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class Dekripsi
    {
        byte[][] internalKey; //digunakan pada generateInternalKey, addRoundKey

        public void generateAllInternalKey(string key) //men-generate seluruh (10) internal key dengan pseudo random
        {
            byte[] byteKey = Encoding.ASCII.GetBytes(key);
            byte[] firstHalfInternalKey = new byte[key.Length / 2];
            byte[] secondHalfInternalKey = new byte[key.Length / 2];
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
            int sumInput = 0;
            for (int i = 0; i < input.Length; i++)
                sumInput += (int)input[i];

            Random rnd = new Random(sumInput);
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = (byte)(rnd.Next(255)); //random max value = 255 (ASCII)
            }
            return input;
        }

        public byte[] addRoundKey(byte[] b) //mengenkripsi b (hasil feistel) dengan menggunakan suatu vector dan internal key --versi sementara, kurang rumit
        {
            byte[] roundKey = generateNewVector(b);
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)((b[i] ^ internalKey[i % 10][i]) ^ roundKey[i]);
            }
            return b;
        }

        public byte[] transpose(byte[] b)
        {
            UInt32 length = (UInt32)b.Length;
            UInt32 maxi = 0, maxj = 0;

            Fibonacci fib = new Fibonacci();
            fib.start();

            //initialize value
            UInt32 i = 0, j = 0;

            //find fibonacci limit
            while (j < length)
            {
                maxi = j; maxj = fib.next();
            }

            //from-start backward-transposition
            fib.start(maxi,maxj);
            j = fib.prev(); i = fib.prev();
            while (i > 0)
            {
                Byte temp = b[length - i - 1];
                b[length - i - 1] = b[length - j - 1];
                b[length - j - 1] = temp;

                j = i; i = fib.prev();
            }

            //from-end backward-transposition
            fib.start(maxi,maxj);
            j = fib.prev(); i = fib.prev();
            while(i > 0)
            {
                Byte temp = b[i];
                b[i] = b[j];
                b[j] = temp;

                j = i; i = fib.prev();
            }

            return b;
        }

        internal byte[] decrypt(byte[] blokCipher, byte[] key)
        {
            byte[] result = blokCipher;
            result = addRoundKey(blokCipher);
            result = transpose(result);
            for (int i = 0; i < 8; i++)
            {
                result = addRoundKey(result);
                result = transpose(result);
            }
            result = addRoundKey(result);
            return result;
        }
    }
}
