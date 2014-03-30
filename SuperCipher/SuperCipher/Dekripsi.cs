using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class Dekripsi
    {
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
    }
}
