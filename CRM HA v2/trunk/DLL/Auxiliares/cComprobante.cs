using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cComprobante
{
    public string id { get; set; }
    public Int16 tipo {get;set;}
    public string GetTipo
    {
        get
        {
            string tipoComprobante = null;

            switch (tipo)
            {
                case (Int16)eComprobante.Recibo:
                    tipoComprobante = eComprobante.Recibo.ToString();
                    break;
                case (Int16)eComprobante.Condonacion:
                    tipoComprobante = "Condonación";
                    break;
            }

            return tipoComprobante;
        }
    }
    public string nro { get; set; }
}

