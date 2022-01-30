using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AjaxJson.Models;
using System.Linq.Dynamic;

namespace AjaxJson.Controllers
{
    public class JsonController : Controller
    {
        // es un true false que indica
        // si se dibujara de nuevo el
        //data table en cada interaccion
        public string draw = "";
        // es la pagina en la que empesara
        public string start = "";
        // es la longitud de la coleccion de datos
        public string length = "";
        // es la columna por la que esta ordenado
        public string sortColumn = "";
        //si es ascendente o descendente
        public string sortColumnDir = "";
        // es si se busca un dato
        public string searchValue = "";
        //pageSize es el numero de registro
        // skip ignorame todos los registro 
        // que esten a la izquierda
        // record total cuantos registros hay
        public int pageSize, skip, recordsTotal;
        // GET: Json
        public ActionResult Index()
        {

                return View();
        }
        [HttpPost]
        public JsonResult Eliminar(int id)
        {
            bool resultado = true;
            if (id >0)
            {
                try
                {
                    Persona persona = new Persona();
                    using (AjaxJsonEntities db = new AjaxJsonEntities())
                    {
                        persona = (from d in db.Personas
                                   select d).Where(d => d.id == id).FirstOrDefault();
                        if (persona != null)
                        {
                            db.Personas.Remove(persona);
                            db.SaveChanges();
                        }
                    }
                }
                catch(Exception ex)
                {
                    resultado = false;
                }
                
            }
            
            
            return Json(new { resultado = resultado }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Guardar(Persona opersona)
        {
            bool respuesta = true;
            try
            {
                if (opersona.id == 0)
                {
                    using (AjaxJsonEntities db = new AjaxJsonEntities())
                    {
                        db.Personas.Add(opersona);
                        db.SaveChanges();
                    }
                }
                else
                {
                    using (AjaxJsonEntities db = new AjaxJsonEntities())
                    {
                        Persona tempPersona =
                         (from d in db.Personas
                          select d).Where(d => d.id == opersona.id).FirstOrDefault();
                        tempPersona.Nombre = opersona.Nombre;
                        tempPersona.Edad = opersona.Edad;
                        db.Entry(tempPersona).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                    }
                }

            }
            catch (Exception ex)
            {
                respuesta = false;
            }
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerPersona(int id)
        {
            Persona persona = new Persona();
            using (AjaxJsonEntities db = new AjaxJsonEntities())
            {
                persona = (from d in db.Personas
                       select d).Where(d => d.id == id).FirstOrDefault();
            }
                return Json( persona, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult ObtenerPersonas()
        {
            List<Persona> lst = new List<Persona>();
            //logistica datatable
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();


            pageSize = length != null ? Convert.ToInt32(length) : 0;
            skip = start != null ? Convert.ToInt32(start) : 0;
            recordsTotal = 0;
            using (AjaxJsonEntities db = new AjaxJsonEntities())
            {
                IQueryable<Persona> query = (from d in db.Personas
                                    select d);
                if(searchValue != "")
                {
                    query = query.Where(x => x.Nombre.Contains(searchValue));
                }
               
                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    query = query.OrderBy(sortColumn + " " + sortColumnDir);
                }
                recordsTotal = query.Count();
                lst = query.Skip(skip).Take(pageSize).ToList();
                
            }
            return Json(
                new {draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = lst }
                 );
        }
    }
}