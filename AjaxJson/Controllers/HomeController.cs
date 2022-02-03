using AjaxJson.Models;
using AjaxJson.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxJson.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult NewPeople()
        {
            ViewBag.Message = "Nueva Persona";
            TablaPersonaViewModel model = new TablaPersonaViewModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult NewPeople(TablaPersonaViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            Persona opersona = new Persona();
            opersona.Nombre = model.Nombre;
            opersona.Edad = model.Edad;
            
            using (AjaxJsonEntities db = new AjaxJsonEntities())
            {
                db.Personas.Add(opersona);
                db.SaveChanges();
            }
            ViewBag.Message = "Nueva Persona";
            return View("index");
        }
        [HttpGet]
        public PartialViewResult SeccionPrueba()
        {
            var peronsas = new List<Persona>();
            using (AjaxJsonEntities db = new AjaxJsonEntities())
            {
                peronsas = (from d in db.Personas
                       select d).ToList();
            }
            return PartialView("_Prueba", peronsas);
        }
        [HttpPost]
        public JsonResult CrearPersona(Persona persona)
        {
            // utilizamos  una clase base para 
            var resultado = new BaseRespuesta();

            try
            {
                if (persona.Edad < 18)
                {
                    throw new ApplicationException("La persona no puede ser menor de edad");
                }

                //codigo para crear persona
                using (AjaxJsonEntities db = new AjaxJsonEntities())
                {
                    db.Personas.Add(persona);
                    db.SaveChanges();

                }
                resultado.Mensaje = "Persona creada correctamente";
                resultado.Ok = true;
            }
            catch(Exception ex)
            {
                resultado.Ok = false;
                resultado.Mensaje = ex.Message;
            }
            return Json(resultado);
        }
        public class BaseRespuesta
        {
            public bool Ok { get; set; }
            public string Mensaje { get; set; }
        }
        [HttpPost]
        public JsonResult Duplicador(int cantidad)
        {
            var duplicado = cantidad * 2;
            return Json(duplicado);
        }
        
    }
}