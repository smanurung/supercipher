using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    public class Enkripsi
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
                    firstHalfInternalKey[j] = (byte)rnd.Next(255); //random max value = 128 (half ASCII)
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
                input[i] = (byte)(rnd.Next(255)); //random max value = 128 (half ASCII)
            }
            return input;
        }

        public byte[] addRoundKey(byte[] b) //mengenkripsi b (hasil feistel) dengan menggunakan suatu vector dan internal key --versi sementara, kurang rumit
        {
            byte[] roundKey = generateNewVector(b);
            for (int i = 0; i < b.Length; i++)
            {
                b[i] = (byte)((b[i] ^ roundKey[i]) ^ internalKey[i % 10][i]);
            }
            return b;
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

        internal byte[] encrypt(byte[] p, byte[] register)
        {
            throw new NotImplementedException();
        }

        internal byte[] encrypt(string p1, byte[] p2)
        {
            throw new NotImplementedException();
        }
    }
}
