using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrafyRysowanie
{
    public partial class Form1 : Form
    {
        private const int r = 10;
        private Graphics g;
        private Pen pWierzcholek;
        private Pen pWierzcholekAktywny;
        private Pen pKrawedz;
        private Pen pNKrawedz;
        private Wierzchlek MouseDownWierzcholek;


        public List<Wierzchlek> wierzcholki = new List<Wierzchlek>();

        public Form1()
        {
            InitializeComponent();

            pictureBox1.Image = new Bitmap(500, 500);

            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            pWierzcholek = new Pen(Color.Orange);
            pWierzcholek.Width = 3;
            pWierzcholekAktywny = new Pen(Color.Red);
            pWierzcholekAktywny.Width = 3;

            pKrawedz = new Pen(Color.Blue);
            pKrawedz.Width = 3;
            pKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;

            pNKrawedz = new Pen(Color.Green);
            pNKrawedz.Width = 5;
            pNKrawedz.EndCap = System.Drawing.Drawing2D.LineCap.ArrowAnchor;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MouseDownWierzcholek = null;
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek = w;
                    }
                }
                odrysujGraf();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                foreach (Wierzchlek w in wierzcholki)
                {
                    if (w.Odleglosc(e.Location) < r)
                    {
                        MouseDownWierzcholek.Nastpniki.Add(w);
                        //w.Nastpniki.Add(MouseDownWierzcholek);
                    }
                }
                MouseDownWierzcholek = null;
                odrysujGraf();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                wierzcholki.Add(new Wierzchlek(e.Location));
                FromComboBox.Items.Add(wierzcholki.Last().Id);
                odrysujGraf();
            }
        }

        private void odrysujGraf()
        {
            g.Clear(Color.White);
            foreach (Wierzchlek w in wierzcholki)
            {

                g.DrawEllipse(pWierzcholek, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                g.DrawString(w.Id.ToString(),
                             new System.Drawing.Font("Microsoft Sans Serif", r),
                             new SolidBrush(Color.Red),
                             w.Polozenie.X + r,
                             w.Polozenie.Y + r);
                if (w == MouseDownWierzcholek)
                {
                    g.DrawEllipse(pWierzcholekAktywny, w.Polozenie.X - r, w.Polozenie.Y - r, 2 * r, 2 * r);
                }
                foreach (Wierzchlek wn in w.Nastpniki)
                {
                    g.DrawLine(pKrawedz, w.Polozenie, wn.Polozenie);
                }
            }
            pictureBox1.Refresh();
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && MouseDownWierzcholek != null)
            {
                odrysujGraf();
                g.DrawLine(pKrawedz, MouseDownWierzcholek.Polozenie, e.Location);
                pictureBox1.Refresh();
            }

        }

        public void BFS(Wierzchlek start)
        {
            label3.Text = "";
            bool[] odwiedzone = new bool[wierzcholki.Count];
            Queue<Wierzchlek> kolejka = new Queue<Wierzchlek>();

            kolejka.Enqueue(start);
            odwiedzone[start.Id - 1] = true;

            while (kolejka.Count > 0)
            {
                Wierzchlek aktualny = kolejka.Dequeue();
                label3.Text += aktualny.Id.ToString()+ "  ";

                foreach (Wierzchlek nastepnik in aktualny.Nastpniki)
                {
                    if (!odwiedzone[nastepnik.Id - 1])
                    {
                        kolejka.Enqueue(nastepnik);
                        odwiedzone[nastepnik.Id - 1] = true;
                    }
                }
            }
        }
        public void DFS(Wierzchlek start)
        {
            bool[] odwiedzone = new bool[wierzcholki.Count];
            label3.Text = "";
            DiveDeeper(start, odwiedzone);
        }

        private void DiveDeeper(Wierzchlek wierzcholek, bool[] odwiedzone)
        {
            label3.Text += wierzcholek.Id.ToString() + "  ";
            odwiedzone[wierzcholek.Id - 1] = true;

            foreach (Wierzchlek nastepnik in wierzcholek.Nastpniki)
            {
                if (!odwiedzone[nastepnik.Id - 1])
                {
                    DiveDeeper(nastepnik, odwiedzone);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (AlgorithmComboBox.SelectedItem == null || FromComboBox.SelectedItem == null)
            {
                MessageBox.Show("One or both of parameters are empty!");
            }
            else
            {
                if (AlgorithmComboBox.SelectedIndex == 0)
                {
                    BFS(wierzcholki[FromComboBox.SelectedIndex]);
                }
                else if (AlgorithmComboBox.SelectedIndex == 1)
                {
                    DFS(wierzcholki[FromComboBox.SelectedIndex]);
                }
            }
        }

        
    }

}


