using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace examAnalysis
{
    public class SubjectEntity:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string Name { set; get; }
        public string  QuestionId{ set; get; }
        public string Id { set; get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
         { 
              PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
          }
        
    }
}
