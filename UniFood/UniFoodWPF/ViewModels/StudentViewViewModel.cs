using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices;
using UniFoodWPF.Models;

namespace UniFoodWPF.ViewModels
{
    public class StudentViewViewModel : INotifyPropertyChanged
    {
        private MobileServiceCollection<Student, Student> _students;

        public MobileServiceCollection<Student, Student> Students
        {
            get { return _students; }
            private set
            {
                _students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public StudentViewViewModel()
        {
            Students = App.Students;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}