using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using necesarios para grafico
using Proyecto01.DatosGraficos;
using Proyecto01.Models;

namespace Proyecto01.Controllers
{
    public class EstadisticasController : Controller
    {
        // GET: Estadisticas
        public ActionResult Graficos()
        {
            return View();
        }

        //orden porcentaje ocupacion / horas + demandadas / Reporte dias con + uso


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