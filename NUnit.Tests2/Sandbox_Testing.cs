using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Sandbox_Testing {
        [Test]
        public void SandBoxTest_1() {
            var list = new List<int> { 1 , 2 , 3 };
            Mutate(list);
            Assert.True(list.Count == 0);

        }

        private static void Mutate(List<int> x) {
            x.Clear();
        }

        [Test]
        public void SandboxTest_2() {
            var list = TestData.TestSlots;
            var result = new SlotGeneralizer().Generalize(list);
            for (int i = 0 ; i < result.Count ; i++) {
                Console.WriteLine(result[i].ToString());
            }
        }

        [Test]
        public void SandboxTest_3() {
            var input = TestData.GetSlotsByName(TestData.Subjects.Hydrology);
            var input2 = TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII);
            var input3 = TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII);
            var input4 = TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation);
            var input5 = TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices);

            int result1 = Permutator.Run_v2_WithConsideringWeekNumber(input.ToArray()).Count;
            int result2 = Permutator.Run_v2_WithConsideringWeekNumber(input2.ToArray()).Count;
            int result3 = Permutator.Run_v2_WithConsideringWeekNumber(input3.ToArray()).Count;
            int result4 = Permutator.Run_v2_WithConsideringWeekNumber(input4.ToArray()).Count;
            int result5 = Permutator.Run_v2_WithConsideringWeekNumber(input5.ToArray()).Count;

            Console.WriteLine("Total combination is " + result1 * result2 * result3 * result4 * result5);
        }

        [Test]
        public void SandboxTest_4() {
            Console.WriteLine("Who live in a pineapple under the sea".TruncateRight(30));
        }

        [Test]
        public void SandboxTest_5() {
            string[] embeddedResources = Assembly.GetAssembly(GetType()).GetManifestResourceNames();
            string all = "";
            for (int i = 0; i < embeddedResources.Length; i++) {
                all += embeddedResources[i] + "\n";
            }
        }
    }
}
