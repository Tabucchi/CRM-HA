using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileServices.Models
{
    public class ClienteModel
    {
        public Int64 Id {get;set;}
        public Int64 IdEmpresa {get;set;}
        public string Nombre {get;set;}
        public string Interno {get;set;}
        public string Mail {get;set;}
        public string ClaveSistema {get;set;}

        public static string Decode(string _pass)
        {
            string newPass = "";
            char character;

            int i = 0;

            if (i < _pass.Length && _pass.Length != 3) //si el tamaño de la clave es mayor a 3 
            {
                while (i < _pass.Length)
                {
                    //se toman de a tres valores
                    string _aux = Convert.ToString(_pass[i]);
                    _aux += Convert.ToString(_pass[i + 1]);
                    _aux += Convert.ToString(_pass[i + 2]);
                                        
                    //se obtiene el caracter
                    int valorCaracter = (Convert.ToInt16(_aux) - 210);
                    character = Convert.ToChar(valorCaracter);

                    Convert.ToString(character);
                    newPass += character;
                    i = i + 3;
                }
            }
            else
            {
                /* int aux = (Convert.ToInt16(_pass) - 210);
                 character = Convert.ToChar(aux);
                 Convert.ToString(character);
                 newPass += character;*/
            }

            return newPass;
        }

        public static ClienteModel clienteModel(tCliente tCliente)
        {
            ClienteModel clienteModel = new ClienteModel();
            clienteModel.Id = Convert.ToInt64(tCliente.id);
            clienteModel.Nombre = tCliente.Nombre;
            clienteModel.Interno = tCliente.Interno;
            clienteModel.Mail = tCliente.Mail;
            clienteModel.ClaveSistema = ClienteModel.Decode(tCliente.ClaveSistema);
            return clienteModel;
        }
    }
}