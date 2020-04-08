using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Crossword.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Newtonsoft.Json;
using Json.Net;

namespace Crossword.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly string letterDir;
        

        public HomeController(IWebHostEnvironment IHostingEnvironment)
        {        
            _hostingEnvironment = IHostingEnvironment;
            letterDir = _hostingEnvironment.WebRootPath + "\\Letters";
        }

        public IActionResult Index()
        {
            ViewData["Title"] = "Главная";

            List<Folder> foldersList = new List<Folder>();

            DirectoryInfo info = new DirectoryInfo(letterDir);
            DirectoryInfo[] folders =  info.GetDirectories();

            for(int i=0; i < folders.Length; i++)
            {
                Folder folder = new Folder();
                folder.FolderName = folders[i].Name;
                FileInfo [] infos = folders[i].GetFiles("*.json");

                for(int j=0; j< infos.Length; j++)
                {
                    folder.files.Add(infos[j].Name);
                }

                foldersList.Add(folder);
            }



            return View(foldersList);
        }

        [HttpGet]
        public IActionResult Letters(string foldername,  string filename)
        {         
            ViewData["Title"] = "letters";
            Newtonsoft.Json.Linq.JObject letters = 
                (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(System.IO.File.ReadAllText(_hostingEnvironment.WebRootPath + "\\Letters\\" + foldername +"\\"+ filename));

            int rowCount = (int)letters["rowCount"];
            int colCount = (int)letters["colCount"];

            string[,] strArr = new string[rowCount, colCount];

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < colCount; j++)
                {
                    strArr[i, j] = " ";            //"\u25A0";
                }
            }

            string[] wrds = new string[letters["words"].Count()];

            for (int l = 0; l < letters["words"].Count(); l++)
            {
                string tmp = letters["words"][l]["word"].ToString();
                wrds[l] = tmp; // !!!
                int rowIndex = (int)letters["words"][l]["rowIndex"];
                int colIndex = (int)letters["words"][l]["colIndex"];

                if ((bool)letters["words"][l]["vertical"] == true)
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        strArr[(i + rowIndex), colIndex] = tmp.Substring(i, 1);
                    }
                }
                else
                {
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        strArr[rowIndex, (i + colIndex)] = tmp.Substring(i, 1);
                    }
                }
            }
            string ltrs = letters["letters"].ToString();

            ExtraWords extraWords = new ExtraWords(_hostingEnvironment);
            List<string> strWrd = extraWords.GetExtraWordsForLetters(ltrs, wrds);

            strWrd.Sort();

            ViewBag.letters = letters;
            ViewBag.strArr = strArr;
            ViewBag.strWrd = strWrd;
            ViewBag.FileName = filename;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
