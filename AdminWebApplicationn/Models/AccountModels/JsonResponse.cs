using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MomensoBackend.Models.AccountModels
{
    public class JsonResponse
    {
        public bool Succeded { get; set; }
        public string Response { get; set; }

        public JsonResponse() { }

        public JsonResponse(bool succeded, string response)
        {
            Succeded = succeded;
            Response = response;
        }
    }
}
