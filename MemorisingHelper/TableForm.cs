using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MemorisingHelper
{
    public partial class TableForm : Form
    {
        public TableForm(Vocabulary v,string name)
        {
            InitializeComponent();
            this.Text = name;
            PrintVocabulary(v);

        }

        public void PrintVocabulary(Vocabulary v)
        {
            listBox1.Items.Clear();

            //listBox1.Items.Add(v.GetHead());

            //listBox1.Items.AddRange(v.GetCore());

            listBox1.Items.AddRange(v.GetTable());

            this.Width = listBox1.Items[0].ToString().Length * 10;
        }
    }

}
