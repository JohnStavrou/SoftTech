using System.ComponentModel;
using Newtonsoft.Json;

namespace UniFoodWPF.Models
{
    public class Student : INotifyPropertyChanged
    {
        private string _id;
        private string _name;
        private bool _lunch;
        private bool _dinner;
        private bool _free;

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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        public bool Lunch
        {
            get { return _lunch; }
            set
            {
                _lunch = value;
                OnPropertyChanged("Lunch");
            }
        }

        public bool Dinner
        {
            get { return _dinner; }
            set
            {
                _dinner = value;
                OnPropertyChanged("Dinner");
            }
        }

        public bool Free
        {
            get { return _free; }
            set
            {
                _free = value;
                OnPropertyChanged("Free");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}