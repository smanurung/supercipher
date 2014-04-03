using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperCipher
{
    class CBC
    {
        private String inputText; 
        private String key; 
        private String outputText; 
        private byte[] outputBytes;

        public CBC()
        {
            this.inputText = "";
            this.key = "";
            this.outputText = "";
        }

        public CBC(String _InputText, String _Key, String _OutputText)
        {
            this.inputText = _InputText;
            this.key = _Key;
            this.outputText = _OutputText;
        }

        public String getInputText()
        {
            return this.inputText;
        }

        public String getKey()
        {
            return this.key;
        }

        public String getOutputText()
        {
            return this.outputText;
        }

        public byte[] getOutputBytes()
        {
            return this.outputBytes;
        }

        public void setInputText(String _InputText)
        {
            this.inputText = _InputText;
        }

        public void setKey(String _Key)
        {
            this.key = _Key;
        }

        public void setOutputText(String _OutputText)
        {
            this.outputText = _OutputText;
        }

        public void setOutputBytes(byte[] _OutputBytes)
        {
            this.outputBytes = _OutputBytes;
        }

        public string generateIV()
        {
            int blockLength = this.key.Length;
            string iv = "";
            Random random = new Random();

            for (int i = 0; i != blockLength; i++)
            {
                iv += ((int)(double)random.Next(0, 2)).ToString();
            }

            Console.Write("\n> IV generated: " + iv + "\n");

            saveIV(iv);

            return iv;
        }

        public String encryptionBlock(String _plainText)
        {
            String plainText = _plainText;
            String output = "";
            String key = this.key;

            for (int i = 0; i != key.Length; i++)
            {
                output += xorBit(plainText[i], key[i]);
            }

            return output;
        }

        public String decryptionBlock(String _cipherText)
        {
            String cipherText = _cipherText;
            String output = "";
            String key = this.key;

            for (int i = 0; i != key.Length; i++)
            {
                output += xorBit(cipherText[i], key[i]);
            }

            return output;
        }

        public byte[] binaryStringToBytes(string _binaryString)
        {
            string binary = _binaryString;
            int i = 0;
            int j = 0;
            BitArray bits = new BitArray(8);
            byte[] bytes = new byte[binary.Length / 8];

            while (j != binary.Length)
            {
                for (i = 0; i != 8; i++)
                {

                    bits.Set(i, (binary[j + i] == '1' ? true : false));
                }

                bits.CopyTo(bytes, j / 8);
                j += 8;
            }

            return bytes;
        }

        public char binaryToChar(string _binaryString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            string binary = _binaryString;
            byte[] bytes = binaryStringToBytes(binary);
            string output = utf8.GetString(bytes);

            return output[0];
        }

        public string binaryToString(string _binaryString)
        {
            string binaries = _binaryString;
            string output = "";
            int i = 0;
            while (i != binaries.Length)
            {
                output += binaryToChar(binaries.Substring(i, 8));
                i += 8;
            }

            return output;
        }

        public char xorBit(char _bit1, char _bit2)
        {
            return ((_bit1 == _bit2) ? '0' : '1');
        }

        public void encipher()
        {
            string inputBit = this.inputText;
            int inputBitLength = inputBit.Length;
            int blockLength = this.key.Length;
            string blockCipher = "";
            string blockTemp = "";
            string iv = generateIV();
            string output = "";
            string outputText = "";
            int i = 0;
            int j = 0;

            while (inputBit.Length % blockLength != 0)
            {
                inputBit += '0';
            }

            for (i = 0; i != blockLength; i++)
            {
                blockTemp += xorBit(inputBit[j], iv[i]);
                j++;
            }

            blockCipher = encryptionBlock(blockTemp);
            output += blockCipher;
            blockTemp = "";

            while (j != inputBit.Length)
            {
                for (i = 0; i != blockLength; i++)
                {
                    blockTemp += xorBit(blockCipher[i], inputBit[j]);
                    j += 1;
                }
                blockCipher = encryptionBlock(blockTemp);
                output += blockCipher;
                blockTemp = "";
            }

            outputText = binaryToString(output.Substring(0, inputBitLength));
            this.outputText = outputText;
            this.outputBytes = binaryStringToBytes(output.Substring(0, inputBitLength));

            Console.WriteLine("\n>Encryption Result:");
            Console.WriteLine(outputText);
        }

        public void decipher()
        {
            string inputBit = this.inputText;
            int inputBitLength = inputBit.Length;
            int blockLength = this.key.Length;
            string blockCipher = "";
            string blockPlain = "";
            string blockTemp = "";
            string iv = loadIV();
            string output = "";
            string outputText = "";
            string tempText = "";
            int i = 0;
            int j = 0;

            while (inputBit.Length % blockLength != 0)
            {
                inputBit += '0';
            }

            blockCipher = decryptionBlock(inputBit.Substring(0, blockLength));

            for (i = 0; i != blockLength; i++)
            {
                blockPlain += xorBit(blockCipher[i], iv[i]);
                j += 1;
            }

            output += blockPlain;
            blockTemp = inputBit.Substring(0, blockLength);
            blockPlain = "";
            blockCipher = "";
            tempText = "";

            while (j != inputBit.Length)
            {
                for (i = 0; i != blockLength; i++)
                {
                    blockCipher += inputBit[j];
                    j += 1;
                }
                tempText = blockCipher;
                blockCipher = decryptionBlock(blockCipher);

                for (i = 0; i != blockLength; i++)
                {
                    blockPlain += xorBit(blockCipher[i], blockTemp[i]);
                }
                output += blockPlain;
                blockTemp = tempText;
                blockPlain = "";
                blockCipher = "";
            }

            outputText = binaryToString(output.Substring(0, inputBitLength));
            this.outputText = outputText;
            this.outputBytes = binaryStringToBytes(output.Substring(0, inputBitLength));

            Console.WriteLine("\n>Decryption Result:");
            Console.WriteLine(outputText);
        }

        public void loadFile()
        {
            string fileString = "";
            UTF8Encoding utf8 = new UTF8Encoding();

            try
            {
                //Console.Write("\nInput file name: ");
                //fileString = Console.ReadLine();
                fileString = "output_encipher.txt";
                Console.WriteLine();
                string output = "";
                byte[] bytes = File.ReadAllBytes(fileString);
                BitArray bits = new BitArray(bytes);

                for (int i = 0; i != bits.Length; i += 1)
                {
                    output += (bits[i] ? '1' : '0');
                }

                String inputText = utf8.GetString(bytes);
                Console.WriteLine(inputText);
                //string[] b = bytes.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')).ToArray();
                //string binaryString = "";
                //foreach (String a in b)
                //{
                //    binaryString += a;
                //}
                this.inputText = output;
                Console.WriteLine("BOO " + this.inputText);

            }
            catch (Exception)
            {
                Console.WriteLine("> File not found. Try again.");
                loadFile();
            }
        }

        public string loadIV()
        {
            StreamReader file;
            string fileString = "";
            string iv = "";

            try
            {
                //Console.Write("\nInput IV File: ");
                //fileString = Console.ReadLine();
                fileString = "iv.txt";
                Console.WriteLine();
                file = new StreamReader(fileString);
                iv = file.ReadToEnd();
                Console.WriteLine(iv);
                file.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("> File not found. Try again.");
                loadFile();
            }

            return iv;
        }

        public void saveIV(string _iv)
        {
            try
            {
                StreamWriter file = new StreamWriter("iv.txt");

                file.AutoFlush = true;
                file.Write(_iv);

                Console.WriteLine("\n> IV has been saved into iv.txt");
                file.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("> File not found.");
            }
        }

        public void saveFile(int _index)
        {
            if (_index == 0)
            {
                try
                {
                    File.WriteAllBytes("output_encipher.txt", this.outputBytes);
                    Console.WriteLine("\n> CBC enciphering result has been saved into output_encipher.txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("> File not found.");
                }
            }
            else if (_index == 1)
            {
                try
                {
                    File.WriteAllBytes("output_decipher.txt", this.outputBytes);
                    Console.WriteLine("\n> CBC deciphering result has been saved into output_decipher.txt");
                }
                catch (Exception)
                {
                    Console.WriteLine("> File not found.");
                }
            }
        }

        public byte[] ConvertToBinary(string str)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(str);
        }

        public String GetStringBytes(byte[] bytes) {
            String res = "";

            foreach (int _bit in bytes) {
                res += _bit.ToString();
            }

            return res;
        }
    }
}
