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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var path = Path.Combine(Environment.CurrentDirectory,"BaseDictionaries");
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
                }
            }

        }
    }
}
