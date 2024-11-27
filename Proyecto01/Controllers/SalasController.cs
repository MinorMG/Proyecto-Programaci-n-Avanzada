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
        private Models.salas_reunion context = new salas_reunion();

        
        [HttpGet]
        public ActionResult GestionSalas()
        {
            var salas = context.Salas_reunion.ToList();
            return View(salas);
        }

        [HttpGet]
        public ActionResult SalasDisponibles()
        {
            var horaActual = DateTime.Now.TimeOfDay;
            var fechaActual = DateTime.Today;
            var salasDispobibles = context.Salas_reunion.Where(s => s.hora_inicio <= horaActual && s.hora_fin >= horaActual).
                Where(s => !context.Reservas.Any(r => r.id_sala == s.id_sala && r.fecha_reserva == fechaActual && r.hora_inicio <= horaActual && r.hora_fin > horaActual)).ToList();
            ViewBag.CantidadSalasDisponibles = salasDispobibles.Count();
            return View(salasDispobibles);
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "id_equipamiento", "nombre_equipamiento");
            return View();
        }
        [HttpPost]
        public ActionResult Agregar(Salas_reunion sala, int[] equipamientosIds)
        {

            if (ModelState.IsValid)
            {
                sala.Equipamientos = context.Equipamientos.Where(e => equipamientosIds.Contains(e.id_equipamiento)).ToList();
                context.Salas_reunion.Add(sala);
                context.SaveChanges();
                return RedirectToAction("GestionSalas");

            }
            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "id_equipamiento", "nombre_equipamiento");
            return View(sala);
        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult Editar(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var sala = context.Salas_reunion.Include("Equipamientos").FirstOrDefault(s => s.id_sala == id);
            if (sala == null)
                return HttpNotFound();

            ViewBag.Equipamientos = new SelectList(context.Equipamientos, "id_equipamiento", "nombre_equipamiento");


            return View(sala);
        }
        [AuthorizeAdmin]
        [HttpPost]
        public ActionResult Editar(Salas_reunion sala, int[] equipamientosIds)
        {


            if (ModelState.IsValid)

            {

                var sala2 = context.Salas_reunion.Include("Equipamientos").FirstOrDefault(s => s.id_sala == sala.id_sala);
                if (sala2 == null)
                    return HttpNotFound();

                sala2.nombre = sala.nombre;
                sala2.capacidad = sala.capacidad;
                sala2.ubicacion = sala.ubicacion;
                sala2.hora_inicio = sala.hora_inicio;
                sala2.hora_fin = sala.hora_fin;
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
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult Eliminar(int? id)
        {

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var sala = context.Salas_reunion.SingleOrDefault(c => c.id_sala == id);
            if (sala == null)
                return HttpNotFound();
            return View(sala);
        }
        [AuthorizeAdmin]
        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmacion(int? id)
        {
            ViewBag.IsLoggedIn = User.Identity.IsAuthenticated;


            var sala = context.Salas_reunion.Find(id);
            context.Salas_reunion.Remove(sala);
            context.SaveChanges();
            return RedirectToAction("GestionSalas");
        }

    }

}