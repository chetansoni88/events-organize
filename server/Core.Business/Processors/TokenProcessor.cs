using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class TokenProcessor : ProcessorBase<IToken>
    {
        public TokenProcessor(Guid id):base(id)
        {

        }

        public TokenProcessor(IToken token) : base(token)
        {

        }

        public override IValidationResult Validate()
        {
            IValidationResult result = new ValidationResult();
            if (Model.UserId == Guid.Empty)
            {
                result.AddFailure("UserId not provided to the token.");
            }
            return result;
        }
    }
}
