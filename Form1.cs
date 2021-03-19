using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment5
{
    public partial class Form1 : Form
    {
        RichTextBox[,] EasyBoxes = new RichTextBox[3, 3];
        RichTextBox[] EasyAnswers = new RichTextBox[7];
        RichTextBox[] EasyTotals = new RichTextBox[7];
        RichTextBox[,] MediumBoxes = new RichTextBox[5, 5];
        RichTextBox[] MediumAnswers = new RichTextBox[11];
        RichTextBox[] MediumTotals = new RichTextBox[11];
        RichTextBox[,] HardBoxes = new RichTextBox[7, 7];
        RichTextBox[] HardAnswers = new RichTextBox[15];
        RichTextBox[] HardTotals = new RichTextBox[15];
        public Form1()
        {
            InitializeComponent();
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
                    this.EasyBoxes[i, x].Text = i.ToString();
                    this.EasyBoxes[i, x].ReadOnly = true;
                    this.EasyBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
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
            this.Controls.Add(EasyAnswers[3]);
            EasyTotals[3] = new System.Windows.Forms.RichTextBox();
            this.EasyTotals[3].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyTotals[3].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
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
                    this.MediumBoxes[i, x].Text = i.ToString();
                    this.MediumBoxes[i, x].ReadOnly = true;
                    this.MediumBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
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
            this.Controls.Add(MediumAnswers[5]);
            MediumTotals[5] = new System.Windows.Forms.RichTextBox();
            this.MediumTotals[5].Font = new System.Drawing.Font("Microsoft Sans Serif", 25F);
            this.MediumTotals[5].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
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
                    this.HardBoxes[i, x].Text = i.ToString();
                    this.HardBoxes[i, x].ReadOnly = true;
                    this.HardBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
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
            this.Controls.Add(HardAnswers[7]);
            HardTotals[7] = new System.Windows.Forms.RichTextBox();
            this.HardTotals[7].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
            this.HardTotals[7].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + panel1.Height + 15);
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
        }

    }
}
