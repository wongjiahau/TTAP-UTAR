using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Pages;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ClashFinder {
        [Test]
        public void Test_ClashFinder_GetTimetableState_1() {
            var chosenSubject = TestData.Subjects.HubunganEtnik;
            var input =
                Permutator.Run_v2_WithConsideringWeekNumber(TestData.GetSlotsByName(chosenSubject)
                    .ToArray());
            var result = ClashFinder.GetSubjectState(input);
            foreach (int dayState in result) {
                Assert.IsTrue(dayState == 0);
            }
        }

        [Test]
        public void Test_ClashFinder_GetTimetableState_2() {
            var chosenSubject = TestData.Subjects.ArtCraftAndDesign;
            var input =
                Permutator.Run_v2_WithConsideringWeekNumber(TestData.GetSlotsByName(chosenSubject)
                    .ToArray());
            var result = ClashFinder.GetSubjectState(input);
            for (int i = 0; i < Day.NumberOfDaysPerWeek; i++) {
                if (i == 0)  { //Monday
                    Assert.IsTrue(result[i] == Convert.ToInt32("00000000000000111100000000000000",2));
                }
                else {
                    Assert.IsTrue(result[i] == 0);
                }
            }
        }

        [Test]
        public void Test_ClashFinder_GetTimetableState_3() {
            var chosenSubject = TestData.Subjects.AdvancedStructuralSteelDesign;
            var input =
                Permutator.Run_v2_WithConsideringWeekNumber(TestData.GetSlotsByName(chosenSubject)
                    .ToArray());
            var result = ClashFinder.GetSubjectState(input);
            for (int i = 0 ; i < Day.NumberOfDaysPerWeek ; i++) {
                if (i == 0) { //Monday
                    Assert.IsTrue(result[i] == Convert.ToInt32("00000000000000111100000000000000" , 2));
                }
                else if (i == 1) { //Tuesday
                    Assert.AreEqual(Convert.ToInt32("00110000000000000000000000000000".Reverse(), 2), result[i]);
                }
                else { //others
                    Assert.IsTrue(result[i] == 0);
                }
            }
        }



    }
}
