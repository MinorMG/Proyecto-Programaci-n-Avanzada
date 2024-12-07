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


        [Authorize]
        [HttpGet]
        public ActionResult ReservasUsuario(int idSala)
        {
            string userId = User.Identity.GetUserId(); 
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            ViewBag.UserId = userId;
            ViewBag.SalaID = idSala;
            return View(new Reserva { IdSala = idSala });
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult ReservasUsuario(Reserva reservas)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            return RedirectToAction("Login", "Account");

            reservas.IdUsuario = userId; 
            reservas.FechaReserva = reservas.FechaReserva.Date;

            var sala = context.salasReunions.FirstOrDefault(s => s.IdSala == reservas.IdSala);
            if (sala == null)
            {
                ModelState.AddModelError("", "La sala especificada no existe.");
                return View(reservas);
            }

            if (reservas.HoraInicio < sala.HoraInicio || reservas.HoraFin > sala.HoraFin)
            {
                ModelState.AddModelError("", $"La reserva debe estar dentro del horario permitido: {sala.HoraInicio} - {sala.HoraFin}.");
                return View(reservas);
            }

            var SalaOcupada = context.Reservas.Any(r =>
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
        [Authorize]
        [HttpGet]
        public ActionResult ReservasPorUsuario(string id)
        {
            ViewBag.UsuarioNombre = User.Identity.GetUserName();
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var reservas = context.Reservas
                .Include(r => r.Sala)
                .Where(r => r.IdUsuario == userId)
                .ToList();

            return View(reservas);
        }
        [Authorize]
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

        [Authorize]
        [HttpPost]
        public ActionResult editarReservaUsuario(Reserva reserva)
        {
            string userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Login", "Account");

            var sala = context.salasReunions.FirstOrDefault(s => s.IdSala == reserva.IdSala);
            if (sala == null)
            {
                ModelState.AddModelError("", "La sala especificada no existe.");
                return View(reserva);
            }

            if (reserva.HoraInicio < sala.HoraInicio || reserva.HoraFin > sala.HoraFin)
            {
                ModelState.AddModelError("", $"La reserva debe estar dentro del horario permitido: {sala.HoraInicio} - {sala.HoraFin}.");
                return View(reserva);
            }

            var SalaOcupada = context.Reservas.Any(r =>
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

        [Authorize]
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
        [Authorize]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult listaReservas()
        {
            ViewBag.Salas = context.salasReunions.ToList();
            ViewBag.Usuarios = context.Users.ToList();

            
            var reservasFiltro = context.Reservas.Include(r => r.Sala).Include(r => r.Usuario).ToList();
            var reservas = context.Reservas.ToList();
            return View(reservas);
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult editarReservaAdmin(Reserva reserva)
        {

            ViewBag.UserId = User.Identity.GetUserId();


            var sala = context.salasReunions.FirstOrDefault(s => s.IdSala == reserva.IdSala);
            if (sala == null)
            {
                ModelState.AddModelError("", "La sala especificada no existe.");
                return View(reserva);
            }

            if (reserva.HoraInicio < sala.HoraInicio || reserva.HoraFin > sala.HoraFin)
            {
                ModelState.AddModelError("", $"La reserva debe estar dentro del horario permitido: {sala.HoraInicio} - {sala.HoraFin}.");
                return View(reserva);
            }

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult FiltrarReservasXUsuario(string usuarioId)
        {
            if (usuarioId == null)
            {
                ModelState.AddModelError("", "Debe seleccionar un usuario.");
                return RedirectToAction("listaReservas");
            }

            var reservas = context.Reservas.Where(r => r.IdUsuario == usuarioId).ToList();

            return View("FiltrarReservasXUsuario", reservas);

        }
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult AgregarReserva()
        {
            ViewBag.Salas = context.salasReunions.ToList();
            ViewBag.Usuarios = context.Users.ToList();
            return View();
        }

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

        [Authorize(Roles = "Admin")]
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
            return RedirectToAction("listaReservas");
        }



    }
}



