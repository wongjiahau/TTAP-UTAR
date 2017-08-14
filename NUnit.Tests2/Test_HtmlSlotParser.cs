using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_HtmlSlotParser {
        [Test]
        public void Test_HtmlSlotParser_1() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var result = new HtmlSlotParser().Parse(input);
            Test_SlotParser.TestForResultCorrectness(result);
        }

        [Test]
        public void Test_HtmlSlotParser_2() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var result = new HtmlSlotParser().Parse(input);
            var expectedUids = new HashSet<int>();
            for (int i = 1 ; i <= 130 ; i++) {
                expectedUids.Add(i);
            }
            var actualUids = new HashSet<int>();
            for (int i = 0 ; i < result.Count ; i++) {
                actualUids.Add(result[i].UID);
            }
            Assert.IsTrue(expectedUids.SetEquals(actualUids));
        }

        [Test]
        public void Test_HtmlSlotParser_3() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var actual = new HtmlSlotParser().Parse(input);
            var expected = new List<Slot>()
            {
                new Slot(1, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "1", "L", Day.Monday, "KB521",new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(2, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "2", "L", Day.Monday, "KB521",new TimePeriod(Time.CreateTime_24HourFormat(14, 00), Time.CreateTime_24HourFormat(17, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(3, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "3", "L", Day.Tuesday, "KB520",new TimePeriod(Time.CreateTime_24HourFormat(14, 00), Time.CreateTime_24HourFormat(17, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(4, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "4", "L", Day.Thursday,"KB316",new TimePeriod(Time.CreateTime_24HourFormat(8, 00), Time.CreateTime_24HourFormat(11, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(5, "MPU3123", "TAMADUN ISLAM DAN TAMADUN ASIA (TITAS)".Beautify(), "1", "L", Day.Monday,"KB520", new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(6,"MPU3123", "TAMADUN ISLAM DAN TAMADUN ASIA (TITAS)".Beautify(), "2", "L", Day.Tuesday,"KB520", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(7,"MPU3123", "TAMADUN ISLAM DAN TAMADUN ASIA (TITAS)".Beautify(), "3", "L", Day.Tuesday,"KB521", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(8,"MPU3123", "TAMADUN ISLAM DAN TAMADUN ASIA (TITAS)".Beautify(), "4", "L", Day.Thursday,"KB316", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(9,"MPU3143", "BAHASA MELAYU KOMUNIKASI 2".Beautify(), "1", "L", Day.Thursday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(10,"MPU3173", "MALAYSIAN STUDIES 3 (FOR INTERNATIONAL STUDENTS)".Beautify(), "1", "L", Day.Thursday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(8,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(11, "MPU32013", "BAHASA KEBANGSAAN A".Beautify(), "1", "L", Day.Tuesday, "KB314",new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(12, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "1", "L", Day.Monday, "KB301",new TimePeriod(Time.CreateTime_24HourFormat(12, 00), Time.CreateTime_24HourFormat(13, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(13, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "2", "L", Day.Monday, "KB300", new TimePeriod(Time.CreateTime_24HourFormat(13, 00), Time.CreateTime_24HourFormat(14, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(14, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "1", "T", Day.Tuesday, "KB320", new TimePeriod(Time.CreateTime_24HourFormat(10, 00), Time.CreateTime_24HourFormat(12, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})), new Slot(15, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "2", "T", Day.Tuesday, "KB323", new TimePeriod(Time.CreateTime_24HourFormat(15, 00), Time.CreateTime_24HourFormat(17, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(16, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "3", "T", Day.Wednesday, "KB318", new TimePeriod(Time.CreateTime_24HourFormat(08, 00), Time.CreateTime_24HourFormat(10, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(17, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "4", "T", Day.Thursday, "KB318", new TimePeriod(Time.CreateTime_24HourFormat(12, 00), Time.CreateTime_24HourFormat(14, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})), new Slot(18, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "5", "T", Day.Monday, "KB314", new TimePeriod(Time.CreateTime_24HourFormat(15, 00), Time.CreateTime_24HourFormat(17, 00)), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(19, "MPU34012", "SOCIAL ENTREPRENEURSHIP PROJECT".Beautify(), "1","L", Day.Monday, "KB322", new TimePeriod(Time.CreateTime_24HourFormat(09, 00), (Time.CreateTime_24HourFormat(11, 00))), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})), new Slot(20, "MPU34022", "ARTS AND CULTURAL PERFORMANCE".Beautify(), "1", "L", Day.Monday, "KB314", new TimePeriod(Time.CreateTime_24HourFormat(09, 00), (Time.CreateTime_24HourFormat(11, 00))), new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(21,"MPU34042", "LANGUAGE, CULTURE AND SOCIAL STUDY ABROAD".Beautify(), "1", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(7,00), Time.CreateTime_24HourFormat(9,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(22,"MPU34042", "LANGUAGE, CULTURE AND SOCIAL STUDY ABROAD".Beautify(), "2", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(23,"MPU34042", "LANGUAGE, CULTURE AND SOCIAL STUDY ABROAD".Beautify(), "3", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(24,"MPU34062", "STUDY SOFT SKILLS AND/OR LIFE SKILLS ABROAD".Beautify(), "1", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(7,00), Time.CreateTime_24HourFormat(9,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(25,"MPU34062", "STUDY SOFT SKILLS AND/OR LIFE SKILLS ABROAD".Beautify(), "2", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(26,"MPU34062", "STUDY SOFT SKILLS AND/OR LIFE SKILLS ABROAD".Beautify(), "4", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(16,00), Time.CreateTime_24HourFormat(18,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(27,"MPU34062", "STUDY SOFT SKILLS AND/OR LIFE SKILLS ABROAD".Beautify(), "5", "L", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(16,00), Time.CreateTime_24HourFormat(18,00)),new WeekNumber(new List<int>(){2,3,4,5,6,7,8,9,10,11})  ),
                new Slot(28,"MPU34072", "ART, CRAFT, AND DESIGN".Beautify(), "1", "L", Day.Monday,"KB322", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(29,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "1", "L", Day.Monday,"KB322", new TimePeriod(Time.CreateTime_24HourFormat(17,00), Time.CreateTime_24HourFormat(19,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(30,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "2", "L", Day.Wednesday,"KB323", new TimePeriod(Time.CreateTime_24HourFormat(17,00), Time.CreateTime_24HourFormat(19,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(31,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "1", "T", Day.Tuesday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(17,00), Time.CreateTime_24HourFormat(18,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(32,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "2", "T", Day.Tuesday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(18,00), Time.CreateTime_24HourFormat(19,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(33,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "3", "T", Day.Thursday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(17,00), Time.CreateTime_24HourFormat(18,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(34,"UALB1003", "INTRODUCTION TO GERMAN LANGUAGE".Beautify(), "4", "T", Day.Thursday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(18,00), Time.CreateTime_24HourFormat(19,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(35, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "1", "L", Day.Wednesday , "KB520", new TimePeriod(Time.CreateTime_24HourFormat(08,00), Time.CreateTime_24HourFormat(10,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(36, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "2", "L", Day.Friday , "KB301", new TimePeriod(Time.CreateTime_24HourFormat(10,30), Time.CreateTime_24HourFormat(12,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(37, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "1", "T", Day.Wednesday , "KB325", new TimePeriod(Time.CreateTime_24HourFormat(12,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(38, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "2", "T", Day.Friday , "KB325", new TimePeriod(Time.CreateTime_24HourFormat(14,30), Time.CreateTime_24HourFormat(15,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(39, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "3", "T", Day.Friday , "KB325", new TimePeriod(Time.CreateTime_24HourFormat(15,30), Time.CreateTime_24HourFormat(16,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(40, "UALE1083", "BASIC PROFESSIONAL WRITING".Beautify(), "4", "T", Day.Wednesday , "KB325", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14}) ),
                new Slot(41,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "1", "L", Day.Monday,"KB324", new TimePeriod(Time.CreateTime_24HourFormat(8,00), Time.CreateTime_24HourFormat(10,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(42,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "2", "L", Day.Tuesday,"KB323", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(43,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "1", "T", Day.Monday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(10,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(44,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "2", "T", Day.Monday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(45,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "3", "T", Day.Tuesday,"KB319", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(46,"UALF1003", "INTRODUCTION TO FRENCH".Beautify(), "4", "T", Day.Tuesday,"KB319", new TimePeriod(Time.CreateTime_24HourFormat(12,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(47,"UALJ2013", "INTRODUCTION TO JAPANESE".Beautify(), "1", "L", Day.Wednesday,"KB324", new TimePeriod(Time.CreateTime_24HourFormat(8,00), Time.CreateTime_24HourFormat(10,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(48,"UALJ2013", "INTRODUCTION TO JAPANESE".Beautify(), "1", "T", Day.Wednesday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(10,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(49,"UALJ2013", "INTRODUCTION TO JAPANESE".Beautify(), "2", "T", Day.Wednesday,"KB318", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(50,"UALL1063", "ORAL COMMUNICATION AND INTERPERSONAL SKILLS".Beautify(), "1", "L", Day.Tuesday,"KB301", new TimePeriod(Time.CreateTime_24HourFormat(16,00), Time.CreateTime_24HourFormat(18,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
				//choo ( 51-60)
                new Slot(61,"UECS1004", "PROGRAMMING AND PROBLEM SOLVING".Beautify(), "1", "L", Day.Wednesday,"KB300", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(62,"UECS1004", "PROGRAMMING AND PROBLEM SOLVING".Beautify(), "1", "P", Day.Monday,"KB606", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(14,00)),new WeekNumber(new List<int>(){1,2,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(63,"UECS1004", "PROGRAMMING AND PROBLEM SOLVING".Beautify(), "2", "P", Day.Tuesday,"KB606", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(14,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(64,"UECS1013", "INTRODUCTION TO COMPUTER ORGANISATION AND ARCHITECTURE".Beautify(), "1", "L", Day.Tuesday,"KB301", new TimePeriod(Time.CreateTime_24HourFormat(8,00), Time.CreateTime_24HourFormat(10,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(64,"UECS1013", "INTRODUCTION TO COMPUTER ORGANISATION AND ARCHITECTURE".Beautify(), "1", "L", Day.Friday,"KB300", new TimePeriod(Time.CreateTime_24HourFormat(9,30), Time.CreateTime_24HourFormat(10,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(65,"UECS1013", "INTRODUCTION TO COMPUTER ORGANISATION AND ARCHITECTURE".Beautify(), "1", "T", Day.Tuesday,"KB518", new TimePeriod(Time.CreateTime_24HourFormat(15,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(66,"UECS1013", "INTRODUCTION TO COMPUTER ORGANISATION AND ARCHITECTURE".Beautify(), "2", "T", Day.Thursday,"KB321", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(15,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(67,"UECS1013", "INTRODUCTION TO COMPUTER ORGANISATION AND ARCHITECTURE".Beautify(), "3", "T", Day.Friday,"KB300", new TimePeriod(Time.CreateTime_24HourFormat(10,30), Time.CreateTime_24HourFormat(11,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(68,"UECS1044", "OBJECT-ORIENTED APPLICATION DEVELOPMENT".Beautify(), "1", "L", Day.Thursday,"KB301", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(69,"UECS1044", "OBJECT-ORIENTED APPLICATION DEVELOPMENT".Beautify(), "1", "P", Day.Monday,"KB604", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(70,"UECS1044", "OBJECT-ORIENTED APPLICATION DEVELOPMENT".Beautify(), "2", "P", Day.Monday,"KB606", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,4,6,7,8,9,10,11,12,13,14})  ),
                //choo ( 71-80)
                new Slot(81,"UECS2083", "PROBLEM SOLVING WITH DATA STRUCTURES AND ALGORITHMS".Beautify(), "2", "P", Day.Thursday,"KB606", new TimePeriod(Time.CreateTime_24HourFormat(11,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(82,"UECS2083", "PROBLEM SOLVING WITH DATA STRUCTURES AND ALGORITHMS".Beautify(), "3", "P", Day.Friday,"KB608", new TimePeriod(Time.CreateTime_24HourFormat(8,30), Time.CreateTime_24HourFormat(10,30)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(83,"UECS2103", "OPERATING SYSTEMS".Beautify(), "1", "L", Day.Wednesday,"KB316", new TimePeriod(Time.CreateTime_24HourFormat(12,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(83,"UECS2103", "OPERATING SYSTEMS".Beautify(), "1", "L", Day.Thursday,"KB211", new TimePeriod(Time.CreateTime_24HourFormat(8,00), Time.CreateTime_24HourFormat(10,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(84,"UECS2103", "OPERATING SYSTEMS".Beautify(), "1", "T", Day.Tuesday,"KB321", new TimePeriod(Time.CreateTime_24HourFormat(13,00), Time.CreateTime_24HourFormat(14,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(85,"UECS2103", "OPERATING SYSTEMS".Beautify(), "2", "T", Day.Tuesday,"KB321", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(15,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(86,"UECS2103", "OPERATING SYSTEMS".Beautify(), "3", "T", Day.Thursday,"KB321", new TimePeriod(Time.CreateTime_24HourFormat(13,00), Time.CreateTime_24HourFormat(14,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(87,"UECS2333", "HUMAN COMPUTER INTERACTION DESIGN".Beautify(), "1", "L", Day.Tuesday,"KB316", new TimePeriod(Time.CreateTime_24HourFormat(10,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(87,"UECS2333", "HUMAN COMPUTER INTERACTION DESIGN".Beautify(), "1", "L", Day.Friday,"KB300", new TimePeriod(Time.CreateTime_24HourFormat(8,30), Time.CreateTime_24HourFormat(9,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(88,"UECS2333", "HUMAN COMPUTER INTERACTION DESIGN".Beautify(), "1", "P", Day.Tuesday,"KB606", new TimePeriod(Time.CreateTime_24HourFormat(16,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(89,"UECS2333", "HUMAN COMPUTER INTERACTION DESIGN".Beautify(), "2", "P", Day.Friday,"KB608", new TimePeriod(Time.CreateTime_24HourFormat(10,30), Time.CreateTime_24HourFormat(11,30)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(90,"UECS2333", "HUMAN COMPUTER INTERACTION DESIGN".Beautify(), "3", "P", Day.Friday,"KB608", new TimePeriod(Time.CreateTime_24HourFormat(11,30), Time.CreateTime_24HourFormat(12,30)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                //choo ( 91-100)
                new Slot(101,"UECS3253", "WIRELESS APPLICATION DEVELOPMENT".Beautify(), "1", "P", Day.Thursday,"KB605", new TimePeriod(Time.CreateTime_24HourFormat(13,00), Time.CreateTime_24HourFormat(15,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(102,"UECS3253", "WIRELESS APPLICATION DEVELOPMENT".Beautify(), "2", "P", Day.Friday,"KB605", new TimePeriod(Time.CreateTime_24HourFormat(8,30), Time.CreateTime_24HourFormat(10,30)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(103,"UECS3263", "IOS APPLICATION DEVELOPMENT".Beautify(), "1", "L", Day.Friday,"KB711", new TimePeriod(Time.CreateTime_24HourFormat(14,30), Time.CreateTime_24HourFormat(16,30)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(104,"UECS3263", "IOS APPLICATION DEVELOPMENT".Beautify(), "1", "P", Day.Tuesday,"KB722", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(11,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(105,"UECS3263", "IOS APPLICATION DEVELOPMENT".Beautify(), "2", "P", Day.Wednesday,"KB722", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(106,"UECS3273", "PROGRAMMING WITH GAME ENGINES".Beautify(), "1", "L", Day.Wednesday,"KB300", new TimePeriod(Time.CreateTime_24HourFormat(10,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(107,"UECS3273", "PROGRAMMING WITH GAME ENGINES".Beautify(), "1", "P", Day.Friday,"KB605", new TimePeriod(Time.CreateTime_24HourFormat(14,30), Time.CreateTime_24HourFormat(17,30)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(108,"UECS3273", "PROGRAMMING WITH GAME ENGINES".Beautify(), "2", "P", Day.Wednesday,"KB605", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,6,7,8,9,10,11,12,13,14})  ),
                new Slot(109,"UECS3583", "PROJECT I".Beautify(), "1", "L", Day.Thursday,"KB524", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(16,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(110,"UECS3596", "PROJECT II".Beautify(), "1", "P", Day.Sunday,"To be arranged", new TimePeriod(Time.CreateTime_24HourFormat(7,00), Time.CreateTime_24HourFormat(13,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
            };

            for (int i = 0 ; i < expected.Count ; i++) {
                if (expected[i].Equals(actual[i])) { }
                else {
                    Console.WriteLine("Error occur at Slot with UID of " + expected[i].UID);
                    Console.WriteLine("Expected is : ");
                    Console.WriteLine(expected[i].ToFullString());
                    Console.WriteLine("\n");
                    Console.WriteLine("Actual is : ");
                    Console.WriteLine(actual[i].ToFullString());
                    Console.WriteLine("--------------------------------------------");
                    Assert.Fail();
                }
            }

            Dictionary<int , Slot> GetDictionary(List<Slot> list)
            {
                var result = new Dictionary<int , Slot>();
                foreach (var slot in list) {
                    result.Add(slot.UID , slot);
                }
                return result;
            }
        }
    }
}
