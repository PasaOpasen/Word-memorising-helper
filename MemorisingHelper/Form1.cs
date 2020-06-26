using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemorisingHelper
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.FormClosing += (o, e) =>
            {
                if (Program.voc != null)
                    Program.voc.Save();
            };

            timer1.Interval = 250;
            this.timer1.Tick += (o, e) =>
            {
                label1.Text = $"left: {Math.Round(5-(DateTime.Now-LastTime).TotalMinutes,2)}";
            };

            HideTexts();
            HideButs();
            button6.Hide();
            button7.Hide();
            label1.Text = "";

            this.Size = new Size(200, 150);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.CurrentDirectory, "BaseDictionaries");
            if (!Directory.Exists(path))
                path = Environment.CurrentDirectory;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = path;
                openFileDialog.Filter = "tsv files (*.tsv)|*.tsv|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    if (Program.voc != null)
                        Program.voc.Save();

                    Program.voc = new Vocabulary(openFileDialog.FileName);

                    new TableForm(Program.voc, Path.GetFileNameWithoutExtension(openFileDialog.FileName)).Show();

                    ShowButs();
                }
            }

        }

        void set(string[] s)
        {
            button2.Text = s[0];
            button3.Text = s[1];
            button4.Text = s[2];
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ShowTexts();
            int k = 0;

            set(Program.voc.Next());

            //this.KeyPreview = true;
            //this.KeyPress += (o, e) =>
            button6.Show();
            button7.Hide();
            button6.Click += (o, e) =>
              {
                  
                  set(Program.voc.Next());
                  k++;
                  label1.Text = $"left: {50 - k}";

                  Debug.WriteLine(Program.voc.counts.Sum());

if (k == 50)
                  {
                      label1.Text = "";
                      HideTexts();
                      button6.Hide();
                      return;
                  }


              };

            label1.Text="left: 50";
        }


        DateTime LastTime;
        private void button9_Click(object sender, EventArgs e)
        {
            ShowTexts();
            LastTime = DateTime.Now;
            timer1.Start();

            set(Program.voc.Next());

            //this.KeyPreview = true;
            //this.KeyPress += (o, e) =>
            button7.Show();
            button6.Hide();
            button7.Click += (o, e) =>
            {
                if ((DateTime.Now-LastTime).TotalMinutes >= 5)
                {
                    timer1.Stop();
                    label1.Text = "";
                    HideTexts();
                    button7.Hide();
                    return;
                }


                set(Program.voc.Next());
                //label1.Text = $"left: {50 - k}";

                Debug.WriteLine(Program.voc.counts.Sum());
            };
        }


        private void ShowTexts()
        {
            button2.Show();
            button3.Show();
            button4.Show();
            HideButs();
        }
        private void ShowButs()
        {
            button5.Show();
            button9.Show();
            this.Size = new Size(330, 200);
        }
        private void HideTexts()
        {
            button2.Hide();
            button3.Hide();
            button4.Hide();
            ShowButs();
        }
        private void HideButs()
        {
            button9.Hide();
            button5.Hide();

            this.Size = new Size(702, 278);
        }
    }
}
