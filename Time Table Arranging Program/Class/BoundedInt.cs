using System;

namespace Time_Table_Arranging_Program.Class {
    public class BoundedInt : IEquatable<BoundedInt> {
        public int UpperLimit;
        public int Value;

        public BoundedInt() {}

        public BoundedInt(BoundedInt x) {
            Value = x.Value;
            UpperLimit = x.UpperLimit;
        }

        public BoundedInt(int upperLimit, int value) {
            UpperLimit = upperLimit;
            Value = value;
        }

        public bool Equals(BoundedInt other) {
            return Value == other.Value && UpperLimit == other.UpperLimit;
        }

        public override string ToString() {
            return $"UpperLimit : {UpperLimit}, Value : {Value}";
        }
    }
}