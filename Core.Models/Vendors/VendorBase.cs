using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class VendorBase : User, IVendor
    {
        public virtual VendorType Type => throw new NotImplementedException();
    }
}
