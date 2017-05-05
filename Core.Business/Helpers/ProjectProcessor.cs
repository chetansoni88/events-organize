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

        public async Task<bool> AddEvents(List<IEvent> events)
        {
            try
            {
                if (events != null && events.Count > 0)
                {
                    var e = await FetchById();
                    e.Data.Events.AddRange(events);
                    await Update();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteEvents(List<IEvent> events)
        {
            try
            {
                if (events != null && events.Count > 0)
                {
                    var e = await FetchById();
                    bool update = false;
                    foreach (var ar in events)
                    {
                        var matchedAr = e.Data.Events.Find(a => a.Id.Equals(ar.Id));
                        if (matchedAr != null)
                        {
                            update = true;
                            e.Data.Events.Remove(matchedAr);
                            var ap = new EventProcessor(ar.Id);
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
