using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Base_de_Datos
{
    public class cTotalesSaldos
    {
        private string id;
        private DateTime fecha;
        private decimal totalSaldoCuotasporObra;
        private decimal totalCuentaCorriente;
        private decimal totalRecibos;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }
        public decimal TotalSaldoCuotasporObra
        {
            get { return totalSaldoCuotasporObra; }
            set { totalSaldoCuotasporObra = value; }
        }
        public decimal TotalCuentaCorriente
        {
            get { return totalCuentaCorriente; }
            set { totalCuentaCorriente = value; }
        }

        public decimal TotalRecibos
        {
            get { return totalRecibos; }
            set { totalRecibos = value; }
        }
    }
}
