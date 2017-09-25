using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Model {
    public class SubjectSchema {
        public string SubjectCode = null;
        public bool GotLecture { get; private set; } = false;
        public bool GotTutorial { get; private set; } = false;
        public bool GotPractical { get; private set; } = false;

        /// <summary>
        /// Create new instance of SubjectSchema
        /// </summary>
        /// <param name="slots">Slots must from the same subject</param>
        public SubjectSchema(List<Slot> slots) {
            CheckForCorrectness(slots);
            GenerateSchema(slots);
        }

        private void GenerateSchema(List<Slot> slots) {
            for (int i = 0; i < slots.Count; i++) {
                switch (slots[i].Type) {
                    case "L":
                        GotLecture = true;
                        break;
                    case "T":
                        GotTutorial = true; 
                        break;
                    case "P":
                        GotPractical = true;
                        break;
                }
            }
        }

        private void CheckForCorrectness(List<Slot> slots) {
            SubjectCode = slots[0].Code;
            for (int i = 0 ; i < slots.Count ; i++) {
                if (slots[i].Code != SubjectCode)
                    throw new ArgumentException("Not all of the slots passed in are from the same subject.");
            }
        }
    }
}