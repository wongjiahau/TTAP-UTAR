using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace Time_Table_Arranging_Program.Class.DataManagerClass {
    public class UserSlotManager {
        public async void SaveSlotDataAsync(string studentId , string loadedHtml) {
            try {
                System.IO.File.WriteAllText(GetPath(studentId) , loadedHtml);
            }
            catch (Exception e) {
                //Surpress exception if failed, because this feature is not neccessarily needed
                //So it shouldn't crash the program
            }

        }

        private string GetPath(string studentId) {
            return UserInfoManager.RootPath + @"\" + studentId + ".html";
        }

        public List<Slot> GetAutoSavedData(string studentId) {
            try {
                var input = System.IO.File.ReadAllText(GetPath(studentId));
                var result = new HtmlSlotParser().Parse(input);
                return result;
            }
            catch (Exception e) {
                return null;
                //Surpress exception if failed, because this feature is not neccessarily needed
                //So it shouldn't crash the program
            }

        }
    }
}
