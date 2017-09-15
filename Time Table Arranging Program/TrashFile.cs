using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.StateSummary;

namespace Time_Table_Arranging_Program {
    class TrashFile {
    }

    public class StateTable_v1 : IStateTable {
        private StateRow[] _stateRows = new StateRow[7];

        public StateTable_v1() {
            for (int i = 0 ; i < _stateRows.Length ; i++) {
                _stateRows[i] = new StateRow();
            }
        }
        public void Add(IndexedSlot s) {
            for (int i = 0 ; i < s.ColumnSpan ; i++) {
                _stateRows[s.RowIndex][s.ColumnIndex + i]++;
            }
        }

        public bool ClashesWithCurrentState(IndexedSlot s) {
            for (int i = 0 ; i < s.ColumnSpan ; i++) {
                if (_stateRows[s.RowIndex][s.ColumnIndex + i] > 0)
                    return true;
            }
            return false;
        }

        public void Reset() {
            _stateRows = new StateRow[7];
            for (int i = 0 ; i < _stateRows.Length ; i++) {
                _stateRows[i] = new StateRow();
            }
        }

        public StateRow this[int index] {
            get { return _stateRows[index]; }
        }

        public IEnumerator<IStateCell> GetEnumerator() {
            throw new NotImplementedException();
        }

        public override string ToString() {
            string result = "\n";
            for (int i = 0 ; i < _stateRows.Length ; i++) {
                for (int j = 0 ; j < StateRow.MaxNumberOfColumn ; j++) {
                    if (_stateRows[i][j] > 0) result += "X";
                    else result += "-";
                }
                result += "\n";
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public string Draw(int maxCount) {
            string result = "\n";
            for (int i = 0 ; i < _stateRows.Length ; i++) {
                for (int j = 0 ; j < StateRow.MaxNumberOfColumn ; j++) {
                    if (_stateRows[i][j] == maxCount) result += "C";
                    else if (_stateRows[i][j] > 0) result += "M";
                    else result += "-";
                }
                result += "\n";
            }

            return result;
        }
    }

    public class StateRow {
        public const int MaxNumberOfColumn = 16 * 2; // 16 hours, 1 column is half hour
        private readonly int[] _columns = new int[MaxNumberOfColumn];


        public StateRow() {
            for (int i = 0 ; i < _columns.Length ; i++) {
                _columns[i] = 0;
            }
        }

        public int this[int index] {
            get { return _columns[index]; }
            set { _columns[index] = value; }
        }


    }

}
