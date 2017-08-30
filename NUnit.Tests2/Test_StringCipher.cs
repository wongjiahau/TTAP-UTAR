using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StringCipher {
        [Test]
        public void Test_Encrypt_And_Decrypt() {
            string input = "who live in a pineapple under the sea";
            string salt = "random_salt";
            string encrypted = StringCipher.Encrypt(input, salt);
            string decrypted = StringCipher.Decrypt(encrypted, salt);
            Assert.AreEqual(input, decrypted);

        }

        [Test]
        public void Sandbox() {
            string input = "token lol";
            string salt = "TTAP";
            Console.WriteLine(StringCipher.Encrypt(input, salt));
        }
    }
}
