using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleTables;

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

        internal int[] counts;

        private void CopyFile(string filename)
        {
            var basedir = Path.Combine(Environment.CurrentDirectory, "BaseDictionaries");
            if (!Directory.Exists(basedir))
                Directory.CreateDirectory(basedir);

            if (Path.GetDirectoryName(filename) != basedir)
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

            using (StreamReader f = new StreamReader(filename))
            {
                var st = f.ReadLine().Split('\t');
                bool hascount = false;

                if (st.Last() == "Counts")
                {
                    hascount = true;
                    this.head = st[..^1];
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
                        core.Add(st[..^1]);
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

        public string[] Next()
        {
            double sum = counts.Sum();
            double sum2 = sum * (counts.Length-1);
            var p = counts.Select(c => (sum - c) / sum2).ToArray();

            Debug.WriteLine(p.Sum());

            double val = new Random().NextDouble();

            int k = 0;
            while (p[k] < val)
            {
                val -= p[k];
                k++;
            }


            counts[k]++;

            return core[k];
        }

        public void Save()
        {
            using (var f = new StreamWriter(itspath))
            {
                for (int i = 0; i < head.Length; i++)
                    f.Write($"{head[i]}\t");
                f.WriteLine("Counts");

                for (int p = 0; p < counts.Length; p++)
                {
                    for (int i = 0; i < core[p].Length; i++)
                        f.Write($"{core[p][i]}\t");
                    f.WriteLine(counts[p]);
                }

            }
        }

        public string GetHead()
        {
            string s = "";
            for (int i = 0; i < head.Length; i++)
                s += $"{head[i]}\t\t";
            s += "Counts";

            return s;
        }
        public string[] GetCore()
        {
            string[] res = new string[counts.Length];

            for (int p = 0; p < counts.Length; p++)
            {
                res[p] = "";
                for (int i = 0; i < core[p].Length; i++)
                    res[p] += $"{core[p][i]}\t\t";
                res[p] += counts[p] - 1;
            }

            return res;
        }


        public string[] GetTable()
        {
            var table = new ConsoleTable(this.head.Concat(new string[] { "Counts" }).ToArray());

            for (int p = 0; p < counts.Length; p++)
            {
                table.AddRow(this.core[p].Concat(new string[] { (counts[p] - 1).ToString() }).ToArray());
            }

            //Debug.Write(table.ToString());

            return table.ToString().Split("\n");
        }
    }


    //internal class PropsGet
    //{

    //}
}
