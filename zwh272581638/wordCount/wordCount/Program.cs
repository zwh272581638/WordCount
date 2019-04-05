using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Data;

namespace wordCount
{
    //获取指定路径txt文件的字符串
    public class Read
    {
        public string Read1(string path)
        {
            string s = System.IO.File.ReadAllText(path);
            return s;
        }
    }

    public interface ICountCharacters
    {
        int Countcharacters(string str);
    }

    //统计TXT文件中字符个数（不包括中文字符）
    public class CountCharacters : ICountCharacters
    {
        public int Countcharacters(string str)
        {
            ValidLine validLine = new ValidLine();
            int i, count, line;
            count = 0;
            line = 0;
            for (i = 0; i < str.Length; i++)
            {
                if (str[i] >= 0 && str[i] <= 127)
                {
                    if (str[i] == '\n')
                    {
                        line++;
                    }
                    count++;
                }
            }
            count = count - line;
            return count;
        }
    }

    //判断是否是单词.单词：至少以4个英文字母开头，跟上字母数字符号，单词以分隔符分割，不区分大小写。
    public class IsWord
    {
        public bool Isword(string str)
        {
            int i;
            if (str.Length < 4) return false;
            else
            {
                for (i = 0; i < 4; i++)
                {
                    if (!((str[i] >= 'A' && str[i] <= 'Z') || (str[i] >= 'a' && str[i] <= 'z')))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public interface ICountWords
    {
        int Countwords(string str);
        string[] Getwords(string str);
    }

    //统计TXT文件中单词个数，该类有两个方法，一个统计单词个数，一个存取所有单词
    public class CountWords : ICountWords
    {
        //统计单词个数
        public int Countwords(string str)
        {
            IsWord isWord = new IsWord();
            int count = 0;
            int i;
            //string[] sArray = str.Split(new char[2] { ' ', '\n' });
            string[] sArray = str.Split(new string[2] { " ", "\r\n" }, StringSplitOptions.None);
            for (i = 0; i < sArray.Length; i++)
            {
                if (sArray[i].Length != 0 && isWord.Isword(sArray[i]))
                {
                    count++;
                }
            }
            return count;
        }
        //判断单词
        public string[] Getwords(string str)
        {
            IsWord isWord = new IsWord();
            string[] words = new string[Countwords(str)];//用于存放单词
            int i, j;
            //string[] sArray = str.Split(new char[2] {' ','\n' });
            string[] sArray = str.Split(new string[2] { " ", "\r\n" }, StringSplitOptions.None);
            for (i = 0, j = 0; i < sArray.Length; i++)
            {
                if (sArray[i].Length != 0 && isWord.Isword(sArray[i]))
                {
                    words[j] = sArray[i];
                    j++;
                }
            }
            return words;
        }
    }

    //三、统计txt中的有效行数
    public class ValidLine
    {
        public int ValidLine1(string path)
        {
            int count = 0;
            string[] contents = File.ReadAllLines(path);
            for (int i = 0; i < contents.Length; i++)
            {
                count++;
                if (contents[i].Equals(string.Empty))
                {
                    count--;
                    continue;
                }
            }
            return count;
        }
    }

    public interface ITopWords
    {
        Dictionary<string, int> Gethotstring(string[] s);
        string Toptenwords(string[] str);
        string Topnwords(string[] str,int n);
    }

    //统计单词出现次数最高的10个或前n个并统计频数
    public class TopWords : ITopWords
    {
        //建立字典，存放单词及其频数并按频数排序
        public Dictionary<string, int> Gethotstring(string[] s)
        {
            Dictionary<string, int> HOT = new Dictionary<string, int>();
            for (int i = 0; i < s.Length; i++)
            {
                if (HOT.ContainsKey(s[i]))
                {
                    HOT[s[i]]++; ;
                }
                else
                {
                    HOT[s[i]] = 1;
                }
            }
            return HOT.OrderByDescending(r => r.Value).ToDictionary(r => r.Key, r => r.Value);

        }
        //返回频数前10的单词
        public string Toptenwords(string[] str)
        {
            Dictionary<string, int> MEWHOT = Gethotstring(str);
            string output = null;
            //遍历字典
            int size = 0;
            foreach (KeyValuePair<string, int> kvp in MEWHOT)
            {
                size++;
                if (size > 10) break;
                output += kvp.Key + " (" + kvp.Value.ToString() + ")";
                output += "\r\n";
            }
            return output;
        }
        //返回频数前n的单词
        public string Topnwords(string[] str,int n)
        {
            int a=0;
            for (int i=0;i<5000;i++)
            {
                a += i;
            }
            Dictionary<string, int> MEWHOT = Gethotstring(str);
            string output = null;
            //遍历字典
            int size = 0;
            foreach (KeyValuePair<string, int> kvp in MEWHOT)
            {
                size++;
                if (size > n) break;
                output += kvp.Key + " (" + kvp.Value.ToString() + ")";
                output += "\r\n";
            }
            return output;
        }

    }
    public class Output
    {
        public void output(string path,int n)
        {
            Read read = new Read();
            string str = read.Read1(path);
            ICountCharacters countcharacters = new CountCharacters();
            ValidLine validline = new ValidLine();
            ICountWords countwords = new CountWords();
            ITopWords toptenwords = new TopWords();
            bool a = false;
            Console.WriteLine("是否输出以上数据至output.txt（是请输入Y，否则输入N）");
            char b='N';
            b = Convert.ToChar(Console.ReadLine());
            if(b=='Y')
            {
                a = true;
            }
            if(a)
            {
                string FileName = @"D:\output.txt";
                //FileStream fs = new FileStream("D:\\wordCount\\wordCount\\wordCount\\bin\\Debug\\123.txt", FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(FileName);
                sw.WriteLine("\n\ncharacters:" + countcharacters.Countcharacters(str));
                sw.WriteLine("validline:" + validline.ValidLine1(path)); 
                sw.WriteLine("countwords:" + countwords.Countwords(str));
                sw.WriteLine("toptenwords:\n\n" + toptenwords.Topnwords(countwords.Getwords(str),n));
                Console.WriteLine("保存成功，可前往D盘查看！");
                sw.Close();

            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //string path = @"E:\subject.txt";
            Console.WriteLine("请输入文件路径：");
            string path = Console.ReadLine();
            string str = null;
            int n;
            Read read = new Read();
            while (true)
            {
                //异常处理文件路径输错的情况
                try
                {
                    str = read.Read1(path);
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                    Console.WriteLine("\n请重新输入文件路径：");
                    path = Console.ReadLine();
                }
            }
            ICountCharacters countcharacters = new CountCharacters();
            ValidLine validline = new ValidLine();
            ICountWords countwords = new CountWords();
            ITopWords toptenwords = new TopWords();
            Console.WriteLine("\n\ncharacters:" + countcharacters.Countcharacters(str));
            Console.WriteLine("\nvalidline:" + validline.ValidLine1(path));
            Console.WriteLine("\ncountwords:" + countwords.Countwords(str));
            Console.WriteLine("\n输入需要显示的前多少个单词 n ：");
            n = Int32.Parse(Console.ReadLine());
            Console.WriteLine("\ntoptenwords:\n\n" + toptenwords.Topnwords(countwords.Getwords(str),n));
            Output output = new Output();
            output.output(path,n);
        }
    }
}
