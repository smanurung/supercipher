using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class CFB
    {
        //private String plain;
        //private String cipher;
        private String key;
        private string iv;

        private byte[] plain;
        private byte[] cipher;

        public CFB(byte[] plain, byte[] cipher, String key, String iv)
        {
            this.plain = plain;
            this.key = key;
            this.iv = iv;
            this.cipher = cipher;
        }

        public byte[] encrypt()
        {
            //form shift register
            byte[] register = Encoding.ASCII.GetBytes(this.iv);
            byte[] ikey = new byte[register.Length];

            //start encryption
            Enkripsi enkripsi = new Enkripsi();

            Int32 blok = Encoding.ASCII.GetBytes(this.key).Length;
            //byte[] pbyte = Encoding.ASCII.GetBytes(this.plain);
            byte[] pbyte = this.plain;
            byte[] cbyte = new byte[pbyte.Length];

            for (int i = 0; i < (pbyte.Length / blok); i++)
            {
                //get internal key
                ikey = enkripsi.encrypt(Encoding.ASCII.GetBytes(key), register);

                //get block-sized plaintext
                byte[] pi = new byte[blok];
                byte[] ci = new byte[blok];

                //if((pbyte.Length - (i * blok)) >= blok)
                //{
                //    pi = pbyte.Skip(i * blok).Take(blok).ToArray();
                //}
                //else
                //{
                //    pi = pbyte.Skip(i * blok).Take(pbyte.Length - (i * blok)).ToArray();
                //    int padsize = blok - pbyte.Length + (i * blok);

                //    insert padding 0
                //    for(int k = 0; k < padsize; k++)
                //    {
                //        pi[blok - k] = 0;
                //    }
                //}

                pi = pbyte.Skip(i * blok).Take(pbyte.Length - (i * blok)).ToArray();

                ci = xor(pi, ikey);
                for (int m = 0; m < blok; m++)
                {
                    cbyte[i * blok + m] = ci[m];
                }

                //wrap register
                register = cbyte;
            }

            //this.cipher = Encoding.ASCII.GetString(cbyte);
            this.cipher = cbyte;
            return this.cipher;
        }

        public byte[] xor(byte[] i, byte[] key)
        {
            byte[] res = new byte[i.Length];

            for (int j = 0; j < i.Length; j++)
            {
                res[j] = (byte)(i[j] ^ key[j]);
            }
            
            return res;
        }

        public byte[] decrypt()
        {
            //form shift register
            byte[] register = Encoding.ASCII.GetBytes(this.iv);
            byte[] ikey = new byte[register.Length];

            //start decryption
            Enkripsi enkripsi = new Enkripsi();

            Int32 blok = Encoding.ASCII.GetBytes(this.key).Length;
            //byte[] cbyte = Encoding.ASCII.GetBytes(this.cipher);
            byte[] cbyte = this.cipher;
            byte[] pbyte = new byte[cbyte.Length];

            for (int i = 0; i < (cbyte.Length / blok); i++)
            {
                //get internal key
                ikey = enkripsi.encrypt(Encoding.ASCII.GetBytes(key), register);

                //get block-sized plaintext
                byte[] ci = new byte[blok];
                byte[] pi = new byte[blok];

                ci = cbyte.Skip(i * blok).Take(cbyte.Length - (i * blok)).ToArray();

                pi = xor(ci, ikey);
                for (int m = 0; m < blok; m++)
                {
                    pbyte[i * blok + m] = pi[m];
                }

                //wrap register
                register = cbyte;
            }

            //this.plain = Encoding.ASCII.GetString(pbyte);
            this.plain = pbyte;
            return this.plain;
        }
    }
}
