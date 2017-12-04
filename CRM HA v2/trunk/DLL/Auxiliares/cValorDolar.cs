using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cValorDolar
{
    private string id;
    private string valorDolar;
    private DateTime registerDate;
    private Int16 _papelera;

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }
    public string ValorDolar
    {
        get { return valorDolar; }
        set { valorDolar = value; }
    }

    public DateTime RegisterDate
    {
        get { return registerDate; }
        set { registerDate = value; }
    }
    public string GetRegisterDate
    {
        get { return String.Format("{0:dd/MM/yyyy}", RegisterDate); }
    }


    public Int16 Papelera
    {
        get { return _papelera; }
        set { _papelera = value; }
    }
    #endregion

    public cValorDolar() { }

    public cValorDolar(string _valorDolar, papelera papeleraId)
    {
        valorDolar = _valorDolar.Replace(".", ",");
        registerDate = DateTime.Now;
        _papelera = Convert.ToInt16(papeleraId);
    }

    public int Save()
    {
        cValorDolarDAO DAO = new cValorDolarDAO();
        return DAO.Save(this);
    }

    public static List<cValorDolar> GetValoresDolar()
    {
        cValorDolarDAO DAO = new cValorDolarDAO();
        return DAO.GetValoresDolar();
    }

    public static decimal LoadActualValue()
    {
        cValorDolarDAO DAO = new cValorDolarDAO();
        return DAO.LoadActualValue();
    }

    public static Decimal ConvertToDolar(decimal _valor)
    {
        decimal asd = LoadActualValue();
        return _valor / LoadActualValue();
    }

    public static Decimal ConvertToDolar(decimal _valor, decimal _dolar)
    {
        return _valor / _dolar;
    }

    public static Decimal ConvertToPeso(decimal _valor)
    {
        return _valor * LoadActualValue();
    }

    public static Decimal ConvertToPeso(decimal _valor, decimal _dolar)
    {
        return _valor * _dolar;
    }    
}

