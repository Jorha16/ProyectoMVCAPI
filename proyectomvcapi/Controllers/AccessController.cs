using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using proyectomvcapi.Models.WS;
using proyectomvcapi.Models;

namespace proyectomvcapi.Controllers
{
    public class AccessController : ApiController
    {
        [HttpGet]
        public Reply HolloWorld()
        {
            Reply Or = new Reply();
            Or.result = 1;
            Or.message = "Hi World";

            return Or;
        }

        [HttpPost]
        
        public Reply Login(AccessViewModel model)
        {
            Reply Or = new Reply();
            Or.result = 0;

            try
            {
                //Contexto, Todo lo que esta creado aqui se destruye aqui (Es un ambito, universo)
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    var lst = db.user.Where(d => d.email == model.email && d.password == model.password && d.idEstatus == 1);

                    if (lst.Count() > 0)
                    {
                        Or.result = 1;
                        //Crear y enviar el toque 32 caracteres no se repiten
                        Or.data = Guid.NewGuid().ToString();

                        user oUser = lst.First();
                        oUser.token = (string)Or.data;
                        db.Entry(oUser).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                    else
                    {
                        Or.message = "Datos Incorrectos";
                    }
                }

               

            }
            catch (Exception ex)
            {
                
                Or.message = "Ocurrio un error, estamos corrigiendo";
            }

            return Or;
        }
    }
}
