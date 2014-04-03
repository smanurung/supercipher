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
            this.filepath = "default";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            //hilangkan IV
            label2.Visible = false;
            ivBox.Visible = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //tampilkan IV
            if(label2.Visible == false)
            {
                label2.Visible = true;
            }
            if(ivBox.Visible == false)
            {
                ivBox.Visible = true;
            }
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
                //this.filepath = openFileDialog1.FileName;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //savefile
            String path = System.IO.Directory.GetCurrentDirectory();
            System.IO.File.WriteAllText(System.IO.Directory.GetCurrentDirectory() + "/" + Path.GetFileName(filepath), textBox3.Text);
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
                if ((!radioButton2.Checked) && (ivBox.Text.Length != keyBox.Text.Length))
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
            byte[] content;
            String mode = "";

            //instead of keyBox.Text use this
            String newKey = "";

            //result for each mode
            byte[] modeResult;

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
            if (header.Length != 5)
            {
                MessageBox.Show("Bukan file yang dapat didekripsi", "Peringatan", MessageBoxButtons.OK);
                return;
            }
            else
            {
                filepath = header[2];
                iv = header[1];
                mode = header[4];
                padding = Int32.Parse(header[5]);
                content = StringToByteArray(header[0]);
                extension = header[3];
                
                //validate key
                if (keyBox.Text.Length != iv.Length)
                {
                    newKey = "";
                    for (int i = 0; i < iv.Length; i++)
                    {
                        newKey += (char)(new Random().Next(255));
                    }
                }
                else 
                    newKey = keyBox.Text;

                if (mode == "ECB")
                {
                    //ECB mode
                    ECB ecb = new ECB(null, content, newKey, iv);
                    modeResult = ecb.decrypt();
                }
                else
                if (mode == "CBC")
                {
                    //Generate IV
                    //CBC mode
                }
                else
                if (mode == "CFB")
                {
                    //CFB mode
                }
                else
                if (mode == "OFB")
                {
                    //OFB mode
                }
                
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
    }
}
