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
using System.Runtime.InteropServices;



namespace Assignment5
{
    public partial class Form1 : Form
    {
        //used to hide cursor on focus
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hWnd);
        
        //all of the text boxes for the sudoku puzzle
        RichTextBox[,] EasyBoxes = new RichTextBox[3, 3];
        RichTextBox[] EasyAnswers = new RichTextBox[8];
        RichTextBox[] EasyTotals = new RichTextBox[8];
        RichTextBox[,] MediumBoxes = new RichTextBox[5, 5];
        RichTextBox[] MediumAnswers = new RichTextBox[12];
        RichTextBox[] MediumTotals = new RichTextBox[12];
        RichTextBox[,] HardBoxes = new RichTextBox[7, 7];
        RichTextBox[] HardAnswers = new RichTextBox[16];
        RichTextBox[] HardTotals = new RichTextBox[16];
        //all the file paths for the puzzles
        String e1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e1.txt";
        String e2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e2.txt";
        String e3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e3.txt";
        String m1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m1.txt";
        String m2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m2.txt";
        String m3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m3.txt";
        String h1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h1.txt";
        String h2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h2.txt";
        String h3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h3.txt";
        //these are all the initial matricies for the puzzles as well as some bool variables that state whether or not they have been completed
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
        //these are the answers for the puzzles
        char[,] easyans1 = new char[3, 3];
        char[,] easyans2 = new char[3, 3];
        char[,] easyans3 = new char[3, 3];
        char[,] mediumans1 = new char[5, 5];
        char[,] mediumans2 = new char[5, 5];
        char[,] mediumans3 = new char[5, 5];
        char[,] hardans1 = new char[7, 7];
        char[,] hardans2 = new char[7, 7];
        char[,] hardans3 = new char[7, 7];
        //current states of the puzzles
        char[,] easycurrent;
        char[,] mediumcurrent;
        char[,] hardcurrent;
        //this is just an int representing the current difficulty selected 1 for easy 3 for hard and -1 for not selected
        int currentdif = -1;

        // current board selection, currently just cycles from 1 to 3
        int curEasyBoard = 0;
        int curMediumBoard = 0;
        int curHardBoard = 0;

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
                if (r != null)
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
            //easycomplete[0] = false;
            //easycomplete[1] = false;
            currentdif = 1;
            panel1.Controls.Clear();
            for (int i = 0; i < 3; i++)
            {
                for (int x = 0; x < 3; x++)
                {
                    EasyBoxes[i, x] = new System.Windows.Forms.RichTextBox();
                    this.EasyBoxes[i, x].Font = new System.Drawing.Font("Microsoft Sans Serif", 88F);
                    this.EasyBoxes[i, x].Location = new System.Drawing.Point(i * (panel1.Width / 3), x * (panel1.Height / 3));
                    this.EasyBoxes[i, x].Name = "EasyBox" + i + x;
                    this.EasyBoxes[i, x].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                    this.EasyBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
                    if (easycomplete[0])
                    {
                        if (!easyinitial1[x, i].Equals('0'))
                        {
                            this.EasyBoxes[i, x].Text = easyinitial1[x, i].ToString();
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (easycomplete[1])
                    {
                        if (!easyinitial2[x, i].Equals('0'))
                        {
                            this.EasyBoxes[i, x].Text = easyinitial2[x, i].ToString();
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (easycomplete[2])
                    {
                        if (!easyinitial3[x, i].Equals('0'))
                        {
                            this.EasyBoxes[i, x].Text = easyinitial3[x, i].ToString();
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
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
                this.EasyTotals[i].BackColor = Color.Red;
                this.EasyTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                this.Controls.Add(EasyTotals[i]);
            }
            EasyAnswers[3] = new System.Windows.Forms.RichTextBox();
            this.EasyAnswers[3].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
            this.EasyAnswers[3].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 3) - 15, panel1.Location.Y - (panel1.Height / 3) - 15);
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
            this.EasyTotals[3].BackColor = Color.Red;
            this.EasyTotals[3].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(EasyTotals[3]);
            for (int i = 4; i < 7; i++)
            {
                EasyAnswers[i] = new System.Windows.Forms.RichTextBox();
                this.EasyAnswers[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyAnswers[i].Location = new System.Drawing.Point(panel1.Location.X - (panel1.Width / 3) - 15, panel1.Location.Y + ((i - 4) * (panel1.Height / 3)));
                this.EasyAnswers[i].Name = "EasyAnswer" + i;
                this.EasyAnswers[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyAnswers[i].Text = i.ToString();
                this.EasyAnswers[i].ReadOnly = true;
                this.EasyAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                this.EasyAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(EasyAnswers[i]);
                EasyTotals[i] = new System.Windows.Forms.RichTextBox();
                this.EasyTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 44F);
                this.EasyTotals[i].Location = new System.Drawing.Point(panel1.Location.X + panel1.Width + 15, panel1.Location.Y + ((i - 4) * (panel1.Height / 3)));
                this.EasyTotals[i].Name = "EasyTotal" + i;
                this.EasyTotals[i].Size = new System.Drawing.Size(panel1.Width / 3, panel1.Height / 3);
                this.EasyTotals[i].Text = i.ToString();
                this.EasyTotals[i].ReadOnly = true;
                this.EasyTotals[i].BackColor = Color.Red;
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
            this.EasyTotals[7].BackColor = Color.Red;
            this.EasyTotals[7].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(EasyTotals[7]);

            if (easycomplete[0])
            {
                easycurrent = easyinitial1;
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans1);
            }
            else if (easycomplete[1])
            {
                easycurrent = easyinitial2;
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans2);
            }
            else if (easycomplete[2])
            {
                easycurrent = easyinitial3;
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans3);
            }

            colorInitVals(easycurrent);

            // bind the event handlers for text change and hiding focus for easy puzzles
            EasyBoxes[0, 0].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 0, 0);
            EasyBoxes[0, 1].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 0, 1);
            EasyBoxes[0, 2].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 0, 2);
            EasyBoxes[1, 0].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 1, 0);
            EasyBoxes[1, 1].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 1, 1);
            EasyBoxes[1, 2].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 1, 2);
            EasyBoxes[2, 0].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 2, 0);
            EasyBoxes[2, 1].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 2, 1);
            EasyBoxes[2, 2].TextChanged += (sender2, e2) => easy_box_changed(sender2, e2, 2, 2);
            EasyBoxes[0, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 0);
            EasyBoxes[0, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 1);
            EasyBoxes[0, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 2);
            EasyBoxes[1, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 0);
            EasyBoxes[1, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 1);
            EasyBoxes[1, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 2);
            EasyBoxes[2, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 0);
            EasyBoxes[2, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 1);
            EasyBoxes[2, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 2);
        }

        protected void easy_box_changed(object sender, EventArgs e, int x, int y)
        {
            if (!(EasyBoxes[x, y].Text.Length == 1))
            {
                easycurrent[y, x] = Convert.ToChar('0');
                EasyBoxes[x, y].Text = "";
            }
            else
                easycurrent[y, x] = Convert.ToChar(EasyBoxes[x, y].Text);

            HideCaret(EasyBoxes[x, y].Handle);

            setCurrentTotals(easycurrent);

            // Set the color based on correctness
            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 7)
                    if (EasyTotals[i].Text == EasyAnswers[i].Text)
                        EasyTotals[i].BackColor = Color.Green;
            if (EasyTotals[3].Text == EasyAnswers[7].Text)
                EasyTotals[3].BackColor = Color.Green;
            if (EasyTotals[7].Text == EasyAnswers[3].Text)
                EasyTotals[7].BackColor = Color.Green;
               
    
            if (easyWin())
            {
                MessageBox.Show("YOU WIN!");
                easycomplete[curEasyBoard] = false;
                panel1.Controls.Clear();
                clearTotals();
                curEasyBoard++;
            }

        }

        public bool easyWin()
        {
            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 7)
                    if (EasyTotals[i].Text != EasyAnswers[i].Text)
                        return false;

            if (EasyTotals[3].Text != EasyAnswers[7].Text || EasyTotals[7].Text != EasyAnswers[3].Text)
                return false;

            return true;
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            currentdif = 2;
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
                    this.MediumBoxes[i, x].SelectionAlignment = HorizontalAlignment.Center;
                    if (mediumcomplete[0])
                    {
                        if (!mediuminitial1[x, i].Equals('0'))
                        {
                            MediumBoxes[i, x].Text = mediuminitial1[x, i].ToString();
                            MediumBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (mediumcomplete[1])
                    {
                        if (!mediuminitial2[x, i].Equals('0'))
                        {
                            MediumBoxes[i, x].Text = mediuminitial2[x, i].ToString();
                            MediumBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (mediumcomplete[2])
                    {
                        if (!mediuminitial3[x, i].Equals('0'))
                        {
                            MediumBoxes[i, x].Text = mediuminitial3[x, i].ToString();
                            MediumBoxes[i, x].ReadOnly = true;
                        }
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
                this.MediumTotals[i].BackColor = Color.Red;
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
            this.MediumTotals[5].BackColor = Color.Red;
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
                this.MediumTotals[i].BackColor = Color.Red;
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
            this.MediumTotals[11].BackColor = Color.Red;
            this.MediumTotals[11].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(MediumTotals[11]);
            if (mediumcomplete[0])
            {
                mediumcurrent = mediuminitial1;
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans1);
            }
            else if (mediumcomplete[1])
            {
                mediumcurrent = mediuminitial2;
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans2);
            }
            else if (mediumcomplete[2])
            {
                mediumcurrent = mediuminitial3;
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans3);
            }
            colorInitVals(mediumcurrent);

            // bind the event handlers for changing text and hiding focus for medium puzzles
            MediumBoxes[0, 0].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 0, 0);
            MediumBoxes[0, 1].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 0, 1);
            MediumBoxes[0, 2].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 0, 2);
            MediumBoxes[0, 3].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 0, 3);
            MediumBoxes[0, 4].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 0, 4);
            MediumBoxes[1, 0].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 1, 0);
            MediumBoxes[1, 1].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 1, 1);
            MediumBoxes[1, 2].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 1, 2);
            MediumBoxes[1, 3].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 1, 3);
            MediumBoxes[1, 4].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 1, 4);
            MediumBoxes[2, 0].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 2, 0);
            MediumBoxes[2, 1].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 2, 1);
            MediumBoxes[2, 2].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 2, 2);
            MediumBoxes[2, 3].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 2, 3);
            MediumBoxes[2, 4].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 2, 4);
            MediumBoxes[3, 0].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 3, 0);
            MediumBoxes[3, 1].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 3, 1);
            MediumBoxes[3, 2].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 3, 2);
            MediumBoxes[3, 3].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 3, 3);
            MediumBoxes[3, 4].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 3, 4);
            MediumBoxes[4, 0].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 4, 0);
            MediumBoxes[4, 1].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 4, 1);
            MediumBoxes[4, 2].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 4, 2);
            MediumBoxes[4, 3].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 4, 3);
            MediumBoxes[4, 4].TextChanged += (sender2, e2) => medium_box_changed(sender2, e2, 4, 4);
            MediumBoxes[0, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 0);
            MediumBoxes[0, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 1);
            MediumBoxes[0, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 2);
            MediumBoxes[0, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 3);
            MediumBoxes[0, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 4);
            MediumBoxes[1, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 0);
            MediumBoxes[1, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 1);
            MediumBoxes[1, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 2);
            MediumBoxes[1, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 3);
            MediumBoxes[1, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 4);
            MediumBoxes[2, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 0);
            MediumBoxes[2, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 1);
            MediumBoxes[2, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 2);
            MediumBoxes[2, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 3);
            MediumBoxes[2, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 4);
            MediumBoxes[3, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 0);
            MediumBoxes[3, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 1);
            MediumBoxes[3, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 2);
            MediumBoxes[3, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 3);
            MediumBoxes[3, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 4);
            MediumBoxes[4, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 0);
            MediumBoxes[4, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 1);
            MediumBoxes[4, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 2);
            MediumBoxes[4, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 3);
            MediumBoxes[4, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 4);
        }

        protected void medium_box_changed(object s, EventArgs e, int x, int y)
        {
            if (!(MediumBoxes[x, y].Text.Length == 1))
            {
                mediumcurrent[y, x] = Convert.ToChar('0');
                MediumBoxes[x, y].Text = "";
            }
            else
                mediumcurrent[y, x] = Convert.ToChar(MediumBoxes[x, y].Text);

            HideCaret(MediumBoxes[x, y].Handle);

            setCurrentTotals(mediumcurrent);

            // Set the color based on correctness
            for (int i = 0; i < 12; i++)
                if (i != 5 && i != 11)
                    if (MediumTotals[i].Text == MediumAnswers[i].Text)
                        MediumTotals[i].BackColor = Color.Green;
            if (MediumTotals[5].Text == MediumAnswers[11].Text)
                MediumTotals[5].BackColor = Color.Green;
            if (MediumTotals[11].Text == MediumAnswers[5].Text)
                MediumTotals[11].BackColor = Color.Green;

            if (mediumWin())
            {
                MessageBox.Show("YOU WIN!");
                mediumcomplete[curMediumBoard] = false;
                panel1.Controls.Clear();
                clearTotals();
                curMediumBoard++;
            }
        }

        public bool mediumWin()
        {
            for (int i = 0; i < 12; i++)
                if (i != 5 && i != 11)
                    if (MediumTotals[i].Text != MediumAnswers[i].Text)
                        return false;

            if (MediumTotals[5].Text != MediumAnswers[11].Text || MediumTotals[11].Text != MediumAnswers[5].Text)
                return false;

            return true;
        }
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //hardcomplete[0] = false;
            //hardcomplete[1] = false;
            currentdif = 3;
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
                            HardBoxes[i, x].Text = hardinitial1[x, i].ToString();
                            HardBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (hardcomplete[1])
                    {
                        if (!hardinitial2[x, i].Equals('0'))
                        {
                            this.HardBoxes[i, x].Text = hardinitial2[x, i].ToString();
                            HardBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (hardcomplete[2])
                    {
                        if (!hardinitial3[x, i].Equals('0'))
                        {
                            this.HardBoxes[i, x].Text = hardinitial3[x, i].ToString();
                            HardBoxes[i, x].ReadOnly = true;
                        }
                    }
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
                this.HardAnswers[i].BackColor = Color.Gray;
                this.Controls.Add(HardAnswers[i]);
                HardTotals[i] = new System.Windows.Forms.RichTextBox();
                this.HardTotals[i].Font = new System.Drawing.Font("Microsoft Sans Serif", 16F);
                this.HardTotals[i].Location = new System.Drawing.Point(panel1.Location.X + (i * (panel1.Width / 7)), panel1.Location.Y + panel1.Height + 15);
                this.HardTotals[i].Name = "HardTotal" + i;
                this.HardTotals[i].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
                this.HardTotals[i].Text = i.ToString();
                this.HardTotals[i].ReadOnly = true;
                this.HardTotals[i].BackColor = Color.Red;
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
            this.HardTotals[7].Size = new System.Drawing.Size(panel1.Width / 7, panel1.Height / 7);
            this.HardTotals[7].Text = 7.ToString();
            this.HardTotals[7].ReadOnly = true;
            this.HardTotals[7].BackColor = Color.Red;
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
                this.HardTotals[i].BackColor = Color.Red;
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
            this.HardTotals[15].BackColor = Color.Red;
            this.HardTotals[15].SelectionAlignment = HorizontalAlignment.Center;
            this.Controls.Add(HardTotals[15]);
            if (hardcomplete[0])
            {
                if (hardcurrent == null)
                    hardcurrent = hardinitial1;
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans1);
            }
            else if (hardcomplete[1])
            {
                if (hardcurrent == null)
                    hardcurrent = hardinitial2;
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans2);
            }
            else if (hardcomplete[2])
            {
                if (hardcurrent == null)
                    hardcurrent = hardinitial3;
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans3);
            }
            colorInitVals(hardcurrent);

            // bind the event handlers for changing editable text boxes in easy puzzles
            HardBoxes[0, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 0);
            HardBoxes[0, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 1);
            HardBoxes[0, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 2);
            HardBoxes[0, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 3);
            HardBoxes[0, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 4);
            HardBoxes[0, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 5);
            HardBoxes[0, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 0, 6);
            HardBoxes[1, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 0);
            HardBoxes[1, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 1);
            HardBoxes[1, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 2);
            HardBoxes[1, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 3);
            HardBoxes[1, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 4);
            HardBoxes[1, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 5);
            HardBoxes[1, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 1, 6);
            HardBoxes[2, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 0);
            HardBoxes[2, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 1);
            HardBoxes[2, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 2);
            HardBoxes[2, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 3);
            HardBoxes[2, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 4);
            HardBoxes[2, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 5);
            HardBoxes[2, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 2, 6);
            HardBoxes[3, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 0);
            HardBoxes[3, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 1);
            HardBoxes[3, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 2);
            HardBoxes[3, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 3);
            HardBoxes[3, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 4);
            HardBoxes[3, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 5);
            HardBoxes[3, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 3, 6);
            HardBoxes[4, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 0);
            HardBoxes[4, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 1);
            HardBoxes[4, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 2);
            HardBoxes[4, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 3);
            HardBoxes[4, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 4);
            HardBoxes[4, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 5);
            HardBoxes[4, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 4, 6);
            HardBoxes[5, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 0);
            HardBoxes[5, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 1);
            HardBoxes[5, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 2);
            HardBoxes[5, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 3);
            HardBoxes[5, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 4);
            HardBoxes[5, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 5);
            HardBoxes[5, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 5, 6);
            HardBoxes[6, 0].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 0);
            HardBoxes[6, 1].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 1);
            HardBoxes[6, 2].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 2);
            HardBoxes[6, 3].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 3);
            HardBoxes[6, 4].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 4);
            HardBoxes[6, 5].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 5);
            HardBoxes[6, 6].TextChanged += (sender2, e2) => hard_box_changed(sender2, e2, 6, 6);
            HardBoxes[0, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 0);
            HardBoxes[0, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 1);
            HardBoxes[0, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 2);
            HardBoxes[0, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 3);
            HardBoxes[0, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 4);
            HardBoxes[0, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 5);
            HardBoxes[0, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 0, 6);
            HardBoxes[1, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 0);
            HardBoxes[1, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 1);
            HardBoxes[1, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 2);
            HardBoxes[1, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 3);
            HardBoxes[1, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 4);
            HardBoxes[1, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 5);
            HardBoxes[1, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 1, 6);
            HardBoxes[2, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 0);
            HardBoxes[2, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 1);
            HardBoxes[2, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 2);
            HardBoxes[2, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 3);
            HardBoxes[2, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 4);
            HardBoxes[2, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 5);
            HardBoxes[2, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 2, 6);
            HardBoxes[3, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 0);
            HardBoxes[3, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 1);
            HardBoxes[3, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 2);
            HardBoxes[3, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 3);
            HardBoxes[3, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 4);
            HardBoxes[3, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 5);
            HardBoxes[3, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 3, 6);
            HardBoxes[4, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 0);
            HardBoxes[4, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 1);
            HardBoxes[4, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 2);
            HardBoxes[4, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 3);
            HardBoxes[4, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 4);
            HardBoxes[4, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 5);
            HardBoxes[4, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 4, 6);
            HardBoxes[5, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 0);
            HardBoxes[5, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 1);
            HardBoxes[5, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 2);
            HardBoxes[5, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 3);
            HardBoxes[5, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 4);
            HardBoxes[5, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 5);
            HardBoxes[5, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 5, 6);
            HardBoxes[6, 0].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 0);
            HardBoxes[6, 1].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 1);
            HardBoxes[6, 2].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 2);
            HardBoxes[6, 3].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 3);
            HardBoxes[6, 4].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 4);
            HardBoxes[6, 5].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 5);
            HardBoxes[6, 6].Click += (sender2, e2) => hide_focus(sender2, e2, 6, 6);

        }

        protected void hard_box_changed(object s, EventArgs e, int x, int y)
        {
            if (!(HardBoxes[x, y].Text.Length == 1))
            {
                hardcurrent[y, x] = Convert.ToChar('0');
                HardBoxes[x, y].Text = "";
            }
            else
                hardcurrent[y, x] = Convert.ToChar(HardBoxes[x, y].Text);

            setCurrentTotals(hardcurrent);

            // Set the color based on correctness
            for (int i = 0; i < 16; i++)
                if (i != 7 && i != 15)
                    if (HardTotals[i].Text == HardAnswers[i].Text)
                        HardTotals[i].BackColor = Color.Green;
            if (HardTotals[7].Text == HardAnswers[15].Text)
                HardTotals[7].BackColor = Color.Green;
            if (HardTotals[15].Text == HardAnswers[7].Text)
                HardTotals[15].BackColor = Color.Green;

            if (hardWin())
                MessageBox.Show("YOU WIN!");
        }

        public bool hardWin()
        {
            for (int i = 0; i < 16; i++)
                if (i != 7 && i != 15)
                    if (HardTotals[i].Text != HardAnswers[i].Text)
                        return false;

            if (HardTotals[7].Text != HardAnswers[15].Text || HardTotals[15].Text != HardAnswers[7].Text)
                return false;

            return true;
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
                        easyans1[i, x] = e1lines[i + 4].ElementAt(x);
                        easyans2[i, x] = e2lines[i + 4].ElementAt(x);
                        easyans3[i, x] = e3lines[i + 4].ElementAt(x);
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        mediuminitial1[i, x] = m1lines[i].ElementAt(x);
                        mediuminitial2[i, x] = m2lines[i].ElementAt(x);
                        mediuminitial3[i, x] = m3lines[i].ElementAt(x);
                        mediumans1[i, x] = m1lines[i + 6].ElementAt(x);
                        mediumans2[i, x] = m2lines[i + 6].ElementAt(x);
                        mediumans3[i, x] = m3lines[i + 6].ElementAt(x);
                    }
                }
                for (int i = 0; i < 7; i++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        hardinitial1[i, x] = h1lines[i].ElementAt(x);
                        hardinitial2[i, x] = h2lines[i].ElementAt(x);
                        hardinitial3[i, x] = h3lines[i].ElementAt(x);
                        hardans1[i, x] = h1lines[i + 8].ElementAt(x);
                        hardans2[i, x] = h2lines[i + 8].ElementAt(x);
                        hardans3[i, x] = h3lines[i + 8].ElementAt(x);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        public void setAnsTotals(char[,] charar)
        {
            if (currentdif == 1)
            {
                int[] total = new int[8];
                for (int i = 0; i < 3; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[3] += (int)Char.GetNumericValue(charar[i, i]);
                    total[4] += (int)Char.GetNumericValue(charar[0, i]);
                    total[5] += (int)Char.GetNumericValue(charar[1, i]);
                    total[6] += (int)Char.GetNumericValue(charar[2, i]);
                    total[7] += (int)Char.GetNumericValue(charar[2 - i, i]);
                }
                for (int i = 0; i < 8; i++)
                {
                    EasyAnswers[i].Text = total[i].ToString();
                    EasyAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                }

            }
            if (currentdif == 2)
            {
                int[] total = new int[12];
                for (int i = 0; i < 5; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[3] += (int)Char.GetNumericValue(charar[i, 3]);
                    total[4] += (int)Char.GetNumericValue(charar[i, 4]);
                    total[5] += (int)Char.GetNumericValue(charar[i, i]);
                    total[6] += (int)Char.GetNumericValue(charar[0, i]);
                    total[7] += (int)Char.GetNumericValue(charar[1, i]);
                    total[8] += (int)Char.GetNumericValue(charar[2, i]);
                    total[9] += (int)Char.GetNumericValue(charar[3, i]);
                    total[10] += (int)Char.GetNumericValue(charar[4, i]);
                    total[11] += (int)Char.GetNumericValue(charar[4 - i, i]);
                }
                for (int i = 0; i < 12; i++)
                {
                    MediumAnswers[i].Text = total[i].ToString();
                    MediumAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                }
            }
            if (currentdif == 3)
            {
                int[] total = new int[16];
                for (int i = 0; i < 7; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[3] += (int)Char.GetNumericValue(charar[i, 3]);
                    total[4] += (int)Char.GetNumericValue(charar[i, 4]);
                    total[5] += (int)Char.GetNumericValue(charar[i, 5]);
                    total[6] += (int)Char.GetNumericValue(charar[i, 6]);

                    total[7] += (int)Char.GetNumericValue(charar[i, i]);
                    total[8] += (int)Char.GetNumericValue(charar[0, i]);
                    total[9] += (int)Char.GetNumericValue(charar[1, i]);
                    total[10] += (int)Char.GetNumericValue(charar[2, i]);
                    total[11] += (int)Char.GetNumericValue(charar[3, i]);
                    total[12] += (int)Char.GetNumericValue(charar[4, i]);
                    total[13] += (int)Char.GetNumericValue(charar[5, i]);
                    total[14] += (int)Char.GetNumericValue(charar[6, i]);

                    total[15] += (int)Char.GetNumericValue(charar[6 - i, i]);
                }
                for (int i = 0; i < 16; i++)
                {
                    HardAnswers[i].Text = total[i].ToString();
                    HardAnswers[i].SelectionAlignment = HorizontalAlignment.Center;
                }
            }
        }
        
        
        public void setCurrentTotals(char[,] charar)
        {
            if (currentdif == 1)
            {
                int[] total = new int[8];
                for (int i = 0; i < 3; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[7] += (int)Char.GetNumericValue(charar[i, i]);
                    total[4] += (int)Char.GetNumericValue(charar[0, i]);
                    total[5] += (int)Char.GetNumericValue(charar[1, i]);
                    total[6] += (int)Char.GetNumericValue(charar[2, i]);
                    total[3] += (int)Char.GetNumericValue(charar[2 - i, i]);
                }
                for (int i = 0; i < 8; i++)
                {
                    EasyTotals[i].Text = total[i].ToString();
                    EasyTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                }

            }
            if (currentdif == 2)
            {
                int[] total = new int[12];
                for (int i = 0; i < 5; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[3] += (int)Char.GetNumericValue(charar[i, 3]);
                    total[4] += (int)Char.GetNumericValue(charar[i, 4]);
                    total[11] += (int)Char.GetNumericValue(charar[i, i]);
                    total[6] += (int)Char.GetNumericValue(charar[0, i]);
                    total[7] += (int)Char.GetNumericValue(charar[1, i]);
                    total[8] += (int)Char.GetNumericValue(charar[2, i]);
                    total[9] += (int)Char.GetNumericValue(charar[3, i]);
                    total[10] += (int)Char.GetNumericValue(charar[4, i]);
                    total[5] += (int)Char.GetNumericValue(charar[4 - i, i]);
                }
                for (int i = 0; i < 12; i++)
                {
                    MediumTotals[i].Text = total[i].ToString();
                    MediumTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                }
            }
            if (currentdif == 3)
            {
                int[] total = new int[16];
                for (int i = 0; i < 7; i++)
                {
                    total[0] += (int)Char.GetNumericValue(charar[i, 0]);
                    total[1] += (int)Char.GetNumericValue(charar[i, 1]);
                    total[2] += (int)Char.GetNumericValue(charar[i, 2]);
                    total[3] += (int)Char.GetNumericValue(charar[i, 3]);
                    total[4] += (int)Char.GetNumericValue(charar[i, 4]);
                    total[5] += (int)Char.GetNumericValue(charar[i, 5]);
                    total[6] += (int)Char.GetNumericValue(charar[i, 6]);

                    total[15] += (int)Char.GetNumericValue(charar[i, i]);
                    total[8] += (int)Char.GetNumericValue(charar[0, i]);
                    total[9] += (int)Char.GetNumericValue(charar[1, i]);
                    total[10] += (int)Char.GetNumericValue(charar[2, i]);
                    total[11] += (int)Char.GetNumericValue(charar[3, i]);
                    total[12] += (int)Char.GetNumericValue(charar[4, i]);
                    total[13] += (int)Char.GetNumericValue(charar[5, i]);
                    total[14] += (int)Char.GetNumericValue(charar[6, i]);

                    total[7] += (int)Char.GetNumericValue(charar[6 - i, i]);
                }
                for (int i = 0; i < 16; i++)
                {
                    HardTotals[i].Text = total[i].ToString();
                    HardTotals[i].SelectionAlignment = HorizontalAlignment.Center;
                }
            }
        }
    

        public void colorInitVals(char[,] charar)
        {
            if (currentdif == 1)
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (!(charar[x, i] == '0'))
                        {
                            EasyBoxes[i, x].BackColor = Color.Gray;
                        }

                    }
                }
            }
            else if (currentdif == 2)
            {
                for (int i = 0; i < 5; i++)
                {
                    for (int x = 0; x < 5; x++)
                    {
                        if (!(charar[x, i] == '0'))
                        {
                            MediumBoxes[i, x].BackColor = Color.Gray;
                        }

                    }
                }
            }
            else if (currentdif == 3)
            {
                for (int i = 0; i < 7; i++)
                {
                    for (int x = 0; x < 7; x++)
                    {
                        if (!(charar[x, i] == '0'))
                        {
                            HardBoxes[i, x].BackColor = Color.Gray;
                        }
                    }
                }
            }
        }

        public void hide_focus(object sender, EventArgs e, int x, int y)
        {
            switch (currentdif)
            {
                case 1:     HideCaret(EasyBoxes[x, y].Handle);      return;
                case 2:     HideCaret(MediumBoxes[x, y].Handle);    return;
                case 3:     HideCaret(HardBoxes[x, y].Handle);      return;
                default:    return;
            }
        }
    }
}
