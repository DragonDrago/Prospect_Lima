using System.Collections.Generic;

namespace Lima.Dictionaries.Api.Domain
{
    public class Response
    {
        public Page Page { get; set; }
        public IEnumerable<object> Result { get; set; }
    }
}
