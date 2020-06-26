using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MemorisingHelper
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        public static Vocabulary voc = null;
    }

    public class Vocabulary
    {
        private string itspath;

        private string[] head;

        private List<string[]> core;

        private int[] counts;

        private void CopyFile(string filename)
        {
            var basedir = Path.Combine(Environment.CurrentDirectory, "BaseDictionaries");
            if (!Directory.Exists(basedir))
                Directory.CreateDirectory(basedir);

            if(Path.GetDirectoryName(filename)!= basedir)
            {
                itspath = Path.Combine(basedir, Path.GetFileName(filename));
                File.Copy(filename, itspath, true);
            }
            else
            itspath = filename;
        }

        public Vocabulary(string filename)
        {
            CopyFile(filename);

            using(StreamReader f = new StreamReader(filename))
            {
                var st = f.ReadLine().Split('\t');
                bool hascount = false;

                if (st.Last() == "Count")
                {
                    hascount = true;
                    this.head = st[..-1];
                }
                else
                    this.head = st;

                this.core = new List<string[]>();
                var ct = new List<int>();

                do
                {
                    st = f.ReadLine().Split("\t");

                    if (hascount)
                    {
                        ct.Add(Convert.ToInt32(st.Last()));
                        core.Add(st[..-1]);
                    }
                    else
                        core.Add(st);

                } while (!f.EndOfStream);

                if (hascount)
                    this.counts = ct.ToArray();
                else
                {
                    this.counts = new int[core.Count];
                    for (int i = 0; i < counts.Length; i++)
                        counts[i] = 1;
                }
                    
            }
        }


        public void Save()
        {
            using(var f = new StreamWriter(itspath))
            {
                for (int i = 0; i < head.Length; i++)
                    f.Write($"{head[i]}\t");
                f.WriteLine("Counts");

                for(int p = 0; p < counts.Length; p++)
                {
                    for (int i = 0; i < core[p].Length; i++)
                        f.Write($"{core[p][i]}\t");
                    f.WriteLine(counts[p]);
                }

            }
        }

    }
}
