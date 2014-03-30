using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class Fibonacci
    {
        //max size: 4GB
        private UInt32 a;
        private UInt32 b;

        public void start()
        {
            a = 0;
            b = 1;
        }

        //value starts from 1
        public UInt32 next()
        {
            UInt32 temp = a;
            a = b;
            b = temp + b;
            return a;
        }
    }
}
