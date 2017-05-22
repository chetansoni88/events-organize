using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models
{
    public class VendorBase : User, IVendor
    {
        public virtual VendorType Type => throw new NotImplementedException();

        public static IVendor GetVendorFromType(VendorType type)
        {
            IVendor e = null;
            switch (type)
            {
                case VendorType.Caterer:
                    e = new Caterer();
                    break;
                case VendorType.Florist:
                    e = new Florist();
                    break;
                case VendorType.Photographer:
                    e = new Photographer();
                    break;
                case VendorType.Videographer:
                    e = new Videographer();
                    break;
                case VendorType.Decor:
                    e = new Decor();
                    break;
                case VendorType.DJ:
                    e = new DJ();
                    break;
                case VendorType.MakeupArtist:
                    e = new MakeupArtist();
                    break;
            }
            return e;
        }

        public VendorBase()
        {
            MinimumTimeRequiredBeforeEvent = 1;
        }

        public virtual decimal MinimumTimeRequiredBeforeEvent { get; set; }
    }
}
