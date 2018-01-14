using System.ComponentModel;
using System.Linq;
using System.Windows;
using UniFoodWPF.Models;
using UniFoodWPF.ViewModels;

namespace UniFoodWPF.Views
{
    public partial class ShiftSettings
    {
        private readonly ShiftViewViewModel _svvm;

        public ShiftSettings(ShiftViewViewModel svvm)
        {
            InitializeComponent();

            _svvm = svvm;
            if (_svvm.Shifts.Count != 0)
            {
                LStartTimePicker.Value = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Lunch).StartTime;
                LEndTimePicker.Value = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Lunch).EndTime;
                DStartTimePicker.Value = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Dinner).StartTime;
                DEndTimePicker.Value = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Dinner).EndTime;
            }
        }

        private void ShiftSettings_OnClosing(object sender, CancelEventArgs e)
        {
            if (_svvm.Shifts.Count == 0)
            {
                MessageBox.Show("Παρακαλώ εισάγετε βάρδιες!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                e.Cancel = true;
            }
        }

        private async void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (LStartTimePicker.Value == null
                || LEndTimePicker.Value == null
                || DStartTimePicker.Value == null
                || DEndTimePicker.Value == null)
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα πεδία!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (LStartTimePicker.Value.Value.Hour.CompareTo(LEndTimePicker.Value.Value.Hour) >= 0)
            {
                MessageBox.Show("Η ώρα λήξης της μεσημεριανής βάρδιας πρέπει να είναι μετά την ώρα έναρξης της!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DStartTimePicker.Value.Value.Hour.CompareTo(LEndTimePicker.Value.Value.Hour) <= 0)
            {
                MessageBox.Show("Η ώρα έναρξης της βραδινής βάρδιας πρέπει να είναι μετά την ώρα λήξης της μεσημεριανής!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (DStartTimePicker.Value.Value.Hour.CompareTo(DEndTimePicker.Value.Value.Hour) >= 0)
            {
                MessageBox.Show("Η ώρα λήξης της βραδινής βάρδιας πρέπει να είναι μετά την ώρα έναρξης της!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (_svvm.Shifts.Count == 0)
            {
                Shift shift = new Shift
                {
                    ShiftType = ShiftType.Lunch,
                    StartTime = LStartTimePicker.Value.Value,
                    EndTime = LEndTimePicker.Value.Value
                };
                _svvm.Shifts.Add(shift);
                await (Application.Current as App).SyncShifts.InsertAsync(shift);

                shift = new Shift
                {
                    ShiftType = ShiftType.Dinner,
                    StartTime = DStartTimePicker.Value.Value,
                    EndTime = DEndTimePicker.Value.Value
                };
                _svvm.Shifts.Add(shift);
                await (Application.Current as App).SyncShifts.InsertAsync(shift);
            }
            else
            {
                Shift shift = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Lunch);
                shift.StartTime = LStartTimePicker.Value.Value;
                shift.EndTime = LEndTimePicker.Value.Value;
                await (Application.Current as App).SyncShifts.UpdateAsync(shift);

                shift = _svvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Dinner);
                shift.StartTime = DStartTimePicker.Value.Value;
                shift.EndTime = DEndTimePicker.Value.Value;
                await (Application.Current as App).SyncShifts.UpdateAsync(shift);
            }

            Close();
        }
    }
}