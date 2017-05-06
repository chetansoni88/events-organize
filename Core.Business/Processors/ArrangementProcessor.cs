using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class ArrangementProcessor : ProcessorBase<IArrangement>
    {
        public ArrangementProcessor(Guid id) : base(id)
        {

        }

        public ArrangementProcessor(IArrangement arr) : base(arr)
        {

        }


        public override IValidationResult Validate()
        {
            IValidationResult result = new ValidationResult();
            return result;
        }
    }
}
