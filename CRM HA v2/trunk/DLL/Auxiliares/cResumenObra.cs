using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Auxiliares
{
    public class cResumenObra
    {
        public string idObra { get; set; }
        public string obra { get; set; }
        public decimal disponible { get; set; }
        public string porcentajeDisponible { get; set; }
        public decimal reservada { get; set; }
        public string porcentajeReservada { get; set; }
        public decimal vendida_sin_boleto { get; set; }
        public string porcentajeVendidaSinBoleto { get; set; }
        public decimal vendida { get; set; }
        public string porcentajeVendida { get; set; }
        public decimal total { get; set; }

    }
}
