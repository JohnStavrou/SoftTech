using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WebEye.Controls.Wpf;
using UniFoodWPF.Models;
using UniFoodWPF.ViewModels;
using ZXing;

namespace UniFoodWPF.Views
{
    public partial class MainWindow
    {
        private Bitmap _image;
        private Thread _thread;
        private readonly IBarcodeReader _reader;
        private StudentViewViewModel _sTvvm;
        private ShiftViewViewModel _sHvvm;

        public MainWindow()
        {
            InitializeComponent();
             
            _reader = new BarcodeReader();

            CapturingDevicesComboBox.ItemsSource = WebCameraControl.GetVideoCaptureDevices();
            if (CapturingDevicesComboBox.Items.Count > 0)
                CapturingDevicesComboBox.SelectedItem = CapturingDevicesComboBox.Items[0];
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StartButton.IsEnabled = e.AddedItems.Count > 0;
        }

        private void OnStartButtonClick(object sender, RoutedEventArgs e)
        {
            if (ShiftLabel.Content == null)
            {
                MessageBox.Show("Παρακαλώ εισάγετε βάρδιες!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                new ShiftSettings(_sHvvm).Show();
                return;
            }

            if (ShiftLabel.Content.ToString() == "")
            {
                MessageBox.Show("Δεν υπάρχει βάρδια αυτή την ώρα!", "Προσοχή", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            WebCameraControl.StartCapture((WebCameraId) CapturingDevicesComboBox.SelectedItem);
            _thread = new Thread(Snap);
            _thread.Start();
            StartButton.IsEnabled = false;
            CapturingDevicesComboBox.IsEnabled = false;
            StopButton.IsEnabled = true;
        }

        private void OnStopButtonClick(object sender, RoutedEventArgs e)
        {
            _thread.Abort();
            WebCameraControl.StopCapture();

            StartButton.IsEnabled = true;
            CapturingDevicesComboBox.IsEnabled = true;
            StopButton.IsEnabled = false;
        }

        private void Snap()
        {
            Capture();
        }

        private void Capture()
        {
            while (true)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _image = WebCameraControl.GetCurrentImage();
                });

                if (_image != null)
                {
                    var result = _reader.Decode(_image);
                    if (result != null)
                    {
                        string str = result.ToString();

                        var student = _sTvvm.Students.FirstOrDefault(x => x.Id == str);
                        if (student == null)
                            MessageBox.Show("Ο χρήστης δε βρέθηκε!", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                        else
                        { 
                            var content = "";
                            Application.Current.Dispatcher.Invoke(() =>
                            {
                                content = ShiftLabel.Content.ToString();
                            });

                            if (content == "Lunch")
                            {
                                if (!student.Lunch)
                                {
                                    if (student.Free)
                                        MessageBox.Show("Ο χρήστης δικαιούται δωρεάν σίτηση!\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                    else
                                        MessageBox.Show("Ο χρήστης ΔΕΝ δικαιούται δωρεάν σίτηση!\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                    student.Lunch = true;
                                }
                                else
                                    MessageBox.Show("Έχετε ήδη καταχωρηθεί για τη μεσημεριανή βάρδια!\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                student.Dinner = false;
                            }
                            else
                            {
                                if (!student.Dinner)
                                {
                                    if (student.Free)
                                        MessageBox.Show("Ο χρήστης δικαιούται δωρεάν σίτηση\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                    else
                                        MessageBox.Show("Ο χρήστης ΔΕΝ δικαιούται δωρεάν σίτηση\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                    student.Dinner = true;
                                }
                                else
                                    MessageBox.Show("Έχετε ήδη καταχωρηθεί για τη βραδινή βάρδια!\nΌνομα φοιτητή/τριας: " + student.Name, "Επιτυχές Σκανάρισμα");
                                student.Lunch = false;
                            }
                        }
                    }
                }
            }
        }

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if(StartButton.IsEnabled)
                    OnStartButtonClick(null, null);
                else
                    OnStopButtonClick(null, null);
            }
        }

        private void SettingsButton_OnClick(object sender, RoutedEventArgs e)
        {
            new ShiftSettings(_sHvvm).Show();
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }

        private void MainWindow_OnInitialized(object sender, EventArgs e)
        {
            _sTvvm = (StudentViewViewModel) DataContext;
            _sHvvm = (ShiftViewViewModel) MainPanel.DataContext;

            if (_sHvvm.Shifts.Count != 0)
            {
                var shift = _sHvvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Lunch);
                if (shift.StartTime.Value.Hour.CompareTo(DateTime.Now.Hour) <= 0 && shift.EndTime.Value.Hour.CompareTo(DateTime.Now.Hour) >= 0)
                {
                    ShiftLabel.Content = "Lunch";
                    return;
                }

                shift = _sHvvm.Shifts.FirstOrDefault(x => x.ShiftType == ShiftType.Dinner);
                if (shift.StartTime.Value.Hour.CompareTo(DateTime.Now.Hour) <= 0 && shift.EndTime.Value.Hour.CompareTo(DateTime.Now.Hour) >= 0)
                {
                    ShiftLabel.Content = "Dinner";
                    return;
                }

                ShiftLabel.Content = "";
            }
        }
    }
}