using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class Photographer : VendorBase
    {
        public override VendorType Type => VendorType.Photographer;
    }

    public class Caterer : VendorBase
    {
        public override VendorType Type => VendorType.Caterer;
    }

    public class Florist : VendorBase
    {
        public override VendorType Type => VendorType.Florist;
    }

    public class Videographer : VendorBase
    {
        public override VendorType Type => VendorType.Videographer;
    }
}
