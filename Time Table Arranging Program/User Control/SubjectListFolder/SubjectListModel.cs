using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.User_Control.SubjectListFolder {
    public class SubjectListModel : ObservableObject {
        private List<SubjectModel> _subjectModels;
        public SubjectListModel(List<SubjectModel> subjectModels) {
            _subjectModels = subjectModels;
        }

        #region ViewModelProperties
            

        #endregion

    }
}
