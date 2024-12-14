using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//usisngs para graficos
using System.Data.SqlClient;
using System.Data;
using Proyecto01.Models;

namespace Proyecto01.DatosGraficos
{
    public class DT_Reporte
    {
        //orden porcentaje ocupacion / horas + demandadas / Reporte dias con + uso

        public List<ReportePorcentajeOcupa> RetornarPorcentajeOcup()
        {
            List<ReportePorcentajeOcupa> objLista = new List<ReportePorcentajeOcupa>();

            using (SqlConnection oconexion = new SqlConnection("Data Source=DESKTOP-PG0PB0E; Initial Catalog=BDProyecAvanzada; Integrated Security=True"))
            {
                string query = "SP_PORCENTAJE_OCUPACION_POR_SALA";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReportePorcentajeOcupa()
                        {
                            nombreSala = dr["Nombre"].ToString(),
                            porcentajeOcup = double.Parse(dr["PORCENTAJE_OCUPACION"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }


        public List<ReporteHorasDemandadas> RetornarHorasDemandadas()
        {
            List<ReporteHorasDemandadas> objLista = new List<ReporteHorasDemandadas>();

          
            using (SqlConnection oconexion = new SqlConnection("Data Source=DESKTOP-PG0PB0E; Initial Catalog=BDProyecAvanzada; Integrated Security=True"))
            {
                string query = "SP_HORAS_MAS_DEMANDADAS";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteHorasDemandadas()
                        {
                            hora = int.Parse(dr["HORA"].ToString()),
                            numReservas = int.Parse(dr["NUMERO_RESERVAS"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }

        //Prueba-----------------------------------------------------------
        public List<ReporteDiasUso> RetornarDiasUso()
        {
            List<ReporteDiasUso> objLista = new List<ReporteDiasUso>();

            
            using (SqlConnection oconexion = new SqlConnection("Data Source=DESKTOP-PG0PB0E; Initial Catalog=BDProyecAvanzada; Integrated Security=True"))
            {
                string query = "SP_DIAS_MAS_ACTIVOS";

                SqlCommand cmd = new SqlCommand(query, oconexion);
                cmd.CommandType = CommandType.StoredProcedure;

                oconexion.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        objLista.Add(new ReporteDiasUso()
                        {
                            DiaSemana = dr["DIA_SEMANA"].ToString(),
                            NumeroReservas = int.Parse(dr["NUMERO_RESERVAS"].ToString()),
                        });
                    }
                }
            }

            return objLista;
        }


    }
}