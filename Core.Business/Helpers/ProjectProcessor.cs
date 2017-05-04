using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class ProjectProcessor : ProcessorBase<IProject>
    {
        public ProjectProcessor(Guid id) : base(id)
        {

        }

        public ProjectProcessor(IProject user) : base(user)
        {

        }

        public override IValidationResult Validate()
        {
            IValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(Model.Name))
            {
                result.AddFailure("Project name cannot be empty.");
            }
            return result;
        }

    }
}
