namespace Time_Table_Arranging_Program.Class.ConfigFileManager {
    public class UserInfo {
        public UserInfo(string userId , string password) {
            UserId = userId;
            EncryptedPassword = StringCipher.Encrypt(password, userId);
        }

        public UserInfo() {
            //For deserialzation purpose
        }

        public string UserId { get; set; }

        public string EncryptedPassword { get; set; }

        public string DecryptedPassword => StringCipher.Decrypt(EncryptedPassword , UserId);

    }
}
