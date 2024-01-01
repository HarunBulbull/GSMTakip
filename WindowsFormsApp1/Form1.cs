using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.IO.Directory.CreateDirectory(@"C:\gsmtakip");
            System.IO.Directory.CreateDirectory(@"C:\gsmtakip\musteriler");
            if (!File.Exists(@"C:\gsmtakip\sipariş.txt"))
            {
                FileStream fs = new FileStream(@"C:\gsmtakip\sipariş.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
            }
            if (!File.Exists(@"C:\gsmtakip\veresiye.txt"))
            {
                FileStream fs = new FileStream(@"C:\gsmtakip\veresiye.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
            }
            musteri();
            guna2ComboBox1.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            guna2ComboBox2.DrawMode = DrawMode.OwnerDrawFixed;
            guna2ComboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        string str,str1;
        private void musteri()
        {
            if (guna2ComboBox1.SelectedIndex > -1)
            {
                str = guna2ComboBox1.SelectedItem.ToString();
            }
            if (guna2ComboBox2.SelectedIndex > -1)
            {
                str1 = guna2ComboBox2.SelectedItem.ToString();
            }
            guna2ComboBox1.Items.Clear();
            guna2ComboBox2.Items.Clear();
            guna2DataGridView1.Rows.Clear();
            guna2DataGridView3.Rows.Clear();
            string[] folders = Directory.GetDirectories(@"C:\gsmtakip\musteriler").Select(Path.GetFileName).ToArray();
            foreach (string folder in folders)
            {
                guna2ComboBox1.Items.Add(folder);
                guna2ComboBox2.Items.Add(folder);
            }
            guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindStringExact(str);
            guna2ComboBox2.SelectedIndex = guna2ComboBox2.FindStringExact(str1);
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(guna2ComboBox1.SelectedIndex > -1)
            {
                guna2DataGridView1.Rows.Clear();
                string[] folders = Directory.GetFiles(@"C:\gsmtakip\musteriler\" + guna2ComboBox1.SelectedItem.ToString()).Select(Path.GetFileName).ToArray();
                foreach (string folder in folders)
                {
                    string[] lineOfContents = File.ReadAllLines(@"C:\gsmtakip\musteriler\" + guna2ComboBox1.SelectedItem.ToString() + "\\" + folder);
                    foreach (var line in lineOfContents)
                    {
                        guna2DataGridView1.Rows.Add(folder.Split('.')[0].Split('•')[0], line.Split('•')[0], line.Split('•')[1], line.Split('•')[2], line.Split('•')[3], line.Split('•')[4], line.Split('•')[5], folder.Split('.')[0].Split('•')[1] + "." + folder.Split('.')[0].Split('•')[2] + "." + folder.Split('.')[0].Split('•')[3].Split(',')[0] + " " + folder.Split('.')[0].Split(',')[1].Split('+')[0] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[1] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[2], line.Split('•')[6]);
                    }
                }
                foreach (DataGridViewRow row in guna2DataGridView1.Rows)
                {
                    if (row.Cells[6].Value.ToString() == "İşlemde")
                    {
                        row.DefaultCellStyle.BackColor = Color.Turquoise;
                    }
                    if (row.Cells[6].Value.ToString() == "Hazır")
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    if (row.Cells[6].Value.ToString() == "İade")
                    {
                        row.DefaultCellStyle.BackColor = Color.Brown;
                    }
                    if (row.Cells[6].Value.ToString() == "Parça Bekliyor")
                    {
                        row.DefaultCellStyle.BackColor = Color.Purple;
                    }
                    if (row.Cells[6].Value.ToString().Contains("Teslim/"))
                    {
                        row.DefaultCellStyle.BackColor = Color.Pink;
                    }
                }
            }
        }

        private void guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            cariekle();
        }

        private void cariekle()
        {
            if (guna2TextBox1.Text != "" && guna2TextBox2.Text.Length > 10)
            {
                if (!Directory.Exists(@"C:\gsmtakip\musteriler\" + guna2TextBox1.Text + " - " + guna2TextBox2.Text))
                {
                    System.IO.Directory.CreateDirectory(@"C:\gsmtakip\musteriler\" + guna2TextBox1.Text + " - " + guna2TextBox2.Text);
                    musteri();
                }
                else
                {
                    MessageBox.Show("Böyle bir müşteri zaten kayıtlı!");
                }
                guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindStringExact(guna2TextBox1.Text + " - " + guna2TextBox2.Text);
                guna2TextBox1.Text = "";
                guna2TextBox2.Text = "";
            }
            else
            {
                MessageBox.Show("Lütfen geçerli bir isim ve 11 haneli numara giriniz!");
            }
        }

        private void guna2TextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("Bu müşteriyi silmek müşteriye ait bütün kayıtları da silecektir. Silmek istediğine emin misin?", "DİKKAT!", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes)
                {
                    Directory.Delete(@"C:\gsmtakip\musteriler\" + guna2ComboBox1.SelectedItem.ToString(), true);
                    musteri();
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce bir müşteri seçin!");
            }
        }

        public string musteriyeni;

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            if(guna2ComboBox1.SelectedIndex > -1)
            {
                Form2 fr= new Form2();
                fr.musteri = guna2ComboBox1.SelectedItem.ToString();
                if(fr.ShowDialog() == DialogResult.Yes)
                {
                    musteri();
                    guna2ComboBox1.SelectedIndex = guna2ComboBox1.FindStringExact(musteriyeni);
                }
            }
            else
            {
                MessageBox.Show("Lütfen önce bir müşteri seçin!");
            }
        }

        private void cihazekle()
        {
            if (guna2ComboBox1.SelectedIndex > -1)
            {
                if (cihaz.Text != "" && ariza.Text != "" && ucret.Text != "")
                {
                    string pwd = şifre.Text;
                    int kpr, gdr;
                    if (şifre.Text == "")
                    {
                        pwd = "Yok";
                    }
                    if (kapora.Text != "")
                    {
                        kpr = int.Parse(kapora.Text);
                    }
                    else
                    {
                        kpr = 0;
                    }
                    if (gider.Text == "")
                    {
                        gdr = 0;
                    }
                    else
                    {
                        gdr = int.Parse(gider.Text);
                    }
                    if (int.Parse(ucret.Text) >= kpr)
                    {
                        string x = @"C:\gsmtakip\musteriler\" + guna2ComboBox1.SelectedItem.ToString() + "\\" + cihaz.Text + "•" + ((DateTime.Now.ToString().Replace(':', '+')).Replace('.', '•')).Replace(' ', ',') + ".txt";
                        if (!File.Exists(x))
                        {
                            FileStream fs = new FileStream(x, FileMode.OpenOrCreate);
                            fs.Flush();
                            fs.Close();
                            using (StreamWriter writer = new StreamWriter(x))
                            {
                                writer.WriteLine(ariza.Text + "•" + pwd + "•" + ucret.Text + "•" + kpr + "•" + gdr + "•İşlemde•00.00.0000");
                            }
                            cihaz.Text = ariza.Text = şifre.Text = ucret.Text = kapora.Text = gider.Text = "";
                            musteri();
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
                    MessageBox.Show("Cihaz, arıza ve ücret bölümleri boş bırakılamaz!");
                }
            }
            else
            {
                MessageBox.Show("Önce bir müşteri seçiniz!");
            }
        }
        private void guna2Button5_Click(object sender, EventArgs e)
        {
            cihazekle();
        }

        private void cihaz_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == (char)Keys.Space || char.IsDigit(e.KeyChar));
        }

        private void guna2DataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form3 fr = new Form3();
            fr.cihaz = guna2DataGridView1.CurrentRow.Cells[0].Value.ToString();
            fr.ariza = guna2DataGridView1.CurrentRow.Cells[1].Value.ToString();
            fr.sifre = guna2DataGridView1.CurrentRow.Cells[2].Value.ToString();
            fr.ucret = guna2DataGridView1.CurrentRow.Cells[3].Value.ToString();
            fr.kapora = guna2DataGridView1.CurrentRow.Cells[4].Value.ToString();
            fr.gider = guna2DataGridView1.CurrentRow.Cells[5].Value.ToString();
            fr.durum = guna2DataGridView1.CurrentRow.Cells[6].Value.ToString();
            fr.tarih = guna2DataGridView1.CurrentRow.Cells[7].Value.ToString();
            fr.tarih1 = guna2DataGridView1.CurrentRow.Cells[8].Value.ToString();
            fr.user = guna2ComboBox1.SelectedItem.ToString();
            fr.path = @"C:\gsmtakip\musteriler\" + guna2ComboBox1.SelectedItem.ToString() + "\\" + guna2DataGridView1.CurrentRow.Cells[0].Value.ToString() + "•" + ((guna2DataGridView1.CurrentRow.Cells[7].Value.ToString().Replace('.', '•')).Replace(' ', ',')).Replace(':', '+') + ".txt";
            if(fr.ShowDialog() == DialogResult.Yes)
            {
                musteri();
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void cihazlist()
        {
            guna2DataGridView2.Rows.Clear();
            string[] directories = Directory.GetDirectories(@"C:\gsmtakip\musteriler\");
            foreach (string dir in directories)
            {
                string lastdir = Path.GetFileName(dir);
                string[] folders = Directory.GetFiles(dir).Select(Path.GetFileName).ToArray();
                foreach (string folder in folders)
                {
                    string[] lineOfContents = File.ReadAllLines(dir + "\\" + folder);
                    foreach (var line in lineOfContents)
                    {
                        guna2DataGridView2.Rows.Add(lastdir, folder.Split('.')[0].Split('•')[0], line.Split('•')[0], line.Split('•')[1], line.Split('•')[2], line.Split('•')[3], line.Split('•')[4], line.Split('•')[5], folder.Split('.')[0].Split('•')[1] + "." + folder.Split('.')[0].Split('•')[2] + "." + folder.Split('.')[0].Split('•')[3].Split(',')[0] + " " + folder.Split('.')[0].Split(',')[1].Split('+')[0] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[1] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[2], line.Split('•')[6]);
                    }
                }
            }
            guna2DataGridView2.Sort(guna2DataGridView2.Columns[9], ListSortDirection.Ascending);
            foreach (DataGridViewColumn column in guna2DataGridView2.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (row.Cells[7].Value.ToString() == "İşlemde")
                {
                    row.DefaultCellStyle.BackColor = Color.Turquoise;
                }
                if (row.Cells[7].Value.ToString() == "Hazır")
                {
                    row.DefaultCellStyle.BackColor = Color.Green;
                }
                if (row.Cells[7].Value.ToString() == "İade")
                {
                    row.DefaultCellStyle.BackColor = Color.Brown;
                }
                if (row.Cells[7].Value.ToString() == "Parça Bekliyor")
                {
                    row.DefaultCellStyle.BackColor = Color.Purple;
                }
                if (row.Cells[7].Value.ToString().Contains("Teslim/"))
                {
                    row.DefaultCellStyle.BackColor = Color.Pink;
                }
            }
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            siparis.Visible = false;
            cihazlar.Visible = true;
            veresiye.Visible = false;
            hesap.Visible = false;
            label4.Text = "TÜM CİHAZLAR";
            
            cihazlist();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            cihazlar.Visible = false;
            siparis.Visible = false;
            veresiye.Visible = false;
            hesap.Visible = false;
            label4.Text = "MÜŞTERİ İŞLEMLERİ";
            musteri();
            guna2ComboBox1.SelectedIndex = -1;
            guna2DataGridView1.Rows.Clear();
            cihaz.Text = ariza.Text = şifre.Text = ucret.Text = kapora.Text = guna2TextBox1.Text = guna2TextBox2.Text = "";
        }

        private void guna2DataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form3 fr = new Form3();
            fr.user = guna2DataGridView2.CurrentRow.Cells[0].Value.ToString();
            fr.cihaz = guna2DataGridView2.CurrentRow.Cells[1].Value.ToString();
            fr.ariza = guna2DataGridView2.CurrentRow.Cells[2].Value.ToString();
            fr.sifre = guna2DataGridView2.CurrentRow.Cells[3].Value.ToString();
            fr.ucret = guna2DataGridView2.CurrentRow.Cells[4].Value.ToString();
            fr.kapora = guna2DataGridView2.CurrentRow.Cells[5].Value.ToString();
            fr.gider = guna2DataGridView2.CurrentRow.Cells[6].Value.ToString();
            fr.durum = guna2DataGridView2.CurrentRow.Cells[7].Value.ToString();
            fr.tarih = guna2DataGridView2.CurrentRow.Cells[8].Value.ToString();
            fr.tarih1 = guna2DataGridView2.CurrentRow.Cells[9].Value.ToString();
            fr.path = @"C:\gsmtakip\musteriler\" + fr.user + "\\" + fr.cihaz + "•" + ((guna2DataGridView2.CurrentRow.Cells[8].Value.ToString().Replace('.', '•')).Replace(' ', ',')).Replace(':', '+') + ".txt";
            if (fr.ShowDialog() == DialogResult.Yes)
            {
                cihazlist();
            }
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            cihazlar.Visible = true;
            siparis.Visible = true;
            veresiye.Visible = false;
            hesap.Visible = false;
            label4.Text = "SİPARİŞ";
            musteri();
            splist();
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            if (sp_siparis.Text != "" && sp_adet.Text != "")
            {
                if (guna2CheckBox1.Checked)
                {
                    File.AppendAllText(@"C:\gsmtakip\sipariş.txt", " • •" + sp_adet.Text + "•" + sp_siparis.Text + "• • •" + DateTime.Now.Date.ToString().Split(' ')[0] + Environment.NewLine);
                    sp_siparis.Text = sp_adet.Text = "";
                }
                else
                {
                    if(guna2ComboBox2.SelectedIndex > -1 && sp_cihaz.Text != "" && sp_kapora.Text != "" && sp_ucret.Text != "")
                    {
                        File.AppendAllText(@"C:\gsmtakip\sipariş.txt", guna2ComboBox2.SelectedItem.ToString() + "•" + sp_cihaz.Text + "•" + sp_adet.Text + "•" + sp_siparis.Text + "•" + sp_ucret.Text + "•" + sp_kapora.Text + "•" + DateTime.Now.Date.ToString().Split(' ')[0] + Environment.NewLine);
                        sp_siparis.Text = sp_kapora.Text = sp_adet.Text = sp_ucret.Text = sp_cihaz.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Lütfen bütün boşlukları doldurun!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bütün boşlukları doldurun!");
            }
            splist();
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                guna2ComboBox2.Enabled = false;
                sp_cihaz.Enabled = false;
                sp_ucret.Enabled = false;
                sp_kapora.Enabled = false;
            }
            else
            {
                guna2ComboBox2.Enabled = true;
                sp_cihaz.Enabled = true;
                sp_ucret.Enabled = true;
                sp_kapora.Enabled = true;
            }
        }
        int i;
        private void guna2DataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(MessageBox.Show("Seçili kayıtı silmek istediğinize emin misiniz?", "DİKKAT!", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                i = 0;
                string mstr = guna2DataGridView3.CurrentRow.Cells[0].Value.ToString();
                string chz = guna2DataGridView3.CurrentRow.Cells[1].Value.ToString();
                string adt = guna2DataGridView3.CurrentRow.Cells[2].Value.ToString();
                string sprs = guna2DataGridView3.CurrentRow.Cells[3].Value.ToString();
                string ucrt = guna2DataGridView3.CurrentRow.Cells[4].Value.ToString();
                string kpr = guna2DataGridView3.CurrentRow.Cells[5].Value.ToString();
                string trh = guna2DataGridView3.CurrentRow.Cells[6].Value.ToString();
                string searchingline = mstr + "•" + chz + "•" + adt + "•" + sprs + "•" + ucrt + "•" + kpr + "•" + trh;
                FileStream fs = new FileStream(@"C:\gsmtakip\sipariş1.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
                string[] lineOfContents = File.ReadAllLines(@"C:\gsmtakip\sipariş.txt");
                foreach (var line in lineOfContents)
                {
                    if(i == 0)
                    {
                        if(line!= searchingline)
                        {
                            File.AppendAllText(@"C:\gsmtakip\sipariş1.txt", line + Environment.NewLine);
                        }
                        else
                        {
                            i = 1;
                        }
                    }
                    else
                    {
                        File.AppendAllText(@"C:\gsmtakip\sipariş1.txt", line + Environment.NewLine);
                    }
                }
                File.Delete(@"C:\gsmtakip\sipariş.txt");
                fs = new FileStream(@"C:\gsmtakip\sipariş.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
                lineOfContents = File.ReadAllLines(@"C:\gsmtakip\sipariş1.txt");
                foreach (var line in lineOfContents)
                {
                    File.AppendAllText(@"C:\gsmtakip\sipariş.txt", line + Environment.NewLine);
                }
                File.Delete(@"C:\gsmtakip\sipariş1.txt");
                splist();
            }
        }

        private void pictureBox2_MouseHover(object sender, EventArgs e)
        {
            timer2.Stop();
            timer1.Start();
        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Start();
        }

        private void pictureBox3_MouseHover(object sender, EventArgs e)
        {
            timer4.Stop();
            timer3.Start();
        }

        private void pictureBox3_MouseLeave(object sender, EventArgs e)
        {
            timer3.Stop();
            timer4.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox2.Height++;
            pictureBox2.Width++;
            if(pictureBox2.Height %2 == 0)
            {
                pictureBox2.Location = new Point(pictureBox2.Location.X-1, pictureBox2.Location.Y-1);
            }
            if(pictureBox2.Height >= 36)
            {
                timer1.Stop();
                pictureBox2.Size = new Size(36, 36);
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            pictureBox2.Height--;
            pictureBox2.Width--;
            if (pictureBox2.Height % 2 == 0)
            {
                pictureBox2.Location = new Point(pictureBox2.Location.X + 1, pictureBox2.Location.Y + 1);
            }
            if (pictureBox2.Height <= 30)
            {
                timer2.Stop();
                pictureBox2.Size = new Size(30, 30);
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox3.Height++;
            pictureBox3.Width++;
            if (pictureBox3.Height % 2 == 0)
            {
                pictureBox3.Location = new Point(pictureBox3.Location.X - 1, pictureBox3.Location.Y - 1);
            }
            if (pictureBox3.Height >= 36)
            {
                timer3.Stop();
                pictureBox3.Size = new Size(36, 36);
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            pictureBox3.Height--;
            pictureBox3.Width--;
            if (pictureBox3.Height % 2 == 0)
            {
                pictureBox3.Location = new Point(pictureBox3.Location.X + 1, pictureBox3.Location.Y + 1);
            }
            if (pictureBox3.Height <= 30)
            {
                timer4.Stop();
                pictureBox3.Size = new Size(30, 30);
            }
        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {
            if(guna2TextBox3.Text != "")
            {
                guna2DataGridView2.Rows.Clear();
                string[] directories = Directory.GetDirectories(@"C:\gsmtakip\musteriler\");
                foreach (string dir in directories)
                {
                    string lastdir = Path.GetFileName(dir);
                    string[] folders = Directory.GetFiles(dir).Select(Path.GetFileName).ToArray();
                    foreach (string folder in folders)
                    {
                        string[] lineOfContents = File.ReadAllLines(dir + "\\" + folder);
                        foreach (var line in lineOfContents)
                        {
                            string search = lastdir + "•" + folder.Split('.')[0] + "•" + line;
                            if (search.Contains(guna2TextBox3.Text))
                            {
                                guna2DataGridView2.Rows.Add(lastdir, folder.Split('.')[0].Split('•')[0], line.Split('•')[0], line.Split('•')[1], line.Split('•')[2], line.Split('•')[3], line.Split('•')[4], line.Split('•')[5], folder.Split('.')[0].Split('•')[1] + "." + folder.Split('.')[0].Split('•')[2] + "." + folder.Split('.')[0].Split('•')[3].Split(',')[0] + " " + folder.Split('.')[0].Split(',')[1].Split('+')[0] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[1] + ":" + folder.Split('.')[0].Split(',')[1].Split('+')[2], line.Split('•')[6]);
                            }
                        }
                    }
                }
                guna2DataGridView2.Sort(guna2DataGridView2.Columns[9], ListSortDirection.Descending);
                foreach (DataGridViewColumn column in guna2DataGridView2.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                {
                    if (row.Cells[7].Value.ToString() == "İşlemde")
                    {
                        row.DefaultCellStyle.BackColor = Color.Turquoise;
                    }
                    if (row.Cells[7].Value.ToString() == "Hazır")
                    {
                        row.DefaultCellStyle.BackColor = Color.Green;
                    }
                    if (row.Cells[7].Value.ToString() == "İade")
                    {
                        row.DefaultCellStyle.BackColor = Color.Brown;
                    }
                    if (row.Cells[7].Value.ToString() == "Parça Bekliyor")
                    {
                        row.DefaultCellStyle.BackColor = Color.Purple;
                    }
                    if (row.Cells[7].Value.ToString().Contains("Teslim/"))
                    {
                        row.DefaultCellStyle.BackColor = Color.Pink;
                    }
                }
            }
            else
            {
                cihazlist();
            }
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            cihazlar.Visible = true;
            siparis.Visible = true;
            veresiye.Visible = true;
            hesap.Visible = false;
            label4.Text = "VERESİYE DEFTERİ";
            vdlist();
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button10_Click(object sender, EventArgs e)
        {
            if(vd_musteri.Text != "" && vd_no.Text.Length == 11 && vd_işlem.Text != "" && vd_ücret.Text != "")
            {
                File.AppendAllText(@"C:\gsmtakip\veresiye.txt", vd_musteri.Text + "•" + vd_no.Text + "•" + vd_işlem.Text + "•" + vd_ücret.Text + "•" + DateTime.Now.Date.ToString().Split(' ')[0] + Environment.NewLine);
                vd_musteri.Text = vd_işlem.Text = vd_no.Text = vd_ücret.Text = "";
                vdlist();
            }
            else
            {
                MessageBox.Show("Lütfen bütün alanları doldurun!");
            }
        }

        private void vdlist()
        {
            guna2DataGridView4.Rows.Clear();
            string[] lineOfContents = File.ReadAllLines(@"C:\gsmtakip\veresiye.txt");
            foreach (var line in lineOfContents)
            {
                guna2DataGridView4.Rows.Add(line.Split('•')[0], line.Split('•')[1], line.Split('•')[2], line.Split('•')[3], line.Split('•')[4]);
            }
            guna2DataGridView4.Sort(guna2DataGridView4.Columns[4], ListSortDirection.Descending);
            foreach (DataGridViewColumn column in guna2DataGridView4.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void guna2DataGridView4_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MessageBox.Show("Seçili kayıtı silmek istediğinize emin misiniz?", "DİKKAT!", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                i = 0;
                string mstr = guna2DataGridView4.CurrentRow.Cells[0].Value.ToString();
                string no = guna2DataGridView4.CurrentRow.Cells[1].Value.ToString();
                string islm = guna2DataGridView4.CurrentRow.Cells[2].Value.ToString();
                string ucrt = guna2DataGridView4.CurrentRow.Cells[3].Value.ToString();
                string trh = guna2DataGridView4.CurrentRow.Cells[4].Value.ToString();
                string searchingline = mstr + "•" + no + "•" + islm + "•" + ucrt + "•" + trh;
                FileStream fs = new FileStream(@"C:\gsmtakip\veresiye1.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
                string[] lineOfContents = File.ReadAllLines(@"C:\gsmtakip\veresiye.txt");
                foreach (var line in lineOfContents)
                {
                    if (i == 0)
                    {
                        if (line != searchingline)
                        {
                            File.AppendAllText(@"C:\gsmtakip\veresiye1.txt", line + Environment.NewLine);
                        }
                        else
                        {
                            i = 1;
                        }
                    }
                    else
                    {
                        File.AppendAllText(@"C:\gsmtakip\veresiye1.txt", line + Environment.NewLine);
                    }
                }
                File.Delete(@"C:\gsmtakip\veresiye.txt");
                fs = new FileStream(@"C:\gsmtakip\veresiye.txt", FileMode.OpenOrCreate);
                fs.Flush();
                fs.Close();
                lineOfContents = File.ReadAllLines(@"C:\gsmtakip\veresiye1.txt");
                foreach (var line in lineOfContents)
                {
                    File.AppendAllText(@"C:\gsmtakip\veresiye.txt", line + Environment.NewLine);
                }
                File.Delete(@"C:\gsmtakip\veresiye1.txt");
                vdlist();
            }
        }
        string lstek;
        int bk,dk,bc,dc;

        private void cihaz_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cihazekle();
            }
        }

        private void guna2TextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cariekle();
            }
        }

        private void guna2Button11_Click(object sender, EventArgs e)
        {
            bk = dk = bc = dc = 0;
            guna2DataGridView5.Rows.Clear();
            string[] directories = Directory.GetDirectories(@"C:\gsmtakip\musteriler\");
            foreach (string dir in directories)
            {
                string lastdir = Path.GetFileName(dir);
                string[] folders = Directory.GetFiles(dir).Select(Path.GetFileName).ToArray();
                foreach (string folder in folders)
                {
                    string[] lineOfContents = File.ReadAllLines(dir + "\\" + folder);
                    foreach (var line in lineOfContents)
                    {
                        if (line.Split('•')[5] == "Teslim/Hazır")
                        {
                            if (lastdir.Contains("Bayi"))
                            {
                                lstek = "//B";
                            }
                            else
                            {
                                lstek = "//D";
                            }
                            DateTime myDate = DateTime.ParseExact(line.Split('•')[6], "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                            if(myDate>=flt_t1.Value && myDate<=guna2DateTimePicker1.Value)
                            {
                                guna2DataGridView5.Rows.Add(line.Split('•')[6], folder.Split('.')[0].Split('•')[0] + lstek, line.Split('•')[0], line.Split('•')[2], line.Split('•')[3], int.Parse(line.Split('•')[2]) - int.Parse(line.Split('•')[3]));
                            }
                        }
                    }
                }
            }
            guna2DataGridView5.Sort(guna2DataGridView5.Columns[0], ListSortDirection.Ascending);
            foreach (DataGridViewColumn column in guna2DataGridView5.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            foreach (DataGridViewRow row in guna2DataGridView5.Rows)
            {
                if (row.Cells[1].Value.ToString().Contains("//B")) { row.DefaultCellStyle.BackColor = Color.Turquoise; bc++; bk = bk + int.Parse(row.Cells[5].Value.ToString()); }
                else { row.DefaultCellStyle.BackColor = Color.Green; dc++; dk = dk + int.Parse(row.Cells[5].Value.ToString()); }
            }
            MessageBox.Show("Bayi cihazları: " + bc + "\nBayi kârı: " + bk + "\nDükkan cihazları: " + dc + "\nDükkan kârı: " + dk + "\nToplam kâr: " + (dk+bk));
        }

        private void guna2Button12_Click(object sender, EventArgs e)
        {
            cihazlar.Visible = true;
            siparis.Visible = true;
            veresiye.Visible = true;
            hesap.Visible = true;
            label4.Text = "HESAP";
        }

        private void splist()
        {
            guna2DataGridView3.Rows.Clear();
            string[] lineOfContents = File.ReadAllLines(@"C:\gsmtakip\sipariş.txt");
            foreach (var line in lineOfContents)
            {
                guna2DataGridView3.Rows.Add(line.Split('•')[0], line.Split('•')[1], line.Split('•')[2], line.Split('•')[3], line.Split('•')[4], line.Split('•')[5], line.Split('•')[6]);
            }
            guna2DataGridView3.Sort(guna2DataGridView3.Columns[6], ListSortDirection.Descending);
            foreach (DataGridViewColumn column in guna2DataGridView3.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

    }
}
