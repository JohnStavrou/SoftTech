using System;
using Microsoft.Azure.Mobile.Server;

namespace softechwebService.DataObjects
{
    public class Shift : EntityData
    {
        public ShiftType ShiftType { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}