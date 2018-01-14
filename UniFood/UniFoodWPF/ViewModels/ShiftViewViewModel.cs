using System.ComponentModel;
using Microsoft.WindowsAzure.MobileServices;
using UniFoodWPF.Models;

namespace UniFoodWPF.ViewModels
{
    public class ShiftViewViewModel : INotifyPropertyChanged
    {
        private MobileServiceCollection<Shift, Shift> _shifts;

        public MobileServiceCollection<Shift, Shift> Shifts
        {
            get { return _shifts; }
            private set
            {
                _shifts = value;
                OnPropertyChanged(nameof(Shifts));
            }
        }

        public ShiftViewViewModel()
        {
            Shifts = App.Shifts;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}