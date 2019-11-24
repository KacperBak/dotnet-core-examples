using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace HelloAzureFunctions
{
    public class CustomObjectResult : ObjectResult
    {
        /// <summary>
        /// Custom headers
        /// </summary>
        private Dictionary<string, string> _headers;

        public CustomObjectResult(Dictionary<string, string> headers, object value) : base(value)
        {
            _headers = headers;
        }

        public Dictionary<string, string> Headers
        {
            get
            {
                return _headers;
            }
        }

        /// <summary>
        /// Add's custom headers to middleware chain
        /// </summary>
        /// <param name="context"></param>
        public override void OnFormatting(ActionContext context)
        {
            base.OnFormatting(context);
            foreach (KeyValuePair<string, string> kvp in Headers) {
                context.HttpContext.Response.Headers.Add(kvp.Key, kvp.Value);                    
            }
        }


    }
}