using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Generate_client_files
{
    class Program
    {
        static void Main(string[] args)
        {
            //string[] isnpc = new string[] { "script", "duplicate"};
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string npc_path = path + @"\npc\";
            string npc_pathRE = path + @"\npc\re\";
            string npc_pathPRE = path + @"\npc\pre-re\";
            string data_path = System.IO.Path.Combine(path, "data");
            System.IO.Directory.CreateDirectory(data_path);
            StreamWriter ntemp = File.CreateText("NPC_Temp");
            StreamWriter mtemp = File.CreateText("Monster_Temp");
            StreamWriter wtemp = File.CreateText("Warp_Temp");
            //string line_result = "";
            StringBuilder strBlder = new StringBuilder();
            //Console.Write(path.ToString());
            //Console.Write(npc_path.ToString());
            //Console.WriteLine();
            foreach (string file in Directory.EnumerateFiles(npc_path, "*.conf"))
            {
                FindNPCFiles(path, ntemp, mtemp, wtemp, file);
            }
            foreach (string file in Directory.EnumerateFiles(npc_pathRE, "*.conf"))
            {
                FindNPCFiles(path, ntemp, mtemp, wtemp, file);
            }
        }

        private static void FindNPCFiles(string path, StreamWriter ntemp, StreamWriter mtemp, StreamWriter wtemp, string file)
        {
            //Console.Write(file.ToString());
            //Console.WriteLine();
            string contents = File.ReadAllText(file);
            var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read);
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                string line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.StartsWith("npc: "))
                    {
                        int ndx = line.IndexOf("npc/");
                        string clean_npc_file = line.Substring(ndx, line.Length - ndx);
                        string[] splits = clean_npc_file.Split('/');
                        string clean_npc_file_last = "";
                        for (int i = 0; i < splits.Length - 1; i++)
                        {
                            clean_npc_file_last += @"\" + splits[i];
                        }
                        var last = splits.Last();
                        string full_p = path + clean_npc_file_last + @"\" + last.ToString();
                        bool exists = System.IO.File.Exists(full_p);
                        if (exists)
                        {
                            foreach (string clean_file in Directory.EnumerateFiles(path + clean_npc_file_last, last.ToString()))
                            {
                                WriteNPCFiles(ntemp, mtemp, wtemp, clean_file);
                            }
                        }
                    }
                }
            }
        }

        private static void WriteNPCFiles(StreamWriter ntemp, StreamWriter mtemp, StreamWriter wtemp, string clean_file)
        {
            string clean_contents = File.ReadAllText(clean_file);
            var clean_fileStream = new FileStream(clean_file, FileMode.Open, FileAccess.Read);
            using (var clean_streamReader = new StreamReader(clean_fileStream, Encoding.UTF8))
            {
                string clean_line;
                while ((clean_line = clean_streamReader.ReadLine()) != null)
                {
                    if (clean_line.Contains("script") || clean_line.Contains("duplicate"))
                    {
                        Console.Write(clean_line.ToString() + Environment.NewLine);
                        ntemp.WriteLine(clean_line.ToString());
                    }
                    if (clean_line.Contains("monster") || clean_line.Contains("bossmonster"))
                    {
                        Console.Write(clean_line.ToString() + Environment.NewLine);
                        mtemp.WriteLine(clean_line.ToString());
                    }
                    if (clean_line.Contains("warp"))
                    {
                        Console.Write(clean_line.ToString() + Environment.NewLine);
                        wtemp.WriteLine(clean_line.ToString());
                    }
                    //for (int i = 0; i < isnpc.Length; i++)
                    //{
                    //    Console.Write(clean_line.ToString() + Environment.NewLine);
                    //    //if (clean_line.StartsWith(isnpc[i].ToString()) )//&& !clean_line.Contains("mapflag") && !clean_line.Contains("warp") && !clean_line.Contains("monster"))
                    //    if (clean_line.Contains(isnpc[i].ToString()))
                    //    {
                    //        
                    //        //string[] separators = clean_line.Split('/', ' ', '\t', ',');
                    //        //line_result = "{ \"" + separators[0] + "\", " + g + ", 101, " + separators[6] + ", \"" + separators[5] + "\", \"\", " + separators[1] + "," + separators[2] + ", },";
                    //        //strBlder.Append(line_result);
                    //        //Console.Write(strBlder.ToString() + Environment.NewLine);
                    //        Console.Write(clean_line.ToString() + Environment.NewLine);
                    //        //Action action = () => textBox1.Text = strBlder.ToString();
                    //        //textBox1.Invoke(action);
                    //        //textBox1.BeginInvoke(action);
                    //        //textBox1.Text
                    //        //g++;
                    //        //strBlder.Append(line + Environment.NewLine);
                    //        //{ "alb2trea", 11984, 101, 100, "Fisk", "", 39, 50, },
                    //    }
                    //}
                }
            }
        }
    }
}
