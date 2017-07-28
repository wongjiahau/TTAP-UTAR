using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using HtmlAgilityPack;

namespace NUnit.Tests2
{
    [TestFixture]
    public class Test_HtmlSlotParser
    {
        [Test]
        public void Test_HtmlSlotParser_1()
        {
            bool check = false;
            //directly read from the file(Must edit the path value)
            string path = @"C:\Users\yihan\Desktop\Sample HTML.txt";

            var doc = new HtmlDocument();
            doc.Load(path);

            //get the subject table
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='overviewSector']/table");

            foreach (HtmlNode table in nodes)
            {
                foreach (HtmlNode row in table.SelectNodes("tr"))
                {
                    //skip one row for the table header
                    if (check == false)
                    {
                        check = true;
                        continue;
                    }
                    else
                    {

                        HtmlNodeCollection cells = row.SelectNodes("th|td");

                        if (cells == null)
                        {
                            continue;
                        }

                        foreach (HtmlNode cell in cells)
                        {
                            //display item
                            Console.WriteLine(cell.InnerText);
                        }
                    }
                }
            }
        }
    }
}
