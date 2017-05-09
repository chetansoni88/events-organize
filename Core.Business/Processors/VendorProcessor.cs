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

        public async Task<IProcessorResult<Guid>> Login()
        {
            UserProcessor up = new UserProcessor(Model);
            return await up.Login();
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

        public async override Task<IProcessorResult<IVendor>> Create()
        {
            var up = new UserProcessor(Model);
            var users = await up.GetUserByUsername();
            if (users.Count > 0)
            {
                IProcessorResult<IVendor> result = new ProcessorResult<IVendor>("Username already exists.");
                return result;
            }
            return await base.Create();
        }
    }
}
