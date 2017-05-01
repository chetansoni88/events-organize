using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Photographer : VendorBase
    {
        public override VendorType Type => VendorType.Photographer;
    }
}
