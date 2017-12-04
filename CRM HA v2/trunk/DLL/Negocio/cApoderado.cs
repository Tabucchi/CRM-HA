using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class cApoderado
{
    private string id;    
    private string razonSocial;    
    private string cuit;
    private string tipoDoc;
    private string documento;
    private string idDomicilio;
    private string telefono;    
    private string mail;
    private Int16 papelera;

    #region Propiedades
    public string Id
    {
        get { return id; }
        set { id = value; }
    }
    public string RazonSocial
    {
        get { return razonSocial; }
        set { razonSocial = value; }
    }
    public string Cuit
    {
        get { return cuit; }
        set { cuit = value; }
    }
    public string TipoDoc
    {
        get { return tipoDoc; }
        set { tipoDoc = value; }
    }
    public string GetTipoDoc
    {
        get
        {
            string tipoDoc = null;
            switch (TipoDoc)
            {
                case "1":
                    tipoDoc = tipoDocumento.DNI.ToString();
                    break;
                case "2":
                    tipoDoc = tipoDocumento.CI.ToString();
                    break;
                case "3":
                    tipoDoc = tipoDocumento.LC.ToString();
                    break;
                case "4":
                    tipoDoc = tipoDocumento.LE.ToString();
                    break;
            }
            return tipoDoc;
        }
    }
    public string Documento
    {
        get { return documento; }
        set { documento = value; }
    }
    public string IdDomicilio
    {
        get { return idDomicilio; }
        set { idDomicilio = value; }
    }
    public string Telefono
    {
        get { return telefono; }
        set { telefono = value; }
    }
    public string Mail
    {
        get { return mail; }
        set { mail = value; }
    }
    public Int16 Papelera
    {
        get { return papelera; }
        set { papelera = value; }
    }
    #endregion

    public cApoderado()
    { }

    public int Save()
    {
        cApoderadoDAO DAO = new cApoderadoDAO();
        return DAO.Save(this);
    }

    public static cApoderado Load(string id)
    {
        cApoderadoDAO empresaDAO = new cApoderadoDAO();
        return empresaDAO.Load(id);
    }
}

