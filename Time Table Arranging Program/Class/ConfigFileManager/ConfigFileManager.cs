using System;
using System.Collections.Generic;
using System.IO;

namespace Time_Table_Arranging_Program.Class.ConfigFileManager {
    public class DataManager {
        private static List<UserInfo> _userInfos;
        private const string Path = @"c:\.ttap";
        private const string DataFilePath = Path + @"\_userIdAndPassword.xml";

        public DataManager() {
            ReadData();
        }

        public void CreateDirectory() {
            try {
                if (Directory.Exists(Path)) return;
                DirectoryInfo di = Directory.CreateDirectory(Path);
            }
            catch (Exception e) {
                //Do nothing if cannot create directory
            }
        }

        public void ReadData() {
            try {
                _userInfos = new ObjectSerializer().DeSerializeObject<List<UserInfo>>(DataFilePath);
            }
            catch (Exception e) {
            }

        }

        private void WriteData() {
            try {
                new ObjectSerializer().SerializeObject(_userInfos , DataFilePath);
            }
            catch (Exception e) {
            }
        }

        public void SaveData(UserInfo newUserInfo) {
            if(_userInfos == null) _userInfos = new List<UserInfo>();
            if (_userInfos.Find(x => x.UserId == newUserInfo.UserId) != null) return;
            _userInfos.Add(newUserInfo);
            WriteData();
            ReadData();
        }

        public string TryGetPassword(string userId) {
            var user = _userInfos?.Find(x => x.UserId == userId);
            return user?.DecryptedPassword;
        }

        public List<string> GetStudentIds() {
            ReadData();
            if(_userInfos == null) return new List<string>();
            var result = new List<string>();
            foreach (var userInfo in _userInfos) {
                result.Add(userInfo.UserId);
            }
            return result;
        }
    }
}