using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twee2Z.ObjectTree
{
    public class Tags
    {
        //List<String> tagarr = new List<String>();
        string[] tagarr;
        public Tags(String s)
        {
            tagarr = new string[s.Split(' ').Length];
            tagarr = s.Split(' ');
            tagarr[0] = tagarr[0].Substring(1);
            if (tagarr.Length > 1)
            {
                tagarr[tagarr.Length-1] = tagarr[tagarr.Length-1].Substring(0, tagarr[tagarr.Length-1].Length-1); 
            }
        }
    }
}
