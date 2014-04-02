using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class CFB
    {
        private String plain;
        private String cipher;
        private String key;
        private String iv;

        public CFB(String plain, String key, String iv)
        {
            this.plain = plain;
            this.key = key;
            this.iv = iv;
            this.cipher = "";
        }

        public String encrypt()
        {
            //form shift register
            byte[] register = Encoding.ASCII.GetBytes(this.iv);
            byte[] ikey = new byte[register.Length];

            //start encryption
            Enkripsi enkripsi = new Enkripsi();

            byte[] pbyte = Encoding.ASCII.GetBytes(this.plain);
            byte[] cbyte = new byte[pbyte.Length];

            int i = 0;
            foreach(byte b in pbyte)
            {
                ikey = enkripsi.encrypt(Encoding.ASCII.GetBytes(key), register);

                //get least significant byte
                Byte first = ikey[0];

                //xor first with b
                Byte temp = (byte)(first ^ b);

                //insert cipher
                cbyte[i] = temp;
                i += 1;

                //wrap register
                Array.Copy(register, 1, register, 0, register.Length-1);
                register[register.Length - 1] = temp;
            }

            return Encoding.ASCII.GetString(cbyte);
        }

        public String decrypt()
        {
            return null;
        }
    }
}
