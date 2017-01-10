using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPF.KeyBoardCount
{
    public class FileCount
    {
        private List<string> types;
        LineCount _lc;
        public FileCount(LineCount lc)
        {
            _lc = lc;
            types = new List<string>() { ".cpp", ".cs", ".c", ".h" };
        }

        /// <summary>
        /// 检测一个C代码文件中的有效代码行数
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>代码行数</returns>
        public static int LinesOfCode(string filename)
        {
            System.IO.StreamReader sr = System.IO.File.OpenText(filename);
            string s = sr.ReadToEnd();
            sr.Close();
            bool isLine = false;  //一行中拥有有效字符时为true，该行可记入代码行数
            bool isCommitLf = false; //注释/*...*/中出现至少一个折行时为true
            int lines = 0;    //代码行数统计
            for (int i = 0; i < s.Length; i++)
            {
                //无效字符
                if (s[i] == ' ' || s[i] == '\r' || s[i] == '\t')
                {
                    continue;
                }
                //搜索到换行，若该行有有效字符
                if (s[i] == '\n')
                {
                    if (isLine)
                    {
                        lines++;
                        isLine = false;
                    }
                    continue;
                }
                //字符串，占多少行按多少行算
                if (s[i] == '\"')
                {
                    while (true)
                    {
                        i++;
                        //如果文件遍历完毕则强行中止
                        if (i >= s.Length)
                        {
                            break;
                        }
                        //再次遇到字符'"'且前方没有或有偶数个'//'时，中止循环并退出
                        if (s[i] == '\"')
                        {
                            int sign = 0, counter = 0;
                            while (true)
                            {
                                sign++;
                                if (i - sign < 0)
                                {
                                    break;
                                }
                                else if (s[i - sign] == '\\')
                                {
                                    counter++;
                                }
                                else
                                {
                                    break;
                                }
                            }
                            if (counter % 2 == 0)
                            {
                                break;
                            }
                        }
                        //字符串中的换行，直接算作一行代码
                        if (s[i] == '\n')
                        {
                            lines++;
                            isLine = true;
                        }
                    }
                    isLine = true;
                    continue;
                }
                //遇到形如 /*...*/ 的注释
                if (s[i] == '/' && i < s.Length - 1)
                {
                    if (s[i + 1] == '*')
                    {
                        i++;
                        while (true)
                        {
                            i++;
                            //如果文件遍历完毕则强行中止
                            if (i >= s.Length)
                            {
                                break;
                            }
                            if (s[i] == '\n')
                            {
                                if (isCommitLf == false)
                                {
                                    if (isLine == true)
                                    {
                                        lines++;
                                        isLine = false;
                                    }
                                    isCommitLf = true;
                                }
                            }
                            if (s[i] == '*' && i < s.Length - 1)
                            {
                                if (s[i + 1] == '/')
                                {
                                    i++;
                                    break;
                                }
                            }
                        }
                        isCommitLf = false;
                        continue;
                    }
                }
                //遇到形如 // 的注释
                if (s[i] == '/' && i < s.Length - 1 && s[i + 1] == '/')
                {
                    if (isLine == true)
                    {
                        lines++;
                        isLine = false;
                    }
                    while (true)
                    {
                        i++;
                        if (i >= s.Length || s[i] == '\n')
                        {
                            break;
                        }
                    }
                    continue;
                }
                //该行有了有效字符，算作一行
                isLine = true;
            }
            //最后一行可能没有字符'\n'结尾
            if (isLine)
            {
                lines++;
            }
            return lines;
        }

        /// <summary>
        /// 检测一个文件夹中所有C代码的行数
        /// </summary>
        /// <param name="foldername">文件夹名称</param>
        /// <returns>代码行数</returns>
        public int LinesOfFolder(string foldername)
        {
            //行数统计
            int lines = 0;
            int files = 0;
            //文件夹信息
            System.IO.DirectoryInfo dif = new System.IO.DirectoryInfo(foldername);
            //遍历文件夹中的各子文件夹
            foreach (System.IO.DirectoryInfo di in dif.GetDirectories())
            {
                lines += LinesOfFolder(di.FullName);
            }
            //统计本文件夹中C语言文件代码
            foreach (System.IO.FileInfo f in dif.GetFiles())
            {
                if (this.types.Contains(f.Extension))
                {
                    lines += LinesOfCode(f.FullName);
                    files++;
                    syncData(lines, files);
                }
            }
            return lines;
        }

        public void syncData(int lines, int files)
        {
            _lc.RefData(lines, files);
        }
    }
}
