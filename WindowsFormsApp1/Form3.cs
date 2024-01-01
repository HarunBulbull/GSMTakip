using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        public string cihaz, ariza, sifre, ucret, kapora, gider, durum, path, user, tarih, tarih1;

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Kayıtı silmek istediğinize emin misiniz?", "DİKKAT!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
            {
                File.Delete(@"C:\gsmtakip\musteriler\" + user + "\\" + cihaz + "•" + ((tarih.Replace('.', '•')).Replace(' ', ',')).Replace(':', '+') + ".txt");
                this.DialogResult = DialogResult.Yes;
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            Form4 fr = new Form4();
            fr.cihaz = chz.Text;
            fr.ariza = arz.Text;
            fr.sifre = sfr.Text;
            fr.ucret = ucrt.Text;
            fr.kapora = kpr.Text;
            fr.user = user;
            fr.ShowDialog();
        }

        string drm;
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (islemde.Checked)
            {
                drm = "İşlemde";
            }
            if (hazir.Checked)
            {
                drm = "Hazır";
            }
            if (iade.Checked)
            {
                drm = "İade";
            }
            if (parca.Checked)
            {
                drm = "Parça Bekliyor";
            }
            if (teslim.Checked)
            {
                drm = "Teslim Edildi";
            }
            if (chz.Text != "" && arz.Text != "" && ucrt.Text != "" && kpr.Text != "" && sfr.Text != "")
            {
                if (int.Parse(ucrt.Text) >= int.Parse(kpr.Text))
                {
                    string x = @"C:\gsmtakip\musteriler\" + user + "\\" + chz.Text + "•" + ((tarih.Replace(':', '+')).Replace('.', '•')).Replace(' ', ',') + ".txt";
                    if (!File.Exists(x))
                    {
                        FileStream fs = new FileStream(x, FileMode.OpenOrCreate);
                        fs.Flush();
                        fs.Close();
                        using (StreamWriter writer = new StreamWriter(x))
                        {
                            if(drm != "Teslim Edildi")
                            {
                                writer.WriteLine(arz.Text + "•" + sfr.Text + "•" + ucrt.Text + "•" + kpr.Text + "•" + gdr.Text + "•" + drm + "•00.00.0000");
                            }
                            else
                            {
                                Form5 fr = new Form5();
                                if(fr.ShowDialog() == DialogResult.OK)
                                {
                                    writer.WriteLine(arz.Text + "•" + sfr.Text + "•" + ucrt.Text + "•" + kpr.Text + "•" + gdr.Text + "•Teslim/Hazır•" + DateTime.Now.Date.ToString().Split(' ')[0]);
                                }
                                else
                                {
                                    writer.WriteLine(arz.Text + "•" + sfr.Text + "•" + ucrt.Text + "•" + kpr.Text + "•" + gdr.Text + "•Teslim/İade•" +  DateTime.Now.Date.ToString().Split(' ')[0]);
                                }
                            }
                            this.DialogResult = DialogResult.Yes;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bu müşteriye ait böyle bir cihaz kaydedilmiş!");
                    }
                }
                else
                {
                    MessageBox.Show("Kapora ücretten büyük olamaz!");
                }
            }
            else
            {
                MessageBox.Show("Lütfen bütün alanları doldurun!");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (!File.Exists(path))
            {
                FileStream fs = new FileStream(@"C:\gsmtakip\musteriler\" + user + "\\" + cihaz + ".txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
                using (StreamWriter writer = new StreamWriter(@"C:\gsmtakip\musteriler\" + user + "\\" + cihaz + ".txt"))
                {
                    if (drm != "Teslim Edildi")
                    {
                        writer.WriteLine(ariza + "•" + sifre + "•" + ucret + "•" + kapora + "•" + durum + "•" + tarih + "•" + tarih1);
                    }
                }
            }
            this.DialogResult = DialogResult.No;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            chz.Text = cihaz;
            arz.Text = ariza;
            sfr.Text = sifre;
            ucrt.Text = ucret;
            kpr.Text = kapora;
            gdr.Text = gider;
            if(durum == "İşlemde")
            {
                islemde.Checked = true;
            }
            else if(durum == "Parça Bekliyor")
            {
                parca.Checked = true;
            }
            else if(durum == "Hazır")
            {
                hazir.Checked = true;
            }
            else if(durum == "İade")
            {
                iade.Checked = true;
            }
            else
            {
                teslim.Checked = true;
            }
        }
    }
}
