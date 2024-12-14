using Proyecto01.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Proyecto01.Controllers
{
    public class SalasController : Controller
    {
        
        private ApplicationDbContext context = new ApplicationDbContext();


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult GestionSalas()
        {
            var salas = context.salasReunions.ToList();
            return View(salas);
        }
        
        [HttpGet]
        public ActionResult SalasDisponibles()
        {
            var horaActual = DateTime.Now.TimeOfDay;
            var fechaActual = DateTime.Today;
            var salasDispobibles = context.salasReunions.Where(s => s.HoraInicio <= horaActual && s.HoraFin >= horaActual).
                Where(s => !context.Reservas.Any(r => r.IdSala == s.IdSala && r.FechaReserva == fechaActual && r.HoraInicio <= horaActual && r.HoraFin > horaActual)).ToList();
            ViewBag.CantidadSalasDisponibles = salasDispobibles.Count();
            return View(salasDispobibles);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Agregar()
        {
            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "IdEquipamiento", "NombreEquipamiento");
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(SalasReunion sala, int[] equipamientosIds)
        {

            if (ModelState.IsValid)
            {
               sala.Equipamientos = context.Equipamientos.Where(e => equipamientosIds.Contains(e.IdEquipamiento)).ToList();
                context.salasReunions.Add(sala);
                context.SaveChanges();
                return RedirectToAction("GestionSalas");

            }
            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "IdEquipamiento", "NombreEquipamiento");
            return View(sala);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var sala = context.salasReunions.Include("Equipamientos").FirstOrDefault(s => s.IdSala == id);
            if (sala == null)
                return HttpNotFound();

            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "IdEquipamiento", "NombreEquipamiento");


            return View(sala);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Editar(SalasReunion sala, int[] equipamientosIds)
        {


            if (ModelState.IsValid)

            {

                var sala2 = context.salasReunions.Include("Equipamientos").FirstOrDefault(s => s.IdSala == sala.IdSala);
                if (sala2 == null)
                    return HttpNotFound();

                sala2.Nombre = sala.Nombre;
                sala2.Capacidad = sala.Capacidad;
                sala2.Ubicacion = sala.Ubicacion;
                sala2.HoraInicio = sala.HoraInicio;
                sala2.HoraFin = sala.HoraFin;
                sala2.Equipamientos.Clear();

                if (equipamientosIds != null)
                {
                    foreach (var equipamiento_id in equipamientosIds)
                    {
                        var equipamiento = context.Equipamientos.Find(equipamiento_id);
                        if (equipamiento != null)
                        {
                            sala2.Equipamientos.Add(equipamiento);
                        }
                    }
                }

                context.SaveChanges();
                return RedirectToAction("GestionSalas");
            }

            return View(sala);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.salasReunions.SingleOrDefault(c => c.IdSala == id);
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmacion(int? id)
        {
            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;


            var sala = context.salasReunions.Find(id);
            context.salasReunions.Remove(sala);
              context.SaveChanges();
            return RedirectToAction("GestionSalas");
        }

    }

}