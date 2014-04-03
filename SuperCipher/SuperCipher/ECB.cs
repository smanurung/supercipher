using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class ECB
    {
        private String plain;
        private String cipher;
        private String key;
        private String iv;

        public ECB(String _plain, String _key, String _cipher, String _iv)
        {
            this.plain = _plain;
            this.cipher = _cipher;
            this.key = _key;
            this.iv = _iv;
        }

        public String encrypt()
        {
            Enkripsi enkripsi = new Enkripsi();
            int leftover = plain.Length % key.Length;
            int blockTotal = plain.Length / key.Length;
            for (int i = 0; i < blockTotal; i++)
            {
                this.cipher += Encoding.ASCII.GetString(enkripsi.encrypt(this.plain.Substring(i,key.Length), Encoding.ASCII.GetBytes(key)));
            }
            if (leftover > 0)
            {
                String tmp = this.plain;
                String paddingChar = "z";
                for (int i = 0; i < key.Length - leftover; i++)
                    tmp += paddingChar;
                this.cipher += Encoding.ASCII.GetString(enkripsi.encrypt(this.plain.Substring(tmp.Length - key.Length - 1, key.Length), Encoding.ASCII.GetBytes(key)));
            }
            return this.cipher;
        }

        public String decrypt()
        {
            Dekripsi dekripsi = new Dekripsi();
            int blockTotal = plain.Length / key.Length;
            for (int i = 0; i < blockTotal; i++)
            {
                this.plain += Encoding.ASCII.GetString(dekripsi.decrypt(this.cipher.Substring(i, key.Length), Encoding.ASCII.GetBytes(key)));
            }
            return this.plain;
        }

    }
}
