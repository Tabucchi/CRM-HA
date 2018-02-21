using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

public class cAuxiliar
{
    public static bool IsNumeric(string expression)
    {
        bool isNum;
        double retNum;
        isNum = Double.TryParse(expression, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        return isNum;
    }

    public static decimal RedondearCentenas(decimal nro)
    {
        decimal asd = Math.Round((nro / 100), 0, MidpointRounding.AwayFromZero) * 100 + 100;
        decimal asr = Math.Round((nro / 100), 0, MidpointRounding.AwayFromZero) * 100;


        return Math.Round((nro / 100), 0, MidpointRounding.AwayFromZero) * 100;
    }

    public static decimal RedondearMillar(decimal nro)
    {
        decimal asd = Math.Round((nro / 1000), 0, MidpointRounding.AwayFromZero) * 1000 + 1000;
        decimal asr = Math.Round((nro / 1000), 0, MidpointRounding.AwayFromZero) * 1000;

        return Math.Round((nro / 1000), 0, MidpointRounding.AwayFromZero) * 1000;
    }

    public static bool ValidarCuit(string _cuit)
    {
        int sumatoria = 0;
        string[] cuit = (_cuit.Replace("-", "")).Select(c => c.ToString()).ToArray();

        int[] serie = { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };

        int sdfdf = cuit.Count();

        for (int i = 0; i < serie.Count(); i++)
        {
            if (sdfdf > i && cAuxiliar.IsNumeric(cuit[i]))
                sumatoria += Convert.ToInt32(cuit[i]) * serie[i];
        }

        int mod = 11 - (sumatoria % 11);

        if (cuit.Last() == Convert.ToString(mod))
            return true;
        else
            return false;
    }

    public static bool ValidarMail(string sEmailAComprobar){
        string sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
        if (Regex.IsMatch(sEmailAComprobar, sFormato))
        {
            if (Regex.Replace(sEmailAComprobar, sFormato, String.Empty).Length == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    
    public static string GetMoneda(string _idMoneda)
    {
        string moneda = null;
        switch (_idMoneda)
        {
            case "0":
                moneda = tipoMoneda.Dolar.ToString();
                break;
            case "1":
                moneda = tipoMoneda.Pesos.ToString();
                break;
        }
        return moneda;
    }

    public static string GetMonedaByDescripcion(string moneda)
    {
        string idMoneda = null;
        switch (moneda)
        {
            case "Dolar":
                idMoneda = Convert.ToString((Int16)tipoMoneda.Dolar);
                break;
            case "Pesos":
                idMoneda = Convert.ToString((Int16)tipoMoneda.Pesos);
                break;
        }
        return idMoneda;
    }

    public static string AgregarCeroRecibo(string _nro)
    {
        int cant = 10 - _nro.Length;
        string aux = "";
        for (int i = 0; i <= cant; i++)
            aux += 0;
        return aux + _nro;
    }

    public static void ExecuteBackUp()
    {
        //Restore Back up: RESTORE DATABASE YourDB FROM DISK = 'D:BackUpYourBaackUpFile.bak'

        string database = "HaCRM";
        //string database = "SabCRM";
        string fileName = "HaCRM_" + String.Format("{0:dd_MM_yyyy}", DateTime.Now) + ".bak";

        string url = HttpContext.Current.Request.PhysicalApplicationPath + "\\Archivos\\Back up\\" + fileName;
        //string url = "C:\\Users\\ntabucchi\\Documents\\Proyectos\\NAEX\\branches\\CRM HA\\trunk\\Archivos\\Back up\\" + fileName;
        //string url = "C:\\PUBLICACIONES\\HA CRM\\Archivos\\Back up\\" + fileName;
        cDataBase.GetInstance().ExecuteBackUp(database, url);
    }

    public static string enLetras(string num)
    {
        string res, dec = "";
        string neg = "";
        Int64 entero;
        int decimales;
        double nro;
        
        try
        {
            if (Convert.ToDouble(num) > 0)
                nro = Convert.ToDouble(num);
            else
            {
                nro = Convert.ToDouble(num) * -1;
                neg = "menos ";
            }
        }
        catch
        {
            return "";
        }

        entero = Convert.ToInt64(Math.Truncate(nro));
        decimales = Convert.ToInt32(Math.Round((nro - entero) * 100, 2));
        if (decimales > 0)
        {
            dec = " con " + toText(Convert.ToDecimal(decimales));
        }

        res = neg + toText(Convert.ToDecimal(entero)) + dec;
        return res;
    }

    public static string toText(decimal value)
    {
        if (value < 0)
            value = value * -1;

        string Num2Text = "";
        value = Math.Truncate(value);
        if (value == 0) Num2Text = "cero";
        else if (value == 1) Num2Text = "uno";
        else if (value == 2) Num2Text = "dos";
        else if (value == 3) Num2Text = "tres";
        else if (value == 4) Num2Text = "cuatro";
        else if (value == 5) Num2Text = "cinco";
        else if (value == 6) Num2Text = "seis";
        else if (value == 7) Num2Text = "siete";
        else if (value == 8) Num2Text = "ocho";
        else if (value == 9) Num2Text = "nueve";
        else if (value == 10) Num2Text = "diez";
        else if (value == 11) Num2Text = "once";
        else if (value == 12) Num2Text = "doce";
        else if (value == 13) Num2Text = "trece";
        else if (value == 14) Num2Text = "catorce";
        else if (value == 15) Num2Text = "quince";
        else if (value < 20) Num2Text = "dieci" + toText(value - 10);
        else if (value == 20) Num2Text = "veinte";
        else if (value < 30) Num2Text = "veinti" + toText(value - 20);
        else if (value == 30) Num2Text = "treinta";
        else if (value == 40) Num2Text = "cuarenta";
        else if (value == 50) Num2Text = "cincuenta";
        else if (value == 60) Num2Text = "sesenta";
        else if (value == 70) Num2Text = "setenta";
        else if (value == 80) Num2Text = "ochenta";
        else if (value == 90) Num2Text = "noventa";
        else if (value < 100) Num2Text = toText(Math.Truncate(value / 10) * 10) + " y " + toText(value % 10);
        else if (value == 100) Num2Text = "cien";
        else if (value < 200) Num2Text = "ciento " + toText(value - 100);
        else if ((value == 200) || (value == 300) || (value == 400) || (value == 600) || (value == 800)) Num2Text = toText(Math.Truncate(value / 100)) + "cientos";
        else if (value == 500) Num2Text = "quinientos";
        else if (value == 700) Num2Text = "setecientos";
        else if (value == 900) Num2Text = "novecientos";
        else if (value < 1000) Num2Text = toText(Math.Truncate(value / 100) * 100) + " " + toText(value % 100);
        else if (value == 1000) Num2Text = "mil";
        else if (value < 2000) Num2Text = "mil " + toText(value % 1000);
        else if (value < 1000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000)) + " mil";
            if ((value % 1000) > 0) Num2Text = Num2Text + " " + toText(value % 1000);
        }

        else if (value == 1000000) Num2Text = "un millon";
        else if (value < 2000000) Num2Text = "un millon " + toText(value % 1000000);
        else if (value < 1000000000000)
        {
            Num2Text = toText(Math.Truncate(value / 1000000)) + " millones";
            if ((value - Math.Truncate(value / 1000000) * 1000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000) * 1000000);
        }

        else if (value == 1000000000000) Num2Text = "un billon";
        else if (value < 2000000000000) Num2Text = "un billon " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);

        else
        {
            Num2Text = toText(Math.Truncate(value / 1000000000000)) + " billones";
            if ((value - Math.Truncate(value / 1000000000000) * 1000000000000) > 0) Num2Text = Num2Text + " " + toText(value - Math.Truncate(value / 1000000000000) * 1000000000000);
        }
        return Num2Text;
    }

    public static void RegistrarCuotasActualizadas(string _text)
    {
        string path = HttpContext.Current.Request.PhysicalApplicationPath + "\\Logs\\";
        string pathFile = HttpContext.Current.Request.PhysicalApplicationPath + "\\Logs\\" + "indice.txt";

        if (Directory.Exists(path))
        {
            DirectoryInfo di = Directory.CreateDirectory(path);
            if (!File.Exists(pathFile))
            {
                StreamWriter sw = File.CreateText(pathFile);
                sw.Close();
            }
        }
        
        StreamWriter writer = new StreamWriter(pathFile, true);
        writer.WriteLine(_text);
        writer.Close();
    }
}
