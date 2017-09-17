using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class.ConfigFileManager {
    public class UserInfo {
        public UserInfo(string userId, string password) {
            UserId = userId;
            Password = password;
        }

        public UserInfo() {
            
        }

        public string UserId { get; set; }
        public string Password { get; set; }
    }
}
