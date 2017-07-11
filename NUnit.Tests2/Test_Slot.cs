using NUnit.Framework;
using System.Collections.Generic;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Slot {
        [Test]
        public void Test_Slot_Equals_1() {
            //Exactly same slots
            var a = new Slot(213,"MPU3113","Hubungan Etnik", "2", "L", Day.Monday, "KB312", new TimePeriod(Time.CreateTime_24HourFormat(10,30),Time.CreateTime_24HourFormat( 12,00) ), new WeekNumber(new List<int>(){1,2,3}));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.True(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_2() {
            //Exactly same slots except for UID
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(212 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.True(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_3() {
            //Exactly same slots except for Code
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3112" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_4() {
            //Exactly same slots except for Name
            var a = new Slot(213 , "MPU3113" , "Xtnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_5() {
            //Exactly same slots except for Number
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "3" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_6() {
            //Exactly same slots except for Type
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "L" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_7() {
            //Exactly same slots except for Day
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Monday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_8() {
            //Exactly same slots except for Venue
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(212 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB310" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_9() {
            //Exactly same slots except for TimePeriod
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(9 , 30) , Time.CreateTime_24HourFormat(11 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB310" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Test_Slot_Equals_10() {
            //Exactly same slots except for WeekNumber
            var a = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB312" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 3 , 5 }));
            var b = new Slot(213 , "MPU3113" , "Hubungan Etnik" , "2" , "T" , Day.Tuesday , "KB310" , new TimePeriod(Time.CreateTime_24HourFormat(10 , 30) , Time.CreateTime_24HourFormat(12 , 00)) , new WeekNumber(new List<int>() { 1 , 2 , 3 }));
            Assert.False(a.Equals(b));
        }


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
