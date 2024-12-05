using Microsoft.AspNet.Identity;
using Proyecto01.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Proyecto01.Controllers
{
    public class ReservasController : Controller
    {
        private ApplicationDbContext context = new ApplicationDbContext();
       
        
        
        [HttpGet]
        public ActionResult ReservasUsuario(int idSala)
        {
            string userId = User.Identity.GetUserId(); // Obtiene el ID del usuario autenticado
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            ViewBag.UserId = userId;
            ViewBag.SalaID = idSala;
            return View(new Reserva { IdSala = idSala });
        }

        [HttpPost]
        public ActionResult ReservasUsuario(Reserva reservas)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            reservas.IdUsuario = userId; // Asigna el ID del usuario autenticado
            reservas.FechaReserva = reservas.FechaReserva.Date;

            bool SalaOcupada = context.Reservas.Any(r =>
                r.IdSala == reservas.IdSala &&
                r.FechaReserva == reservas.FechaReserva &&
                ((reservas.HoraInicio >= r.HoraInicio && reservas.HoraInicio < r.HoraFin) ||
                 (reservas.HoraFin > r.HoraInicio && reservas.HoraFin <= r.HoraFin)));

            if (SalaOcupada)
            {
                ModelState.AddModelError("", "El horario de reserva es inválido o ya está ocupado, elija otro horario.");
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
        //hace falta tropicalizar con los usuarios indetity
        [HttpGet]
        public ActionResult ReservasPorUsuario(int id)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var reservas = context.Reservas
                .Include(r => r.Sala)
                .Where(r => r.IdUsuario == userId)
                .ToList();

            return View(reservas);
        }

        [HttpGet]
        public ActionResult editarReservaUsuario(int? id)
        {

            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var reserva = context.Reservas.SingleOrDefault(r => r.IdReserva == id && r.IdUsuario == userId);
            if (reserva == null)
                return HttpNotFound();

            return View(reserva);

        }
        //----------------------

        [HttpPost]
        public ActionResult editarReservaUsuario(Reserva reserva)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            bool SalaOcupada = context.Reservas.Any(r =>
                r.IdSala == reserva.IdSala &&
                r.FechaReserva == reserva.FechaReserva &&
                r.IdReserva != reserva.IdReserva &&
                ((reserva.HoraInicio >= r.HoraInicio && reserva.HoraInicio < r.HoraFin) ||
                 (reserva.HoraFin > r.HoraInicio && reserva.HoraFin <= r.HoraFin)));

            if (SalaOcupada)
            {
                ModelState.AddModelError("", "El horario de reserva es inválido o ya está ocupado, elija otro horario.");
                return View(reserva);
            }

            if (ModelState.IsValid)
            {
                context.Entry(reserva).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("ReservasPorUsuario");
            }

            return View(reserva);

        }

        [HttpGet]
        public ActionResult Eliminar(int? id)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var reserva = context.Reservas.SingleOrDefault(r => r.IdReserva == id && r.IdUsuario == userId);
            if (reserva == null)
                return HttpNotFound();

            return View(reserva);
        }

        [HttpPost, ActionName("Eliminar")]
        public ActionResult EliminarConfirmacion(int? id)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var reserva = context.Reservas.SingleOrDefault(r => r.IdReserva == id && r.IdUsuario == userId);
            if (reserva == null)
                return HttpNotFound();

            context.Reservas.Remove(reserva);
            context.SaveChanges();
            return RedirectToAction("ReservasPorUsuario");
        }
        //[AuthorizeAdmin]
        [HttpGet]
        public ActionResult listaReservas()
        {
            var reservas = context.Reservas.Include(r => r.Sala).Include(r => r.Usuario).ToList();
            return View(reservas);
        }

       // [AuthorizeAdmin]---------------------------------------
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
       // [AuthorizeAdmin]
        [HttpPost]
        public ActionResult editarReservaAdmin(Reserva reserva)
        {

            ViewBag.UserId = (int)Session["idUsuario"];
            var SalaOcupda = context.Reservas.Any(r =>
            r.IdSala == reserva.IdSala &&
            r.FechaReserva == reserva.FechaReserva &&
            ((reserva.HoraInicio >= r.HoraInicio && reserva.HoraInicio < r.HoraFin) ||
            (reserva.HoraFin > r.HoraInicio && reserva.HoraFin <= r.HoraFin)));

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
       // [AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXSala(int? salaId)
        {
            if (salaId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar una sala.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.IdSala == salaId).ToList();

            return View("FiltrarReservasXSala", reservas);

        }
        //tema del usuario
        //[AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXUsuario(string? usuarioId)
        {
            if (usuarioId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un usuario.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.IdUsuario == usuarioId).ToList();

            return View("FiltrarReservasXUsuario", reservas);

        }
        //[AuthorizeAdmin]
        [HttpGet]
        public ActionResult FiltrarReservasXFecha(DateTime? fecha)
        {
            if (fecha == null)
            {
                ModelState.AddModelError("", "Debe seleccionar una fecha.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.FechaReserva == fecha).ToList();

            return View("FiltrarReservasXFecha", reservas);

        }
       // [AuthorizeAdmin]
        [HttpGet]
        public ActionResult AgregarReserva()
        {
            ViewBag.Salas = context.salasReunions.ToList();
            ViewBag.Usuarios = context.Users.ToList();
            return View();
        }


        [HttpPost]
        public ActionResult AgregarReserva(Reserva reserva)
        {
            ViewBag.Salas = context.salasReunions.ToList();
            ViewBag.Usuarios = context.Users.ToList();

            var sala = context.salasReunions.FirstOrDefault(s => s.IdSala == reserva.IdSala);
            if (sala == null)
            {
                ModelState.AddModelError("", "La sala seleccionada no existe.");
                return View(reserva);
            }

            if (reserva.HoraInicio < sala.HoraInicio || reserva.HoraInicio >= sala.HoraFin)
            {
                ModelState.AddModelError("", "La hora de inicio de la reserva no está dentro del horario habilitado para esta sala.");
                return View(reserva);
            }

            var existeConflicto = context.Reservas.Any(r =>
                r.IdSala == reserva.IdSala &&
                r.FechaReserva == reserva.FechaReserva &&
                ((r.HoraInicio < reserva.HoraFin && r.HoraInicio >= reserva.HoraInicio) ||
                 (reserva.HoraInicio < r.HoraFin && reserva.HoraInicio >= r.HoraInicio)));

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
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            if (id == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            var reserva = context.Reservas.SingleOrDefault(r => r.IdReserva == id && r.IdUsuario == userId);
            if (reserva == null)
                return HttpNotFound();

            return View(reserva);
        }

        //tema de usuario
        [HttpPost, ActionName("EliminarAdmin")]
        public ActionResult EliminarConfirmacionAdmin(int? id)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var reserva = context.Reservas.SingleOrDefault(r => r.IdReserva == id && r.IdUsuario == userId);
            if (reserva == null)
                return HttpNotFound();

            context.Reservas.Remove(reserva);
            context.SaveChanges();
            return RedirectToAction("ReservasPorUsuario");
        }



    }
}



