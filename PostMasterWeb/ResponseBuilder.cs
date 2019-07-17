using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostMasterWeb
{
    public class ResponseBuilder
    {
        public bool status { get; set; } = true;
        private List<string> errors = new List<string>();

        public void AddError(string error)
        {
            status = false;
            errors.Add(error);
        }

        public IDictionary<string, object> Build()
        {
            var response = new Dictionary<string, object>()
            {
                { "status", status },
                { "errors", errors }
            };

            return response;
        }
    }
}
