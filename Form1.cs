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

namespace Assignment5
{
    public partial class Form1 : Form
    {
        RichTextBox[,] EasyBoxes = new RichTextBox[3, 3];
        RichTextBox[] EasyAnswers = new RichTextBox[8];
        RichTextBox[] EasyTotals = new RichTextBox[8];
        RichTextBox[,] MediumBoxes = new RichTextBox[5, 5];
        RichTextBox[] MediumAnswers = new RichTextBox[12];
        RichTextBox[] MediumTotals = new RichTextBox[12];
        RichTextBox[,] HardBoxes = new RichTextBox[7, 7];
        RichTextBox[] HardAnswers = new RichTextBox[16];
        RichTextBox[] HardTotals = new RichTextBox[16];
        String e1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e1.txt";
        String e2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e2.txt";
        String e3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e3.txt";
        String m1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m1.txt";
        String m2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m2.txt";
        String m3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m3.txt";
        String h1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h1.txt";
        String h2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h2.txt";
        String h3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h3.txt";
        char[,] easyinitial1 = new char[3, 3];
        char[,] easyinitial2 = new char[3, 3];
        char[,] easyinitial3 = new char[3, 3];
        bool[] easycomplete = new bool[3];
        char[,] mediuminitial1 = new char[5, 5];
        char[,] mediuminitial2 = new char[5, 5];
        char[,] mediuminitial3 = new char[5, 5];
        bool[] mediumcomplete = new bool[3];
        char[,] hardinitial1 = new char[7, 7];
        char[,] hardinitial2 = new char[7, 7];
        char[,] hardinitial3 = new char[7, 7];
        bool[] hardcomplete = new bool[3];

        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i < 3; i++)
            {
                easycomplete[i] = true;
                mediumcomplete[i] = true;
                hardcomplete[i] = true;
            }
            readin();
        }
        public void clearTotals()
        {
            foreach (RichTextBox r in EasyAnswers)
            {
                if(r != null)
                    r.Dispose();
            }
            foreach (RichTextBox r in EasyTotals)
            {
                if (r != null)
                    r.Dispose();
            }
            foreach (RichTextBox r in MediumAnswers)
            {
                if (r != null)
                    r.Dispose();
            }
            foreach (RichTextBox r in MediumTotals)
            {
                if (r != null)
                    r.Dispose();
            }
            foreach (RichTextBox r in HardAnswers)
            {
                if (r != null)
                    r.Dispose();
            }
            foreach (RichTextBox r in HardTotals)
            {
                if (r != null)
                    r.Dispose();
            }
        }
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            for (int i = 0; i < 3; i++)
            { 
                for(int x = 0; x < 3; x++)
                {
                    EasyBoxes[i,x] = new System.Windows.Forms.RichTextBox();
                    this.EasyBoxes[i,x].Font = new System.Drawing.Font("Microsoft Sans Serif", 88F);
                    this.EasyBoxes[i, x].Location = new System.Drawing.Point(i*(panel1.Width/3), x*(panel1.Height/3));
                    this.EasyBoxes[i, x].Name = "EasyBox" + i + x;
                    this.EasyBoxes[i, x].Size = new System.Drawing.Size(panel1.Width/3, panel1.Height/3);
                    this.EasyBoxes[i, x].ReadOnly = true;
                    this.EasyBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
                    if(easycomplete[0])
                    {
                        if(!easyinitial1[x, i].Equals('0'))
                            this.EasyBoxes[i, x].Text = easyinitial1[x, i].ToString();
                    }
                    else if (easycomplete[1])
                    {
                        if (!easyinitial2[x, i].Equals('0'))
                            this.EasyBoxes[i, x].Text = easyinitial2[x, i].ToString();
                    }
                    else if (easycomplete[2])
                    {
                        if (!easyinitial3[x, i].Equals('0'))
                            this.EasyBoxes[i, x].Text = easyinitial3[x, i].ToString();
                    }
                    panel1.Controls.Add(EasyBoxes[i, x]);
                }
            }
            panel1.PerformLayout();
            clearTotals();
            for (int i = 0; i < 3; i++)
            {
                EasyAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.EasyAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyAnswers[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 3)), panel1.Location.Y - (panel1.Height / 3) - 15);
                this.EasyAnswers[i].Name = "EasyAnswer" + i;
                this.EasyAnswers[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyAnswers[i].Text = i.ToString();
                this.EasyAnswers[i].ReadOnly = true;
                this.EasyAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.EasyAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(EasyAnswers[i]);
                EasyTotals[i] = new System.Windows.Forms.RichTextBox();
                this.EasyTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyTotals[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 3)), panel1.Location.Y + panel1.Height + 15);
                this.EasyTotals[i].Name = "EasyTotal" + i;
                this.EasyTotals[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyTotals[i].Text = i.ToString();
                this.EasyTotals[i].ReadOnly = true;
                this.EasyTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(EasyTotals[i]);
            }
            EasyAnswers[3] = new System.Windows.Forms.RichTextBox();
            this.EasyAnswers[3].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyAnswers[3].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 3) - 15, panel1.Location.Y  - (panel1.Height / 3) - 15);
            this.EasyAnswers[3].Name = "EasyAnswer" + 3;
            this.EasyAnswers[3].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
            this.EasyAnswers[3].Text = 3.ToString();
            this.EasyAnswers[3].ReadOnly = true;
            this.EasyAnswers[3].SelectionAlignment = HorizontalAlignment.Center;
            this.EasyAnswers[3].BackColor = Color.Gray;
            this.Controls.Add(EasyAnswers[3]);
            EasyTotals[3] = new System.Windows.Forms.RichTextBox();
            this.EasyTotals[3].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyTotals[3].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y - (panel1.Height / 3) - 15);
            this.EasyTotals[3].Name = "EasyTotal" + 3;
            this.EasyTotals[3].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
            this.EasyTotals[3].Text = 3.ToString();
            this.EasyTotals[3].ReadOnly = true;
            this.EasyTotals[3].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(EasyTotals[3]);
            for (int i = 4; i < 7; i++)
            {
                EasyAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.EasyAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyAnswers[i].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 3) - 15, panel1.Location.Y + ((i-4) * (panel1.Height / 3)));
                this.EasyAnswers[i].Name = "EasyAnswer" + i;
                this.EasyAnswers[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyAnswers[i].Text = i.ToString();
                this.EasyAnswers[i].ReadOnly = true;
                this.EasyAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.EasyAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(EasyAnswers[i]);
                EasyTotals[i] = new System.Windows.Forms.RichTextBox();
                this.EasyTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyTotals[i].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + ((i-4) * (panel1.Height / 3)));
                this.EasyTotals[i].Name = "EasyTotal" + i;
                this.EasyTotals[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyTotals[i].Text = i.ToString();
                this.EasyTotals[i].ReadOnly = true;
                this.EasyTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(EasyTotals[i]);
            }
            EasyAnswers[7] = new System.Windows.Forms.RichTextBox();
            this.EasyAnswers[7].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyAnswers[7].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 3) - 15, panel1.Location.Y + panel1.Height + 15);
            this.EasyAnswers[7].Name = "EasyAnswer" + 7;
            this.EasyAnswers[7].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
            this.EasyAnswers[7].Text = 7.ToString();
            this.EasyAnswers[7].ReadOnly = true;
            this.EasyAnswers[7].SelectionAlignment = HorizontalAlignment.Center;
            this.EasyAnswers[7].BackColor = Color.Gray;
            this.Controls.Add(EasyAnswers[7]);
            EasyTotals[7] = new System.Windows.Forms.RichTextBox();
            this.EasyTotals[7].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyTotals[7].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
            this.EasyTotals[7].Name = "EasyTotal" + 7;
            this.EasyTotals[7].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
            this.EasyTotals[7].Text = 7.ToString();
            this.EasyTotals[7].ReadOnly = true;
            this.EasyTotals[7].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(EasyTotals[7]);
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            for (int i = 0; i < 5; i++)
            {
                for (int x = 0; x < 5; x++)
                {
                    MediumBoxes[i, x] = new System.Windows.Forms.RichTextBox();
                    this.MediumBoxes[i, x].Font = new System.Drawing.Font("Microsoft Sans Serif", 50F);
                    this.MediumBoxes[i, x].Location = new System.Drawing.Point(i * (panel1.Width / 5), x * (panel1.Height / 5));
                    this.MediumBoxes[i, x].Name = "EasyBox" + i + x;
                    this.MediumBoxes[i, x].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
                    this.MediumBoxes[i, x].ReadOnly = true;
                    this.MediumBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
                    if(mediumcomplete[0])
                    {
                        if (!mediuminitial1[x, i].Equals('0'))
                            this.MediumBoxes[i, x].Text = mediuminitial1[x,i].ToString();
                    }
                    else if (mediumcomplete[1])
                    {
                        if (!mediuminitial2[x, i].Equals('0'))
                            this.MediumBoxes[i, x].Text = mediuminitial2[x, i].ToString();
                    }
                    else if (mediumcomplete[2])
                    {
                        if (!mediuminitial3[x, i].Equals('0'))
                            this.MediumBoxes[i, x].Text = mediuminitial3[x, i].ToString();
                    }
                    panel1.Controls.Add(MediumBoxes[i, x]);
                }
            }
            panel1.PerformLayout();
            clearTotals();
            for (int i = 0; i < 5; i++)
            {
                MediumAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.MediumAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
                this.MediumAnswers[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 5)), panel1.Location.Y - (panel1.Height / 5) - 15);
                this.MediumAnswers[i].Name = "MediumAnswer" + i;
                this.MediumAnswers[i].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
                this.MediumAnswers[i].Text = i.ToString();
                this.MediumAnswers[i].ReadOnly = true;
                this.MediumAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.MediumAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(MediumAnswers[i]);
                MediumTotals[i] = new System.Windows.Forms.RichTextBox();
                this.MediumTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
                this.MediumTotals[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 5)), panel1.Location.Y + panel1.Height + 15);
                this.MediumTotals[i].Name = "MediumTotal" + i;
                this.MediumTotals[i].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
                this.MediumTotals[i].Text = i.ToString();
                this.MediumTotals[i].ReadOnly = true;
                this.MediumTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(MediumTotals[i]);
            }
            MediumAnswers[5] = new System.Windows.Forms.RichTextBox();
            this.MediumAnswers[5].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.MediumAnswers[5].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 5) - 15, panel1.Location.Y - (panel1.Height / 5) - 15);
            this.MediumAnswers[5].Name = "MediumAnswer" + 5;
            this.MediumAnswers[5].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
            this.MediumAnswers[5].Text = 5.ToString();
            this.MediumAnswers[5].ReadOnly = true;
            this.MediumAnswers[5].SelectionAlignment = HorizontalAlignment.Center;
            this.MediumAnswers[5].BackColor = Color.Gray;
            this.Controls.Add(MediumAnswers[5]);
            MediumTotals[5] = new System.Windows.Forms.RichTextBox();
            this.MediumTotals[5].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.MediumTotals[5].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y - (panel1.Height / 5) - 15);
            this.MediumTotals[5].Name = "MediumTotal" + 5;
            this.MediumTotals[5].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
            this.MediumTotals[5].Text = 5.ToString();
            this.MediumTotals[5].ReadOnly = true;
            this.MediumTotals[5].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(MediumTotals[5]);
            for (int i = 6; i < 11; i++)
            {
                MediumAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.MediumAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
                this.MediumAnswers[i].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 5) - 15, panel1.Location.Y + ((i - 6) * (panel1.Height / 5)));
                this.MediumAnswers[i].Name = "MediumAnswer" + i;
                this.MediumAnswers[i].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
                this.MediumAnswers[i].Text = i.ToString();
                this.MediumAnswers[i].ReadOnly = true;
                this.MediumAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.MediumAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(MediumAnswers[i]);
                MediumTotals[i] = new System.Windows.Forms.RichTextBox();
                this.MediumTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
                this.MediumTotals[i].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + ((i - 6) * (panel1.Height / 5)));
                this.MediumTotals[i].Name = "EasyTotal" + i;
                this.MediumTotals[i].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
                this.MediumTotals[i].Text = i.ToString();
                this.MediumTotals[i].ReadOnly = true;
                this.MediumTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(MediumTotals[i]);
            }
            MediumAnswers[11] = new System.Windows.Forms.RichTextBox();
            this.MediumAnswers[11].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.MediumAnswers[11].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 5) - 15, panel1.Location.Y + panel1.Height + 15);
            this.MediumAnswers[11].Name = "MediumAnswer" + 11;
            this.MediumAnswers[11].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
            this.MediumAnswers[11].Text = 11.ToString();
            this.MediumAnswers[11].ReadOnly = true;
            this.MediumAnswers[11].SelectionAlignment = HorizontalAlignment.Center;
            this.MediumAnswers[11].BackColor = Color.Gray;
            this.Controls.Add(MediumAnswers[11]);
            MediumTotals[11] = new System.Windows.Forms.RichTextBox();
            this.MediumTotals[11].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.MediumTotals[11].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
            this.MediumTotals[11].Name = "MediumTotal" + 11;
            this.MediumTotals[11].Size = new System.Drawing.Size(panel1.Width / 5, panel1.Height / 5);
            this.MediumTotals[11].Text = 11.ToString();
            this.MediumTotals[11].ReadOnly = true;
            this.MediumTotals[11].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(MediumTotals[11]);
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            for (int i = 0; i < 7; i++)
            {
                for (int x = 0; x < 7; x++)
                {
                    HardBoxes[i, x] = new System.Windows.Forms.RichTextBox();
                    this.HardBoxes[i, x].Font = new System.Drawing.Font("Microsoft Sans Serif", 32F);
                    this.HardBoxes[i, x].Location = new System.Drawing.Point(i * (panel1.Width / 7), x * (panel1.Height / 7));
                    this.HardBoxes[i, x].Name = "EasyBox" + i + x;
                    this.HardBoxes[i, x].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                    if (hardcomplete[0])
                    {
                        if (!hardinitial1[x, i].Equals('0'))
                        {
                            this.HardBoxes[i, x].Text = hardinitial1[x, i].ToString();
                        }
                    }
                    else if (hardcomplete[1])
                    {
                        if (!hardinitial2[x, i].Equals('0'))
                        {
                            this.HardBoxes[i, x].Text = hardinitial2[x, i].ToString();
                        }
                    }
                    else if (hardcomplete[2])
                    {
                        if (!hardinitial3[x, i].Equals('0'))
                            this.HardBoxes[i, x].Text = hardinitial3[x, i].ToString();
                    }
                    this.HardBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
                    this.HardBoxes[i, x].ReadOnly = true;
                    panel1.Controls.Add(HardBoxes[i, x]);
                }
            }
            panel1.PerformLayout();
            clearTotals();
            for (int i = 0; i < 7; i++)
            {
                HardAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.HardAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                this.HardAnswers[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 7)), panel1.Location.Y - (panel1.Height / 7) - 15);
                this.HardAnswers[i].Name = "HardAnswer" + i;
                this.HardAnswers[i].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                this.HardAnswers[i].Text = i.ToString();
                this.HardAnswers[i].ReadOnly = true;
                this.HardAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.HardAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(HardAnswers[i]);
                HardTotals[i] = new System.Windows.Forms.RichTextBox();
                this.HardTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                this.HardTotals[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 7)), panel1.Location.Y + panel1.Height + 15);
                this.HardTotals[i].Name = "HardTotal" + i;
                this.HardTotals[i].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                this.HardTotals[i].Text = i.ToString();
                this.HardTotals[i].ReadOnly = true;
                this.HardTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(HardTotals[i]);
            }
            HardAnswers[7] = new System.Windows.Forms.RichTextBox();
            this.HardAnswers[7].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.HardAnswers[7].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 7) - 15, panel1.Location.Y - (panel1.Height / 7) - 15);
            this.HardAnswers[7].Name = "EasyAnswer" + 7;
            this.HardAnswers[7].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
            this.HardAnswers[7].Text = 7.ToString();
            this.HardAnswers[7].ReadOnly = true;
            this.HardAnswers[7].SelectionAlignment = HorizontalAlignment.Center;
            this.HardAnswers[7].BackColor = Color.Gray;
            this.Controls.Add(HardAnswers[7]);
            HardTotals[7] = new System.Windows.Forms.RichTextBox();
            this.HardTotals[7].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.HardTotals[7].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y - (panel1.Height / 7) - 15);
            this.HardTotals[7].Name = "EasyTotal" + 7;
            this.HardTotals[7].Size = new System.Drawing.Size(panel1.Width /7, panel1.Height / 7);
            this.HardTotals[7].Text = 7.ToString();
            this.HardTotals[7].ReadOnly = true;
            this.HardTotals[7].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(HardTotals[7]);
            for (int i = 8; i < 15; i++)
            {
                HardAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.HardAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                this.HardAnswers[i].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 7) - 15, panel1.Location.Y + ((i - 8) * (panel1.Height / 7)));
                this.HardAnswers[i].Name = "HardAnswer" + i;
                this.HardAnswers[i].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                this.HardAnswers[i].Text = i.ToString();
                this.HardAnswers[i].ReadOnly = true;
                this.HardAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.HardAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(HardAnswers[i]);
                HardTotals[i] = new System.Windows.Forms.RichTextBox();
                this.HardTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                this.HardTotals[i].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + ((i - 8) * (panel1.Height / 7)));
                this.HardTotals[i].Name = "HardTotal" + i;
                this.HardTotals[i].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                this.HardTotals[i].Text = i.ToString();
                this.HardTotals[i].ReadOnly = true;
                this.HardTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(HardTotals[i]);
            }
            HardAnswers[15] = new System.Windows.Forms.RichTextBox();
            this.HardAnswers[15].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.HardAnswers[15].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 7) - 15, panel1.Location.Y + panel1.Height + 15);
            this.HardAnswers[15].Name = "EasyAnswer" + 15;
            this.HardAnswers[15].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
            this.HardAnswers[15].Text = 15.ToString();
            this.HardAnswers[15].ReadOnly = true;
            this.HardAnswers[15].SelectionAlignment = HorizontalAlignment.Center;
            this.HardAnswers[15].BackColor = Color.Gray;
            this.Controls.Add(HardAnswers[15]);
            HardTotals[15] = new System.Windows.Forms.RichTextBox();
            this.HardTotals[15].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.HardTotals[15].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
            this.HardTotals[15].Name = "EasyTotal" + 15;
            this.HardTotals[15].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
            this.HardTotals[15].Text = 15.ToString();
            this.HardTotals[15].ReadOnly = true;
            this.HardTotals[15].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(HardTotals[15]);
        }
        public void readin()
        {
            string[] e1lines;
            string[] e2lines;
            string[] e3lines;
            string[] m1lines;
            string[] m2lines;
            string[] m3lines;
            string[] h1lines;
            string[] h2lines;
            string[] h3lines;
            try
            {
                e1lines = System.IO.File.ReadAllLines(e1path);
                e2lines = System.IO.File.ReadAllLines(e2path);
                e3lines = System.IO.File.ReadAllLines(e3path);
                m1lines = System.IO.File.ReadAllLines(m1path);
                m2lines = System.IO.File.ReadAllLines(m2path);
                m3lines = System.IO.File.ReadAllLines(m3path);
                h1lines = System.IO.File.ReadAllLines(h1path);
                h2lines = System.IO.File.ReadAllLines(h2path);
                h3lines = System.IO.File.ReadAllLines(h3path);
                for (int i = 0; i < 3; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        easyinitial1[i, x] = e1lines[i].ElementAt(x);
                        easyinitial2[i, x] = e2lines[i].ElementAt(x);
                        easyinitial3[i, x] = e3lines[i].ElementAt(x);
                    }
                }
                for (int i = 0; i < 5; i++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        mediuminitial1[i, x] = m1lines[i].ElementAt(x);
                        mediuminitial2[i, x] = m2lines[i].ElementAt(x);
                        mediuminitial3[i, x] = m3lines[i].ElementAt(x);
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        hardinitial1[i, x] = h1lines[i].ElementAt(x);
                        hardinitial2[i, x] = h2lines[i].ElementAt(x);
                        hardinitial3[i, x] = h3lines[i].ElementAt(x);
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.ToString());
            }
            
        }
        }
}
