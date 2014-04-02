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
    }
}
