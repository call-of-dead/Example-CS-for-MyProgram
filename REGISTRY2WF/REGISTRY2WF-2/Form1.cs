﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace REGISTRY2WF_2
{
    public partial class Form1 : Form
    {
        private string fileName;
        private const string PO = "DEdit ";
        private bool edit = false;
        private RegistryKey test9999;

        public Form1()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
           
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (edit)
            {
                DialogResult res = MessageBox.Show("Вы не сохранили документ. Сохранить?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    return;
                }
                if (res == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, e);
                }
            }
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.fileName = openFileDialog1.FileName;
                textBox1.Enabled = true;
                textBox1.Text = System.IO.File.ReadAllText(fileName);
                this.Text = PO+fileName;
                this.edit = false;
                this.button1.Enabled = true;
                test9999.SetValue("FileName", fileName);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (fileName != null && fileName!="")
            {
                System.IO.File.WriteAllText(fileName,textBox1.Text);
                this.Text = PO+fileName;
                edit = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            this.edit = true;
            this.Text = PO + fileName + "*";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            test9999 = Registry.CurrentUser.CreateSubKey("DEdit");
            comboBox1.SelectedIndex = 0;
            Object o = test9999.GetValue("Width");
            Point x=new Point(400,400);
            if (o != null)
            {
                x.X = (int)o;
            }
            o = test9999.GetValue("Height");
            if (o != null)
            {
                x.Y = (int)o;
            }
            this.Size = new Size(x.X, x.Y);
            o = test9999.GetValue("StartPosX");
            if (o != null)
            {
               x.X = (int)o;
                //this.StartPosition = (FormStartPosition)o;
            }
            o = test9999.GetValue("StartPosY");
            if (o != null)
            {
                x.Y = (int)o;
                //this.StartPosition = (FormStartPosition)o;
            }
            this.Location = x;
            o = test9999.GetValue("FileName");
            if (o != null)
            {
                fileName=(string)o;
                textBox1.Enabled = true;
                textBox1.Text = System.IO.File.ReadAllText(fileName);
                this.Text = PO + fileName;
                this.edit = false;
                this.button1.Enabled = true;
            }
            textBox1.Select();
        }
        string NumToSTR(int t)
        {
            string res;
            if (t >= 0 && t < 10)
            {
                res = (char)('0' + t) + "";
                return res;
            }
            res = (char)('A' + (t-10)) + "";
            return res;
        }
        int CharToInt(char c)
        {
            int n = (int)c - '0';
            if (n < 10)
            {
                return n;
            }
            switch (c)
            {
                case 'A':
                    return 10;
                case 'B':
                    return 11;
                case 'C':
                    return 12;
                case 'D':
                    return 13;
                case 'E':
                    return 14;
                case 'F':
                    return 15;
                default:
                    throw new Exception();
            }
        }
        string convert(string txt, string original, string news)
        {
            string result = "";
            if (original.Length <= 0)
            {
                return txt;
            }
            if (news.Length <= 0)
            {
                return txt;
            }
            if (txt.Length <= 0)
            {
                return txt;
            }
            int Num = Int32.Parse(original);
            int New = Int32.Parse(news);
            int tmp = 1;
            int res = 0;
            for (int i = txt.Length-1; i >=0; i--)
            {
                int n;
                try
                {
                    n = CharToInt(txt[i]);
                }
                catch (Exception e)
                {
                    return txt;
                }
                if (n < 0 || n > Num)
                {
                    return txt;
                }
                n = n * tmp;
                tmp = tmp * Num;
                res += n;
            }
            while (res > 0)
            {
                int g = res % New;
                result = NumToSTR(g) + result;
                res = res / New;
            }
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.SelectedText != "")
            {
                string re = comboBox1.Text;
                string[] reg = re.Split('→');
                textBox1.SelectedText = convert(textBox1.SelectedText, reg[0], reg[1]);
                this.edit = true;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (edit)
            {
                DialogResult res = MessageBox.Show("Вы не сохранили документ. Сохранить?", "Предупреждение", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (res == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click(this, e);
                }
                
            }
            test9999.SetValue("PosRead", textBox1.SelectionStart);
            test9999.SetValue("LengthRead", textBox1.SelectionLength);
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            test9999.SetValue("Width", this.Width);
            test9999.SetValue("Height", this.Height);
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            if (test9999 == null)
                return;
            test9999.SetValue("StartPosX", Location.X);
            test9999.SetValue("StartPosY", Location.Y);
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            Object o = test9999.GetValue("PosRead");
            if (o != null)
            {
                int i = (int)o;
                int v = 0;
                o = test9999.GetValue("LengthRead");
                if (o != null)
                {
                    v = (int)o;
                }
                this.textBox1.SelectionStart = i;
                textBox1.SelectionLength = v;
                //textBox1.Select(0,0);
                // textBox1.Select();
            }
        }
    }
}
