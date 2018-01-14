using System;
using System.ComponentModel;
using Newtonsoft.Json;

namespace UniFoodWPF.Models
{
    public class Shift : INotifyPropertyChanged
    {
        private string _id;
        private ShiftType _shiftType;
        private DateTime? _startTime;
        private DateTime? _endTime;

        [JsonProperty("Id")]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public ShiftType ShiftType
        {
            get { return _shiftType; }
            set
            {
                _shiftType = value;
                OnPropertyChanged("ShiftType");
            }
        }

        public DateTime? StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        public DateTime? EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}