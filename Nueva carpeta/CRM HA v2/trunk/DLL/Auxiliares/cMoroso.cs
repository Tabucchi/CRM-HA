using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DLL.Auxiliares
{
    public class cMoroso
    {
        public string empresa { get; set; }
        public decimal monto { get; set; }
        public string GetMonto
        {
            get{
                return String.Format("{0:#,#0.00}", monto);
            }
        }

        public string idCuentaCorriente { get; set; }
    }
}
