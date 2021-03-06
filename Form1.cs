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
using System.Diagnostics;

namespace Assignment5
{
   /*
     Cameron Lloyd, Bradley Graves
     z1853137, z1853328
     Assignment 5
     CSCI 473
     Creates a sudoku puzzle from dynamically created rich text boxes 
   */
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern bool HideCaret(IntPtr hwnd);
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

        //all the file paths for the puzzles and scoreboards
        String e1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e1.txt";
        String e2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e2.txt";
        String e3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\e3.txt";
        String m1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m1.txt";
        String m2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m2.txt";
        String m3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\m3.txt";
        String h1path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h1.txt";
        String h2path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h2.txt";
        String h3path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\h3.txt";
        String highScorePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\highscores.txt";
        String easyScoresPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\easy\\scores.txt";
        String medScorePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\medium\\scores.txt";
        String hardScorePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + "\\hard\\scores.txt";

        //The current richTextbox being edited
        RichTextBox currentBox = null;

        // average solve time in milliseconds of each difficulty 
        int avgEasy = 0,
            avgMed = 0,
            avgHard = 0;

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

        // flag to tell if game paused or not
        bool paused = false;

        // holds the records to use for comparison
        int easyRecord = 0;
        int medRecord = 0;
        int hardRecord = 0;

        // flag to determine what needs to be re-read for reset function
        bool initialRead = true;

        // stopwatch used to track time to completion
        Stopwatch timer = new Stopwatch();

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
        //deallocates the rich text boxes as required
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
        //creates all the text boxes when a new game is started
        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //starts the timer as the game starts
            timer.Start();
            //stes the difficulty and then clears the controls
            currentdif = 1;
            panel1.Controls.Clear();
            //sets up the inner boxes and their values
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
                    //depending on the dificulty sets the initial numbers in the inner boxes or the initial values if there is not current game
                    if (easycomplete[0])
                    {
                        if (!easyinitial1[x, i].Equals('0') || (easycurrent != null && !easycurrent[x, i].Equals('0')))
                        {
                            if (easycurrent != null)
                            {
                                this.EasyBoxes[i, x].Text = easycurrent[x, i].ToString();
                            }
                            else
                            {
                                this.EasyBoxes[i, x].Text = easyinitial1[x, i].ToString();
                            }
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
                    }

                    else if (easycomplete[1])
                    {
                        if (!easyinitial2[x, i].Equals('0') || (easycurrent != null && !easycurrent[x, i].Equals('0')))
                        {
                            if (easycurrent != null)
                            {
                                this.EasyBoxes[i, x].Text = easycurrent[x, i].ToString();
                            }
                            else
                            {
                                this.EasyBoxes[i, x].Text = easyinitial2[x, i].ToString();
                            }
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (easycomplete[2])
                    {
                        if (!easyinitial3[x, i].Equals('0') || (easycurrent != null && !easycurrent[x, i].Equals('0')))
                        {
                            if (easycurrent != null)
                            {
                                this.EasyBoxes[i, x].Text = easycurrent[x, i].ToString();
                            }
                            else
                            {
                                this.EasyBoxes[i, x].Text = easyinitial3[x, i].ToString();
                            }
                            this.EasyBoxes[i, x].ReadOnly = true;
                        }
                    }

                    panel1.Controls.Add(EasyBoxes[i, x]);
                }
            }
            panel1.PerformLayout();
            clearTotals();
            //sets upp all the outer boxes for all the totals both current and the answers
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
            //sets the values for the totals and then changes the inital values of the puzzle to gray
            if (easycomplete[0])
            {
                if (easycurrent == null)
                    easycurrent = (char[,])easyinitial1.Clone();
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans1);
                colorInitVals(easyinitial1);
            }
            else if (easycomplete[1])
            {
                if (easycurrent == null)
                    easycurrent = (char[,])easyinitial2.Clone();
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans2);
                colorInitVals(easyinitial2);
            }
            else if (easycomplete[2])
            {
                if (easycurrent == null)
                    easycurrent = (char[,])easyinitial3.Clone();
                setCurrentTotals(easycurrent);
                setAnsTotals(easyans3);
                colorInitVals(easyinitial3);
            }



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
        //handles when the text is changed
        protected void easy_box_changed(object sender, EventArgs e, int x, int y)
        {
            //if there is already some text in the box it clears that text
            if (!(EasyBoxes[x, y].Text.Length == 1))
            {
                easycurrent[y, x] = Convert.ToChar('0');
                EasyBoxes[x, y].Text = "";
            }
            //if there is no text it checks if the input is in fact a number 1-9 and if not it either clears what's there or adds in the number
            else
            {
                if (Convert.ToChar(EasyBoxes[x, y].Text) == '1' || Convert.ToChar(EasyBoxes[x, y].Text) == '2' || Convert.ToChar(EasyBoxes[x, y].Text) == '3' ||
                        Convert.ToChar(EasyBoxes[x, y].Text) == '4' || Convert.ToChar(EasyBoxes[x, y].Text) == '5' || Convert.ToChar(EasyBoxes[x, y].Text) == '6' || Convert.ToChar(EasyBoxes[x, y].Text) == '7'
                        || Convert.ToChar(EasyBoxes[x, y].Text) == '8' || Convert.ToChar(EasyBoxes[x, y].Text) == '9')
                {
                    easycurrent[y, x] = Convert.ToChar(EasyBoxes[x, y].Text);
                }
                else
                {
                    EasyBoxes[x, y].Text = "";
                    easycurrent[y, x] = '0';
                }
            }

            HideCaret(EasyBoxes[x, y].Handle);
            //adjusts the total values
            setCurrentTotals(easycurrent);

            // Set the color based on correctness
            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 7)
                    EasyTotals[i].BackColor = (EasyTotals[i].Text == EasyAnswers[i].Text ? Color.Green : Color.Red);
            EasyTotals[3].BackColor = (EasyTotals[3].Text == EasyAnswers[7].Text ? Color.Green : Color.Red);
            EasyTotals[7].BackColor = (EasyTotals[7].Text == EasyAnswers[3].Text ? Color.Green : Color.Red);
        }
        //if all the answers where correct it gives a message and adds the score to the score file
        public void easyWin()
        {
            timer.Stop();
            bool winner = true;
            for (int i = 0; i < 8; i++)
                if (i != 3 && i != 7)
                    if (EasyTotals[i].Text != EasyAnswers[i].Text)
                        winner = false;

            if (EasyTotals[3].Text != EasyAnswers[7].Text || EasyTotals[7].Text != EasyAnswers[3].Text)
                winner = false;

            if (winner)
            {
                MessageBox.Show("Answer correct, you win!\nCompleted in " + timer.Elapsed.Minutes + " minutes and " + timer.Elapsed.Seconds + " seconds.\n" +
                                "Current best time is " + (easyRecord / 1000) / 60 + " minutes and " + (easyRecord / 1000) % 60 + " seconds." + "\nAverage time: " + (avgEasy / 1000) / 60 +
                                " minutes and " + (avgEasy / 1000) % 60 + " seconds." + (timer.ElapsedMilliseconds < easyRecord ? "\n\nYou set a new record!" : ""), "Winner");

                //if high score, update high score file
                if (timer.ElapsedMilliseconds < easyRecord)
                {
                    easyRecord = (int)timer.ElapsedMilliseconds;
                    string[] lines = { easyRecord.ToString(), medRecord.ToString(), hardRecord.ToString() };
                    File.WriteAllLines(highScorePath, lines);
                }

                //write score to score file
                using (StreamWriter sw = File.AppendText(easyScoresPath))
                {
                    sw.WriteLine(timer.ElapsedMilliseconds.ToString());
                }

                timer.Reset();
                easycomplete[curEasyBoard] = false;
                panel1.Controls.Clear();
                clearTotals();
                if (curEasyBoard == 2)
                {
                    curEasyBoard = 0;
                    for (int i = 0; i < 2; i++)
                        easycomplete[i] = true;
                }
                else
                {
                    curEasyBoard++;
                    currentdif = -1;
                }
                easycurrent = null;
                currentBox = null;
            }
            else
            {
                MessageBox.Show("Answer incorrect, try again.", "Not Quite");
            }
        }
        //creates all the text boxes when a new game is started same as easy but with more boxes
        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
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
                        if (!mediuminitial1[x, i].Equals('0') || (mediumcurrent != null && !mediumcurrent[x, i].Equals('0')))
                        {
                            if (mediumcurrent != null)
                            {
                                this.MediumBoxes[i, x].Text = mediumcurrent[x, i].ToString();
                            }
                            else
                            {
                                MediumBoxes[i, x].Text = mediuminitial1[x, i].ToString();
                            }
                            MediumBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (mediumcomplete[1])
                    {
                        if (!mediuminitial2[x, i].Equals('0') || (mediumcurrent != null && !mediumcurrent[x, i].Equals('0')))
                        {
                            if (mediumcurrent != null)
                            {
                                this.MediumBoxes[i, x].Text = mediumcurrent[x, i].ToString();
                            }
                            else
                            {
                                MediumBoxes[i, x].Text = mediuminitial2[x, i].ToString();
                            }
                            MediumBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (mediumcomplete[2])
                    {
                        if (!mediuminitial3[x, i].Equals('0') || (mediumcurrent != null && !mediumcurrent[x, i].Equals('0')))
                        {
                            if (mediumcurrent != null)
                            {
                                this.MediumBoxes[i, x].Text = mediumcurrent[x, i].ToString();
                            }
                            else
                            {
                                MediumBoxes[i, x].Text = mediuminitial3[x, i].ToString();
                            }
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
                if (mediumcurrent == null)
                    mediumcurrent = (char[,])mediuminitial1.Clone();
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans1);
                colorInitVals(mediuminitial1);
            }
            else if (mediumcomplete[1])
            {
                if (mediumcurrent == null)
                    mediumcurrent = (char[,])mediuminitial2.Clone();
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans2);
                colorInitVals(mediuminitial2);
            }
            else if (mediumcomplete[2])
            {
                if (mediumcurrent == null)
                    mediumcurrent = (char[,])mediuminitial3.Clone();
                setCurrentTotals(mediumcurrent);
                setAnsTotals(mediumans3);
                colorInitVals(mediuminitial3);
            }


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
        //handles when the text is changed same as easy but with more boxes
        protected void medium_box_changed(object s, EventArgs e, int x, int y)
        {
            if (!(MediumBoxes[x, y].Text.Length == 1))
            {
                mediumcurrent[y, x] = Convert.ToChar('0');
                MediumBoxes[x, y].Text = "";
            }
            else
            {
                if (Convert.ToChar(MediumBoxes[x, y].Text) == '1' || Convert.ToChar(MediumBoxes[x, y].Text) == '2' || Convert.ToChar(MediumBoxes[x, y].Text) == '3' || Convert.ToChar(MediumBoxes[x, y].Text) == '4'
                     || Convert.ToChar(MediumBoxes[x, y].Text) == '5' || Convert.ToChar(MediumBoxes[x, y].Text) == '6' || Convert.ToChar(MediumBoxes[x, y].Text) == '7' || Convert.ToChar(MediumBoxes[x, y].Text) == '8'
                  || Convert.ToChar(MediumBoxes[x, y].Text) == '9')
                {
                    mediumcurrent[y, x] = Convert.ToChar(MediumBoxes[x, y].Text);
                }
                else
                {
                    mediumcurrent[y, x] = Convert.ToChar('0');
                    MediumBoxes[x, y].Text = "";
                }
            }

            HideCaret(MediumBoxes[x, y].Handle);

            setCurrentTotals(mediumcurrent);

            // Set the color based on correctness
            for (int i = 0; i < 12; i++)
                if (i != 5 && i != 11)
                    MediumTotals[i].BackColor = (MediumTotals[i].Text == MediumAnswers[i].Text ? Color.Green : Color.Red);
            MediumTotals[5].BackColor = (MediumTotals[5].Text == MediumAnswers[11].Text ? Color.Green : Color.Red);
            MediumTotals[11].BackColor = (MediumTotals[11].Text == MediumAnswers[5].Text ? Color.Green : Color.Red);
        }
        //if all the answers where correct it gives a message and adds the score to the score file
        public void mediumWin()
        {
            timer.Stop();
            bool winner = true;
            for (int i = 0; i < 12; i++)
                if (i != 5 && i != 11)
                    if (MediumTotals[i].Text != MediumAnswers[i].Text)
                        winner = false;

            if (MediumTotals[5].Text != MediumAnswers[11].Text || MediumTotals[11].Text != MediumAnswers[5].Text)
                winner = false;

            if (winner)
            {
                //win message
                MessageBox.Show("Answer correct, you win!\nCompleted in " + timer.Elapsed.Minutes + " minutes and " + timer.Elapsed.Seconds + " seconds.\n" +
                                "Current best time is " + (medRecord / 1000) / 60 + " minutes and " + (medRecord / 1000) % 60 + " seconds." + "\nAverage time: " + (avgMed / 1000) / 60 +
                                " minutes and " + (avgMed / 1000) % 60 + " seconds." + (timer.ElapsedMilliseconds < medRecord ? "\n\nYou set a new record!" : ""), "Winner");

                //if high score, update high score file
                if (timer.ElapsedMilliseconds < medRecord)
                {
                    medRecord = (int)timer.ElapsedMilliseconds;
                    string[] lines = { easyRecord.ToString(), medRecord.ToString(), hardRecord.ToString() };
                    File.WriteAllLines(highScorePath, lines);
                }

                //write score to score file
                using (StreamWriter sw = File.AppendText(medScorePath))
                {
                    sw.WriteLine(timer.ElapsedMilliseconds.ToString());
                }

                timer.Reset();
                mediumcomplete[curMediumBoard] = false;
                panel1.Controls.Clear();
                clearTotals();
                if (curMediumBoard == 2)
                {
                    curMediumBoard = 0;
                    for (int i = 0; i < 2; i++)
                        mediumcomplete[i] = true;
                }
                else
                {
                    curMediumBoard++;
                    currentdif = -1;
                }
                mediumcurrent = null;
                currentBox = null;
            }
            else
            {
                MessageBox.Show("Answer incorrect, try again.", "Not Quite");
            }
        }
        //creates all the text boxes when a new game is started same as easy but with more boxes
        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer.Start();
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
                        if (!hardinitial1[x, i].Equals('0') || (hardcurrent != null && !hardcurrent[x, i].Equals('0')))
                        {
                            if (hardcurrent != null)
                            {
                                HardBoxes[i, x].Text = hardcurrent[x, i].ToString();
                            }
                            else
                            {
                                HardBoxes[i, x].Text = hardinitial1[x, i].ToString();
                            }
                            HardBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (hardcomplete[1])
                    {
                        if (!hardinitial2[x, i].Equals('0') || (hardcurrent != null && !hardcurrent[x, i].Equals('0')))
                        {
                            if (hardcurrent != null)
                            {
                                HardBoxes[i, x].Text = hardcurrent[x, i].ToString();
                            }
                            else
                            {
                                this.HardBoxes[i, x].Text = hardinitial2[x, i].ToString();
                            }
                            HardBoxes[i, x].ReadOnly = true;
                        }
                    }
                    else if (hardcomplete[2])
                    {
                        if (!hardinitial3[x, i].Equals('0') || (hardcurrent != null && !hardcurrent[x, i].Equals('0')))
                        {
                            if (hardcurrent != null)
                            {
                                HardBoxes[i, x].Text = hardcurrent[x, i].ToString();
                            }
                            else
                            {
                                this.HardBoxes[i, x].Text = hardinitial3[x, i].ToString();
                            }
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
                    hardcurrent = (char[,])hardinitial1.Clone();
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans1);
                colorInitVals(hardinitial1);
            }
            else if (hardcomplete[1])
            {
                if (hardcurrent == null)
                    hardcurrent = (char[,])hardinitial2.Clone();
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans2);
                colorInitVals(hardinitial2);
            }
            else if (hardcomplete[2])
            {
                if (hardcurrent == null)
                    hardcurrent = (char[,])hardinitial3.Clone();
                setCurrentTotals(hardcurrent);
                setAnsTotals(hardans3);
                colorInitVals(hardinitial3);
            }
            else


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
        //handles when the text is changed same as easy but with more boxes
        protected void hard_box_changed(object s, EventArgs e, int x, int y)
        {
            if (!(HardBoxes[x, y].Text.Length == 1))
            {
                hardcurrent[y, x] = Convert.ToChar('0');
                HardBoxes[x, y].Text = "";
            }
            else
            {
                if (Convert.ToChar(HardBoxes[x, y].Text) == '1' || Convert.ToChar(HardBoxes[x, y].Text) == '2' || Convert.ToChar(HardBoxes[x, y].Text) == '3' || Convert.ToChar(HardBoxes[x, y].Text) == '4' || Convert.ToChar(HardBoxes[x, y].Text) == '5'
                    || Convert.ToChar(HardBoxes[x, y].Text) == '6' || Convert.ToChar(HardBoxes[x, y].Text) == '7' || Convert.ToChar(HardBoxes[x, y].Text) == '8' || Convert.ToChar(HardBoxes[x, y].Text) == '9')
                {
                    hardcurrent[y, x] = Convert.ToChar(HardBoxes[x, y].Text);
                }
                else
                {
                    hardcurrent[y, x] = Convert.ToChar('0');
                    HardBoxes[x, y].Text = "";
                }
            }

            HideCaret(HardBoxes[x, y].Handle);

            setCurrentTotals(hardcurrent);

            // Set the color based on correctness
            for (int i = 0; i < 16; i++)
                if (i != 7 && i != 15)
                    HardTotals[i].BackColor = (HardTotals[i].Text == HardAnswers[i].Text ? Color.Green : Color.Red);
            HardTotals[7].BackColor = (HardTotals[7].Text == HardAnswers[15].Text ? Color.Green : Color.Red);
            HardTotals[15].BackColor = (HardTotals[15].Text == HardAnswers[7].Text ? Color.Green : Color.Red);
        }
        //if all the answers where correct it gives a message and adds the score to the score file
        public void hardWin()
        {
            timer.Stop();
            bool winner = true;
            for (int i = 0; i < 16; i++)
                if (i != 7 && i != 15)
                    if (HardTotals[i].Text != HardAnswers[i].Text)
                        winner = false;

            if (HardTotals[7].Text != HardAnswers[15].Text || HardTotals[15].Text != HardAnswers[7].Text)
                winner = false;

            if (winner)
            {
                //win message
                MessageBox.Show("Answer correct, you win!\nCompleted in " + timer.Elapsed.Minutes + " minutes and " + timer.Elapsed.Seconds + " seconds.\n" +
                                "Current best time is " + (hardRecord / 1000) / 60 + " minutes and " + (hardRecord / 1000) % 60 + " seconds." + "\nAverage time: " + (avgHard / 1000) / 60 +
                                " minutes and " + (avgHard / 1000) % 60 + " seconds." + (timer.ElapsedMilliseconds < hardRecord ? "\n\nYou set a new record!" : ""), "Winner");

                //if high score, update high score file
                if (timer.ElapsedMilliseconds < hardRecord)
                {
                    hardRecord = (int)timer.ElapsedMilliseconds;
                    string[] lines = { easyRecord.ToString(), medRecord.ToString(), hardRecord.ToString() };
                    File.WriteAllLines(highScorePath, lines);
                }

                //write score to score file
                using (StreamWriter sw = File.AppendText(hardScorePath))
                {
                    sw.WriteLine(timer.ElapsedMilliseconds.ToString());
                }

                timer.Reset();
                hardcomplete[curHardBoard] = false;
                panel1.Controls.Clear();
                clearTotals();
                if (curHardBoard == 2)
                {
                    curHardBoard = 0;
                    for (int i = 0; i < 2; i++)
                        hardcomplete[i] = true;
                }
                else
                {
                    curHardBoard++;
                    currentdif = -1;
                }
                hardcurrent = null;
                currentBox = null;
            }
            else
            {
                MessageBox.Show("Answer incorrect, try again.", "Not Quite");
            }
        }
        //reads in the files for the puzzles
        public void readin()
        {
            string[] records;
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
                records = File.ReadAllLines(highScorePath);
                e1lines = System.IO.File.ReadAllLines(e1path);
                e2lines = System.IO.File.ReadAllLines(e2path);
                e3lines = System.IO.File.ReadAllLines(e3path);
                m1lines = System.IO.File.ReadAllLines(m1path);
                m2lines = System.IO.File.ReadAllLines(m2path);
                m3lines = System.IO.File.ReadAllLines(m3path);
                h1lines = System.IO.File.ReadAllLines(h1path);
                h2lines = System.IO.File.ReadAllLines(h2path);
                h3lines = System.IO.File.ReadAllLines(h3path);

                easyRecord = Convert.ToInt32(records[0]);
                medRecord = Convert.ToInt32(records[1]);
                hardRecord = Convert.ToInt32(records[2]);

                //MessageBox.Show(records[2]);

                string line;
                int eScoreVal = 0;
                int eScoreCount = 0;
                int mScoreVal = 0;
                int mScoreCount = 0;
                int hScoreVal = 0;
                int hScoreCount = 0;

                if (initialRead)
                {
                    // read easy scores
                    StreamReader file = new StreamReader(easyScoresPath);
                    while ((line = file.ReadLine()) != null)
                    {
                        eScoreVal += Convert.ToInt32(line);
                        eScoreCount++;
                    }
                    if (eScoreCount > 0)
                        avgEasy = eScoreVal / eScoreCount;
                    file.Close();

                    // read medium scores
                    file = new StreamReader(medScorePath);
                    while ((line = file.ReadLine()) != null)
                    {
                        mScoreVal += Convert.ToInt32(line);
                        mScoreCount++;
                    }
                    if (mScoreCount > 0)
                        avgMed = mScoreVal / mScoreCount;
                    file.Close();

                    // read hard scores
                    file = new StreamReader(hardScorePath);
                    while ((line = file.ReadLine()) != null)
                    {
                        hScoreVal += Convert.ToInt32(line);
                        hScoreCount++;
                    }
                    if (hScoreCount > 0)
                        avgHard = eScoreVal / eScoreCount;
                    file.Close();
                }

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

            initialRead = false;
        }
        //sets the totals for the answers to the puzzles
        public void setAnsTotals(char[,] charar)
        {
            //sets the answers if its an easy puzzle
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
            //sets the answers if its an medium puzzle
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
            //sets the answers if its an hard puzzle
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

        //sets the current totals for the puzzle's current state
        public void setCurrentTotals(char[,] charar)
        {
            //sets the current totals if its an easy puzzle
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
            //sets the current totals if its an easy puzzle
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
            //sets the current totals if its an easy puzzle
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

        //grays out the initial values for the puzzle
        public void colorInitVals(char[,] charar)
        {
            //checks through the inner boxes of the initial puzzles and if it is not a 0 it changes that box to gray
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
        //hides the cursor if you select a text box
        public void hide_focus(object sender, EventArgs e, int x, int y)
        {
            switch (currentdif)
            {
                case 1:
                    HideCaret(EasyBoxes[x, y].Handle);
                    if (EasyBoxes[x, y].BackColor != Color.Gray)
                    {
                        if (currentBox == null)
                        {
                            currentBox = EasyBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                        else
                        {
                            currentBox.BackColor = Color.White;
                            currentBox = EasyBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                    }
                    return;
                case 2:
                    HideCaret(MediumBoxes[x, y].Handle);
                    if (MediumBoxes[x, y].BackColor != Color.Gray)
                    {
                        if (currentBox == null)
                        {
                            currentBox = MediumBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                        else
                        {
                            currentBox.BackColor = Color.White;
                            currentBox = MediumBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                    }
                    return;
                case 3:
                    HideCaret(HardBoxes[x, y].Handle);
                    if (HardBoxes[x, y].BackColor != Color.Gray)
                    {
                        if (currentBox == null)
                        {
                            currentBox = HardBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                        else
                        {
                            currentBox.BackColor = Color.White;
                            currentBox = HardBoxes[x, y];
                            currentBox.BackColor = Color.Yellow;
                        }
                    }
                    return;
                default:
                    return;
            }
        }
        //checks to see if you solved the puzzle
        private void check_answer_button_Click(object sender, EventArgs e)
        {
            switch (currentdif)
            {
                case 1: easyWin(); return;
                case 2: mediumWin(); return;
                case 3: hardWin(); return;
                default: return;
            }
        }
        //fills in one answer for you
        private void cheat_button_Click(object sender, EventArgs e)
        {
            if (currentdif == -1)
                return;

            char[,] easyans = new char[3, 3];
            char[,] medans = new char[5, 5];
            char[,] hardans = new char[7, 7];

            bool found = false;
            switch (currentdif)
            {
                // Easy
                case 1:
                    {
                        if (curEasyBoard == 0)
                            easyans = easyans1;
                        else if (curEasyBoard == 1)
                            easyans = easyans2;
                        else
                            easyans = easyans3;

                        for (int i = 0; i < 3; i++)
                        {
                            for (int x = 0; x < 3; x++)
                            {
                                if (EasyBoxes[i, x].Text == "" || EasyBoxes[i, x].Text != easyans[x, i].ToString())
                                {
                                    EasyBoxes[i, x].Text = easyans[x, i].ToString();
                                    easycurrent[x, i] = easyans[x, i];
                                    found = true;
                                }
                                if (found)
                                    break;
                            }
                            if (found)
                                break;
                        }
                        return;
                    }
                // Medium
                case 2:
                    {
                        if (curMediumBoard == 0)
                            medans = mediumans1;
                        else if (curMediumBoard == 1)
                            medans = mediumans2;
                        else
                            medans = mediumans3;

                        for (int i = 0; i < 5; i++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                if (MediumBoxes[i, x].Text == "" || MediumBoxes[i, x].Text != medans[x, i].ToString())
                                {
                                    MediumBoxes[i, x].Text = medans[x, i].ToString();
                                    mediumcurrent[x, i] = medans[x, i];
                                    found = true;
                                }
                                if (found)
                                    break;
                            }
                            if (found)
                                break;
                        }
                        return;
                    }

                // Hard
                case 3:
                    {
                        if (curHardBoard == 0)
                            hardans = hardans1;
                        else if (curHardBoard == 1)
                            hardans = hardans2;
                        else
                            hardans = hardans3;

                        for (int i = 0; i < 7; i++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (HardBoxes[i, x].Text == "" || HardBoxes[i, x].Text != hardans[x, i].ToString())
                                {
                                    HardBoxes[i, x].Text = hardans[x, i].ToString();
                                    hardcurrent[x, i] = hardans[x, i];
                                    found = true;
                                }
                                if (found)
                                    break;
                            }
                            if (found)
                                break;
                        }
                        return;
                    }
                default: return;
            }
        }
        //resets the puzzle
        public void reset_button_Click(object sender, EventArgs e)
        {
            if (currentdif == -1)
                return;
            // re-read the initial values and reset selected box color
            if (currentBox != null)
                currentBox.BackColor = Color.White;
            currentBox = null;
            readin();
            timer.Restart();

            char[,] easyinit = new char[3, 3];
            char[,] medinit = new char[5, 5];
            char[,] hardinit = new char[7, 7];

            switch (currentdif)
            {
                //reset easy board
                case 1:
                    {
                        if (curEasyBoard == 0)
                            easyinit = easyinitial1;
                        else if (curEasyBoard == 1)
                            easyinit = easyinitial2;
                        else
                            easyinit = easyinitial3;

                        for (int i = 0; i < 3; i++)
                        {
                            for (int x = 0; x < 3; x++)

                            {
                                if (EasyBoxes[i, x].Text != easyinit[x, i].ToString())
                                {
                                    EasyBoxes[i, x].Text = "";
                                    easycurrent[x, i] = '0';
                                }
                            }
                        }
                        return;
                    }   
                //reset medium board
                case 2:
                    {
                        if (curMediumBoard == 0)
                            medinit = mediuminitial1;
                        else if (curMediumBoard == 1)
                            medinit = mediuminitial2;
                        else
                            medinit = mediuminitial3;

                        for (int i = 0; i < 5; i++)
                        {
                            for (int x = 0; x < 5; x++)
                            {
                                if (MediumBoxes[i, x].Text != medinit[x, i].ToString())
                                {
                                    MediumBoxes[i, x].Text = "";
                                    mediumcurrent[x, i] = '0';
                                }
                            }
                        }
                        return;
                     }
                //reset hard board
                case 3:
                    {
                        if (curHardBoard == 0)
                            hardinit = hardinitial1;
                        else if (curHardBoard == 1)
                            hardinit = hardinitial2;
                        else
                            hardinit = hardinitial3;

                        for (int i = 0; i < 7; i++)
                        {
                            for (int x = 0; x < 7; x++)
                            {
                                if (HardBoxes[i, x].Text != hardinit[x, i].ToString())
                                {
                                    HardBoxes[i, x].Text = "";
                                    hardcurrent[x, i] = '0';
                                }
                            }
                        }
                        return;
                    }
                default: return;
            }
        }
        //pauses the timer
        private void pause_button_Click(object sender, EventArgs e)
        {
            if (currentdif == -1)
                return;
            if (!paused)
            {
                timer.Stop();
                panel1.Hide();
                paused = true;
                pause_button.Text = "Resume";
            }
            else
            {
                timer.Start();
                panel1.Show();
                paused = false;
                pause_button.Text = "Pause";
            }
        }
    }
}
