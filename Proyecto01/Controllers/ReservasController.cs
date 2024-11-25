using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorksSpacesG9.Attributes;

namespace WorksSpacesG9.Controllers
{
    public class ReservasController : Controller
    {
        private WorkSpacesG9Entities context = new WorkSpacesG9Entities();
        [HttpGet]
        public ActionResult ReservasUsuario(int idSala)
        {
            ViewBag.UserId = (int)Session["idUsuario"];
            ViewBag.SalaID = idSala;
            return View(new Reservas { id_sala = idSala });
        }

        [HttpPost]
        public ActionResult ReservasUsuario(Reservas reservas)
        {

            if (Session["idUsuario"] != null)
            {
                reservas.id_usuario = (int)Session["idUsuario"];

            }
            else
            {
                return RedirectToAction("Login", "Login");
            }

            reservas.fecha_reserva = reservas.fecha_reserva.Date;

            var SalaOcupda = context.Reservas.Any(r =>
            r.id_sala == reservas.id_sala &&
            r.fecha_reserva == reservas.fecha_reserva &&
            ((reservas.hora_inicio >= r.hora_inicio && reservas.hora_inicio < r.hora_fin) ||
             (reservas.hora_fin > r.hora_inicio && reservas.hora_fin <= r.hora_fin)));

            if (SalaOcupda)
            {

                ModelState.AddModelError("", "El horario de reserva es invalido o ya esta ocupado, elija otro horario.");
                return View(reservas);

            }
            if (ModelState.IsValid)
            {
                context.Reservas.Add(reservas);
                context.SaveChanges();
                return RedirectToAction("Index", "Home");
            }



            return View(reservas);

        }

        [HttpGet]
        public ActionResult ReservasPorUsuario(int id)
        {
            var usuario = context.Usuarios.Find(id);
            ViewBag.UsuarioNombre = usuario.nombre;

            var reservas = context.Reservas.Where(r => r.id_usuario == id).ToList();
            return View(reservas);

        }

        [HttpGet]
        public ActionResult editarReservaUsuario(int? id)
        {

            ViewBag.UserId = (int)Session["idUsuario"];
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var reserva = context.Reservas.Find(id);
            if (reserva == null)
                return HttpNotFound();
            return View(reserva);


        }

        [HttpPost]
        public ActionResult editarReservaUsuario(Reservas reserva)
        {

            ViewBag.UserId = (int)Session["idUsuario"];
            var SalaOcupda = context.Reservas.Any(r =>
            r.id_sala == reserva.id_sala &&
            r.fecha_reserva == reserva.fecha_reserva &&
            ((reserva.hora_inicio >= r.hora_inicio && reserva.hora_inicio < r.hora_fin) ||
            (reserva.hora_fin > r.hora_inicio && reserva.hora_fin <= r.hora_fin)));

            if (SalaOcupda)
            {

                ModelState.AddModelError("", "El horario de reserva es invalido o ya esta ocupado, elija otro horario.");
                return View(reserva);

            }
            if (ModelState.IsValid)
            {
                context.Entry(reserva).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ReservasPorUsuario", "Reservas", new { id = reserva.id_usuario });
            }
            return View(reserva);

        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            ViewBag.UserId = (int)Session["idUsuario"];
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var reserva = context.Reservas.SingleOrDefault(r => r.id_reserva == id);
            if (reserva == null)
                return HttpNotFound();
            return View(reserva);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmacion(int? id)
        {
            ViewBag.UserId = (int)Session["idUsuario"];
            var reserva = context.Reservas.Find(id);
            var idUsuario = reserva.id_usuario;
            context.Reservas.Remove(reserva);
            context.SaveChanges();
            return RedirectToAction("ReservasPorUsuario", "Reservas", new { id = idUsuario });
        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult listaReservas()
        {
            ViewBag.Salas = context.Salas_reunion.ToList();
            ViewBag.Usuarios = context.Usuarios.ToList();

            var reservasFiltro = context.Reservas.Include(r => r.Salas_reunion).Include(r => r.Usuarios).ToList();

            var reservas = context.Reservas.ToList();
            return View(reservas);
        }

        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult editarReservaAdmin(int? id)
        {

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var reserva = context.Reservas.Find(id);
            if (reserva == null)
                return HttpNotFound();
            return View(reserva);


        }
        [AuthorizeAdmin]
        [HttpPost]
        public ActionResult editarReservaAdmin(Reservas reserva)
        {

            ViewBag.UserId = (int)Session["idUsuario"];
            var SalaOcupda = context.Reservas.Any(r =>
            r.id_sala == reserva.id_sala &&
            r.fecha_reserva == reserva.fecha_reserva &&
            ((reserva.hora_inicio >= r.hora_inicio && reserva.hora_inicio < r.hora_fin) ||
            (reserva.hora_fin > r.hora_inicio && reserva.hora_fin <= r.hora_fin)));

            if (SalaOcupda)
            {

                ModelState.AddModelError("", "El horario de reserva es invalido o ya esta ocupado, elija otro horario.");
                return View(reserva);

            }
            if (ModelState.IsValid)
            {
                context.Entry(reserva).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("listaReservas", "Reservas");
            }
            return View(reserva);

        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXSala(int? salaId)
        {
            if (salaId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar una sala.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.id_sala == salaId).ToList();

            return View("FiltrarReservasXSala", reservas);

        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXUsuario(int? usuarioId)
        {
            if (usuarioId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un usuario.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.id_usuario == usuarioId).ToList();

            return View("FiltrarReservasXUsuario", reservas);

        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXFecha(DateTime? fecha)
        {
            if (fecha == null)
            {
                ModelState.AddModelError("", "Debe seleccionar una fecha.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.fecha_reserva == fecha).ToList();

            return View("FiltrarReservasXFecha", reservas);

        }
        [AuthorizeAdmin]
        [HttpGet]
        public ActionResult AgregarReserva()
        {
            ViewBag.Salas = context.Salas_reunion.ToList();
            ViewBag.Usuarios = context.Usuarios.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult AgregarReserva(Reservas reserva)
        {
            ViewBag.Salas = context.Salas_reunion.ToList();
            ViewBag.Usuarios = context.Usuarios.ToList();

            var sala = context.Salas_reunion.FirstOrDefault(s => s.id_sala == reserva.id_sala);
            if (sala == null)
            {
                ModelState.AddModelError("", "La sala seleccionada no existe.");
                return View(reserva);
            }

            if (reserva.hora_inicio < sala.hora_inicio || reserva.hora_inicio >= sala.hora_fin)
            {
                ModelState.AddModelError("", "La hora de inicio de la reserva no está dentro del horario habilitado para esta sala.");
                return View(reserva);
            }

            var existeConflicto = context.Reservas.Any(r =>
                r.id_sala == reserva.id_sala &&
                r.fecha_reserva == reserva.fecha_reserva &&
                ((r.hora_inicio < reserva.hora_fin && r.hora_inicio >= reserva.hora_inicio) ||
                 (reserva.hora_inicio < r.hora_fin && reserva.hora_inicio >= r.hora_inicio)));

            if (existeConflicto)
            {
                ModelState.AddModelError("", "Ya existe una reserva en este horario para la sala seleccionada.");
                return View(reserva);
            }


            if (ModelState.IsValid)
            {
                context.Reservas.Add(reserva);
                context.SaveChanges();
                return RedirectToAction("listaReservas");
            }

            return View(reserva);
        }

        [HttpGet]
        public ActionResult EliminarAdmin(int? id)
        {
            ViewBag.UserId = (int)Session["idUsuario"];
            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var reserva = context.Reservas.SingleOrDefault(r => r.id_reserva == id);
            if (reserva == null)
                return HttpNotFound();
            return View(reserva);
        }

        [HttpPost, ActionName("EliminarAdmin")]
        public ActionResult EliminarConfirmacionAdmin(int? id)
        {
            ViewBag.UserId = (int)Session["idUsuario"];
            var reserva = context.Reservas.Find(id);
            var idUsuario = reserva.id_usuario;
            context.Reservas.Remove(reserva);
            context.SaveChanges();
            return RedirectToAction("listaReservas", "Reservas");
        }



    }
}



