using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crossword.Models
{
    public class Folder
    {
        public string FolderName { get; set; }
        public List<string> files { get; set; } = new List<string>();
        public override string ToString()
        {
            return FolderName;
        }
    }
}
