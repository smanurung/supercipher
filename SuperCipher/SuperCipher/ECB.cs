using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class ECB
    {
        private byte[] plain;
        private byte[] cipher;
        private String key;
        private String iv;

        public ECB(byte[] _plain, byte[] _cipher, String _key, String _iv)
        {
            this.plain = _plain;
            this.cipher = _cipher;
            this.key = _key;
            this.iv = _iv;
        }

        public byte[] encrypt()
        {
            Enkripsi enkripsi = new Enkripsi();
            int leftover = plain.Length % key.Length;
            int blockTotal = plain.Length / key.Length;
            for (int i = 0; i < blockTotal; i++)
            {
                byte[] blockPlain = new byte [key.Length];
                Array.Copy(plain, key.Length * i, blockPlain, 0, key.Length);
                this.cipher = enkripsi.encrypt(blockPlain, Encoding.ASCII.GetBytes(key));
            }
            if (leftover > 0)
            {
                byte[] tmp = new byte[this.plain.Length + key.Length - leftover];
                byte paddingByte = 0;
                for (int i = 0; i < key.Length - leftover; i++)
                    tmp[blockTotal * key.Length + i] = paddingByte;
                byte[] blockPlain = new byte[key.Length];
                Array.Copy(tmp, key.Length * blockTotal, blockPlain, 0, key.Length);
                this.cipher = enkripsi.encrypt(blockPlain, Encoding.ASCII.GetBytes(key));
            }
            return this.cipher;
        }

        public byte[] decrypt()
        {
            Dekripsi dekripsi = new Dekripsi();
            int blockTotal = plain.Length / key.Length;
            for (int i = 0; i < blockTotal; i++)
            {
                byte[] blockCipher = new byte[key.Length];
                Array.Copy(cipher, key.Length * i, blockCipher, 0, key.Length);
                this.plain = dekripsi.decrypt(blockCipher, Encoding.ASCII.GetBytes(key)));
            }
            return this.plain;
        }

    }
}
