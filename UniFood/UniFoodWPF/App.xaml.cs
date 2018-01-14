using System;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.WindowsAzure.MobileServices;
using UniFoodWPF.Models;
using UniFoodWPF.Views;

namespace UniFoodWPF
{
    public partial class App : Application
    {
        public MobileServiceClient Client { get; set; }
        public IMobileServiceTable<Shift> SyncShifts { get; set; }
        public IMobileServiceTable<Student> SyncStudents { get; set; }
        public static MobileServiceCollection<Shift, Shift> Shifts { get; set; }
        public static MobileServiceCollection<Student, Student> Students { get; set; }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Init();
            new MainWindow().Show();
        }

        public async Task Init()
        {
            try
            {
                Client = new MobileServiceClient("https://softechweb.azurewebsites.net");

                SyncShifts = Client.GetTable<Shift>();
                await ShiftFetchData();

                SyncStudents = Client.GetTable<Student>();
                await StudentFetchData();
            }
            catch
            {
                Console.WriteLine("Database Connection Error!");
            }
        }

        public async Task ShiftFetchData()
        {
            Shifts = await SyncShifts.ToCollectionAsync();
        }

        public async Task StudentFetchData()
        {
            Students = await SyncStudents.ToCollectionAsync();
            if (Students.Count == 0)
            {
                Student student = new Student
                {
                    Id = "athanbonis",
                    Name = "Αθανάσιος Μπόνης",
                    Free = false,
                    Lunch = false,
                    Dinner = false
                };
                await SyncStudents.InsertAsync(student);
            }
        }
    }
}