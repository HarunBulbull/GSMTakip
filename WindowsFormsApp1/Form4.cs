using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        public string cihaz, ariza, sifre, ucret, kapora, user;

        private void Form4_Load(object sender, EventArgs e)
        {
            //See if any printers are installed  
            if (PrinterSettings.InstalledPrinters.Count <= 0)
            {
                MessageBox.Show("Yazıcı bulunamadı!");
                return;
            }

            //Get all available printers and add them to the combo box  
            foreach (String printer in PrinterSettings.InstalledPrinters)
            {
                guna2ComboBox1.Items.Add(printer.ToString());
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex > -1)
            {
                //Create a PrintDocument object  
                PrintDocument pd = new PrintDocument();

                //Set PrinterName as the selected printer in the printers list  
                pd.PrinterSettings.PrinterName =
                guna2ComboBox1.SelectedItem.ToString();

                //Add PrintPage event handler  
                pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

                //Print the document  
                PrintPreviewDialog p2d = new PrintPreviewDialog();
                p2d.Document = pd;
                p2d.ShowIcon = false;
                p2d.ShowDialog();
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("Lütfen bir yazıcı seçin!");
            }
        }

        public void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            Graphics g = ev.Graphics;
            g.DrawString("Müşteri: " + user + "\nCihaz: " + cihaz + "\nArıza: " + ariza + "\nŞifre: " + sifre + "\nÜcret: " + ucret + "\nKapora: " + kapora, new Font("Century Gothic", 10, FontStyle.Bold), new SolidBrush(Color.Black), new Rectangle(10, 20, 200, 200));

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}
