using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class Enkripsi
    {
        byte[][] internalKey; //digunakan pada generateInternalKey, addRoundKey

        public byte[][] generateAllInternalKey(string key) //men-generate seluruh (10) internal key dengan pseudo random
        {
            byte[] byteKey = Encoding.ASCII.GetBytes(key);
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (i == 0)
                        internalKey[i][j] = (byte)rnd.Next((int)byteKey[j]);
                    else
                        internalKey[i][j] = (byte)rnd.Next((int)internalKey[i-1][j]);
                }
            }
            return internalKey;
        }

        public byte[] generateNewVector(byte[] input) //men-generate satu vector terutama IV secara pseudo random --versi sementara, ada kemungkinan di non-random-kan
        {
            Random rnd = new Random();
            for (int i = 0; i < input.Length; i++)
            {
                input[i] = (byte)(rnd.Next(input[0]));
            }
            return input;
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

        public byte[] addRoundKey(byte[] b) //mengenkripsi b (hasil feistel) dengan menggunakan suatu vector dan internal key --versi sementara, kurang rumit
        {
            byte[] roundKey = generateNewVector(b);
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)(b[i] ^ roundKey[i] ^ internalKey[2][i]);
            }
            return b;
        }
    }
}
