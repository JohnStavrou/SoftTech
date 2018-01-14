using Microsoft.Azure.Mobile.Server;

namespace softechwebService.DataObjects
{
    public class Student : EntityData
    {
        public string Name { get; set; }
        public bool Lunch { get; set; }
        public bool Dinner { get; set; }
        public bool Free { get; set; }
    }
}