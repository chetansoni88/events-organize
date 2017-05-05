using Core.Models;
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

        public async Task<bool> AddArrangements(List<IArrangement> arrangements)
        {
            try
            {
                if (arrangements != null && arrangements.Count > 0)
                {
                    var e = await FetchById();
                    e.Data.Arrangements.AddRange(arrangements);
                    await Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteArrangements(List<IArrangement> arrangements)
        {
            try
            {
                if (arrangements != null && arrangements.Count > 0)
                {
                    bool update = false;
                    var e = await FetchById();
                    foreach (var ar in arrangements)
                    {
                        var matchedAr = e.Data.Arrangements.Find(a => a.Id.Equals(ar.Id));
                        if (matchedAr != null)
                        {
                            update = true;
                            e.Data.Arrangements.Remove(matchedAr);
                            var ap = new ArrangementProcessor(ar.Id);
                            await ap.Delete();
                        }
                    }
                    if (update)
                        await Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
