using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Arrangement : IArrangement
    {
        public IVendor Vendor { get ; set ; }
        public DateTime StartTime { get ; set ; }
        public DateTime EndTime { get ; set ; }
        public ArrangementStatus Status { get ; set ; }
        public Guid Id { get ; set ; }

        public void Clone(IArrangement arrangement)
        {
            Vendor = arrangement.Vendor;
            StartTime = arrangement.StartTime;
            EndTime = arrangement.EndTime;
            Status = arrangement.Status;
            Id = arrangement.Id;
        }
    }
}
