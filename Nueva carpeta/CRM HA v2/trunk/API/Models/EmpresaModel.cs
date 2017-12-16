using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileServices.Models
{
    public class EmpresaModel
    {
        public Int64 Id {get;set;}
        public string Nombre {get;set;}
        public string Direccion {get;set;}
        public string Telefono {get;set;}
        public string Cuit {get;set;}

        public static EmpresaModel empresaModel(tEmpresa tEmpresa)
        {
            EmpresaModel empresaModel = new EmpresaModel();
            empresaModel.Id = Convert.ToInt64(tEmpresa.id);
            empresaModel.Nombre = tEmpresa.Nombre;
            empresaModel.Direccion = tEmpresa.Direccion;
            empresaModel.Telefono = tEmpresa.Telefono;
            empresaModel.Cuit = tEmpresa.Cuit;
            return empresaModel;
        }
    }
}