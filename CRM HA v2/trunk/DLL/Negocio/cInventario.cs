using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public enum Empresa { Naex = 1, Betchom = 2, HA = 3 };

public class cInventario
{
    private string id;    
    private string descripcion;
    private string idImagen;    
    private string idCategoria;    
    private string empresa;    
    private string numero;
    private decimal valor;   
    private int cantUnidades;    
    private int idResponsable;

    #region Acceso a Datos
    public static cInventario Load(string id)
    {
        cInventarioDAO DAO = new cInventarioDAO();
        return DAO.Load(id);
    }

    public int Save()
    {
        cInventarioDAO DAO = new cInventarioDAO();
        return DAO.Save(this);
    }
    #endregion

    public static List<cInventario> GetInventarios(string empresa)
    {
        cInventarioDAO empresaDao = new cInventarioDAO();
        return empresaDao.GetInventarios(empresa);
    }

    public static Decimal GetValor(string empresa)
    {
        cInventarioDAO DAO = new cInventarioDAO();
        return DAO.GetValor(empresa);
    }

    public bool Delete(string id)
    {
        cInventarioDAO invDAO = new cInventarioDAO();
        return invDAO.Delete(id);
    }

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Descripcion
    {
        get { return descripcion; }
        set { descripcion = value; }
    }

    public string IdImagen
    {
        get { return idImagen; }
        set { idImagen = value; }
    }

    public string IdCategoria
    {
        get { return idCategoria; }
        set { idCategoria = value; }
    }

    public string Empresa
    {
        get { return empresa; }
        set { empresa = value; }
    }

    public string Numero
    {
        get { return numero; }
        set { numero = value; }
    }


    public decimal Valor
    {
        get { return valor; }
        set { valor = value; }
    }

    public int CantUnidades
    {
        get { return cantUnidades; }
        set { cantUnidades = value; }
    }

    public int IdResponsable
    {
        get { return idResponsable; }
        set { idResponsable = value; }
    }

    public string GetCategoria
    {
        get { return cCategoriaInventario.Load(IdCategoria).Descripcion; }
    }

    public string GetEmpresa
    {
        get {
            string empresa = null;
            switch (Empresa)
            {
                case "1":
                    empresa = "Naex";
                    break;
                case "2":
                    empresa = "Bethcom";
                    break;
                case "3":
                    empresa = "HA";
                    break;
            }

            return empresa;
        }
    }

    public string GetResponsable
    {
        get
        {
            return cCampoGenerico.Load(idResponsable.ToString(), Tablas.tResponsableInventario).Descripcion; }
    }
    #endregion
}
