using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests2 {
    public static class Helper {
        public static string RawStringOfTestFile(string fileName) {
            string parentDirectory = "NUnit.Tests2.TestFiles.";
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = parentDirectory + fileName;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }
}
