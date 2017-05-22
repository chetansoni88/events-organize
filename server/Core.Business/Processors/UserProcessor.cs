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

        public async Task<IProcessorResult<Guid>> Login()
        {
            IProcessorResult<Guid> result;
            try
            {
                var users = await DataHelper.FetchQuery(string.Format(@"(Username eq '{0}') and(Password eq '{1}')", Model.Username, Model.Password));
                if (users.Count == 1)
                {
                    IToken t = new Token();
                    t.Id = Guid.NewGuid();
                    t.UserId = users[0].Id;
                    //Set up a token after login is successful
                    var tp = new TokenProcessor(t);
                    await tp.Create();
                    result = new ProcessorResult<Guid>(t.Id);
                }
                else
                {
                    result = new ProcessorResult<Guid>("No user found.");
                }
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<Guid>(ex.Message);
            }
            return result;
        }

        public async Task<List<IUser>> GetUserByUsername()
        {
            return await DataHelper.FetchQuery(string.Format(@"(Username eq '{0}')", Model.Username));
        }

        public async override Task<IProcessorResult<IUser>> Create()
        {
            var users = await GetUserByUsername();
            if (users.Count > 0)
            {
                IProcessorResult<IUser> result = new ProcessorResult<IUser>("Username already exists.");
                return result;
            }
            return await base.Create();
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
