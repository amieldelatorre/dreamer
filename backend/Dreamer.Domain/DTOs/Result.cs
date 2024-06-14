using System.Net;
using Dreamer.Shared.Constants;
using Serilog.Core;

namespace Dreamer.Domain.DTOs
{
    public class Result<T>
    {
        public bool ShouldSerializeItem() => Errors.Count == 0;
        public T? Item { get; set; }

        public bool ShouldSerializeRequestResultStatus() => false;

        public RequestResultStatusTypes RequestResultStatus { get; set; }

        public bool ShouldSerializeErrors() => Errors.Count > 0;
        public readonly Dictionary<string, List<string>> Errors = new();
        public void AddError(string key, string message)
        {
            if (Errors.ContainsKey(key))
                Errors[key].Add(message);
            else
                Errors[key] = [ message ];
        }
    }
}
