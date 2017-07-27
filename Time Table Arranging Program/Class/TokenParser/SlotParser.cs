using System;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Class.TokenParser {
    public class SlotParser {
        public List<Slot> Parse(string input) {
            ITokenStream ts = new TokenStream(Tokenizer.Tokenize(input));
            var finalResult = new List<Slot>();
            var isReadingSubjectName = false;
            var resultSlot = new Slot();
            try {
                while (true) {
                    if (ts.IsAtLastToken()) break;
                    if (TryParseSubjectCode(ts, ref resultSlot)) goto here;
                    if (TryParseSubjectName(ts, ref resultSlot, ref isReadingSubjectName)) goto here;
                    if (TryParseSlotTypeAndUID(ts , ref resultSlot)) goto here;
                    if (TryParseSlotNumber(ts, ref resultSlot)) goto here;                    
                    if (TryParseDay(ts, ref resultSlot)) goto here;
                    if (TryParseStartTime(ts, ref resultSlot)) goto here;
                    if (TryParseEndTime(ts, ref resultSlot)) goto here;
                    if (TryParseWeekAndVenue(ts, ref resultSlot)) {
                        ts.GoToNextToken();
                        if (resultSlot.Venue != "-" && !resultSlot.WeekNumber.IsNull()) ts.GoToNextToken();
                        TryParseLecturerName(ts, ref resultSlot);
                        if (resultSlot.SubjectName == null) goto here;
                        resultSlot.SubjectName = resultSlot.SubjectName.Beautify();
                        if (finalResult.Any(s => s.Equals(resultSlot))) {
                            /*do nothing*/
                        }
                        else {
                            finalResult.Add(resultSlot.GetDuplicate());
                            resultSlot.WeekNumber.Clear();
                        }
                    }
                    here:
                    ts.GoToNextToken();
                }
            }
            catch (Exception e) {
                DialogBox.Show("Error", e.Message + "at token " + ts.CurrentIndex);
            }        
            return finalResult;
        }

        private bool TryParseLecturerName(ITokenStream ts , ref Slot resultSlot) {
            if (ts.CurrentToken().IsPossiblyLecturerName()) {
                var lecturer1Name = GetLecturerName(ts.CurrentToken().Value());
                resultSlot.LecturerName = lecturer1Name;
                while (ts.NextToken().IsPossiblyLecturerName()) {
                    var lecturer2Name = GetLecturerName(ts.NextToken().Value());
                    resultSlot.LecturerName += ", " + lecturer2Name;
                    ts.GoToNextToken();
                }
                return true;
            }
            return false;
            string GetLecturerName(string s)
            {
                //s shall be in the format of 99999(Iqmal), 99999 = id, Iqmal = name
                const int idPart = 0;
                const int namePart = 1;
                return s.Split('(')[namePart]
                    .Replace(")" , "")
                    .Replace("," , "");
            }
        }

        private bool TryParseWeekAndVenue(ITokenStream ts , ref Slot resultSlot) {
            if (!ts.PreviousToken().IsPositiveNumberThatContainDecimalPoint()) return false;
            if (VenueAndWeekNumberExist(ts)) {
                resultSlot.WeekNumber = WeekNumber.Parse(ts.CurrentToken().Value());
                resultSlot.Venue = ts.NextToken().Value();
                return true;
            }
            if (WeekNumberDoesNotExist(ts)) {
                resultSlot.WeekNumber = new NullWeekNumber();
                resultSlot.Venue = ts.CurrentToken().Value();
                return true;
            }
            if (VenueDoesNotExist(ts)) {
                resultSlot.WeekNumber = WeekNumber.Parse(ts.CurrentToken().Value());
                resultSlot.Venue = "-";
                return true;
            }
            //else if both week number and venue does not exist? (will not include code for this first, since not encountered yet)
            return false;
            bool VenueDoesNotExist(ITokenStream tokenSteam)
            {
                return tokenSteam.PreviousToken().IsPositiveNumberThatContainDecimalPoint() &&
                       tokenSteam.NextToken().IsPossiblyLecturerName();
            }
            bool WeekNumberDoesNotExist(ITokenStream tokenStream)
            {
                return tokenStream.CurrentToken().IsPossiblyVenueValue();
            }
            bool VenueAndWeekNumberExist(ITokenStream tokenStream)
            {
                return tokenStream.NextToken().IsPossiblyVenueValue();
            }

        }



        private bool TryParseSubjectCode(ITokenStream ts , ref Slot resultSlot) {
            if (ts.CurrentToken().IsPossiblySubjectCode() && ts.NextToken().Value() == "-") {
                resultSlot.Code = ts.CurrentToken().Value().Split(']')[1];
                return true;
            }
            return false;
        }

        private bool TryParseEndTime(ITokenStream ts , ref Slot resultSlot) {
            if (ts.CurrentToken().IsTime() && ts.PreviousToken().Value() == "-") {
                resultSlot.EndTime = Time.CreateTime_12HourFormat(ts.CurrentToken().Value() , ts.NextToken().Value());
                return true;
            }
            return false;
        }

        private bool TryParseStartTime(ITokenStream ts , ref Slot resultSlot) {
            if (!ts.CurrentToken().IsTime()) return false;
            if (!ts.PreviousToken().IsDay()) return false;
            resultSlot.StartTime = Time.CreateTime_12HourFormat(ts.CurrentToken().Value() , ts.NextToken().Value());
            return true;
        }

        private bool TryParseDay(ITokenStream ts , ref Slot resultSlot) {
            if (ts.CurrentToken().IsDay()) {
                resultSlot.Day = Day.Parse(ts.CurrentToken().Value());
                //resultSlot.Day = new Day( ts.CurrentToken().Value());
                return true;
            }
            return false;
        }

        private bool TryParseSlotTypeAndUID(ITokenStream ts , ref Slot resultSlot) {
            if (ts.CurrentToken().IsSlotType()) {
                resultSlot.Type = ts.CurrentToken().Value();
                resultSlot.UID = int.Parse(ts.PreviousToken().Value());
                return true;
            }
            return false;
        }

        private bool TryParseSlotNumber(ITokenStream ts , ref Slot resultSlot) {
            if (!ts.CurrentToken().IsPositiveInteger()) return false;
            if (!ts.PreviousToken().IsSlotType()) return false;
            resultSlot.Number = ts.CurrentToken().Value();
            return true;
        }

        private bool TryParseSubjectName(ITokenStream ts , ref Slot resultSlot , ref bool isReadingSubjectName) {
            if (isReadingSubjectName) {
                if (ts.CurrentToken().Value().Length != 0 && ts.CurrentToken().Value()[0] == '[') {
                    isReadingSubjectName = false;
                    return true;
                }
                resultSlot.SubjectName += " " + ts.CurrentToken().Value();
                return true;
            }

            if (ts.CurrentToken().Value() == "-" && ts.PreviousToken().IsPossiblySubjectCode()) {
                isReadingSubjectName = true;
                resultSlot.SubjectName = "";
                return true;
            }

            return false;
        }
    }
}