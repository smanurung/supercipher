using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace SuperCipher
{
    public partial class Form1 : Form
    {
        private String filepath;
        private String extension;

        public Form1()
        {
            InitializeComponent();
            textBox3.ReadOnly = true;
            this.filepath = "harus.diganti";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //hilangkan IV
            label2.Visible = false;
            ivBox.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //hilangkan IV
            label2.Visible = false;
            ivBox.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            //tampilkan IV
            if (label2.Visible == false)
            {
                label2.Visible = true;
            }
            if (ivBox.Visible == false)
            {
                ivBox.Visible = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            //tampilkan IV
            if (label2.Visible == false)
            {
                label2.Visible = true;
            }
            if (ivBox.Visible == false)
            {
                ivBox.Visible = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("File: "+openFileDialog1.FileName, "Confirm", MessageBoxButtons.YesNo);
                this.filepath = openFileDialog1.FileName;
                Console.WriteLine("filepath:{0}",this.filepath);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //savefile
            String path = System.IO.Directory.GetCurrentDirectory();
            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/" + "default.txt", textBox3.Text);
        }

        //enkripsi
        private void button2_Click(object sender, EventArgs e)
        {
            //menyimpan hasil paling akhir
            String result;

            //menyimpan hasil tiap mode
            byte[] modeResult = null;
            String mode = "";

            //check input file
            if (openFileDialog1.FileName.Equals("openFileDialog1"))
            {
                MessageBox.Show("Anda belum memasukan input file", "Peringatan", MessageBoxButtons.OK);
                return;
            }

            //validate key and IV
            //key.length >= 8-byte
            if (keyBox.Text.Length < 8)
            {
                MessageBox.Show("Panjang kunci kurang dari 64-bit", "Peringatan", MessageBoxButtons.OK);
                return;
            }
            else
            {
                //IV.length = key.length
                if ((!radioButton2.Checked && !radioButton1.Checked) && (ivBox.Text.Length != keyBox.Text.Length))
                {
                    MessageBox.Show("Panjang IV tidak sama dengan panjang kunci", "Peringatan", MessageBoxButtons.OK);
                    return;
                }
            }

            //read file content (plaintext)
            String dialogfilename = openFileDialog1.FileName;
            byte[] plain = System.IO.File.ReadAllBytes(dialogfilename);

            //check used mode
            if (radioButton1.Checked)
            {
                //ECB mode
                mode = "ECB";
                ECB ecb = new ECB (plain, null, keyBox.Text, ivBox.Text);
                modeResult = ecb.encrypt();
            }
            else if (radioButton2.Checked)
            {
                //generate IV

                //CBC mode
                mode = "CBC";
            }
            else if (radioButton3.Checked)
            {
                //CFB mode
                mode = "CFB";
                CFB cfb = new CFB(plain, null ,keyBox.Text,ivBox.Text);
                modeResult = cfb.encrypt();
            }
            else if (radioButton4.Checked)
            {
                //OFB mode
                mode = "OFB";
                OFB ofb = new OFB(plain, null, keyBox.Text, ivBox.Text);
                modeResult = ofb.encrypt();
            }
            //convert byte to hex
            result = ByteArrayToString(modeResult);

            //set header
            result += "." + ivBox.Text + "." + Path.GetFileNameWithoutExtension(filepath) + Path.GetExtension(filepath) + "." + mode + "." + (keyBox.Text.Length- (plain.Length % keyBox.Text.Length)).ToString();
            textBox3.Text = result;
        }

        //dekripsi
        private void button3_Click(object sender, EventArgs e)
        {
            //header variable
            String iv;
            int padding;
            byte[] content = new byte[1];
            String mode = "";
            
            //instead of keyBox.Text use this
            String newKey = keyBox.Text;

            //result for each mode
            byte[] modeResult = new byte[keyBox.Text.Length];

            //validate input file
            if (openFileDialog1.FileName.Equals("openFileDialog1"))
            {
                MessageBox.Show("Anda belum memasukan input file", "Peringatan", MessageBoxButtons.OK);
                return;
            }

            //validasi kunci
            //key.length >= 8-byte
            if (keyBox.Text.Length < 8)
            {
                MessageBox.Show("Panjang kunci kurang dari 64-bit", "Peringatan", MessageBoxButtons.OK);
                return;
            }

            //read file content (cipher)
            String dialogfilename = openFileDialog1.FileName;
            String cipher = System.IO.File.ReadAllText(dialogfilename);

            //no need for iv

            //get header
            String[] header = cipher.Split('.');
            if (header.Length != 6)
            {
                MessageBox.Show("Bukan file yang dapat didekripsi", "Peringatan", MessageBoxButtons.OK);
                return;
            }
            else
            {
                filepath = header[2];
                iv = header[1];
                //Console.WriteLine("iv: {0}",iv[0]);
                mode = header[4];
                padding = Int32.Parse(header[5]);
                content = StringToByteArray(header[0]);
                Console.WriteLine(content[0]);
                extension = header[3];
                Console.WriteLine("ext:{0}",extension);
                
                //validate key 
                if (content.Length % keyBox.Text.Length != 0)
                {
                    newKey = "";
                    Random rnd = new Random(content.Length);
                    for (int i = 0; i < (content.Length); i++)
                    {
                        newKey += rnd.Next() % 255;
                    }
                }

                if (mode.Equals("ECB"))
                {
                    //ECB mode
                    ECB ecb = new ECB(null, content, newKey, iv);
                    modeResult = ecb.decrypt();
                    Console.WriteLine();
                }
                else
                if (mode.Equals("CBC"))
                {
                    //Generate IV
                    //CBC mode
                }
                else
                if (mode.Equals("CFB"))
                {
                    //CFB mode
                    CFB cfb = new CFB(null, content, keyBox.Text, iv);
                    Console.WriteLine(iv);
                    byte[] pbytes = cfb.decrypt();
                    Console.WriteLine("hasil dekripsi: {0}", ByteArrayToString(pbytes));
                    textBox3.Text = ByteArrayToString(pbytes);

                    //show plaintext if using text extension
                    if (extension.Equals("txt"))
                    {
                        textBox3.Text += Environment.NewLine + Environment.NewLine + Encoding.ASCII.GetString(pbytes);
                    }
                }
                else
                if (mode.Equals("OFB"))
                {
                    //OFB mode
                    OFB ofb = new OFB(null, content, keyBox.Text, iv);
                    Console.WriteLine(iv);
                    byte[] pbytes = ofb.decrypt();
                    Console.WriteLine("hasil dekripsi: {0}", ByteArrayToString(pbytes));
                    textBox3.Text = ByteArrayToString(pbytes);

                    //show plaintext if using text extension
                    if (extension.Equals("txt"))
                    {
                        textBox3.Text += Environment.NewLine + Environment.NewLine + Encoding.ASCII.GetString(pbytes);
                    }
                }

                textBox3.Text = Encoding.ASCII.GetString(modeResult);
            }
        }

        private String ByteArrayToString(byte[] b)
        {
            StringBuilder hex = new StringBuilder(b.Length * 2);
            foreach (byte a in b)
            {
                hex.AppendFormat("{0:x2}", a);
            }
            return hex.ToString();
        }

        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
