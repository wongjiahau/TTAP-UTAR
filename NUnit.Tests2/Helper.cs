using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NUnit.Tests2 {
    public static class Helper {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="nameSpace">Must end with a period</param>
        /// <returns></returns>
        public static string RawStringOfTestFile(string fileName , string nameSpace = "NUnit.Tests2.TestFiles.") {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = nameSpace + fileName;
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }
    }
}
