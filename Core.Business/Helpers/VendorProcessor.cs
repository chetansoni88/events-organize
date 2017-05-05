using Core.Data;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class VendorProcessor : ProcessorBase<IVendor>
    {
        public VendorProcessor(Guid id) : base(id)
        {

        }

        public VendorProcessor(IVendor user) : base(user)
        {

        }

        public async Task<IProcessorResult<IVendor>> Login()
        {
            IProcessorResult<IVendor> result;
            try
            {
                UserProcessor up = new UserProcessor((IUser)Model);
                var user = await up.Login();

                if (user.Data != null)
                {
                    DataEntityHelper<IVendor> uHelper = new DataEntityHelper<IVendor>(user.Data.Id);
                    IVendor vendor = await uHelper.FetchById();
                    if (vendor != null)
                        result = new ProcessorResult<IVendor>(vendor);
                    else
                    {
                        result = new ProcessorResult<IVendor>("No user found.");
                    }
                }
                else
                {
                    result = new ProcessorResult<IVendor>("No user found.");
                }
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<IVendor>(ex.Message);
            }
            return result;
        }

        public override IValidationResult Validate()
        {
            IValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(Model.Username) || string.IsNullOrEmpty(Model.Password))
            {
                result.AddFailure("Username or password cannot be empty.");
            }
            if (string.IsNullOrEmpty(Model.Name))
            {
                result.AddFailure("Name cannot be empty.");
            }
            if (Model.Contact == null || string.IsNullOrEmpty(Model.Contact.Email))
            {
                result.AddFailure("User email cannot be empty.");
            }
            return result;
        }
    }
}
