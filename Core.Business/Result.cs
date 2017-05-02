using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Core.Business
{

    public class ProcessorResult<T> : IProcessorResult<T>
    {
        public bool Success { get; }

        public T Data { get; }

        public string FailureReason { get; }

        public ProcessorResult(T data)
        {
            Success = true;
            Data = data;
        }

        public ProcessorResult(string message)
        {
            Success = false;
            FailureReason = message;
        }
    }

    public class ValidationResult : IValidationResult
    {
        public bool Success { get { return Failures.Count == 0; } }

        public List<string> Failures { get; }

        public void AddFailure(string failureMessage)
        {
            Failures.Add(failureMessage);
        }

        public ValidationResult()
        {
            Failures = new List<string>();
        }
    }
}
