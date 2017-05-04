using Core.Data;
using Core.Models;
using System;
using System.Threading.Tasks;

namespace Core.Business
{
    public abstract class ProcessorBase<T> : IProcessor<T> where T : IModel
    {
        protected T Model { get; }

        DataEntityHelper<T> _dataHelper = null;

        protected DataEntityHelper<T> DataHelper
        {
            get
            {
                return _dataHelper;
            }
        }

        public ProcessorBase(Guid id)
        {
            _dataHelper = new DataEntityHelper<T>(id);
        }

        public ProcessorBase(T model)
        {
            Model = model;
            _dataHelper = new DataEntityHelper<T>(model);
        }

        public virtual async Task<IProcessorResult<T>> Create()
        {
            IProcessorResult<T> result;
            var v = Validate();
            if(!v.Success)
            {
                return new ProcessorResult<T>("Validation failed : " + v.Failures[0]);
            }
           
            try
            {
                var save = await _dataHelper.Save();
                if (save != null)
                    result = new ProcessorResult<T>(save);
                else
                    result = new ProcessorResult<T>("Save failed.");
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<T>(ex.Message);
            }
            return result;
        }

        public virtual async Task<int> Delete()
        {
            return await _dataHelper.Delete();
        }

        public virtual async Task<IProcessorResult<T>> FetchById()
        {
            IProcessorResult<T> result;
            try
            {
                var fetch = await _dataHelper.FetchById();
                result = new ProcessorResult<T>(fetch);
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<T>(ex.Message);
            }
            return result;
        }

        public virtual async Task<IProcessorResult<T>> Update()
        {
            var v = Validate();
            if (!v.Success)
            {
                return new ProcessorResult<T>("Validation failed : " + v.Failures[0]);
            }

            IProcessorResult<T> result;
            try
            {
                var save = await _dataHelper.Save();
                if (save != null)
                    result = new ProcessorResult<T>(save);
                else
                    result = new ProcessorResult<T>("Save failed.");
            }
            catch (Exception ex)
            {
                result = new ProcessorResult<T>(ex.Message);
            }
            return result;
        }

        public virtual IValidationResult Validate()
        {
            throw new NotImplementedException();
        }
    }
}
