using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public string musteri;

        private void Form2_Load(object sender, EventArgs e)
        {
            string ad = musteri.Split('-')[0].Remove(musteri.Split('-')[0].Length-1, 1);
            guna2TextBox1.Text = ad;
            string tel = musteri.Split('-')[1].Remove(0, 1);
            guna2TextBox2.Text = tel;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult= DialogResult.No;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if(musteri != guna2TextBox1.Text + " - " + guna2TextBox2.Text)
            {
                if (guna2TextBox1.Text != "" && guna2TextBox2.Text.Length > 10)
                {
                    if (!Directory.Exists(@"C:\gsmtakip\musteriler\" + guna2TextBox1.Text + " - " + guna2TextBox2.Text))
                    {
                        System.IO.Directory.Move(@"C:\gsmtakip\musteriler\" + musteri, @"C:\gsmtakip\musteriler\" + guna2TextBox1.Text + " - " + guna2TextBox2.Text);
                        Form1 f = new Form1();
                        foreach (Form _f in Application.OpenForms)
                        {
                            if (_f.Name == "Form1")
                                f = (Form1)_f;
                        }
                        f.musteriyeni = guna2TextBox1.Text + " - " + guna2TextBox2.Text;
                        this.DialogResult = DialogResult.Yes;
                    }
                    else
                    {
                        MessageBox.Show("Böyle bir müşteri zaten kayıtlı!");
                    }
                }
                else
                {
                    MessageBox.Show("Lütfen geçerli bir isim ve 11 haneli numara giriniz!");
                }
            }
            else
            {
                MessageBox.Show("Hiçbir değişiklik yapılmadı!");
                this.DialogResult = DialogResult.No;
            }
        }
    }
}
