using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class Enkripsi
    {
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
    }
}
