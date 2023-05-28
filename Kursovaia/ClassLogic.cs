using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Kursovaia
{
    internal class ClassLogic
    {
        public string makeContract(int id)
        {
            string filePath = "document1.txt";

            // Прочитайте текст из файла
            string text = File.ReadAllText(filePath);
            int replaceIndex = 0;
            int index = 0;
            Class1 class1 = new Class1();
            string[] s = class1.GetInfoForDocument(id);
            while ((replaceIndex = text.IndexOf("?!?", replaceIndex)) != -1)
            {
                text = text.Remove(replaceIndex, 3);
                text = text.Insert(replaceIndex, s[index]);
                index++;
            }
            return text;
        }

    }
}
