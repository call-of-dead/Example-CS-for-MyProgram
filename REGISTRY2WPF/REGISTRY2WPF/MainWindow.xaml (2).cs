using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace REGISTRY2WPF
{
    struct PointD
    {
        public double X;
        public  double Y;
        public PointD(double X, double Y)
        {
            this.X = X;
            this.Y = Y;
        }
    }
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string fileName;
        private const string PO = "DEdit ";
        private bool edit = false;
        private RegistryKey test9999;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (edit)
            {
                MessageBoxResult res= MessageBox.Show("Вы не сохранили документ. Сохранить?", "Предупреждение",MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    return;
                }
                if (res == MessageBoxResult.Yes)
                {
                    MenuItem_Click_1(this, e);
                }
            }
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.fileName = openFileDialog1.FileName;
                textBox1.IsEnabled = true;
                textBox1.Text = System.IO.File.ReadAllText(fileName);
                this.Title = PO + fileName;
                this.edit = false;
                this.button1.IsEnabled = true;
                test9999.SetValue("FileName", fileName);
            }
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            if (fileName != null && fileName != "")
            {
                System.IO.File.WriteAllText(fileName, textBox1.Text);
                this.Title = PO + fileName;
                edit = false;
            }
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.edit = true;
            this.Title = PO + fileName + "*";
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            test9999 = Registry.CurrentUser.CreateSubKey("DEdit2");
            comboBox1.SelectedIndex = 0;
            Object o= test9999.GetValue("FileName");
            if (o != null)
            {
                fileName = (string)o;
                textBox1.IsEnabled = true;
                textBox1.Text = System.IO.File.ReadAllText(fileName);
                this.Title = PO + fileName;
                this.edit = false;
                this.button1.IsEnabled = true;
                //textBox1.Select();
            }
            Point x = new Point();
            o = test9999.GetValue("StartPosX");
            PointD x1 = new PointD();
            if (o != null)
            {
                x1.X = Double.Parse((string)o);
                //this.StartPosition = (FormStartPosition)o;
            }
            o = test9999.GetValue("StartPosY");
            if (o != null)
            {
                x1.Y = Double.Parse((string)o);
                //this.StartPosition = (FormStartPosition)o;
            }
            //SystemParameters.FullPrimaryScreenHeight = x.X;
            this.Top = x1.X;
            this.Left=x1.Y;
             o = test9999.GetValue("Width");
             x1 = new PointD(400, 400);
            if (o != null)
            {
                x1.X = Double.Parse((string)o);
            }
            o = test9999.GetValue("Height");
            if (o != null)
            {
                x1.Y = Double.Parse((string)o);
            }
            this.Width = x1.X;
            this.Height = x1.Y;
            textBox1.Focus();
             o = test9999.GetValue("PosRead");
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
            //this.Location = x;
        }


        string NumToSTR(int t)
        {
            string res;
            if (t >= 0 && t < 10)
            {
                res = (char)('0' + t) + "";
                return res;
            }
            res = (char)('A' + (t - 10)) + "";
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
            for (int i = txt.Length - 1; i >= 0; i--)
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (textBox1.SelectedText != "")
            {
                string re = comboBox1.Text;
                string[] reg = re.Split('→');
                textBox1.SelectedText = convert(textBox1.SelectedText, reg[0], reg[1]);
                this.edit = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (edit)
            {
                MessageBoxResult res = MessageBox.Show("Вы не сохранили документ. Сохранить?", "Предупреждение", MessageBoxButton.YesNoCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                    return;
                }
                if (res == MessageBoxResult.Yes)
                {
                    MenuItem_Click_1(this, null);
                }

            }
            test9999.SetValue("PosRead", textBox1.SelectionStart);
            test9999.SetValue("LengthRead", textBox1.SelectionLength);
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            if (test9999 == null)
                return;

            test9999.SetValue("Width", this.ActualWidth);
            test9999.SetValue("Height", this.ActualHeight);
        }

        private void Window_LocationChanged(object sender, EventArgs e)
        {
            test9999.SetValue("StartPosX", this.Top);
            test9999.SetValue("StartPosY", this.Left);
        }

        private void textBox1_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
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

        private void Window_Activated(object sender, EventArgs e)
        {
        }
    }
}
