using System;

namespace LeadChina.PM.Mediatr.Validators
{
    public class MediatrPipelineException : Exception
    {
        public MediatrPipelineException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}