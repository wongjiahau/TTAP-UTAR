using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class.TokenParser {
    public class HtmlSlotParser {
        public List<Slot> Parse(string html) {
            var result = new List<Slot>();
            bool firstRowIsSkipped = false;
            string currentSubjectCode = "";
            string currentSubjectName = "";
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            //get the subject table
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='overviewSector']/table");
            foreach (HtmlNode table in nodes) {
                foreach (HtmlNode row in table.SelectNodes("tr")) {
                    if (firstRowIsSkipped == false) {  //skip one row for the table header
                        firstRowIsSkipped = true;
                        continue;
                    }
                    HtmlNodeCollection cells = row.SelectNodes("th|td"); //select table header or table data
                    if (cells == null) {
                        continue;
                    }
                    if (cells.Count == 1) {
                        var tokens = cells[0].InnerText.Split('-');
                        currentSubjectCode = tokens[0].Trim();
                        currentSubjectName = tokens[1].Split('[')[0].Trim().Beautify();
                        continue;
                    }
                    var slot = new Slot {
                        Code = currentSubjectCode ,
                        SubjectName = currentSubjectName
                    };
                    for (var k = 0 ; k < cells.Count ; k++) {
                        int offset = 0;
                        if (row.GetAttributeValue("id" , "").Contains("subRow")) {
                            offset = 4;
                            slot.UID = result.Last().UID;
                            slot.Type = result.Last().Type;
                            slot.Number = result.Last().Number;
                        }
                        string data = cells[k].InnerText;
                        switch (k + offset) {
                            case 0:
                                if (data.IsInteger()) slot.UID = int.Parse(data);
                                break;
                            case 1: slot.Type = data; break;
                            case 2: slot.Number = data; break;
                            case 3: /*not storing this data*/ break; //class size
                            case 4: slot.Day = Day.Parse(data); break;
                            case 5: slot.TimePeriod = TimePeriod.Parse(data); break;
                            case 6: /*not storing this data*/ break; //credit hour
                            case 7: slot.WeekNumber = WeekNumber.Parse(data); break;
                            case 8: slot.Venue = data; break;
                            case 9: /*not storing this data*/ break; //remark
                        }
                    }
                    result.Add(slot);
                }
            }
            return result;
        }
    }
}
