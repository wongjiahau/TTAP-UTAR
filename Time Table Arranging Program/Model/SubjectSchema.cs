using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Model {
    public class SubjectSchema {
        private string _subjectName;
        public string SubjectCode { get; private set; } = null;
        public bool GotLecture { get; private set; } = false;
        public bool GotTutorial { get; private set; } = false;
        public bool GotPractical { get; private set; } = false;

        /// <summary>
        /// Create new instance of SubjectSchema
        /// </summary>
        /// <param name="slots">Slots must from the same subject</param>
        public SubjectSchema(List<Slot> slots) {
            if (slots.Count == 0) return;
            CheckForCorrectness(slots);
            GenerateSchema(slots);
        }

        /// <summary>
        /// To check whether the timetable conforms to this schema
        /// </summary>
        /// <param name="timetable"></param>
        /// <returns>Return null if the timetable does conform this schema
        /// Else return error message
        /// </returns>
        public string Validate(List<Slot> timetable) {
            SubjectSchema schema;   
            if (timetable != null) {
                var matchingSlots = timetable.FindAll(x => x.Code == SubjectCode);
                schema = new SubjectSchema(matchingSlots);
            }
            else {
                schema = new NullSubjectSchema();
            }
            return schema.ConformsTo(this);
        }

        public string ConformsTo(SubjectSchema subjectSchema) {
            string result = "";
            if (subjectSchema.GotLecture && this.GotLecture == false)
                result += $"At least one LECTURE is needed for {subjectSchema._subjectName}.\n";
            if(subjectSchema.GotTutorial && this.GotTutorial == false)
                result += $"At least one TUTORIAL is needed for {subjectSchema._subjectName}.\n";
            if(subjectSchema.GotPractical && this.GotPractical == false) 
                result += $"At least one PRACTICAL is needed for {subjectSchema._subjectName}.\n";
            return result.Length == 0 ? null : result;
        }

        private void GenerateSchema(List<Slot> slots) {
            foreach (Slot s in slots) {
                switch (s.Type) {
                    case "L": GotLecture = true; break;
                    case "T": GotTutorial = true; break;
                    case "P": GotPractical = true; break;
                }
            }
        }

        private void CheckForCorrectness(List<Slot> slots) {
            SubjectCode = slots[0].Code;
            _subjectName = slots[0].SubjectName;
            for (int i = 0 ; i < slots.Count ; i++) {
                if (slots[i].Code != SubjectCode)
                    throw new ArgumentException("Not all of the slots passed in are from the same subject.");
            }
        }
    }

    public class NullSubjectSchema : SubjectSchema {
        public NullSubjectSchema() : base(new List<Slot>()) {
            
        }
    }
}