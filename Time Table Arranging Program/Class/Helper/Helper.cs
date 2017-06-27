using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Arranging_Program.Class.Helper {
    public static class Helper {
        public static BitArray ToBitArray(this List<int> input) {
            int length = 32;
            var vector = new BitArray(length);
            for (int i = 0 ; i < input.Count ; i++) {
                vector[input[i]] = true;
            }
            return vector;
        }

        public static BitArray ToBitArray(this int x) {
            string s = Convert.ToString(x , 2); //Convert to binary in a string

            int[] bits = s.PadLeft(8 , '0') // Add 0's from left
                         .Select(c => int.Parse(c.ToString())) // convert each char to int
                         .ToArray(); // Convert IEnumerable from select to Array
            return new BitArray(bits);
        }
        public static int ToInt(this BitArray bitArray) {
            if (bitArray.Length > 32)
                throw new ArgumentException("Argument length shall be at most 32 bits.");

            int[] array = new int[1];
            bitArray.CopyTo(array , 0);
            return array[0];

        }
    }
}
