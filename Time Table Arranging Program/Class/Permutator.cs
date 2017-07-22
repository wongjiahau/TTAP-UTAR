using System;
using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Arranging_Program.Class {
    public abstract class IPermutator {
        public List<List<Slot>> Permutate(Slot[] input) {
            if (input == null || input.Length == 0) return null;
            var result = new List<List<Slot>>();
            var partitioned = Partitionize(input);
            var indices = GenerateIndices(partitioned);
            var prototype = new List<Slot>(partitioned.Count);

            var crashedIndexList = new List<Permutator.KeyValuePair[]>();
            while (true) {
                prototype.Add(partitioned[0][indices[0].Value]);
                for (var i = 1 ; i < indices.Count ; i++) {
                    var current = partitioned[i][indices[i].Value];
                    for (var j = prototype.Count - 1 ; j >= 0 ; j--) {
                        if (!IntersectWith(prototype[j],current)) continue;
                        crashedIndexList.Add(new Permutator.KeyValuePair[2]
                            {new Permutator.KeyValuePair(j, indices[j].Value), new Permutator.KeyValuePair(i, indices[i].Value)});
                        goto here;
                    }
                    prototype.Add(current);
                }
                result.Add(new List<Slot>(prototype));
                here:
                do {
                    indices = Increment(indices);
                    if (indices == null) goto final;
                } while (crashedIndexList.Any(x => ContainCrashedIndex(indices , x)));
                prototype.Clear();
            }
            final:
            return result;

        }

        protected abstract bool IntersectWith(Slot slot, Slot current);
        private static bool ContainCrashedIndex(List<BoundedInt> indices , Permutator.KeyValuePair[] crashedIndex) {
            if (indices[crashedIndex[0].Key].Value == crashedIndex[0].Value
                &&
                indices[crashedIndex[1].Key].Value == crashedIndex[1].Value
            ) return true;
            return false;
        }

        public static List<BoundedInt> GenerateIndices<T>(List<List<T>> x) {
            var result = new List<BoundedInt>();
            var bi = new BoundedInt();
            bi.Value = 0;
            for (var i = 0 ; i < x.Count ; i++) {
                bi.UpperLimit = x[i].Count - 1;
                result.Add(new BoundedInt(bi));
            }

            return result;
        }

        public static List<List<T>> Partitionize<T>(T[] input) where T : Slot {
            var result = new List<List<T>>();
            var column = new List<T>();
            var copy = input.ToList();
            while (copy.Count > 0) {
                column.Add(copy[0]);
                var i = 1;
                while (i < copy.Count) {
                    for (var j = 0 ; j < column.Count ; j++) {
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
                result.Add(new List<T>(column));
                column.Clear();
                copy.RemoveAt(0);
            }
            return result;
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

    }

    public class Permutator_WithConsideringWeekNumber : IPermutator{
        protected override bool IntersectWith(Slot a, Slot b) {
            return a.IntersectWith(b);
        }
    }

    public class Permutator_WithoutConsideringWeekNumber : IPermutator {
        protected override bool IntersectWith(Slot a, Slot b) {
            if (a.Code == b.Code && a.Type == b.Type && a.Number != b.Number) return true;
            if (!a.Day.Equals(b.Day)) return false;
            return a.TimePeriod.IntersectWith(b.TimePeriod);

        }
    }
    public static class Permutator {
        public static List<List<Slot>> Run_v2_WithConsideringWeekNumber(Slot[] input) {
            if (input == null || input.Length == 0) return null;
            var result = new List<List<Slot>>();
            var partitioned = Partitionize(input);
            var indices = GenerateIndices(partitioned);
            var prototype = new List<Slot>(partitioned.Count);

            var crashedIndexList = new List<KeyValuePair[]>();
            while (true) {
                prototype.Add(partitioned[0][indices[0].Value]);
                for (var i = 1 ; i < indices.Count ; i++) {
                    var current = partitioned[i][indices[i].Value];
                    for (var j = prototype.Count - 1 ; j >= 0 ; j--) {                        
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
                } while (crashedIndexList.Any(x => ContainCrashedIndex(indices , x)));
                prototype.Clear();
            }
            final:
            return result;
        }
        
        public static List<List<Slot>> Run_v2_withoutConsideringWeekNumber(Slot[] input) {
            if (input == null || input.Length == 0) return null;
            var result = new List<List<Slot>>();
            var partitioned = Partitionize(input);
            var indices = GenerateIndices(partitioned);
            var prototype = new List<Slot>(partitioned.Count);

            var crashedIndexList = new List<KeyValuePair[]>();
            while (true) {
                prototype.Add(partitioned[0][indices[0].Value]);
                for (var i = 1 ; i < indices.Count ; i++) {
                    var current = partitioned[i][indices[i].Value];
                    for (var j = prototype.Count - 1 ; j >= 0 ; j--) {
                        if (!IntersectWithoutConsideringWeekNumber(prototype[j],current)) continue;
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
                } while (crashedIndexList.Any(x => ContainCrashedIndex(indices , x)));
                prototype.Clear();
            }
            final:
            return result;
        }

        private static bool IntersectWithoutConsideringWeekNumber(Slot a , Slot b) {
            if (a.Code == b.Code && a.Type == b.Type && a.Number != b.Number) return true;
            if (!a.Day.Equals(b.Day)) return false;
            return a.TimePeriod.IntersectWith(b.TimePeriod);                        
        }

        [Obsolete("Incomplete yet")]
        public static List<List<Slot>> Run_v3(Slot[] input) {
            if (input == null || input.Length == 0) return null;
            var result = new List<List<Slot>>();
            var indexedSlots = new List<IndexedSlot>();
            for (int i = 0 ; i < input.Length ; i++) {
                indexedSlots.Add(new IndexedSlot(input[i]));
            }
            var partitioned = Partitionize(indexedSlots.ToArray());
            var indices = GenerateIndices(partitioned);
            const int numberOfRows = 7;
            const int numberOfColumns = 16 * 2;
            const int totalLength = numberOfRows * numberOfColumns;
            var prototype = new int[totalLength];
            var mockResult = new List<Slot>();
            while (true) {
                for (var i = 0 ; i < indices.Count ; i++) {
                    var current = partitioned[i][indices[i].Value];
                    for (int j = 0 ; j < current.ColumnSpan ; j++) {
                        int position = current.RowIndex + (current.ColumnIndex + j) * numberOfRows;
                        if (prototype[position] != 0) goto here;
                        prototype[position] = current.UID;
                    }
                }
                result.Add(mockResult);
                here:
                indices = Increment(indices);
                if (indices == null) goto final;
                prototype = new int[totalLength];
            }
            final:
            return result;

        }


        private static bool ContainCrashedIndex(List<BoundedInt> indices , KeyValuePair[] crashedIndex) {
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

        [Obsolete]
        public static void Increment_v2(List<BoundedInt> indices) {
            var pointer = indices.Count - 1;
            while (true) {
                indices[pointer].Value++;
                if (indices[pointer].Value > indices[pointer].UpperLimit) {
                    indices[pointer].Value = 0;
                    pointer--;
                    if (pointer < 0) {
                        indices = null;
                        return;
                    }
                }
                else return;
            }
        }

        public static List<BoundedInt> GenerateIndices<T>(List<List<T>> x) {
            var result = new List<BoundedInt>();
            var bi = new BoundedInt();
            bi.Value = 0;
            for (var i = 0 ; i < x.Count ; i++) {
                bi.UpperLimit = x[i].Count - 1;
                result.Add(new BoundedInt(bi));
            }

            return result;
        }

        public static List<List<T>> Partitionize<T>(T[] input) where T : Slot {
            var result = new List<List<T>>();
            var column = new List<T>();
            var copy = input.ToList();
            while (copy.Count > 0) {
                column.Add(copy[0]);
                var i = 1;
                while (i < copy.Count) {
                    for (var j = 0 ; j < column.Count ; j++) {
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
                result.Add(new List<T>(column));
                column.Clear();
                copy.RemoveAt(0);
            }
            return result;
        }

        public struct KeyValuePair {
            public readonly int Key;
            public readonly int Value;

            public KeyValuePair(int key , int value) {
                Key = key;
                Value = value;
            }
        }
    }
}