﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Proyecto01.DatosGraficos;
using Proyecto01.Models;

namespace Proyecto01.Controllers
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
        //-------------------------------------------------------
        [HttpGet]
        public JsonResult ReportePorcentajeOcupaJson()
        {
            DT_Reporte objDT_Reporte = new DT_Reporte();

            List<ReportePorcentajeOcupa> objLista = objDT_Reporte.RetornarPorcentajeOcup();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteHorasDemandadasJson()
        {
            DT_Reporte objDT_Reporte = new DT_Reporte();

            List<ReporteHorasDemandadas> objLista = objDT_Reporte.RetornarHorasDemandadas();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ReporteDiasUsoJson()
        {
            DT_Reporte objDT_Reporte = new DT_Reporte();

            List<ReporteDiasUso> objLista = objDT_Reporte.RetornarDiasUso();

            return Json(objLista, JsonRequestBehavior.AllowGet);
        }
        //-------------------------------------------------------
    }
}