using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Gif.Service.Crm
{

    [Serializable]
    public class CrmApiException :Exception
    {
        public HttpStatusCode HttpStatus { get; private set; }

        public CrmApiException(string message, HttpStatusCode httpStatus) : base(message)
        {
            HttpStatus = httpStatus;
        }
    }
}
