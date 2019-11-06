using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SIGESDOC.Web
{
    public class IconLogic
    {
        public static string ClaseEstadoIcon(string estado_envio)
        {
            switch (estado_envio)
            {
                case "1":
                    return "glyphicon glyphicon-minus";
                case "2":
                    return "glyphicon glyphicon-file";
                case "3":
                    return "glyphicon glyphicon-arrow-right";
                case "4":
                    return "glyphicon glyphicon-ok";
                case "5":
                case "6":
                    return "glyphicon glyphicon-warning-sign";
                default:
                    return "";
            }
        }

        public static string ColorEstadoIcon(string estado_envio) 
        {
            switch (estado_envio)
            {
                case "1":
                    return "gray";
                case "2":
                    return "yellow";
                case "3":
                    return "blue";
                case "4":
                    return "green";
                case "5":
                case "6":
                    return "red";
                default:
                    return "";
            }
        }

        public static string AccionesJavaScriptColaborador(string estado_envio)
        {
            switch (estado_envio)
            {
                case "1":
                    return "Registrar";
                case "2":
                case "5":
                case "6":
                    return "Editar";
                case "3":
                case "4":
                    return "Visualizar";
                default:
                    return "";
            }
        }

        public static string AccionesJavaScriptAnalista(string estado_envio)
        {
            switch (estado_envio)
            {
                case "3":
                case "4":
                case "5":
                case "6":
                    return "Visualizar";
                default:
                    return "";
            }
        }


        public static string ClaseAcccionesIcon(string estado_envio)
        {
            switch (estado_envio)
            {
                case "1":
                    return "glyphicon glyphicon-registration-mark";
                case "2":
                case "5":
                case "6":
                    return "glyphicon glyphicon-pencil";
                case "3":
                case "4":
                    return "glyphicon glyphicon-eye-open";
                default:
                    return "";
            }
        }

        public static string ColorAccionesIcon(string estado_envio)
        {
            switch (estado_envio)
            {
                case "2":
                case "5":
                case "6":
                    return "yellow";
                default:
                    return "black";
            }
        }

    }
}