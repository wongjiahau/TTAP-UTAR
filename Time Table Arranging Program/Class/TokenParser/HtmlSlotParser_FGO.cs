using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class.TokenParser {
    public class HtmlSlotParser_FGO {
        public List<Slot> Parse(string html) {
            var result = new List<Slot>();
            bool firstRowIsSkipped = false;
            string currentSubjectCode = "";
            string currentSubjectName = "";
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            //get the subject table
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes("//div[@id='withClass']/table/tbody");
            foreach (HtmlNode table in nodes) {
                foreach (HtmlNode row in table.SelectNodes("tr")) {
                    if (firstRowIsSkipped == false) {
                        //skip one row for the table header
                        firstRowIsSkipped = true;
                        continue;
                    }
                    HtmlNodeCollection cells = row.SelectNodes("th|td"); //select table header or table data
                    if (cells == null) {
                        continue;
                    }
                    if (cells[0].GetAttributeValue("class" , "") == "normalTbl-sub3header") {
                        (currentSubjectCode, currentSubjectName) =
                            GrepSubjectCodeAndName(cells[0].InnerText);
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
                            var previousSlot = result.Last();
                            slot.UID = previousSlot.UID;
                            slot.Number = previousSlot.Number;
                            slot.Type = previousSlot.Type;
                            slot.LecturerName = previousSlot.LecturerName;
                        }
                        string data = cells[k].InnerText.Trim();
                        switch (k + offset) {
                            case 0:
                                if (data.IsInteger()) slot.UID = int.Parse(data);
                                break;
                            case 1:
                                slot.Type = data;
                                break;
                            case 2:
                                slot.Number = data;
                                break;
                            case 3: /*not storing this data*/ break; //class size
                            case 4:
                                slot.Day = Day.Parse(data);
                                break;
                            case 5:
                                slot.TimePeriod = TimePeriod.Parse(data);
                                break;
                            case 6: /*not storing this data*/ break; //credit hour
                            case 7:          
                                slot.WeekNumber = data == "" ? new NullWeekNumber() : WeekNumber.Parse(data);
                                break;
                            case 8:
                                slot.Venue = data == "" ? "Unassigned venue" : data;
                                break;
                            case 9:
                                slot.LecturerName = data == "" ? "Unassigned lecturer" : GrepLecturerName(data);
                                break;
                            case 10: break; //Reg (I'm not sure what does it mean)
                            case 11: break; //Available
                            case 12: break; //Reserve
                            case 13: break; //Remark
                        }

                    }
                    result.Add(slot);
                }
            }

            return result;
        }

        public static string GrepLecturerName(string s) {
            var tokens = s.Split(',');
            string result = "";
            for (var i = 0; i < tokens.Length; i++) {
                var t = tokens[i];
                result += t.Split('(')[1].Trim().Replace(")", "");
                if (i != tokens.Length - 1) result += ", ";
            }
            return result;
        }

        public static (string code, string subjectName) GrepSubjectCodeAndName(string s) {
            //input example : Barred List by week 5th/12th [by Hour: 0]MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) - [View All] [437]            
            var tokens = s.Split('-');
            string code, subjectName;
            code = tokens[0].Split(']')[1].Trim();
            subjectName = tokens[1].Trim().Beautify();
            return (code, subjectName);
        }
    }
}
