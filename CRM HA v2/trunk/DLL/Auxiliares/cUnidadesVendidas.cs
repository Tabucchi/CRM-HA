using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DLL.Negocio;

namespace DLL.Auxiliares
{
    /*
     SELECT p.descripcion, eu.precioAcordado, o.monedaAcordada, o.valorDolar, u.supTotal
	FROM tEmpresaUnidad eu INNER JOIN tOperacionVenta o ON eu.idOv = o.id INNER JOIN tProyecto p ON eu.idProyecto=p.id INNER JOIN tUnidad u ON eu.idUnidad=u.id
	WHERE p.id='16' AND u.idEstado='3' AND o.estado ='1' AND eu.precioAcordado<>'0'     
     */
    public class cUnidadesVendidas
    {
        public string idProyecto { get; set; }

        public string GetProyecto
        {
            get
            {
                return cProyecto.Load(idProyecto).Descripcion;
            }
        }

        public int cantidad { get; set; }

        public int cantidadSinBoleto { get; set; }

        public string valorM2 { get; set; }

        public string precioAcordado { get; set; }

        public string supTotal { get; set; }
    }
}
