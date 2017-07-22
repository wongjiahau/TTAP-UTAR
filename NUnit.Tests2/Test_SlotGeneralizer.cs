using NUnit.Framework;
using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SlotGeneralizer {
        [Test]
        public void Test_SlotGeneralizer_1() {
            var a = new Slot(1 , "xxx" , "name" , "2" , "T" , Day.Monday , "KB102" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(2 , "xxx" , "name" , "3" , "T" , Day.Monday , "KB104" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var result = new SlotGeneralizer().Generalize(new List<Slot>() { a , b , });
            Assert.True(result.Count == 1);
            Assert.True(result[0].Number == "2/3");

        }

        [Test]
        public void Test_SlotGeneralizer_2() {
            var a = new Slot(1 , "xxx" , "name" , "2" , "T" , Day.Monday , "KB102" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(2 , "xxx" , "name" , "3" , "L" , Day.Monday , "KB104" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var result = new SlotGeneralizer().Generalize(new List<Slot>() { a , b , });
            Assert.True(result.Count == 2);
        }

        [Test]
        public void Test_SlotGeneralizer_PartitionBySubject_1() {
            var input = TestData.GetSlotsByName(TestData.Subjects.HubunganEtnik);
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            var s = new SlotGeneralizer();
            var result = s.PartitionBySubject(input);
            Assert.True(result.Count == 2);
        }

        [Test]
        public void Test_SlotGeneralizer_PartitionBySubject_2() {
            var input = TestData.GetSlotsByName(TestData.Subjects.HubunganEtnik);
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            var s = new SlotGeneralizer();
            var result = s.PartitionBySubject(input);
            Assert.True(result.Count == 3);
        }

        [Test]
        public void Test_Sandbox() {
            var input = new List<Slot>();
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.Hydrology));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.StructuralAnalysisII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.HighwayAndTransportation));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.FluidMechanicsII));
            input.AddRange(TestData.GetSlotsByName(TestData.Subjects.IntroductionToBuildingServices));
            var result = new SlotGeneralizer().Generalize(input);
            Console.WriteLine("Result's count is " + result.Count);
            Console.WriteLine("input's count is " + input.Count);
        }
    }



}
