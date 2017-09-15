using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class {
    public class IndexedSlot : Slot {
        private readonly SlotIndex _slotIndex;
        public IndexedSlot(Slot s) : base(s) {
            _slotIndex = new SlotIndex(s.Day , s.StartTime , s.EndTime.Minus(s.StartTime));
            RowIndex = _slotIndex.RowIndex;
            ColumnIndex = _slotIndex.ColumnIndex;
            ColumnSpan = _slotIndex.ColumnSpan;
        }

        //public int RowIndex => _slotIndex.RowIndex;
        //public int ColumnIndex => _slotIndex.ColumnIndex;
        //public int ColumnSpan => _slotIndex.ColumnSpan;

        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
        public int ColumnSpan { get; private set; }

    }



    public class SlotIndex : IEquatable<SlotIndex>, IIntersectionCheckable<SlotIndex> {
        /// <summary>
        /// This value depends on Global.Constant.MinTime
        /// </summary>
        public int ColumnIndex { get; private set; }

        /// <summary>
        /// One hour of duration means 2 ColumnSpan,
        /// Half hour means 1 ColumnSpan
        /// </summary>
        public int ColumnSpan { get; private set; }

        /// <summary>
        /// RowIndex represent days, 0 means Monday, 6 means Sunday
        /// </summary>
        public int RowIndex { get; private set; }

        public SlotIndex(Day day , ITime startTime , TimeSpan duration) {
            RowIndex = GetRowIndex(day);
            ColumnIndex = GetColumnIndex(startTime);
            ColumnSpan = GetColumnSpan(duration);
        }

        public SlotIndex(int rowIndex , int columnIndex , int columnSpan) {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            ColumnSpan = columnSpan;
        }
        private int GetColumnSpan(TimeSpan time) {
            double result = time.TotalHours * 2;
            return (int)result;
        }

        private int GetRowIndex(Day day) {
            return (day.IntValue - 1);
        }

        private int GetColumnIndex(ITime startTime) {
            var result = 0;
            result = (startTime.Hour - Global.Constant.MinTime) * 2;
            if (startTime.Minute == 30) result++;
            return result;
        }


        public bool Equals(SlotIndex other) {
            Assert.IsTrue(other != null);
            return
            RowIndex == other.RowIndex &&
            ColumnIndex == other.ColumnIndex &&
            ColumnSpan == other.ColumnSpan;
        }

        [Obsolete("Still have bugs")]
        public bool IntersectWith(SlotIndex other) {
            if (RowIndex != other.RowIndex) return false;
            if (ColumnSpan > other.ColumnSpan) {
                for (int i = 0 ; i < ColumnSpan ; i++)
                    if (ColumnIndex + i == other.ColumnIndex)
                        return true;
            }
            else {
                for (int j = 0 ; j < other.ColumnSpan ; j++)
                    if (other.ColumnIndex + j == ColumnIndex)
                        return true;
            }
            return false;
        }

        public override string ToString() {
            return "RowIndex : " + RowIndex + "\n" +
                   "ColumnIndex : " + ColumnIndex + "\n" +
                   "ColumnSpan : " + ColumnSpan + "\n";
        }
    }
}
