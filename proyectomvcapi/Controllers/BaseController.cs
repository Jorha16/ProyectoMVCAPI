using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using proyectomvcapi.Models;

namespace proyectomvcapi.Controllers
{
    public class BaseController : ApiController
    {
        public string error = "";

        public bool Verify(string token)
        {
            using (cursomvcapiEntities db = new cursomvcapiEntities())
            {
                if (db.user.Where(d => d.token == token && d.idEstatus == 1).Count() >0 )
                    return true;
            }

            return false;

        }
    }
}
