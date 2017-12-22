using DLL.Negocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cUnidadDAO
    {
        public string GetTable
        { get { return "tUnidad"; } }

        public string GetOrderBy
        { get { return "unidadFuncional ASC"; } }

        public List<cAtributo> AttributesClass(cUnidad unidad)
        {
            List<cAtributo> lista = new List<cAtributo>();
            lista.Add(new cAtributo("idProyecto", unidad.IdProyecto));
            lista.Add(new cAtributo("codUF", unidad.CodigoUF));
            lista.Add(new cAtributo("unidadFuncional", unidad.UnidadFuncional));
            lista.Add(new cAtributo("nroUnidad", unidad.NroUnidad));
            lista.Add(new cAtributo("nivel", unidad.Nivel));
            lista.Add(new cAtributo("ambiente", unidad.Ambiente));
            lista.Add(new cAtributo("supCubierta", unidad.SupCubierta));
            lista.Add(new cAtributo("supSemiDescubierta", unidad.SupSemiDescubierta));
            lista.Add(new cAtributo("supDescubierta", unidad.SupDescubierta));
            lista.Add(new cAtributo("supTotal", unidad.SupTotal));
            lista.Add(new cAtributo("porcentaje", unidad.Porcentaje));
            lista.Add(new cAtributo("precioBaseOriginal", unidad.PrecioBaseOriginal));
            lista.Add(new cAtributo("precioBase", unidad.PrecioBase));
            lista.Add(new cAtributo("idEstado", unidad.IdEstado));
            lista.Add(new cAtributo("moneda", unidad.Moneda));
            lista.Add(new cAtributo("papelera", unidad.Papelera));
            lista.Add(new cAtributo("idUsuario", unidad.IdUsuario));
            return lista;
        }

        public int Save(cUnidad unidad)
        {
            if (string.IsNullOrEmpty(unidad.Id))
                return cDataBase.GetInstance().InsertObject(GetTable, AttributesClass(unidad));
            else
                return cDataBase.GetInstance().UpdateObject(unidad.Id, GetTable, AttributesClass(unidad));
        }

        public cUnidad Load(string id)
        {
            Hashtable atributos = cDataBase.GetInstance().LoadObject(id, GetTable);
            cUnidad unidad = new cUnidad();
            unidad.Id = Convert.ToString(atributos["id"]);
            unidad.IdProyecto = Convert.ToString(atributos["idProyecto"]);
            unidad.CodigoUF = Convert.ToString(atributos["codUF"]);
            unidad.UnidadFuncional = Convert.ToString(atributos["unidadFuncional"]);
            unidad.NroUnidad = Convert.ToString(atributos["nroUnidad"]);
            unidad.Nivel = Convert.ToString(atributos["nivel"]);
            unidad.Ambiente = Convert.ToString(atributos["ambiente"]);
            unidad.SupCubierta = Convert.ToString(atributos["supCubierta"]);
            unidad.SupSemiDescubierta = Convert.ToString(atributos["supSemiDescubierta"]);
            unidad.SupDescubierta = Convert.ToString(atributos["supDescubierta"]);
            unidad.SupTotal = Convert.ToString(atributos["supTotal"]);
            unidad.Porcentaje = Convert.ToDecimal(atributos["porcentaje"]);
            unidad.PrecioBaseOriginal = Convert.ToDecimal(atributos["precioBaseOriginal"]);
            unidad.PrecioBase = Convert.ToDecimal(atributos["precioBase"]);
            unidad.IdEstado = Convert.ToString(atributos["idEstado"]);
            unidad.Moneda = Convert.ToString(atributos["moneda"]);
            unidad.Papelera = Convert.ToInt16(atributos["papelera"]);
            unidad.IdUsuario = Convert.ToString(atributos["idUsuario"]);
            return unidad;
        }

        public ArrayList LoadTable(string _idProyecto)
        {
            ArrayList clientes = new ArrayList();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto =" + _idProyecto + " ORDER BY " + GetOrderBy;
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                clientes.Add(Load(Convert.ToString(idList[i])));
            }
            return clientes;
        }

        public cUnidad LoadByCodUF(string _codUF, string _idProyecto)
        {
            cUnidad unidad = new cUnidad();
            string query = "SELECT id FROM " + GetTable + " WHERE codUF='" + _codUF + "' AND idProyecto ='" + _idProyecto + "' AND papelera = " + (Int16)papelera.Activo + " ORDER BY " + GetOrderBy;
            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            unidad = Load(idList);
            return unidad;
        }

        public cUnidad LoadByIdEmpresaUnidad(string _idEmpresaUnidad)
        {
            cUnidad unidad = new cUnidad();
            string query = "SELECT u.id FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad WHERE eu.id ='" + _idEmpresaUnidad + "'";
            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            unidad = Load(idList);
            return unidad;
        }

        public List<cUnidad> GetUnidadesByIdProyecto(string idProyecto)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto= " + idProyecto + " AND papelera = " + (Int16)papelera.Activo + " Order by unidadFuncional, ";
            query += "CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END, ";
            query += "CASE WHEN ISNUMERIC(nroUnidad) = 1 THEN RIGHT( '0000000000' + nroUnidad, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nroUnidad, 10 ) END ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public List<cUnidad> GetUnidadesByIdProyectoSinUnidadesModificadas(string idProyecto)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto= " + idProyecto + " AND idEstado <> " + (Int16)estadoUnidad.Modificado + " AND papelera = " + (Int16)papelera.Activo + " Order by unidadFuncional, ";
            //query += "CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END, ";
            query += "CASE WHEN ISNUMERIC(codUF) = 1 THEN RIGHT( '0000000000' + codUF, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + codUF, 10 ) END, ";
            query += "CASE WHEN ISNUMERIC(nroUnidad) = 1 THEN RIGHT( '0000000000' + nroUnidad, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nroUnidad, 10 ) END ASC";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public List<cUnidad> GetUnidadesByIdEmpresa(string _idEmpresa)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT * FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad ";
            query += " WHERE u.idEstado <> 0 AND u.papelera = 1 AND eu.idEmpresa = " + _idEmpresa + "AND eu.papelera='" + (Int16)papelera.Activo + "' ORDER BY u.codUF asc";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public List<cUnidad> Search(string _filtro, string _idProyecto, string _idEstado, string _idUnidad, string _idAmbiente, string _superficie, string _supMin, string _supMax, string _precioMin, string _precioMax)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string blanco = " ";
            string query = "SELECT * FROM tUnidad WHERE idProyecto= " + _idProyecto + " AND papelera=1";

            if (_idEstado != "0")
            {
                if (_idEstado != "4")
                {
                    if (_idEstado == Convert.ToString((Int16)estadoUnidad.Vendido))
                        query += blanco + "AND idEstado=" + _idEstado + " OR " + " idEstado=" + (Int16)estadoUnidad.Socios;
                    else
                        query += blanco + "AND idEstado=" + _idEstado;
                }
                else
                {
                    query += blanco + "AND (idEstado=" + (Int16)estadoUnidad.Disponible + " OR " + " idEstado=" + (Int16)estadoUnidad.Reservado + ")";
                }
            }

            if (_idUnidad != "0")
            {
                query += blanco + "AND unidadFuncional= '" + cCampoGenerico.Load(_idUnidad, Tablas.tTipoUnidad).Descripcion + "'";
            }

            if (_idAmbiente != "Todos")
                query += blanco + "AND ambiente= '" + _idAmbiente + "'";

            if (!string.IsNullOrEmpty(_supMin) && !string.IsNullOrEmpty(_supMax))
            {
                query += blanco + "AND " + _superficie + " BETWEEN '" + _supMin + "' AND '" + _supMax + "' ";
            }

            if (!string.IsNullOrEmpty(_precioMin) && !string.IsNullOrEmpty(_precioMax))
                query += blanco + "AND precioBase BETWEEN '" + Convert.ToDecimal(_precioMin) + "' AND '" + Convert.ToDecimal(_precioMax) + "'";

            if (_filtro != "0")
            {
                switch (_filtro)
                {
                    case "0":
                        query += blanco + "Order by unidadFuncional";
                        break;
                    case "1":
                        query += blanco + "order by (CASE WHEN ISNUMERIC(" + _superficie + ") = 1 THEN RIGHT( '0000000000' +" + _superficie + ", 10 ) ELSE RIGHT( 'AAAAAAAAAA' + " + _superficie + ", 10 ) END) ASC, CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END ";
                        break;
                    case "2":
                        query += blanco + "order by (CASE WHEN ISNUMERIC(" + _superficie + ") = 1 THEN RIGHT( '0000000000' +" + _superficie + ", 10 ) ELSE RIGHT( 'AAAAAAAAAA' + " + _superficie + ", 10 ) END) DESC, CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END ";
                        break;
                    case "3":
                        query += blanco + "order by precioBase ASC";
                        break;
                    case "4":
                        query += blanco + "order by precioBase DESC";
                        break;
                }
            }else
                query += " AND idEstado <> '5' Order by unidadFuncional, CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END, CASE WHEN ISNUMERIC(nroUnidad) = 1 THEN RIGHT( '0000000000' + nroUnidad, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nroUnidad, 10 ) END ASC";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public List<cUnidad> GetUnidadesByRango(string nivel, string nivelHasta, string idProyecto)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + idProyecto + " AND papelera = " + (Int16)papelera.Activo + " AND idEstado = " + (Int16)estadoUnidad.Disponible + "AND (nivel = '" + nivel + "' OR nivel = '" + nivelHasta + "')";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public DataTable SearchExport(string _filtro, string _idProyecto, string _idEstado, string _superficie, string _supMin, string _supMax, string _precioMin, string _precioMax)
        {
            string blanco = " ";
            string query = "SELECT codUF, unidadFuncional, nivel, nroUnidad, ambiente, supCubierta, supSemiDescubierta, supDescubierta, supTotal, porcentaje, moneda, precioBase, idEstado FROM tUnidad WHERE idProyecto= " + _idProyecto + " AND papelera=1";

            if (_idEstado != "0")
                query += blanco + "AND idEstado=" + _idEstado;

            if (!string.IsNullOrEmpty(_supMin) && !string.IsNullOrEmpty(_supMax))
                query += blanco + "AND " + _superficie + " BETWEEN '" + _supMin + "' AND '" + _supMax + "' ";

            if (!string.IsNullOrEmpty(_precioMin) && !string.IsNullOrEmpty(_precioMax))
                query += blanco + "AND precioBase BETWEEN '" + _precioMin + "' AND '" + _precioMax + "'";

            if (_filtro != "0")
            {
                switch (_filtro)
                {
                    case "0":
                        query += blanco + "Order by unidadFuncional";
                        break;
                    case "1":
                        query += blanco + "order by " + _superficie + " ASC";
                        break;
                    case "2":
                        query += blanco + "order by " + _superficie + " DESC";
                        break;
                    case "3":
                        query += blanco + "order by precioBase ASC";
                        break;
                    case "4":
                        query += blanco + "order by precioBase DESC";
                        break;
                }
            }

            SqlCommand com = new SqlCommand();
            com.CommandText = query.ToString();
            return cDataBase.GetInstance().GetDataReader(com);
        }

        public ArrayList LoadNivelByIdProyecto(string _idProyecto, string _unidadFuncional)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT id, nivel FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND idEstado='" + (Int16)estadoUnidad.Disponible + "' AND papelera = 1";
            query += " AND unidadFuncional = '" + _unidadFuncional + "'";
            query += " ORDER BY CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public ArrayList LoadNroUnidadByIdProyectoCC(string _idProyecto, string _nivel)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT id, nroUnidad FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND idEstado='" + (Int16)estadoUnidad.Disponible + "' AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' ORDER BY nroUnidad";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public ArrayList GetNroUnidadByIdProyecto(string _idProyecto, string _nivel)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT nroUnidad FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND (idEstado='" + (Int16)estadoUnidad.Disponible + "' OR idEstado='" + (Int16)estadoUnidad.Modificado + "') AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY nroUnidad ORDER BY nroUnidad";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        public ArrayList GetNroUnidadMotivoByIdProyecto(string _idProyecto, string _nivel)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT nroUnidad FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY nroUnidad ORDER BY nroUnidad";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        public ArrayList GetNroUnidadReservadaByIdProyecto(string _idProyecto, string _nivel, string _idEmpresa)
        {
            ArrayList unidades = new ArrayList();
            /*string query = "SELECT nroUnidad FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND idEstado='" + (Int16)estadoUnidad.Reservado + "' AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY nroUnidad ORDER BY nroUnidad";*/


            string query = "SELECT u.nroUnidad FROM tUnidad u INNER JOIN tReserva r ON u.id=r.idUnidad WHERE u.idProyecto = " + _idProyecto + " AND (u.idEstado='" + (Int16)estadoUnidad.Reservado + "' OR u.idEstado ='" + (Int16)estadoUnidad.Disponible + "')";
            query += " AND u.papelera = 1 AND u.nivel = '" + _nivel + "' AND r.idEmpresa= '" + _idEmpresa + "' GROUP BY u.nroUnidad ORDER BY u.nroUnidad";



            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        //CAMBIAR POR ArrayList GetNroUnidadByIdProyecto
        public List<cUnidad> GetNroUnidadByIdProyecto1(string _idProyecto, string _nivel)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND (idEstado='" + (Int16)estadoUnidad.Disponible + "' OR idEstado='" + (Int16)estadoUnidad.Modificado + "') AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY id ORDER BY id";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public List<cUnidad> GetNroUnidadReservadoByIdProyecto(string _idProyecto, string _nivel)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND idEstado='" + (Int16)estadoUnidad.Reservado + "' AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY id ORDER BY id";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public ArrayList GroupByUnidadFuncional(string _idProyecto)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT unidadFuncional FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND papelera = 1 GROUP BY unidadFuncional";
            query += " order by CASE WHEN ISNUMERIC(unidadFuncional) = 1 THEN RIGHT( '0000000000' + unidadFuncional, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + unidadFuncional, 10 ) END";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        //Agrupa los niveles de una obra
        public ArrayList GroupByNivel(string _idProyecto)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT nivel FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND papelera = 1 GROUP BY nivel";
            query += " order by CASE WHEN ISNUMERIC(nivel) = 1 THEN RIGHT( '0000000000' + nivel, 10 ) ELSE RIGHT( 'AAAAAAAAAA' + nivel, 10 ) END";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }

        public ArrayList GetTotalesByMoneda(string _idProyecto)
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT SUM(precioBase) FROM tUnidad WHERE idProyecto = " + _idProyecto + " GROUP BY moneda;";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public string GetMonedaByProyecto(string _idProyecto)
        {
            string query = "SELECT Top(1)moneda FROM tUnidad WHERE idProyecto='" + _idProyecto + "' AND idEstado='" + (Int16)estadoUnidad.Disponible + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));       
        }

        public cUnidad GetUnidadByProyecto(string _idProyecto, string _nivel, string _nroUnidad)
        {
            cUnidad unidades = new cUnidad();

            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND nivel = '" + _nivel + "' AND nroUnidad = '" + _nroUnidad + "'";

            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            unidades = Load(idList);
            return unidades;
        }

        public cUnidad GetUnidadIguales(string _idProyecto, string _nivel, string _nroUnidad, string _codUF)
        {
            cUnidad unidades = new cUnidad();

            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND nivel = '" + _nivel + "' AND nroUnidad = '" + _nroUnidad + "' AND codUF <>'" + _codUF + "'";

            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            unidades = Load(idList);
            return unidades;
        }

        public List<cUnidad> GetListUnidadByProyecto(string _idProyecto, string _nivel, string _nroUnidad)
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND nivel = '" + _nivel + "' AND nroUnidad = '" + _nroUnidad + "' AND papelera = 1";
            query += " AND nivel = '" + _nivel + "' GROUP BY id ORDER BY id";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public cUnidad GetUnidadByProyectoAndUF(string _idProyecto, string _nroUnidad)
        {
            cUnidad unidades = new cUnidad();

            string query = "SELECT id FROM " + GetTable + " WHERE idProyecto = " + _idProyecto + " AND codUF = '" + _nroUnidad + "'";

            SqlCommand com = new SqlCommand(query);
            string idList = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(com));
            if (idList == null) return null;
            unidades = Load(idList);
            return unidades;
        }

        public string GetEstadoUnidadesPorProyecto(string _idProyecto)
        {
            string cant = null;
            int count = 0;
            string query = "SELECT  u.idEstado, COUNT(u.id) FROM tProyecto p INNER JOIN tUnidad u ON p.id = u.idProyecto WHERE p.id=" + _idProyecto + " GROUP BY p.descripcion, u.idEstado ORDER BY u.idEstado ASC;";

            SqlCommand com = new SqlCommand(query);
            DataTable dt = cDataBase.GetInstance().GetDataReader(com);

            foreach (DataRow dr in dt.Rows)
            {
                if (dt.Rows.Count == 3)
                    cant += count != 2 ? Convert.ToString(dr[1].ToString()) + ", " : Convert.ToString(dr[1].ToString());

                //if (!string.IsNullOrEmpty(dr[0].ToString()))
                //    cant += count != 2 ? Convert.ToString(dr[1].ToString()) + ", " : Convert.ToString(dr[1].ToString());
                //else
                //    cant += count != 2 ? "0" + ", " : "0";
                //count++;
                //    }
                //    else { 

                //    }
            }


            //    cant += i!=2 ? Convert.ToString(idList[i]) + ", " : Convert.ToString(idList[i]);

            return cant;
        }

        public string GetClienteByUnidad(string _idUnidad)
        {
            string query = "SELECT e.nombre FROM tCuentaCorriente cc INNER JOIN tUnidad u ON cc.idUnidad = u.id INNER JOIN tEmpresa e ON cc.idEmpresa = e.id WHERE u.id = " + _idUnidad;
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public List<cUnidad> GetUnidadByIdOV(string _idOperacionVenta)
        {
            List<cUnidad> cc = new List<cUnidad>();
            string query = "SELECT u.id FROM tEmpresaUnidad eu INNER JOIN tUnidad u ON eu.idUnidad = u.id WHERE eu.idOv='" + _idOperacionVenta + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        /*public ArrayList GetTotalSupTotalDisponibles()
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT u.supTotal FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE u.idEstado = '1' OR u.idEstado = '2' OR u.idEstado = '7' OR u.idEstado = '8' AND p.papelera = '1'";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }*/



        public DateTime GetFechaBoletoByUnidad(string _idUnidad)
        {
            string query = "SELECT o.fecha FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad INNER JOIN tOperacionVenta o ON o.idEmpresaUnidad=eu.id ";
            query += " WHERE u.id='" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDateTime(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public string GetFechaPosesionByUnidad(string _idUnidad)
        {
            string query = "SELECT o.fechaPosesion FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad INNER JOIN tOperacionVenta o ON o.idEmpresaUnidad=eu.id ";
            query += " WHERE u.id='" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            string result = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));

            if (!string.IsNullOrEmpty(result))
                return Convert.ToDateTime(result).ToString("dd/MM/yyyy");
            else
                return "-";
        }

        public string GetFechaEscrituraByUnidad(string _idUnidad)
        {
            string query = "SELECT o.fechaEscritura FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id = eu.idUnidad INNER JOIN tOperacionVenta o ON o.idEmpresaUnidad=eu.id ";
            query += " WHERE u.id='" + _idUnidad + "'";
            SqlCommand cmd = new SqlCommand(query);
            string result = Convert.ToString(cDataBase.GetInstance().ExecuteScalar(cmd));

            if (!string.IsNullOrEmpty(result))
                return Convert.ToDateTime(result).ToString("dd/MM/yyyy");
            else
                return "-";
        }

        public decimal GetTotalSupTotal(Int16 _idEstado)
        {
            /*string query = "SELECT SUM(u.supTotal) FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE p.papelera = '" + (Int16)estadoUnidad.Disponible + "'";

            if(_idEstado != -1)
                query += " AND u.idEstado = '" + _idEstado + "'";

            SqlCommand cmd = new SqlCommand(query);
            string r = cDataBase.GetInstance().ExecuteScalar(cmd);
            if (!string.IsNullOrEmpty(r))
                return Convert.ToDecimal(r);
            else
                return Convert.ToDecimal("0");*/

            ArrayList unidades = new ArrayList();
            string query = "SELECT u.supTotal FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE u.papelera = '1' AND p.id <> '50' AND p.papelera = '" + (Int16)estadoUnidad.Disponible + "'";

            if (_idEstado != -1)
                query += " AND u.idEstado = '" + _idEstado + "'";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            decimal sum = 0;
            foreach (var a in unidades)
            {
                sum = sum + Convert.ToDecimal(a);
            }

            return sum;
        }

        /*public ArrayList GetTotalSupTotalVendidos()
        {
            ArrayList unidades = new ArrayList();
            string query = "SELECT u.supTotal FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE u.idEstado = '3' AND p.papelera = '1'";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            return unidades;
        }*/

        public List<cUnidad> GetUnidadesDisponibles()
        {
            List<cUnidad> unidades = new List<cUnidad>();
            string query = "SELECT id FROM tUnidad WHERE idEstado = '1' OR idEstado = '2' OR idEstado = '7' OR idEstado = '8'";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public decimal GetTotalSupByProyecto(Int16 _idEstado, string _idProyecto)
        {
            /*string query = "SELECT SUM(u.supTotal) FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE u.papelera = '1' AND p.papelera = '1' AND u.idProyecto = '" + _idProyecto + "'";

            if(_idEstado != -1)
                query += " AND u.idEstado = '" + _idEstado + "'";

            SqlCommand cmd = new SqlCommand(query);
            string r = cDataBase.GetInstance().ExecuteScalar(cmd);
            if(!string.IsNullOrEmpty(r))
                return Convert.ToDecimal(r);
            else
                return Convert.ToDecimal("0");*/

            ArrayList unidades = new ArrayList();
            string query = "SELECT u.supTotal FROM tUnidad u INNER JOIN tProyecto p ON u.idProyecto = p.id WHERE u.papelera = '1' AND p.papelera = '1' AND u.idProyecto = '" + _idProyecto + "'";

            if (_idEstado != -1)
                query += " AND u.idEstado = '" + _idEstado + "'";

            SqlCommand com = new SqlCommand(query);
            unidades = cDataBase.GetInstance().ExecuteReader(com);

            decimal sum = 0;
            foreach (var a in unidades)
            {
                sum = sum + Convert.ToDecimal(a);
            }

            return sum;
        }

        public decimal GetTotalValorAVenta(string _idProyecto)
        {
            string query = "SELECT SUM(precioBase) FROM tUnidad WHERE idProyecto='" + _idProyecto + "' AND (idEstado='" + (Int16)estadoUnidad.Disponible + "' OR idEstado='" + (Int16)estadoUnidad.Reservado + "')";

            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public Int16 GetCantUnidadesDisponibles(string _idProyecto)
        {
            string query = "SELECT Count(u.id) FROM tUnidad u INNER JOIN tProyecto p ON p.id=u.idProyecto where (u.idEstado='" + (Int16)estadoUnidad.Disponible + "' OR u.idEstado='" + (Int16)estadoUnidad.Reservado + "') ";
            query += " AND p.papelera='" + (Int16)papelera.Activo +"'";

            if (_idProyecto != null)
                query += " AND u.idProyecto='" + _idProyecto + "'";

            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToInt16(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public byte[] exportFile
        {
            get
            {
                string query = "SELECT archivo FROM tArchivo WHERE id = " + "1";
                SqlCommand cmd = new SqlCommand(query);
                return cDataBase.GetInstance().ExecuteScalarByte(cmd);
            }
        }

        public ArrayList LoadTableByIdEmpresa(string _idEmpresa)
        {
            ArrayList proyecto = new ArrayList();
            string query = "SELECT u.id FROM tEmpresaUnidad eu INNER JOIN tProyecto p ON eu.idProyecto = p.id INNER JOIN tEmpresa e ON e.id = eu.idEmpresa ";
            query += " INNER JOIN tUnidad u ON u.id = eu.idUnidad INNER JOIN tOperacionVenta pv ON eu.idOv = pv.id ";
            query += " WHERE e.id='" + _idEmpresa + "' AND eu.idOV<>'-1'  AND pv.estado =" + (Int16)estadoOperacionVenta.Activo + " GROUP BY u.id";
            
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                proyecto.Add(Load(Convert.ToString(idList[i])));
            }
            return proyecto;
        }

        public List<cUnidad> GetListUnidadesVendidas(string _idProyecto)
        {
            List<cUnidad> unidades = new List<cUnidad>();
           /* string query = "SELECT u.id ";
            query += " FROM tEmpresaUnidad eu INNER JOIN tOperacionVenta o ON eu.idOv = o.id INNER JOIN tProyecto p ON eu.idProyecto=p.id INNER JOIN tUnidad u ON eu.idUnidad=u.id ";
            query += " WHERE p.id='" + _idProyecto + "' AND u.idEstado='" + (Int16)estadoUnidad.Vendido + "' AND o.estado ='" + (Int16)estadoOperacionVenta.Activo + "' AND eu.precioAcordado<>'0' ORDER BY u.codUF";*/
            
            /*string query = "SELECT u.id  FROM tUnidad u INNER JOIN tEmpresaUnidad eu ON u.id=eu.idUnidad INNER JOIN tEmpresa e ON eu.idEmpresa=e.id";
            query += " WHERE u.idProyecto= '" + _idProyecto + "' AND u.papelera=1 AND u.idEstado=3 OR  u.idEstado=6 AND u.idEstado <> '5' group by u.id,e.apellido, e.nombre order by e.apellido, e.nombre ";*/


            string query = "SELECT u.id ";
            query += " FROM tEmpresaUnidad eu INNER JOIN tOperacionVenta o ON eu.idOv = o.id INNER JOIN tProyecto p ON eu.idProyecto=p.id INNER JOIN tUnidad u ON eu.idUnidad=u.id ";
            query += " WHERE p.id='" + _idProyecto + "' AND u.idEstado='" + (Int16)estadoUnidad.Vendido + "' AND o.estado ='" + (Int16)estadoOperacionVenta.Activo + "' AND eu.precioAcordado<>'0'";

            
            
            
            
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                unidades.Add(Load(Convert.ToString(idList[i])));
            }
            return unidades;
        }

        public DataTable GetUnidadesVendidas(string _idProyecto)
        {
            DateTime date = Convert.ToDateTime(DateTime.Now.Year + " -  " + DateTime.Now.Month + " -  " + 20);

            string query = "SELECT p.id AS idProyecto, eu.precioAcordado AS precio, o.monedaAcordada AS moneda, o.valorDolar AS valorDolar, u.supTotal AS sup, u.id AS idUnidad ";
            query += " FROM tEmpresaUnidad eu INNER JOIN tOperacionVenta o ON eu.idOv = o.id INNER JOIN tProyecto p ON eu.idProyecto=p.id INNER JOIN tUnidad u ON eu.idUnidad=u.id ";
            query += " WHERE p.id='" + _idProyecto + "' AND u.idEstado='" + (Int16)estadoUnidad.Vendido +"' AND o.estado ='" + (Int16)estadoOperacionVenta.Activo + "' AND eu.precioAcordado<>'0'";

            SqlCommand com = new SqlCommand(query);
            com.Parameters.Add("@fecha", SqlDbType.DateTime);
            com.Parameters["@fecha"].Value = date;

            DataTable dt = cDataBase.GetInstance().GetDataReader(com);

            return dt;
        }

        public decimal GetUnidadesByEstado(Int16 _idEstado, string _idProyecto)
        {
            string query = "SELECT COUNT(id) FROM tUnidad WHERE idEstado='" + _idEstado + "' AND idProyecto='" + _idProyecto + "' AND papelera='" + (Int16)papelera.Activo + "'";
            SqlCommand cmd = new SqlCommand(query);
            return Convert.ToDecimal(cDataBase.GetInstance().ExecuteScalar(cmd));
        }

        public DataTable GetMinCantidadEstado(string _idProyecto)
        {
            ArrayList proyecto = new ArrayList();
            string query = "SELECT Count(id) as Total, idEstado FROM tUnidad WHERE idProyecto='" + _idProyecto + "' AND papelera='" + (Int16)papelera.Activo + "'";
            query += " AND (idEstado=" + (Int16)estadoUnidad.Disponible + " OR idEstado=" + (Int16)estadoUnidad.Reservado;
            query += " OR idEstado=" + (Int16)estadoUnidad.Vendido_sin_boleto + " OR idEstado=" + (Int16)estadoUnidad.Vendido + ") GROUP BY idEstado ORDER BY Total";

            SqlCommand com = new SqlCommand(query);
            DataTable dt = cDataBase.GetInstance().GetDataReader(com);

            return dt;
        }

        public List<cUnidad> GetUnidadByOV(string _idOperacionVenta)
        {
            List<cUnidad> cc = new List<cUnidad>();
            string query = "SELECT u.* FROM tEmpresaUnidad eu INNER JOIN tUnidad u ON eu.idUnidad=u.id WHERE eu.idOv = '" + _idOperacionVenta + "'";

            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            if (idList == null)
                return null;
            for (int i = 0; idList.Count > i; i++)
            {
                cc.Add(Load(Convert.ToString(idList[i])));
            }
            return cc;
        }

        public ArrayList GetCantProyectosByOV(string _idOperacionVenta)
        {
            ArrayList cantidad = new ArrayList();
            string query = "SELECT COUNT(u.id), u.idProyecto FROM tEmpresaUnidad eu INNER JOIN tUnidad u ON eu.idUnidad=u.id WHERE eu.idOv = '" + _idOperacionVenta + "' Group by u.idProyecto";
            SqlCommand com = new SqlCommand(query);
            ArrayList idList = cDataBase.GetInstance().ExecuteReader(com);
            for (int i = 0; idList.Count > i; i++)
            {
                cantidad.Add(Load(Convert.ToString(idList[i])));
            }
            return cantidad;
        }

        public Int16 GetCantidadUnidadesVendidas(string _idProyecto)
        {
            string query = "SELECT COUNT(u.id) FROM tProyecto p INNER JOIN tUnidad u ON u.idProyecto=p.id WHERE p.id='" + _idProyecto + "' AND u.papelera='" + (Int16)papelera.Activo + "' AND (u.idEstado='" + (Int16)estadoUnidad.Vendido + "' OR u.idEstado='" + (Int16)estadoUnidad.Vendido_sin_boleto + "')";
            SqlCommand cmd = new SqlCommand(query);
            string result = cDataBase.GetInstance().ExecuteScalar(cmd);

            if (result != null)
                return Convert.ToInt16(cDataBase.GetInstance().ExecuteScalar(cmd));
            else
                return 0;
        }
    }
}

