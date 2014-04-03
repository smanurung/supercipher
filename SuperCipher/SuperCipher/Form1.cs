using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperCipher
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox3.ReadOnly = true;
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
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
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

            //check used mode
            if (radioButton1.Checked)
            {
                //ECB mode
            }
            else if (radioButton2.Checked)
            {
                //CBC mode
            }
            else if (radioButton3.Checked)
            {
                //CFB mode
            }
            else if (radioButton4.Checked)
            {
                //OFB mode
            }
        }
    }
}
