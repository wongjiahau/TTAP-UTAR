using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program {
    public static class PermutateV4 {
        public static List<List<Slot>> Run_v2(Slot[] input) {
            if (input == null || input.Length == 0) return null;
            var result = new List<List<Slot>>();
            var partitioned = Partitionize(input);
            var indices = GenerateIndices(partitioned);
            var prototype = new List<Slot>(partitioned.Count);

            var crashedIndexList = new List<KeyValuePair[]>();
            while (true) {
                prototype.Add(partitioned[0][indices[0].Value]);
                for (var i = 1; i < indices.Count; i++) {
                    var current = partitioned[i][indices[i].Value];
                    for (var j = prototype.Count - 1; j >= 0; j--) {
                        if (!prototype[j].IntersectWith(current)) continue;
                        crashedIndexList.Add(new KeyValuePair[2]
                        {new KeyValuePair(j, indices[j].Value), new KeyValuePair(i, indices[i].Value)});
                        goto here;
                    }
                    prototype.Add(current);
                }
                result.Add(new List<Slot>(prototype));
                here:
                do {
                    indices = Increment(indices);
                    if (indices == null) goto final;
                } while (crashedIndexList.Any(x => ContainCrashedIndex(indices, x)));
                prototype.Clear();
            }
            final:
            return result;
        }


        private static bool ContainCrashedIndex(List<BoundedInt> indices, KeyValuePair[] crashedIndex) {
            if (indices[crashedIndex[0].Key].Value == crashedIndex[0].Value
                &&
                indices[crashedIndex[1].Key].Value == crashedIndex[1].Value
                ) return true;
            return false;
        }


        public static List<BoundedInt> Increment(List<BoundedInt> indices) {
            var pointer = indices.Count - 1;
            var x = new List<BoundedInt>(indices);
            while (true) {
                x[pointer].Value++;
                if (x[pointer].Value > x[pointer].UpperLimit) {
                    x[pointer].Value = 0;
                    pointer--;
                    if (pointer < 0) return null;
                }
                else return x;
            }
        }


        public static List<BoundedInt> GenerateIndices(List<List<Slot>> x) {
            var result = new List<BoundedInt>();
            var bi = new BoundedInt();
            bi.Value = 0;
            for (var i = 0; i < x.Count; i++) {
                bi.UpperLimit = x[i].Count - 1;
                result.Add(new BoundedInt(bi));
            }

            return result;
        }

        public static List<List<Slot>> Partitionize(Slot[] input) {
            var result = new List<List<Slot>>();
            var column = new List<Slot>();
            var copy = input.ToList();
            while (copy.Count > 0) {
                column.Add(copy[0]);
                var i = 1;
                while (i < copy.Count) {
                    for (var j = 0; j < column.Count; j++) {
                        if (copy[i].Code != column[j].Code || copy[i].Type != column[j].Type ||
                            copy[i].Number == column[j].Number) {
                            i++;
                            goto there;
                        }
                    }
                    column.Add(copy[i]);
                    copy.RemoveAt(i);

                    there:
                    ;
                }
                result.Add(new List<Slot>(column));
                column.Clear();
                copy.RemoveAt(0);
            }
            return result;
        }

        public struct KeyValuePair {
            public readonly int Key;
            public readonly int Value;

            public KeyValuePair(int key, int value) {
                Key = key;
                Value = value;
            }
        }
    }
}