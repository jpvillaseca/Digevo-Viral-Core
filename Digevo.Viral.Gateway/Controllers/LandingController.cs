using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Digevo.Viral.Gateway.Controllers
{
    public class LandingController : ApiController
    {
        // POST api/landing
        public void Post([FromBody]string value)
        {
            //TODO: Parsear input para que sea válido
            //TODO: Ejecutar secuencia de triggers para gatillar envío de SMS (si es que hay que hacerlo, IsOneTimeOnly)
            //TODO: Guardar en base de datos los datos del usuario (EF), aunque consiste en la ejecución de un trigger.
        }

    }
}
