﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public class EventProcessor : ProcessorBase<IEvent>
    {
        public EventProcessor(Guid id) : base(id)
        {

        }

        public EventProcessor(IEvent eveent) : base(eveent)
        {

        }

        public override IValidationResult Validate()
        {
            IValidationResult result = new ValidationResult();
            if (string.IsNullOrEmpty(Model.Name))
            {
                result.AddFailure("Event name cannot be empty.");
            }
            if (string.IsNullOrEmpty(Model.Venue.Street))
            {
                result.AddFailure("Event venue cannot be empty.");
            }

            return result;
        }
    }
}