using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.MVVM_Framework;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;

namespace Time_Table_Arranging_Program.Model {
    public class SubjectModel : ObservableObject, IFocusable {
        public event EventHandler SlotSelectionChanged;
        public SubjectModel() {
            Name = "Testing Subject 123";
            CodeAndNameInitials = "MPU329999";
            Slots = TestData.GetSlotRange(3 , 9);
            IsSelected = true;
        }

        public SubjectModel(string name , string code , int creditHour , List<Slot> slots) {
            Name = name;
            Code = code;
            CodeAndNameInitials = code + " [" + name.GetInitial() + "]";
            CreditHour = creditHour;
            Slots = slots;
        }

        public string Name { get; }
        public string CodeAndNameInitials { get; private set; }
        public string Code { get; }
        public int CreditHour { get; private set; }


        public List<Slot> Slots { get; private set; }

        public List<SubjectModel> ClashingCounterparts { get; private set; } = new List<SubjectModel>();

        public List<Slot> GetSelectedSlots() {
            var result = new List<Slot>();
            for (int i = 0 ; i < Slots.Count ; i++) {
                if (Slots[i].IsSelected)
                    result.Add(Slots[i]);
            }
            return result;
        }

        public void ToggleSlotSelection(int slotUid) {
            foreach (Slot s in Slots) {
                if (s.UID != slotUid) continue;
                s.IsSelected = !s.IsSelected;
            }
            IsAllSlotsSelected = Slots.All(x => x.IsSelected);
            SlotSelectionChanged?.Invoke(this , null);
        }

        public static List<SubjectModel> Parse(List<Slot> slots) {
            var result = new List<SubjectModel>();
            var dic = new Dictionary<string , List<Slot>>();
            foreach (Slot s in slots) {
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code , new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            foreach (KeyValuePair<string , List<Slot>> entry in dic) {
                var v = entry.Value[0];
                result.Add(new SubjectModel(v.SubjectName , v.Code , 0 , entry.Value));
            }
            result = result.OrderBy(o => o.Name).ToList();
            return result;
        }

        public override string ToString() {
            return $"{Name}, IsFocused={IsFocused}, IsSelected={IsSelected}, IsVisible={IsVisible}";
        }

        #region ViewModelProperties

        public event EventHandler Selected;
        public event EventHandler Deselected;
        private bool _isSelected = false;

        public bool IsSelected {
            get => _isSelected;
            set {
                SetProperty(ref _isSelected , value);
                for (int i = 0 ; i < Slots.Count ; i++) {
                    Slots[i].IsSelected = value;
                }
                if (value) {
                    Selected?.Invoke(this , null);
                }
                else Deselected?.Invoke(this , null);
                IsAllSlotsSelected = value;
            }
        }

        public event EventHandler Focused;
        private bool _isFocused;

        public bool IsFocused {
            get => _isFocused;
            set {
                SetProperty(ref _isFocused , value);
                if (value) Focused?.Invoke(this , null);
            }
        }


        private ISupervisor _supervisor;

        public void SetSupervisor(ISupervisor supervisor) {
            _supervisor = supervisor;
        }

        public void FocusMe() {
            _supervisor.FocusMe(this);
        }

        private bool _isVisible = true;

        public bool IsVisible {
            get => _isVisible;
            set { SetProperty(ref _isVisible , value); }
        }

        private string _highlightedText;

        public string HighlightedText {
            get => _highlightedText;
            set { SetProperty(ref _highlightedText , value); }
        }

        private ClashingErrorType _clashingErrorType = ClashingErrorType.NoError;

        public ClashingErrorType ClashingErrorType {
            get => _clashingErrorType;
            private set {
                SetProperty(ref _clashingErrorType , value);
                switch (value) {
                    case ClashingErrorType.NoError:
                        break;
                    case ClashingErrorType.SingleClashingError:
                        IsSelected = false;
                        break;
                    case ClashingErrorType.GroupClashingError:
                        IsSelected = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value) , value , null);
                }
            }
        }

        public ClashReport ClashReport {
            set {
                if (value.GetType() == typeof(NullClashReport)) {
                    ClashingErrorType = value.ClashingErrorType;
                    return;
                }
                NameOfClashingCounterpart = value.ClashingCounterpart?.Name;
                ClashingErrorType = value.ClashingErrorType;
                ClashingCounterparts.Add(value.ClashingCounterpart);
            }
        }

        public string NameOfClashingCounterpart { get; private set; }

        private bool _isAllSlotsSelected = false;
        public bool IsAllSlotsSelected {
            get => _isAllSlotsSelected;
            set => SetProperty(ref _isAllSlotsSelected , value);
        }
        #region Commands

        private ICommand _toggleAllSelectionCommand;
        public ICommand ToggleAllSlotSelectionCommand
            => _toggleAllSelectionCommand ??
               (_toggleAllSelectionCommand = new RelayCommand(() => {
                   if (IsAllSlotsSelected) {
                       foreach (Slot s in Slots) {
                           s.IsSelected = false;
                       }
                   }
                   else {
                       foreach (Slot s in Slots) {
                           s.IsSelected = true;
                       }
                   }
                   IsAllSlotsSelected = !IsAllSlotsSelected;
                   SlotSelectionChanged?.Invoke(this , null);
               }));
        #endregion

        #endregion

    }
}