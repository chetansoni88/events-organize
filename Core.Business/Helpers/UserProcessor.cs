using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class UserProcessor : ProcessorBase<IUser>
    {
        public UserProcessor(Guid id) : base(id)
        {

        }

        public UserProcessor(IUser user) : base(user)
        {

        }

        public async Task<IProcessorResult<IUser>> Login()
        {
            IProcessorResult<IUser> result;
            try
            {
                var users = await DataHelper.FetchQuery(string.Format(@"(Username eq '{0}') and(Password eq '{1}')", Model.Username, Model.Password));
                if (users.Count == 1)
                {
                    result = new ProcessorResult<IUser>(users[0]);

                    //Set up a token after login is successful
                }
                else
                {
                    result = new ProcessorResult<IUser>("No user found.");
                }
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<IUser>(ex.Message);
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
