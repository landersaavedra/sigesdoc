using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SIGESDOC.Web
{

    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Inicio", action = "Index", id = UrlParameter.Optional }
            );



            routes.MapRoute("HT_cancelar_recepcion",
                        "HojaTramite/HT_cancelar_recepcion/",
                        new
                        {
                            controller = "HojaTramite",
                            action = "HT_cancelar_recepcion"
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("descargar_anexo",
                        "HojaTramite/descargar_anexo/",
                        new
                        {
                            controller = "HojaTramite",
                            action = "descargar_anexo"
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_operacion_exportados",
                       "General/llenar_operacion_exportados",
                       new
                       {
                           controller = "General",
                           action = "llenar_operacion_exportados"
                       },
                       new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("imprimir_reporte_usuario_ss_new",
                        "HojaTramite/imprimir_reporte_usuario_ss_new/{fecini}/{fecfin}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "imprimir_reporte_usuario_ss_new",
                            fecini = UrlParameter.Optional,
                            fecfin = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("subir_archivo_anexo_ht",
                        "HojaTramite/subir_archivo_anexo_ht/{files}/{lbl_id_documento_anexo_ht}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "subir_archivo_anexo_ht",
                            files = UrlParameter.Optional,
                            lbl_id_documento_anexo_ht = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Imprimir_pendientes_new",
                        "HojaTramite/Imprimir_pendientes_new/{aniodesde}/{aniohasta}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "Imprimir_pendientes_new",
                            aniodesde = UrlParameter.Optional,
                            aniohasta = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Consulta_comprobantes_x_mes_pdf",
                        "General/Consulta_comprobantes_x_mes_pdf/{mes}/{anio}",
                        new
                        {
                            controller = "General",
                            action = "Consulta_comprobantes_x_mes_pdf",
                            mes = UrlParameter.Optional,
                            anio = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Consultar_documentos_pendientes_detalle_desagregado",
                        "Account/Consultar_documentos_pendientes_detalle_desagregado/{fecha}",
                        new
                        {
                            controller = "Account",
                            action = "Consultar_documentos_pendientes_detalle_desagregado",
                            fecha = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("consultar_documentos_pendientes_principal_detalle",
                        "Account/consultar_documentos_pendientes_principal_detalle/",
                        new
                        {
                            controller = "Account",
                            action = "consultar_documentos_pendientes_principal_detalle"
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("consultar_documentos_pendientes_principal",
                        "Account/consultar_documentos_pendientes_principal/",
                        new
                        {
                            controller = "Account",
                            action = "consultar_documentos_pendientes_principal"
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("buscar_persona_entidad_por_nombre",
                        "General/buscar_persona_entidad_por_nombre/{nombre}",
                        new
                        {
                            controller = "General",
                            action = "buscar_persona_entidad_por_nombre",
                            nombre = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Listar_Planta_antigua_x_Texto",
                        "Habilitaciones_Protocolos/Listar_Planta_antigua_x_Texto/{buscador}",
                        new
                        {
                            controller = "Habilitaciones_Protocolos",
                            action = "Listar_Planta_antigua_x_Texto",
                            buscador = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_transporte",
                           "Habilitaciones/Generar_data_transporte/",
                           new { controller = "Habilitaciones", action = "Generar_data_transporte" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_autorizacion_instalacion",
                           "Habilitaciones/Generar_data_autorizacion_instalacion/",
                           new { controller = "Habilitaciones", action = "Generar_data_autorizacion_instalacion" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_licencia_operacion",
                           "Habilitaciones/Generar_data_licencia_operacion/",
                           new { controller = "Habilitaciones", action = "Generar_data_licencia_operacion" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_almacen",
                           "Habilitaciones/Generar_data_almacen/",
                           new { controller = "Habilitaciones", action = "Generar_data_almacen" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_embarcacion",
                           "Habilitaciones/Generar_data_embarcacion/",
                           new { controller = "Habilitaciones", action = "Generar_data_embarcacion" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_planta",
                           "Habilitaciones/Generar_data_planta/",
                           new { controller = "Habilitaciones", action = "Generar_data_planta" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_concesiones",
                           "Habilitaciones/Generar_data_concesiones/",
                           new { controller = "Habilitaciones", action = "Generar_data_concesiones" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_data_desembarcadero",
                           "Habilitaciones/Generar_data_desembarcadero/",
                           new { controller = "Habilitaciones", action = "Generar_data_desembarcadero" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llena_consumo_humano",
                           "General/Llena_consumo_humano/",
                           new { controller = "General", action = "Llena_consumo_humano" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_tipo_planta",
                           "General/Llenar_tipo_planta/",
                           new { controller = "General", action = "Llenar_tipo_planta" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_tipo_actividad_planta",
                           "General/Llenar_tipo_actividad_planta/",
                           new { controller = "General", action = "Llenar_tipo_actividad_planta" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_OD",
                           "General/llenar_OD/",
                           new { controller = "General", action = "llenar_OD" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_furgon_x_carroceria",
                           "Habilitaciones/llenar_furgon_x_carroceria/",
                           new { controller = "Habilitaciones", action = "llenar_furgon_x_carroceria" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_provincia_x_departamento",
                           "General/llenar_provincia_x_departamento/",
                           new { controller = "General", action = "llenar_provincia_x_departamento" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_inspector_x_si",
                           "Inspeccion/llenar_inspector_x_si/",
                           new { controller = "Inspeccion", action = "llenar_inspector_x_si " },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_distrito_x_provincia",
                           "General/llenar_distrito_x_provincia/",
                           new { controller = "General", action = "llenar_distrito_x_provincia " },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Consultar_DNI_vista",
                           "Hojatramite/Consultar_DNI_vista/",
                           new { controller = "Hojatramite", action = "Consultar_DNI_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("consultar_tipo_expediente",
                           "Hojatramite/consultar_tipo_expediente/",
                           new { controller = "Hojatramite", action = "consultar_tipo_expediente", id = UrlParameter.Optional },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Consultar_DNI_DIRECCION_vista",
                           "Hojatramite/Consultar_DNI_DIRECCION_vista/",
                           new { controller = "Hojatramite", action = "Consultar_DNI_DIRECCION_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("recupera_sub_tupa_x_monto",
                           "General/recupera_sub_tupa_x_monto/",
                           new { controller = "General", action = "recupera_sub_tupa_x_monto" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_DNI_DIRECCION_serie1",
                           "General/recupera_RUC_DNI_DIRECCION_serie1/",
                           new { controller = "General", action = "recupera_RUC_DNI_DIRECCION_serie1" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_DNI_recibo_serie1",
                           "General/recupera_RUC_DNI_recibo_serie1/",
                           new { controller = "General", action = "recupera_RUC_DNI_recibo_serie1" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_operacion",
                           "General/recupera_operacion/",
                           new { controller = "General", action = "recupera_operacion" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_vista",
                           "Hojatramite/recupera_RUC_vista/",
                           new { controller = "Hojatramite", action = "recupera_RUC_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_vista_seguimiento",
                           "Habilitaciones/recupera_RUC_vista_seguimiento/",
                           new { controller = "Habilitaciones", action = "recupera_RUC_vista_seguimiento" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_DIRECCION_vista",
                           "Hojatramite/recupera_RUC_DIRECCION_vista/",
                           new { controller = "Hojatramite", action = "recupera_RUC_DIRECCION_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_PLANTA_vista",
                           "Hojatramite/recupera_PLANTA_vista/",
                           new { controller = "Hojatramite", action = "recupera_PLANTA_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_ALMACEN_vista",
                           "Hojatramite/recupera_ALMACEN_vista/",
                           new { controller = "Hojatramite", action = "recupera_ALMACEN_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_CONCESION_vista",
                           "Hojatramite/recupera_CONCESION_vista/",
                           new { controller = "Hojatramite", action = "recupera_CONCESION_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("recupera_DESEMBARCADERO_vista",
                           "Habilitaciones/recupera_DESEMBARCADERO_vista/",
                           new { controller = "Habilitaciones", action = "recupera_DESEMBARCADERO_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_RUC_NOM_vista",
                           "Hojatramite/recupera_RUC_NOM_vista/",
                           new { controller = "Hojatramite", action = "recupera_RUC_NOM_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("recupera_RUC_NOM_vista_seguimiento",
                           "Habilitaciones/recupera_RUC_NOM_vista_seguimiento/",
                           new { controller = "Habilitaciones", action = "recupera_RUC_NOM_vista_seguimiento" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_DNI_NOM_vista",
                           "Hojatramite/recupera_DNI_NOM_vista/",
                           new { controller = "Hojatramite", action = "recupera_DNI_NOM_vista" },
                           new[] { "SIGESDOC.Web.Controllers" });



            routes.MapRoute("Nuevo_Documento",
                           "HojaTramite/Nuevo_Documento/{id_det_documento}/{HT}/{id_HT}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Nuevo_Documento",
                               id_det_documento = UrlParameter.Optional,
                               HT = UrlParameter.Optional,
                               id_HT = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });





            routes.MapRoute("Agregar_Destino",
                           "HojaTramite/Agregar_Destino/{id_documento_detalle}/{id_documento}/{oficina_destino}/{encargado}/{var_observacion}/{v_ind_01}/{v_ind_02}/{v_ind_03}/{v_ind_04}/{v_ind_05}/{v_ind_06}/{v_ind_07}/{v_ind_08}/{v_ind_09}/{v_ind_10}/{v_ind_11}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Agregar_Destino",
                               id_documento_detalle = UrlParameter.Optional,
                               id_documento = UrlParameter.Optional,
                               oficina_destino = UrlParameter.Optional,
                               encargado = UrlParameter.Optional,
                               var_observacion = UrlParameter.Optional,
                               v_ind_01 = UrlParameter.Optional,
                               v_ind_02 = UrlParameter.Optional,
                               v_ind_03 = UrlParameter.Optional,
                               v_ind_04 = UrlParameter.Optional,
                               v_ind_05 = UrlParameter.Optional,
                               v_ind_06 = UrlParameter.Optional,
                               v_ind_07 = UrlParameter.Optional,
                               v_ind_08 = UrlParameter.Optional,
                               v_ind_09 = UrlParameter.Optional,
                               v_ind_10 = UrlParameter.Optional,
                               v_ind_11 = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Editar_Destino",
                           "HojaTramite/Editar_Destino/{id_documento_detalle}/{oficina_destino}/{encargado}/{var_observacion}/{v_ind_01}/{v_ind_02}/{v_ind_03}/{v_ind_04}/{v_ind_05}/{v_ind_06}/{v_ind_07}/{v_ind_08}/{v_ind_09}/{v_ind_10}/{v_ind_11}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Editar_Destino",
                               id_documento_detalle = UrlParameter.Optional,
                               oficina_destino = UrlParameter.Optional,
                               encargado = UrlParameter.Optional,
                               var_observacion = UrlParameter.Optional,
                               v_ind_01 = UrlParameter.Optional,
                               v_ind_02 = UrlParameter.Optional,
                               v_ind_03 = UrlParameter.Optional,
                               v_ind_04 = UrlParameter.Optional,
                               v_ind_05 = UrlParameter.Optional,
                               v_ind_06 = UrlParameter.Optional,
                               v_ind_07 = UrlParameter.Optional,
                               v_ind_08 = UrlParameter.Optional,
                               v_ind_09 = UrlParameter.Optional,
                               v_ind_10 = UrlParameter.Optional,
                               v_ind_11 = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            //AJAX

            routes.MapRoute("HT_Llenar",
                           "HojaTramite/HT_Llenar/",
                           new { controller = "HojaTramite", action = "HT_Llenar" },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Llenar_personal",
                           "HojaTramite/Llenar_personal/",
                           new { controller = "HojaTramite", action = "Llenar_personal" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_oficina_sede_externo",
                           "HojaTramite/Llenar_oficina_sede_externo/",
                           new { controller = "HojaTramite", action = "Llenar_oficina_sede_externo" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_destino_adjunto_HT",
                           "HojaTramite/Llenar_destino_adjunto_HT/{expediente}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Llenar_destino_adjunto_HT",
                               expediente = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_oficina_externa_x_bus",
                           "HojaTramite/Llenar_oficina_externa_x_bus/{page}/{RUC}/{NOM}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Llenar_oficina_externa_x_bus",
                               page = UrlParameter.Optional,
                               RUC = UrlParameter.Optional,
                               NOM = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });



            routes.MapRoute("Llenar_oficina_sede",
                           "Account/Llenar_oficina_sede/{dni}/{id_sede}",
                           new
                           {
                               controller = "Account",
                               action = "Llenar_oficina_sede",
                               dni = UrlParameter.Optional,
                               id_sede = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_datos_del_ruc",
                           "Oficina/recupera_datos_del_ruc/{persona_num_documento}",
                           new
                           {
                               controller = "Oficina",
                               action = "recupera_datos_del_ruc",
                               persona_num_documento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Nueva_Oficina",
                           "Oficina/Nueva_Oficina/{page}/{TXT_RUC}",
                           new
                           {
                               controller = "Oficina",
                               action = "recupera_datos_del_ruc",
                               page = UrlParameter.Optional,
                               TXT_RUC = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_embarcacion",
                           "General/recupera_embarcacion/{MATRICULA}",
                           new
                           {
                               controller = "General",
                               action = "recupera_embarcacion",
                               MATRICULA = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_transporte",
                           "General/recupera_transporte/{PLACA}",
                           new
                           {
                               controller = "General",
                               action = "recupera_transporte",
                               PLACA = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recuperar_transporte_x_id_transporte",
                           "General/recuperar_transporte_x_id_transporte/{id_transporte}",
                           new
                           {
                               controller = "General",
                               action = "recuperar_transporte_x_id_transporte",
                               id_transporte = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("ajax_llenar_codigo_desembarcadero",
                           "General/ajax_llenar_codigo_desembarcadero/{id_tipo_desembarcadero}",
                           new
                           {
                               controller = "General",
                               action = "ajax_llenar_codigo_desembarcadero",
                               id_tipo_desembarcadero = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recupera_almacen",
                           "General/recupera_almacen/{COD_ALMACEN}/{VAR_ID_OFI_DIR}",
                           new
                           {
                               controller = "General",
                               action = "recupera_almacen",
                               COD_ALMACEN = UrlParameter.Optional,
                               VAR_ID_OFI_DIR = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });



            routes.MapRoute("recupera_concesion",
                           "General/recupera_concesion/{COD_CONCESION}/{VAR_ID_OFI_DIR}",
                           new
                           {
                               controller = "General",
                               action = "recupera_concesion",
                               COD_CONCESION = UrlParameter.Optional,
                               VAR_ID_OFI_DIR = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Recupera_planta_seguimiento",
                           "Habilitaciones/Recupera_planta_seguimiento/{id_documento_seg}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Recupera_planta_seguimiento",
                               id_documento_seg = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Recupera_embarcacion_seguimiento",
                           "Habilitaciones/Recupera_embarcacion_seguimiento/{id_documento_seg}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Recupera_embarcacion_seguimiento",
                               id_documento_seg = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Recupera_expediente",
                           "Habilitaciones/Recupera_expediente/{id_documento_seg}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Recupera_expediente",
                               id_documento_seg = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Recupera_facturas",
                           "Habilitaciones/Recupera_facturas/{id_documento_seg}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Recupera_facturas",
                               id_documento_seg = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_documentos_adjuntos_sol_insp_sdhpa",
                           "Habilitaciones/llenar_documentos_adjuntos_sol_insp_sdhpa/{id_sol_ins}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_documentos_adjuntos_sol_insp_sdhpa",
                               id_sol_ins = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Existe_matricula",
                           "General/Existe_matricula/{var_matricula}",
                           new
                           {
                               controller = "General",
                               action = "Existe_matricula",
                               var_matricula = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Existe_factura",
                           "General/Existe_factura/{var_num1}/{var_num2}",
                           new
                           {
                               controller = "General",
                               action = "Existe_factura",
                               var_num1 = UrlParameter.Optional,
                               var_num2 = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Existe_persona",
                           "General/Existe_persona/{var_persona_num_doc}",
                           new
                           {
                               controller = "General",
                               action = "Existe_persona",
                               var_persona_num_doc = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Existe_expediente",
                           "General/Existe_expediente/{num_expediente}/{id_tipo_expediente}",
                           new
                           {
                               controller = "General",
                               action = "Existe_expediente",
                               num_expediente = UrlParameter.Optional,
                               id_tipo_expediente = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_empresa",
                           "General/Llenar_empresa/",
                           new
                           {
                               controller = "General",
                               action = "Llenar_empresa"
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_Sedes_empresa",
                           "General/Llenar_Sedes_empresa/{ruc}/",
                           new
                           {
                               controller = "General",
                               action = "Llenar_Sedes_empresa",
                               ruc = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_Sedes_empresa_planta",
                           "General/Llenar_Sedes_empresa_planta/{ruc}/",
                           new
                           {
                               controller = "General",
                               action = "Llenar_Sedes_empresa",
                               ruc = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Consultar_DNI_total",
                           "Hojatramite/Consultar_DNI_total/",
                           new
                           {
                               controller = "Hojatramite",
                               action = "Consultar_DNI_total"
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llena_actividad_concesion",
                           "General/Llena_actividad_concesion/",
                           new
                           {
                               controller = "General",
                               action = "Llena_actividad_concesion"
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("variable_Nuevo_Protocolo_concesion",
                           "Habilitaciones/variable_Nuevo_Protocolo_concesion/{expediente}/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "variable_Nuevo_Protocolo_concesion",
                               expediente = UrlParameter.Optional,
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("variable_Nuevo_Protocolo_transporte",
                           "Habilitaciones/variable_Nuevo_Protocolo_transporte/{expediente}/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "variable_Nuevo_Protocolo_transporte",
                               expediente = UrlParameter.Optional,
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("variable_Nuevo_Protocolo_autorizacion_instalacion",
                           "Habilitaciones/variable_Nuevo_Protocolo_autorizacion_instalacion/{expediente}/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "variable_Nuevo_Protocolo_autorizacion_instalacion",
                               expediente = UrlParameter.Optional,
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("variable_Nuevo_Protocolo_licencia_operacion",
                           "Habilitaciones/variable_Nuevo_Protocolo_licencia_operacion/{expediente}/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "variable_Nuevo_Protocolo_licencia_operacion",
                               expediente = UrlParameter.Optional,
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("variable_Nuevo_Protocolo_almacen",
                           "Habilitaciones/variable_Nuevo_Protocolo_almacen/{expediente}/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "variable_Nuevo_Protocolo_almacen",
                               expediente = UrlParameter.Optional,
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("Generar_informe_tecnico",
                           "Habilitaciones/Generar_informe_tecnico/{id_seguimiento}/{observaciones}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Generar_informe_tecnico",
                               id_seguimiento = UrlParameter.Optional,
                               observaciones = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_solicitud_inspeccion_transporte",
                           "Habilitaciones/Generar_solicitud_inspeccion_transporte/{id_seguimiento},{resolucion},{persona_contacto},{telefono_oficina},{telefono_planta},{correo},{observacion},{serv_habilitacion},{filial},{destino},{cond_manual},{norma_aplicar}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Generar_solicitud_inspeccion_transporte",
                               id_seguimiento = UrlParameter.Optional,
                               resolucion = UrlParameter.Optional,
                               persona_contacto = UrlParameter.Optional,
                               telefono_oficina = UrlParameter.Optional,
                               telefono_planta = UrlParameter.Optional,
                               correo = UrlParameter.Optional,
                               observacion = UrlParameter.Optional,
                               serv_habilitacion = UrlParameter.Optional,
                               filial = UrlParameter.Optional,
                               destino = UrlParameter.Optional,
                               cond_manual = UrlParameter.Optional,
                               norma_aplicar = UrlParameter.Optional

                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Generar_solicitud_inspeccion",
                           "Habilitaciones/Generar_solicitud_inspeccion/{id_seguimiento},{resolucion},{persona_contacto},{telefono_oficina},{telefono_planta},{correo},{observacion},{serv_habilitacion},{filial}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Generar_solicitud_inspeccion",
                               id_seguimiento = UrlParameter.Optional,
                               resolucion = UrlParameter.Optional,
                               persona_contacto = UrlParameter.Optional,
                               telefono_oficina = UrlParameter.Optional,
                               telefono_planta = UrlParameter.Optional,
                               correo = UrlParameter.Optional,
                               observacion = UrlParameter.Optional,
                               serv_habilitacion = UrlParameter.Optional,
                               filial = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });



            routes.MapRoute("Ver_Protocolos_x_planta",
                           "General/Ver_Protocolos_x_planta/{page}/{id_planta}",
                           new
                           {
                               controller = "General",
                               action = "Ver_Protocolos_x_planta",
                               page = UrlParameter.Optional,
                               id_planta = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_destino_emitidos",
                           "Habilitaciones/llenar_destino_emitidos/{id_doc_dhcpa}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_destino_emitidos",
                               id_doc_dhcpa = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("llenar_solicitud_seguimiento",
                           "Habilitaciones/llenar_solicitud_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_solicitud_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_informe_seguimiento",
                           "Habilitaciones/llenar_informe_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_informe_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("recuperar_correo_solicitud",
                           "Habilitaciones/recuperar_correo_solicitud/{id_solicitud}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "recuperar_correo_solicitud",
                               id_solicitud = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("eviar_correo_solicitud_insp",
                           "Habilitaciones/eviar_correo_solicitud_insp/{id_solicitud}/{destinos}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "recuperar_correo_solicitud",
                               id_solicitud = UrlParameter.Optional,
                               destinos = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_documentos_seguimiento_recibido",
                           "Habilitaciones/llenar_documentos_seguimiento_recibido/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_documentos_seguimiento_recibido",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_documentos_adjuntos",
                           "Habilitaciones/llenar_documentos_adjuntos/{id_documento_seg}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_documentos_adjuntos",
                               id_documento_seg = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("inactivar_activar_protocolo",
                           "Habilitaciones/inactivar_activar_protocolo/{id_protocolo}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "inactivar_activar_protocolo",
                               id_protocolo = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("llenar_documentos_emitidos_dhcpa_seguimiento",
                           "Habilitaciones/llenar_documentos_emitidos_dhcpa_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_documentos_emitidos_dhcpa_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_protocolos_transporte",
                           "Habilitaciones/llenar_protocolos_transporte/{id_transporte}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_protocolos_transporte",
                               id_transporte = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("consulta_protocolo_reemplaza",
                           "Habilitaciones/consulta_protocolo_reemplaza/{id_protocolo}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "consulta_protocolo_reemplaza",
                               id_protocolo = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_protocolos_seguimiento",
                           "Habilitaciones/llenar_protocolos_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_protocolos_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_documento_anexo",
                           "Hojatramite/llenar_documento_anexo/{id_documento}",
                           new
                           {
                               controller = "Hojatramite",
                               action = "llenar_documento_anexo",
                               id_documento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_documentos_hoja_tramite",
                           "Hojatramite/llenar_documentos_hoja_tramite/{numero_ht}",
                           new
                           {
                               controller = "Hojatramite",
                               action = "llenar_documentos_hoja_tramite",
                               numero_ht = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Listar_Observacion_x_seguimiento",
                           "Habilitaciones/Listar_Observacion_x_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Listar_Observacion_x_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Listar_historial_evaluador",
                           "Habilitaciones/Listar_historial_evaluador/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Listar_historial_evaluador",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Listar_Observacion_x_documento_ht",
                           "HojaTramite/Listar_Observacion_x_documento_ht/{id_det_documento}",
                           new
                           {
                               controller = "HojaTramite",
                               action = "Listar_Observacion_x_documento_ht",
                               id_det_documento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("llenar_haccp_seguimiento",
                           "Habilitaciones/llenar_haccp_seguimiento/{id_seguimiento}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_haccp_seguimiento",
                               id_seguimiento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_expediente_sin_seguimiento",
                           "Habilitaciones/llenar_expediente_sin_seguimiento",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "llenar_expediente_sin_seguimiento"
                           },
                           new[] { "SIGESDOC.Web.Controllers" });



            routes.MapRoute("Asignar_Expediente_seguimiento",
                           "Habilitaciones/Asignar_Expediente_seguimiento/{id_seguimiento}/{id_expediente}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Asignar_Expediente_seguimiento",
                               id_seguimiento = UrlParameter.Optional,
                               id_expediente = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Buscar_expediente_documento_externo",
                        "Habilitaciones/Buscar_expediente_documento_externo/{expediente}",
                        new
                        {
                            controller = "Habilitaciones",
                            action = "Buscar_expediente_documento_externo",
                            expediente = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_HT_padre",
                        "HojaTramite/llenar_HT_padre/{buscador}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "llenar_HT_padre",
                            buscador = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_datos_HT",
                        "HojaTramite/llenar_datos_HT/{numero}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "llenar_datos_HT",
                            numero = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_Historial_HT",
                        "HojaTramite/llenar_Historial_HT/{numero}",
                        new
                        {
                            controller = "HojaTramite",
                            action = "llenar_Historial_HT",
                            numero = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("llenar_Especie_habilitaciones",
                        "Habilitaciones/llenar_Especie_habilitaciones/{nombre_comun}/{nombre_cientifico}",
                        new
                        {
                            controller = "Habilitaciones",
                            action = "llenar_Especie_habilitaciones",
                            nombre_comun = UrlParameter.Optional,
                            nombre_cientifico = UrlParameter.Optional
                        },
                        new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("LLENAR_DIR_LEGAL_X_ENTIDAD",
                       "General/LLENAR_DIR_LEGAL_X_ENTIDAD/{RUC}",
                       new
                       {
                           controller = "General",
                           action = "LLENAR_DIR_LEGAL_X_ENTIDAD",
                           RUC = UrlParameter.Optional
                       },
                       new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("LLENAR_PERSONA_LEGAL_X_ENTIDAD",
                       "General/LLENAR_PERSONA_LEGAL_X_ENTIDAD/{RUC}",
                       new
                       {
                           controller = "General",
                           action = "LLENAR_PERSONA_LEGAL_X_ENTIDAD",
                           RUC = UrlParameter.Optional
                       },
                       new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("LLENAR_PERSONA_LEGAL_X_DNI",
                       "General/LLENAR_PERSONA_LEGAL_X_DNI/{DNI}",
                       new
                       {
                           controller = "General",
                           action = "LLENAR_PERSONA_LEGAL_X_DNI",
                           DNI = UrlParameter.Optional
                       },
                       new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_codigo_almacen",
                           "General/Llenar_codigo_almacen/",
                           new { controller = "General", action = "Llenar_codigo_almacen" },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("Llenar_actividad_almacen",
                           "General/Llenar_actividad_almacen/",
                           new { controller = "General", action = "Llenar_actividad_almacen" },
                           new[] { "SIGESDOC.Web.Controllers" });


            routes.MapRoute("llenar_zona_produccion_x_ubigeo",
                           "General/llenar_zona_produccion_x_ubigeo/{ubigeo}",
                           new
                           {
                               controller = "General",
                               action = "llenar_zona_produccion_x_ubigeo ",
                               ubigeo = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            routes.MapRoute("llenar_area_produccion_x_zona_produccion",
                           "General/llenar_area_produccion_x_zona_produccion/{id_zona_produccion}",
                           new
                           {
                               controller = "General",
                               action = "llenar_area_produccion_x_zona_produccion ",
                               id_zona_produccion = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            //Add by HM
            routes.MapRoute("recupera_datos_del_SUNAT",
                           "Oficina/recupera_datos_del_SUNAT/{persona_num_documento}",
                           new
                           {
                               controller = "Oficina",
                               action = "recupera_datos_del_SUNAT",
                               persona_num_documento = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });

            //Add by HM
            routes.MapRoute("ObtieneUbigeo",
                           "Oficina/ObtieneUbigeo/{id_departamento}/{desc_Provincia}/{desc_Distrito}",
                           new
                           {
                               controller = "Oficina",
                               action = "ObtieneUbigeo",
                               id_departamento = UrlParameter.Optional,
                               desc_Provincia = UrlParameter.Optional,
                               desc_Distrito = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });



            /*
            routes.MapRoute("Seguimiento_finalizar",
                           "Habilitaciones/Seguimiento_finalizar/{id}/{tiempo_tramite}/{tiempo_sdhpa}/{observacion_final}/{inspector_designado}/{fecha_auditoria}/{fecha_acta}/{fecha_oficio}/{con_proceso}",
                           new
                           {
                               controller = "Habilitaciones",
                               action = "Seguimiento_finalizar",
                               id = UrlParameter.Optional,
                               tiempo_tramite = UrlParameter.Optional,
                               tiempo_sdhpa = UrlParameter.Optional, 
                               observacion_final = UrlParameter.Optional,
                               inspector_designado = UrlParameter.Optional,
                               fecha_auditoria = UrlParameter.Optional,
                               fecha_acta = UrlParameter.Optional,
                               fecha_oficio = UrlParameter.Optional,
                               con_proceso = UrlParameter.Optional
                           },
                           new[] { "SIGESDOC.Web.Controllers" });*/


        }
    }
}
