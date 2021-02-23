using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using proyectomvcapi.Models.WS;
using proyectomvcapi.Models;
using System.Threading.Tasks;
using System.Web;
using System.IO;
using System.Text;

namespace proyectomvcapi.Controllers
{
    public class AnimalController : BaseController
    {

        [HttpPost]
        public Reply Get(SecurityViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            

            try
            {
                using(cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    List<LstAnimalesViewModel> lst = List(db);

                    oR.data = lst;
                    oR.result = 1;

                }

            }catch(Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor, intentelo mas tarde";
            }

            return oR;

        }

        [HttpPost]

        public Reply Add([FromBody]AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No Autorizado";
                return oR;
            }

            //Validaciones

            if (!Validate(model))
            {
                oR.message = error;
                return oR;

            }

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    animal oAnimal = new animal();
                    oAnimal.idState = 1;
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.animal.Add(oAnimal);
                    db.SaveChanges();

                    List<LstAnimalesViewModel> lst = List(db);

                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor, intentelo mas tarde";
            }

            return oR;
        }


        [HttpPost]

        public Reply Edit([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No Autorizado";
                return oR;
            }

            //Validaciones

            if (!Validate(model))
            {
                oR.message = error;
                return oR;

            }

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.idState = 1;
                    oAnimal.name = model.Name;
                    oAnimal.patas = model.Patas;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<LstAnimalesViewModel> lst = List(db);

                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor, intentelo mas tarde";
            }

            return oR;
        }


        [HttpDelete]

        public Reply Delete([FromBody] AnimalViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            if (!Verify(model.token))
            {
                oR.message = "No Autorizado";
                return oR;
            }

            //Validaciones

            try
            {
                using (cursomvcapiEntities db = new cursomvcapiEntities())
                {
                    animal oAnimal = db.animal.Find(model.Id);
                    oAnimal.idState = 2;

                    db.Entry(oAnimal).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    List<LstAnimalesViewModel> lst = List(db);

                    oR.data = lst;
                    oR.result = 1;
                }
            }
            catch (Exception ex)
            {
                oR.message = "Ocurrio un error en el servidor, intentelo mas tarde";
            }

            return oR;
        }

        [HttpPost]

        public async Task<Reply> Photo([FromUri]AnimalPictureViewModel model)
        {
            Reply oR = new Reply();
            oR.result = 0;

            //Subir el archivo a las carpeta por medio de esta ruta.
            string root = HttpContext.Current.Server.MapPath("~/App_Data");
            var provider = new MultipartFormDataStreamProvider(root);

            if (!Verify(model.token))
            {
                oR.message = "No Autorizado";
                return oR;
            }
            //view multipart
            if (!Request.Content.IsMimeMultipartContent())
            {
                oR.message = "No viene imagen";
                return oR;
            }

            //Asegurar que termine el proceso
            await Request.Content.ReadAsMultipartAsync(provider);

            FileInfo fileInfoPicture = null;

            foreach (MultipartFileData fileData in provider.FileData)
            {
                if (fileData.Headers.ContentDisposition.Name.Equals("pictue"))
                    fileInfoPicture = new FileInfo(fileData.LocalFileName);

            }

            if(fileInfoPicture!= null)
            {
                using (FileStream fs = fileInfoPicture.Open(FileMode.Open,FileAccess.Read))
                {
                    byte[] b = new byte[fileInfoPicture.Length];
                    UTF8Encoding temp = new UTF8Encoding(true);
                    while (fs.Read(b, 0, b.Length) > 0);

                    try
                    {

                    }catch(Exception ex)
                    {
                        oR.message = "Intente mas tarde";
                    }
                }


            }

            return oR;
        }


        #region HELPERS

        private bool Validate(AnimalViewModel model)
        {
            if (model.Name == "") 
            {
                error = "El nombre es obligatorio";
                return false;
            }
            return true;
        }

        private List<LstAnimalesViewModel> List(cursomvcapiEntities db)
        {
            List<LstAnimalesViewModel> lst = (from d in db.animal
             where d.idState == 1
             select new LstAnimalesViewModel
             {
                 name = d.name,
                 patas = d.patas
             }).ToList();

            return lst;
        }

        #endregion


    }
}
