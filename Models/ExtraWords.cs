using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace Crossword.Models
{
    public class ExtraWords
    {
        private IWebHostEnvironment _hostingEnvironment;
        public ExtraWords(IWebHostEnvironment _hostingEnvironment2)
        {
            _hostingEnvironment = _hostingEnvironment2;
        }
        public string [] GetWords()
        {
            string path = _hostingEnvironment.WebRootPath + "\\Letters\\extra_words.txt";
            string[] words = null;

            if (File.Exists(path))
            {
                words = File.ReadAllLines(path);
            }

            for(int i=0; i< words.Length; i++)
            {
                words[i] = words[i].ToUpper();
            }

            return words;
        }

        public int DigitCount(char[] a, char b)
        {
            int count = 0;
            foreach (var i in a)
            {
                if (i == b)
                {
                    count++;
                }
            }               
            return count;
        }

        public char[] Distinct(char[] a)
        {
            HashSet<char> list = new HashSet<char>(a);
            char[] b = new char[list.Count];
            list.CopyTo(b);

            return b;
        }

        public List<string> GetExtraWordsForLetters(string letters, string [] wordL)
        {
            List<string> extraw = new List<string>();
            // удаляем. те, что у же есть + меньшей длинны
            string[] words = GetWords().Except(wordL).Where(x => x.ToString().Length <= letters.Length).ToArray();

            for (int i = 0; i < wordL.Length; i++)
            {
                wordL[i] = wordL[i].ToUpper();
            }

            if (words != null && words.Length > 0)
            {
                char[] letterChar = letters.ToUpper().ToCharArray();

                Parallel.For(0, words.Length, new ParallelOptions() { MaxDegreeOfParallelism = 50 },
                i =>
                {
                    char[] wordsTmp = words[i].ToUpper().ToCharArray();
                    bool flag = true;

                    for (int j = 0; j < wordsTmp.Length; j++)
                    {
                        if (!letterChar.Contains(wordsTmp[j]))
                        {
                            flag = false;
                            break;
                        }
                        else
                        {
                            if (DigitCount(wordsTmp, wordsTmp[j]) > DigitCount(letterChar, wordsTmp[j]))
                            {
                                flag = false;
                                break;
                            }
                        }
                    }

                    if (flag)
                    {
                        extraw.Add(words[i]);
                    }
                });
            }
                return extraw;
        }
    }
}
