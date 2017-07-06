using NUnit.Framework;
using System.Collections.Generic;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Slot {

        [Test]
        public void Test_Slot_EqualsExceptNumberAndVenue_1() {
            var a = new Slot(1 , "xxx" , "name" , "2" , "T" , Day.Monday , "KB102" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(2 , "xxx" , "name" , "3" , "T" , Day.Monday , "KB104" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));

            Assert.True(a.EqualsExceptNumberAndVenue(b));
        }

        [Test]
        public void Test_Slot_EqualsExceptNumberAndVenue_2() {
            var a = new Slot(1 , "xxx" , "name" , "2" , "T" , Day.Monday , "KB102" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(2 , "xxx" , "name" , "3" , "L" , Day.Monday , "KB104" , new TimePeriod(Time.CreateTime_24HourFormat(8 , 0) , Time.CreateTime_24HourFormat(10 , 0)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));

            Assert.False(a.EqualsExceptNumberAndVenue(b));
        }
    }
}
