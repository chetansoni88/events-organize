using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Business
{
    public interface IProcessor<T>
    {
        IValidationResult Validate();
        Task<IProcessorResult<T>> Create();
        Task<IProcessorResult<T>> Update();
        Task<int> Delete();
        Task<IProcessorResult<T>> FetchById();
    }

    public interface ILogger
    {
        Task LogAsync(string message);
    }

    public interface IValidationResult
    {
        bool Success { get; }
        List<string> Failures { get; }
        void AddFailure(string failureMessage);
    }

    public interface IProcessorResult<T>
    {
        bool Success { get; }
        T Data { get; }
        string FailureReason { get; }
    }


}
