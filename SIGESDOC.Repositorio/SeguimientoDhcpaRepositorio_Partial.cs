
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Contexto;
using SIGESDOC.IRepositorio;
using SIGESDOC.Response;

namespace SIGESDOC.Repositorio
{
    public partial class SeguimientoDhcpaRepositorio : ISeguimientoDhcpaRepositorio
    {



        public DbGeneralMaeTransporteResponse actualizar_nuevo_transporte(int id_transporte, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MTRANS in _dataContext.SP_ACTUALIZAR_DB_GENERAL_MAE_TRANSPORTE(id_transporte, nueva_codigo_habilitacion, nueva_carroceria, tipo_furgon, nueva_carga_util, nueva_unidad_medida, usuario)

                          select new DbGeneralMaeTransporteResponse()
                           {
                               id_transporte = MTRANS.ID_TRANSPORTE,
                               placa = MTRANS.PLACA,
                               cod_habilitacion = MTRANS.COD_HABILITACION,
                               id_tipo_carroceria = MTRANS.ID_TIPO_CARROCERIA,
                               nombre_carroceria = MTRANS.NOMBRE_CARROCERIA,
                               id_um = MTRANS.ID_UM,
                               nombre_um = MTRANS.NOMBRE_UM,
                               siglas_um = MTRANS.SIGLAS_UM,
                               carga_util = MTRANS.CARGA_UTIL,
                               estado = MTRANS.ESTADO,
                               nombre_estado = MTRANS.NOMBRE_ESTADO
                           }).First();
            return result;
        }
            public DbGeneralMaeTransporteResponse registrar_nuevo_transporte(string nueva_placa, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MTRANS in _dataContext.SP_INSERTAR_DB_GENERAL_MAE_TRANSPORTE(nueva_placa, nueva_codigo_habilitacion, nueva_carroceria, tipo_furgon, nueva_carga_util, nueva_unidad_medida, usuario)

                          select new DbGeneralMaeTransporteResponse()
                           {
                               id_transporte = MTRANS.ID_TRANSPORTE,
                               placa = MTRANS.PLACA,
                               cod_habilitacion = MTRANS.COD_HABILITACION,
                               id_tipo_carroceria = MTRANS.ID_TIPO_CARROCERIA,
                               nombre_carroceria = MTRANS.NOMBRE_CARROCERIA,
                               id_um = MTRANS.ID_UM,
                               nombre_um = MTRANS.NOMBRE_UM,
                               siglas_um = MTRANS.SIGLAS_UM,
                               carga_util = MTRANS.CARGA_UTIL,
                               estado = MTRANS.ESTADO,
                               nombre_estado = MTRANS.NOMBRE_ESTADO
                           }).First();
            return result;
        }
        public IEnumerable<SeguimientoDhcpaObservacionesResponse> Listar_Observacion_x_seguimiento(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES

                          from VWDNI in _dataContext.vw_CONSULTAR_DNI
                          .Where(VWDNI => MSEG.USUARIO_CREA == VWDNI.persona_num_documento)
                          .DefaultIfEmpty() // <== makes join left join

                          where MSEG.ID_SEGUIMIENTO == id_seguimiento && MSEG.ACTIVO=="1"

                          select new SeguimientoDhcpaObservacionesResponse()
                          {
                              id_seg_dhcpa_observacion = MSEG.ID_SEG_DHCPA_OBSERVACION,
                              id_seguimiento = MSEG.ID_SEGUIMIENTO,
                              nombre_persona_crea = VWDNI.paterno + " " + VWDNI.materno + " " + VWDNI.nombres,
                              observacion = MSEG.OBSERVACION
                          };

            return result;
        }

        public IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> CONSULTA_HISTORIAL_EVALUADOR(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from HIS_EV in _dataContext.SP_CONSULTA_HISTORIAL_EVALUADOR(id_seguimiento)

                          select new Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result()
                          {
                              paterno = HIS_EV.paterno,
                              materno = HIS_EV.materno,
                              nombres = HIS_EV.nombres,
                              fecha_recibido = HIS_EV.FECHA_RECIBIDO,
                              estado = HIS_EV.ESTADO
                          }).AsEnumerable();
            return result;
        }
        public IEnumerable<ProtocoloResponse> lista_protocolo_x_id_transporte(int id_transporte)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from DATPROTRA in _dataContext.DAT_PROTOCOLO_TRANSPORTE

                         from MPRO in _dataContext.MAE_PROTOCOLO
                         .Where(MPRO => DATPROTRA.ID_PROTOCOLO == MPRO.ID_PROTOCOLO)
                         .DefaultIfEmpty() // <== makes join left join

                         where DATPROTRA.ID_TRANSPORTE == id_transporte

                         select new ProtocoloResponse()
                         {
                             id_protocolo = MPRO.ID_PROTOCOLO,
                             nombre = MPRO.NOMBRE,
                             fecha_inicio = MPRO.FECHA_INICIO,
                             fecha_fin = MPRO.FECHA_FIN,
                             activo = MPRO.ACTIVO,
                             id_ind_pro_esp = MPRO.ID_IND_PRO_ESP,
                             id_est_pro = MPRO.ID_EST_PRO
                         }).OrderByDescending(x => x.id_protocolo);

            return result;
        }

        public SeguimientoDhcpaResponse Consulta_Seguimiento_x_id_seguimiento(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA


                          //UNION EMPRESA
                          from VWDNI in _dataContext.vw_CONSULTAR_DNI
                          .Where(VWDNI => MSEG.PERSONA_NUM_DOCUMENTO == VWDNI.persona_num_documento)
                          .DefaultIfEmpty() // <== makes join left join

                          from MDNIPL in _dataContext.VW_CONSULTAR_DNI_PERSONAL_LEGAL
                          .Where(MDNIPL => VWDNI.persona_num_documento == MDNIPL.DNI && MDNIPL.ACTIVO == 1)
                          .DefaultIfEmpty() // <== makes join left join

                          //UNION EMPRESA
                          from MOFDIR in _dataContext.vw_CONSULTAR_DIRECCION
                          .Where(MOFDIR => MSEG.ID_OFI_DIR == MOFDIR.ID_OFICINA_DIRECCION)
                          .DefaultIfEmpty() // <== makes join left join

                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(MOFIC => MOFDIR.ID_OFICINA == MOFIC.ID_OFICINA)
                          .DefaultIfEmpty() // <== makes join left join

                          from MSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                          .Where(MSEDE => MOFDIR.ID_SEDE == MSEDE.ID_SEDE)
                          .DefaultIfEmpty() // <== makes join left join

                          from MEPL in _dataContext.VW_CONSULTAR_EMPRESA_PERSONA_LEGAL
                          .Where(MEPL => MOFIC.RUC == MEPL.RUC && MEPL.ACTIVO == 1)
                          .DefaultIfEmpty() // <== makes join left join

                          from MODL in _dataContext.VW_CONSULTAR_OFICINA_DIRECCION_LEGAL
                          .Where(MODL => MOFIC.RUC == MODL.RUC && MODL.ACTIVO == 1)
                          .DefaultIfEmpty() // <== makes join left join

                          from MOFI_PADRE in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(MOFI_PADRE => MOFIC.RUC == MOFI_PADRE.RUC && MOFI_PADRE.ID_OFI_PADRE == null)
                          .DefaultIfEmpty() // <== makes join left join

                          //UNION EXPEDIENTE
                          from MEX in _dataContext.MAE_EXPEDIENTES
                          .Where(MEX => MSEG.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                          .DefaultIfEmpty() // <== makes join left join

                          from MTX in _dataContext.MAE_TIPO_EXPEDIENTE
                          .Where(MTX => MEX.ID_TIPO_EXPEDIENTE == MTX.ID_TIPO_EXPEDIENTE)
                          .DefaultIfEmpty() // <== makes join left join

                          where MSEG.ID_SEGUIMIENTO == id_seguimiento

                          select new SeguimientoDhcpaResponse()
                          {
                              id_seguimiento = MSEG.ID_SEGUIMIENTO,
                              id_habilitante = MSEG.ID_HABILITANTE,
                              id_tipo_seguimiento = MSEG.ID_TIPO_SEGUIMIENTO,
                              persona_num_documento = MSEG.PERSONA_NUM_DOCUMENTO,
                              nom_persona_ext = MSEG.ID_OFI_DIR != null ? "" : VWDNI.paterno + " " + VWDNI.materno + " " + VWDNI.nombres,
                              id_sede_ext = MSEG.ID_OFI_DIR == null ? 0 : MSEDE.ID_SEDE,
                              ruc = MSEG.ID_OFI_DIR == null ? "" : MOFIC.RUC,
                              nom_direccion_ext = MSEG.ID_OFI_DIR == null ? "" : MSEDE.DIRECCION,
                              str_direccion_persona_natural = VWDNI.direccion,
                              id_ofi_dir = MSEG.ID_OFI_DIR,
                              Expediente = MEX.NOM_EXPEDIENTE,
                              nom_tipo_expediente = MTX.NOMBRE,
                              Nom_direccion_legal = MSEG.ID_OFI_DIR == null ? "" : MODL.DIRECCION,
                              Nom_persona_legal = MSEG.ID_OFI_DIR == null ? "" : MEPL.NOMBRES_Y_APELLIDOS,
                              correo_legal = MSEG.ID_OFI_DIR == null ? "" : MEPL.CORREO,
                              telefono_legal = MSEG.ID_OFI_DIR == null ? "" : MEPL.TELEFONO,
                              nom_oficina_ext = MSEG.ID_OFI_DIR == null ? "" : MOFI_PADRE.NOMBRE,
                              id_direccion_legal = MSEG.ID_OFI_DIR == null ? 0 : MODL.ID_OFICINA_DIRECCION_LEGAL,
                              id_persona_legal = MSEG.ID_OFI_DIR == null ? 0 : MEPL.ID_PERSONA_LEGAL,
                              id_dni_persona_legal = MSEG.ID_OFI_DIR != null ? 0 : MDNIPL.ID_DNI_PERSONA_LEGAL,
                              Nom_persona_legal_DNI = MSEG.ID_OFI_DIR != null ? "" : MDNIPL.NOMBRES_Y_APELLIDOS,
                              telefono_legal_DNI = MSEG.ID_OFI_DIR != null ? "" : MDNIPL.TELEFONO,
                              correo_legal_DNI = MSEG.ID_OFI_DIR != null ? "" : MDNIPL.CORREO
                          }).First();

            return result;
        }
        public IEnumerable<SeguimientoDhcpaResponse> Consulta_Seguimiento(string persona_num_documento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (persona_num_documento != "")
            {
                var result = (from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA

                             //UNION EXPEDIENTE
                             from MEX in _dataContext.MAE_EXPEDIENTES
                             .Where(MEX => MSEG.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                             .DefaultIfEmpty() // <== makes join left join

                             from MTX in _dataContext.MAE_TIPO_EXPEDIENTE
                             .Where(MTX => MEX.ID_TIPO_EXPEDIENTE == MTX.ID_TIPO_EXPEDIENTE)
                             .DefaultIfEmpty() // <== makes join left join

                             from MTP in _dataContext.MAE_TIPO_PROCEDIMIENTO
                             .Where(MTP => MSEG.ID_TIPO_PROCEDIMIENTO == MTP.ID_TIPO_PROCEDIMIENTO)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCD in _dataContext.vw_CONSULTAR_DIRECCION
                             .Where(VCD => MSEG.ID_OFI_DIR == VCD.ID_OFICINA_DIRECCION)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCO in _dataContext.vw_CONSULTAR_OFICINA
                             .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCS in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                             .Where(VCS => VCD.ID_SEDE == VCS.ID_SEDE)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCDNI in _dataContext.vw_CONSULTAR_DNI
                             .Where(VCDNI => MSEG.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                             .Where(VCEMB => MSEG.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                             .DefaultIfEmpty() // <== makes join left join

                             where MSEG.EVALUADOR == persona_num_documento && MSEG.ESTADO != "3" && MSEG.ESTADO != "4"

                             select new SeguimientoDhcpaResponse()
                             {
                                 id_seguimiento = MSEG.ID_SEGUIMIENTO,
                                 fecha_inicio = MSEG.FECHA_INICIO,
                                 Expediente = MEX.NOM_EXPEDIENTE,
                                 nom_tipo_expediente = MTX.NOMBRE,
                                 nom_tipo_procedimiento = MTP.NOMBRE,
                                 ruc = VCO.RUC,
                                 nom_oficina_ext = VCO.NOMBRE != null ? VCO.NOMBRE + " - " + VCS.NOMBRE : null,
                                 persona_num_documento = MSEG.PERSONA_NUM_DOCUMENTO,
                                 nom_persona_ext = VCDNI.paterno != null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : null,
                                 nom_embarcacion = VCDNI.paterno != null ? VCEMB.MATRICULA +"-"+VCEMB.NOMBRE : null
                             }).OrderBy(x => x.fecha_inicio);
                return result;
            }
            else
            {
                var result = (from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA

                             //UNION EXPEDIENTE
                             from MEX in _dataContext.MAE_EXPEDIENTES
                             .Where(MEX => MSEG.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                             .DefaultIfEmpty() // <== makes join left join

                             from MTX in _dataContext.MAE_TIPO_EXPEDIENTE
                             .Where(MTX => MEX.ID_TIPO_EXPEDIENTE == MTX.ID_TIPO_EXPEDIENTE)
                             .DefaultIfEmpty() // <== makes join left join

                             from MTP in _dataContext.MAE_TIPO_PROCEDIMIENTO
                             .Where(MTP => MSEG.ID_TIPO_PROCEDIMIENTO == MTP.ID_TIPO_PROCEDIMIENTO)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCD in _dataContext.vw_CONSULTAR_DIRECCION
                             .Where(VCD => MSEG.ID_OFI_DIR == VCD.ID_OFICINA_DIRECCION)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCO in _dataContext.vw_CONSULTAR_OFICINA
                             .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCS in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                             .Where(VCS => VCD.ID_SEDE == VCS.ID_SEDE)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCDNI in _dataContext.vw_CONSULTAR_DNI
                             .Where(VCDNI => MSEG.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                             .DefaultIfEmpty() // <== makes join left join

                             from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                             .Where(VCEMB => MSEG.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                             .DefaultIfEmpty() // <== makes join left join

                              where MSEG.ESTADO != "3" && MSEG.ESTADO != "4"

                             select new SeguimientoDhcpaResponse()
                             {
                                 id_seguimiento = MSEG.ID_SEGUIMIENTO,
                                 Expediente = MEX.NOM_EXPEDIENTE,
                                 fecha_inicio = MSEG.FECHA_INICIO,
                                 nom_tipo_expediente = MTX.NOMBRE,
                                 nom_tipo_procedimiento = MTP.NOMBRE,
                                 ruc = VCO.RUC,
                                 nom_oficina_ext = VCO.NOMBRE != null ? VCO.NOMBRE + " - " + VCS.NOMBRE : null,
                                 persona_num_documento = MSEG.PERSONA_NUM_DOCUMENTO,
                                 nom_persona_ext = VCDNI.paterno != null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : null,
                                 nom_embarcacion = VCDNI.paterno != null ? VCEMB.MATRICULA + "-" + VCEMB.NOMBRE : null
                             }).OrderBy(x => x.fecha_inicio);
                return result;
            }
        }

        public IEnumerable<DocumentoSeguimientoAdjuntoResponse> lita_documento_seguimiento_x_documento_seg(int id_documento_seg)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

                var result = (from MDOC in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO_ADJUNTO

                              //UNION EXPEDIENTE
                              from MTIAD in _dataContext.MAE_TIPO_DOCUMENTO_SEGUIMIENTO_ADJUNTO
                              .Where(MTIAD => MDOC.ID_TIPO_DOC_SEG_ADJUNTO == MTIAD.ID_TIPO_DOC_SEG_ADJUNTO)
                              .DefaultIfEmpty() // <== makes join left join

                              where MDOC.ACTIVO == "1" && MDOC.ID_DOCUMENTO_SEG == id_documento_seg

                              select new DocumentoSeguimientoAdjuntoResponse()
                              {
                                  id_doc_seg_adjunto = MDOC.ID_DOC_SEG_ADJUNTO,
                                  tipo_documento_seguimiento_adjunto = new TipoDocumentoSeguimientoAdjuntoResponse()
                                  {
                                      nombre = MTIAD.NOMBRE
                                  }
                              });
                return result;
        }
        /*
        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta(int pageIndex, int pageSize, string expediente, string evaluador, string externo, string matricula, string cmbestado)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            if(matricula.Trim()!="")
            {
                #region con matricula
                if (externo.Trim() != "")
                {
                    #region con externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCOF.NOMBRE.Contains(externo)
                                          && VCEMB.MATRICULA.Contains(matricula)
                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region sin externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region sin matricula
                if (externo.Trim() != "")
                {
                    #region con externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCOF.NOMBRE.Contains(externo)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCOF.NOMBRE.Contains(externo) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCOF.NOMBRE.Contains(externo)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCOF.NOMBRE.Contains(externo) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region sin externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from MTUPA in _dataContext.MAE_TUPA
                                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              num_tupa = MTUPA.NUMERO,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                              cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).OrderByDescending(r => r.id_seguimiento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
                            return result;
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
        }
        */

        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_sin_paginado(string expediente, string evaluador, string externo, string habilitante, string cmbestado, int id_oficina_filtro, int id_tupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
             
            if(id_oficina_filtro==0)
            {
                var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                              from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                              .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                              .DefaultIfEmpty()

                              from MTUPA in _dataContext.MAE_TUPA
                              .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                              .DefaultIfEmpty()

                              from MTTUPA in _dataContext.MAE_TIPO_TUPA
                              .Where(MTTUPA => MTUPA.ID_TIPO_TUPA == MTTUPA.ID_TIPO_TUPA)
                              .DefaultIfEmpty()

                              from MEX in _dataContext.MAE_EXPEDIENTES
                              .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                              .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                              .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                              .DefaultIfEmpty()

                              from VCEV in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                              .DefaultIfEmpty()


                              from MTSEG in _dataContext.MAE_TIPO_SEGUIMIENTO
                              .Where(MTSEG => MSDHCPA.ID_TIPO_SEGUIMIENTO == MTSEG.ID_TIPO_SEGUIMIENTO)
                              .DefaultIfEmpty()

                              where (expediente == "" || (expediente != "" && (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE).Contains(expediente))) &&
                                (evaluador == "" || (evaluador != "" && MSDHCPA.EVALUADOR == evaluador)) &&
                                (cmbestado == "" || (cmbestado != "" && MAESTDHC.ID_ESTADO.Contains(cmbestado))) &&
                                (externo == "" || (externo != "" && MSDHCPA.NOMBRE_EXTERNO.Contains(externo))) &&
                                (habilitante == "" || (habilitante != "" && MSDHCPA.COD_HABILITANTE.Contains(habilitante))) &&
                                (id_tupa == 0 || (id_tupa != 0 && MTUPA.ID_TUPA == id_tupa))
                                && MAESTDHC.ID_ESTADO != "4"
                              select new SeguimientoDhcpaResponse
                              {
                                  id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                  id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                  Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.ID_TIPO_EXPEDIENTE == 90 ? MEX.NOM_EXPEDIENTE : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE)),
                                  tupa = MSDHCPA.TUPA,
                                  num_tupa = MTUPA.NUMERO,
                                  num_tupa_cadena = MTUPA.NUMERO == null ? "" : MTUPA.NUMERO.ToString(),
                                  nom_tipo_tupa = MTTUPA.NOMBRE,
                                  id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                  nom_tipo_procedimiento = MTPRO.NOMBRE,
                                  fecha_inicio = MSDHCPA.FECHA_INICIO,
                                  fecha_fin = MSDHCPA.FECHA_FIN,
                                  id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                  nom_oficina_ext = MSDHCPA.NOMBRE_EXTERNO,
                                  nom_estado = MAESTDHC.NOMBRE,
                                  //ruc = VCOF.RUC,
                                  persona_num_documento = MSDHCPA.PERSONA_NUM_DOCUMENTO,
                                  nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                  cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false,
                                  cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                  cond_planta = (MSDHCPA.ID_TIPO_SEGUIMIENTO == 1 && MSDHCPA.ID_HABILITANTE == 0) ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                  cond_embarcacion = (MSDHCPA.ID_TIPO_SEGUIMIENTO == 2 && MSDHCPA.ID_HABILITANTE == 0) ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                  id_tipo_seguimiento = MSDHCPA.ID_TIPO_SEGUIMIENTO,
                                  cond_habilitante = (MSDHCPA.ID_HABILITANTE == 0 ? true : false),
                                  cod_habilitante = ((MSDHCPA.ID_TIPO_SEGUIMIENTO != 0 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 7 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 8) ? ((MSDHCPA.COD_HABILITANTE == "" || MSDHCPA.COD_HABILITANTE == null) ? "" : MTSEG.NOMBRE + " : " + MSDHCPA.COD_HABILITANTE) : ""),
                                  duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                  duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                  observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                  cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              }).OrderByDescending(r => r.id_seguimiento).Take(200).AsEnumerable();
                return result;
            }
            else
            {
                var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                              from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                              .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                              .DefaultIfEmpty()

                              from MTUPA in _dataContext.MAE_TUPA
                              .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                              .DefaultIfEmpty()

                              from MTTUPA in _dataContext.MAE_TIPO_TUPA
                              .Where(MTTUPA => MTUPA.ID_TIPO_TUPA == MTTUPA.ID_TIPO_TUPA)
                              .DefaultIfEmpty()

                              from MEX in _dataContext.MAE_EXPEDIENTES
                              .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                              .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                              .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                              .DefaultIfEmpty()

                              from VCEV in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                              .DefaultIfEmpty()


                              from MTSEG in _dataContext.MAE_TIPO_SEGUIMIENTO
                              .Where(MTSEG => MSDHCPA.ID_TIPO_SEGUIMIENTO == MTSEG.ID_TIPO_SEGUIMIENTO)
                              .DefaultIfEmpty()

                              where (expediente == "" || (expediente != "" && (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE).Contains(expediente))) &&
                                (evaluador == "" || (evaluador != "" && MSDHCPA.EVALUADOR == evaluador)) &&
                                (cmbestado == "" || (cmbestado != "" && MAESTDHC.ID_ESTADO.Contains(cmbestado))) &&
                                (externo == "" || (externo != "" && MSDHCPA.NOMBRE_EXTERNO.Contains(externo))) &&
                                (habilitante == "" || (habilitante != "" && MSDHCPA.COD_HABILITANTE.Contains(habilitante))) &&
                                (id_tupa == 0 || (id_tupa != 0 && MTUPA.ID_TUPA == id_tupa))
                                && MAESTDHC.ID_ESTADO != "4"
                                && MSDHCPA.OFICINA_CREA==id_oficina_filtro
                              select new SeguimientoDhcpaResponse
                              {
                                  id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                  id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                  Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.ID_TIPO_EXPEDIENTE == 90 ? MEX.NOM_EXPEDIENTE : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE)),
                                  tupa = MSDHCPA.TUPA,
                                  num_tupa = MTUPA.NUMERO,
                                  num_tupa_cadena = MTUPA.NUMERO == null ? "" : MTUPA.NUMERO.ToString(),
                                  nom_tipo_tupa = MTTUPA.NOMBRE,
                                  id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                  nom_tipo_procedimiento = MTPRO.NOMBRE,
                                  fecha_inicio = MSDHCPA.FECHA_INICIO,
                                  fecha_fin = MSDHCPA.FECHA_FIN,
                                  id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                  nom_oficina_ext = MSDHCPA.NOMBRE_EXTERNO,
                                  nom_estado = MAESTDHC.NOMBRE,
                                  //ruc = VCOF.RUC,
                                  persona_num_documento = MSDHCPA.PERSONA_NUM_DOCUMENTO,
                                  nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                  cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false,
                                  cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                  cond_planta = (MSDHCPA.ID_TIPO_SEGUIMIENTO == 1 && MSDHCPA.ID_HABILITANTE == 0) ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                  cond_embarcacion = (MSDHCPA.ID_TIPO_SEGUIMIENTO == 2 && MSDHCPA.ID_HABILITANTE == 0) ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,

                                  id_tipo_seguimiento = MSDHCPA.ID_TIPO_SEGUIMIENTO,
                                  cond_habilitante = (MSDHCPA.ID_HABILITANTE == 0 ? true : false),

                                  cod_habilitante = ((MSDHCPA.ID_TIPO_SEGUIMIENTO != 0 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 7 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 8) ? ((MSDHCPA.COD_HABILITANTE == "" || MSDHCPA.COD_HABILITANTE == null) ? "" : MTSEG.NOMBRE + " : " + MSDHCPA.COD_HABILITANTE) : ""),
                                  duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                  duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                  observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                  cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              }).OrderByDescending(r => r.id_seguimiento).Take(200).AsEnumerable();
                return result;
            }
            
        }

        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_excel(int id_oficina) 
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (id_oficina == 18) {
                var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                              from VCODIROFICREA in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VCODIROFICREA => MSDHCPA.OFICINA_CREA == VCODIROFICREA.ID_OFICINA_DIRECCION)
                              .DefaultIfEmpty()

                              from VCOFOFICREA in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOFOFICREA => VCODIROFICREA.ID_OFICINA == VCOFOFICREA.ID_OFICINA)
                              .DefaultIfEmpty()

                              from VCSEDEOFICREA in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VCSEDEOFICREA => VCODIROFICREA.ID_SEDE == VCSEDEOFICREA.ID_SEDE)
                              .DefaultIfEmpty()

                              from VCDNIUSUCREA in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCDNIUSUCREA => MSDHCPA.PERSONA_CREA == "20565429656 - " + VCDNIUSUCREA.persona_num_documento)
                              .DefaultIfEmpty()

                              from MTISEG in _dataContext.MAE_TIPO_SEGUIMIENTO
                              .Where(MTISEG => MSDHCPA.ID_TIPO_SEGUIMIENTO == MTISEG.ID_TIPO_SEGUIMIENTO)
                              .DefaultIfEmpty()

                              from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                              .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                              .DefaultIfEmpty()

                              from MTUPA in _dataContext.MAE_TUPA
                              .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                              .DefaultIfEmpty()


                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                              .Where(TTUPA => MTUPA.ID_TIPO_TUPA == TTUPA.ID_TIPO_TUPA)
                              .DefaultIfEmpty()

                              from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                              .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                              .DefaultIfEmpty()

                              from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                              .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                              .DefaultIfEmpty()

                              from MEX in _dataContext.MAE_EXPEDIENTES
                              .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                              .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                              .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                              .DefaultIfEmpty()

                              from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                              .DefaultIfEmpty()

                              from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                              .DefaultIfEmpty()

                              from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                              .DefaultIfEmpty()

                              from VCDNI in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                              .DefaultIfEmpty()

                              from VCEV in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                              .DefaultIfEmpty()

                              select new SeguimientoDhcpaResponse
                              {
                                  id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                  excel_usuario_crea = VCDNIUSUCREA.paterno + " " + VCDNIUSUCREA.materno + " " + VCDNIUSUCREA.nombres,
                                  excel_oficina_crea = VCOFOFICREA.NOMBRE,
                                  excel_sede_crea = VCSEDEOFICREA.NOMBRE,
                                  id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                  Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.ID_TIPO_EXPEDIENTE == 90 ? MEX.NOM_EXPEDIENTE : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE)),
                                  tupa = MSDHCPA.TUPA,
                                  num_tupa = MTUPA.NUMERO,
                                  nom_tipo_tupa = TTUPA.NOMBRE,
                                  id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                  nom_tipo_procedimiento = MTPRO.NOMBRE,
                                  fecha_inicio = MSDHCPA.FECHA_INICIO,
                                  fecha_fin = MSDHCPA.FECHA_FIN,
                                  id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                  nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                  nom_estado = MAESTDHC.NOMBRE,
                                  nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                  nom_tipo_seguimiento = MTISEG.NOMBRE,
                                  cod_habilitante = MSDHCPA.COD_HABILITANTE,
                                  cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                  cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                  cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                  duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                  duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                  observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                  cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              }).OrderByDescending(r => r.id_seguimiento).AsEnumerable();
                return result;
            }
            else
            {
                var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA


                              from VCODIROFICREA in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VCODIROFICREA => MSDHCPA.OFICINA_CREA == VCODIROFICREA.ID_OFICINA_DIRECCION)
                              .DefaultIfEmpty()

                              from VCOFOFICREA in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOFOFICREA => VCODIROFICREA.ID_OFICINA == VCOFOFICREA.ID_OFICINA)
                              .DefaultIfEmpty()

                              from VCSEDEOFICREA in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VCSEDEOFICREA => VCODIROFICREA.ID_SEDE == VCSEDEOFICREA.ID_SEDE)
                              .DefaultIfEmpty()

                              from VCDNIUSUCREA in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCDNIUSUCREA => MSDHCPA.PERSONA_CREA == "20565429656 - " + VCDNIUSUCREA.persona_num_documento)
                              .DefaultIfEmpty()

                              from MTISEG in _dataContext.MAE_TIPO_SEGUIMIENTO
                              .Where(MTISEG => MSDHCPA.ID_TIPO_SEGUIMIENTO == MTISEG.ID_TIPO_SEGUIMIENTO)
                              .DefaultIfEmpty()

                              from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                              .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                              .DefaultIfEmpty()

                              from MTUPA in _dataContext.MAE_TUPA
                              .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                              .DefaultIfEmpty()

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                              .Where(TTUPA => MTUPA.ID_TIPO_TUPA == TTUPA.ID_TIPO_TUPA)
                              .DefaultIfEmpty()

                              from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                              .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                              .DefaultIfEmpty()

                              from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                              .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                              .DefaultIfEmpty()

                              from MEX in _dataContext.MAE_EXPEDIENTES
                              .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                              .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                              .DefaultIfEmpty()

                              from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                              .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                              .DefaultIfEmpty()

                              from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                              .DefaultIfEmpty()

                              from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                              .DefaultIfEmpty()

                              from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                              .DefaultIfEmpty()

                              from VCDNI in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                              .DefaultIfEmpty()

                              from VCEV in _dataContext.vw_CONSULTAR_DNI
                              .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento
                              )
                              .DefaultIfEmpty()

                              where MSDHCPA.OFICINA_CREA == id_oficina
                              select new SeguimientoDhcpaResponse
                              {
                                  id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                  excel_usuario_crea = VCDNIUSUCREA.paterno + " " + VCDNIUSUCREA.materno + " " + VCDNIUSUCREA.nombres,
                                  excel_oficina_crea = VCOFOFICREA.NOMBRE,
                                  excel_sede_crea = VCSEDEOFICREA.NOMBRE,
                                  id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                  Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.ID_TIPO_EXPEDIENTE == 90 ? MEX.NOM_EXPEDIENTE : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE)),
                                  tupa = MSDHCPA.TUPA,
                                  num_tupa = MTUPA.NUMERO,
                                  nom_tipo_tupa = TTUPA.NOMBRE,
                                  id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                  nom_tipo_procedimiento = MTPRO.NOMBRE,
                                  fecha_inicio = MSDHCPA.FECHA_INICIO,
                                  fecha_fin = MSDHCPA.FECHA_FIN,
                                  id_ofi_dir = MSDHCPA.ID_OFI_DIR == null ? 0 : MSDHCPA.ID_OFI_DIR,
                                  nom_oficina_ext = VCOF.NOMBRE == null ? (VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres) : (VCOF.NOMBRE + " - " + VCSEDE.NOMBRE),
                                  nom_estado = MAESTDHC.NOMBRE,
                                  nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                  nom_tipo_seguimiento = MTISEG.NOMBRE,
                                  cod_habilitante = MSDHCPA.COD_HABILITANTE,
                                  cond_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                  cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                  cond_planta = MSDHCPA.ID_PLANTA == 0 ? (MSDHCPA.ID_OFI_DIR == null ? false : true) : false,
                                  duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                  duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                  observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                                  cond_no_tiene_expediente = (MSDHCPA.ID_EXPEDIENTE == null || MSDHCPA.ID_EXPEDIENTE == 0) ? true : false
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              }).OrderByDescending(r => r.id_seguimiento).AsEnumerable();
                return result;
            }

            
        }
        /*
        public int CountSeguimiento_Consulta(string expediente, string evaluador, string externo, string matricula, string cmbestado)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            if(matricula.Trim()!="")
            {
                #region con matricula
                if (externo.Trim() != "")
                {
                    #region con externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)
                                          && VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula)
                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion

                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCOF.NOMBRE.Contains(externo) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region sin externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion

                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado) && VCEMB.MATRICULA.Contains(matricula)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCEMB.MATRICULA.Contains(matricula) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            else
            {
                #region sin matricula
                if (externo.Trim() != "")
                {
                    #region con externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)
                                          && VCOF.NOMBRE.Contains(externo)
                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && VCOF.NOMBRE.Contains(externo) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion

                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && VCOF.NOMBRE.Contains(externo) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where VCOF.NOMBRE.Contains(externo) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                else
                {
                    #region sin externo
                    if (expediente.Trim() != "")
                    {
                        #region con expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MEX.NOM_EXPEDIENTE.Contains(expediente) && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion

                    }
                    else
                    {
                        #region sin expediente
                        if (evaluador != "")
                        {
                            #region con evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MSDHCPA.EVALUADOR == evaluador && MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        else
                        {
                            #region sin evaluador
                            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                                          .DefaultIfEmpty()

                                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                                          .DefaultIfEmpty()

                                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                                          .DefaultIfEmpty()

                                          from MEX in _dataContext.MAE_EXPEDIENTES
                                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                                          .DefaultIfEmpty()

                                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                                          .DefaultIfEmpty()

                                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                                          .DefaultIfEmpty()

                                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                                          .DefaultIfEmpty()

                                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                                          .DefaultIfEmpty()

                                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                                          .DefaultIfEmpty()

                                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                                          .DefaultIfEmpty()

                                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                                          .DefaultIfEmpty()

                                          where MAESTDHC.ID_ESTADO.Contains(cmbestado)

                                          select new SeguimientoDhcpaResponse
                                          {
                                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                                              tupa = MSDHCPA.TUPA,
                                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                                              fecha_fin = MSDHCPA.FECHA_FIN,
                                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                                              nom_estado = MAESTDHC.NOMBRE,
                                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES
                                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                          }).AsEnumerable();
                            return result.Count();
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }

           
          
        }
        */
        public IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa(string evaluador, int tipo_doc_dhcpa, string asunto, int anno)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

                    var result = (from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA

                                  from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                  .Where(MTDOC => MDCHPA.ID_TIPO_DOCUMENTO == MTDOC.ID_TIPO_DOCUMENTO)
                                  .DefaultIfEmpty()

                                  where (evaluador=="" || (evaluador!="" && MDCHPA.USUARIO_REGISTRO == evaluador)) &&
                                  (tipo_doc_dhcpa==0 || (tipo_doc_dhcpa!=0 && MDCHPA.ID_TIPO_DOCUMENTO == tipo_doc_dhcpa)) && 
                                  (asunto.Trim() == "" || (asunto.Trim() != "" && MDCHPA.ASUNTO.Contains(asunto.Trim()))) &&
                                  anno == MDCHPA.FECHA_DOC.Value.Year

                                  select new DocumentoDhcpaResponse
                                  {
                                      id_doc_dhcpa = MDCHPA.ID_DOC_DHCPA,
                                      fecha_doc = MDCHPA.FECHA_DOC,
                                      num_doc = MDCHPA.NUM_DOC,
                                      nom_doc = MDCHPA.NOM_DOC,
                                      nom_tipo_documento = MTDOC.NOMBRE,
                                      asunto = MDCHPA.ASUNTO,
                                      anexos = MDCHPA.ANEXOS,
                                      pdf = MDCHPA.PDF,
                                      id_oficina_direccion = MDCHPA.ID_OFICINA_DIRECCION,

                                      //Add by HM - 13/11/2019
                                     
                                      ruc = MDCHPA.RUC,
                                      evaluador_cdl_notif = MDCHPA.EVALUADOR_CDL_NOTIF,
                                      direccion_cdl_notif = MDCHPA.DIRECCION_CDL_NOTIF,
                                      empresa_cdl_notif = MDCHPA.EMPRESA_CDL_NOTIF,
                                      folia_cdl_notif = MDCHPA.FOLIA_CDL_NOTIF,
                                      doc_notificar_cdl_notif = MDCHPA.DOC_NOTIFICAR_CDL_NOTIF,
                                      exp_o_ht_cdl_notif = MDCHPA.EXP_O_HT_CDL_NOTIF,
                                      exp_o_ht_n_cdl_notif = MDCHPA.EXP_O_HT_N_CDL_NOTIF

                                      //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                                  }).OrderByDescending(r => r.id_doc_dhcpa).Take(500).AsEnumerable();
                    return result;      
        }

        public IEnumerable<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result> consulta_correo_x_solicitud(int id_solicitud)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from sp_data in _dataContext.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA(id_solicitud)

                          select new Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result
                          {
                              correo_responsable = sp_data.CORREO_RESPONSABLE,
                              persona_num_documento = sp_data.PERSONA_NUM_DOCUMENTO,
                              id_cargo = sp_data.ID_CARGO,
                              nombre_cargo = sp_data.NOMBRE_CARGO
                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                          }).Take(10).AsEnumerable();
            return result;
        }

        public string enviar_correo_notificacion_solicitud_sdhpa(int id_solicitud, string destinos)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            _dataContext.SP_CORREO_ALERTA_GENERA_SI(id_solicitud, destinos);

            return "ok";
        }

        public IEnumerable<DocumentoDhcpaResponse> Lista_destino_documentos_dhcpa(int id_documento_dhcpa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

                var result = (from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA

                              from DDDD in _dataContext.DAT_DOCUMENTO_DHCPA_DETALLE
                              .Where(DDDD => MDCHPA.ID_DOC_DHCPA == DDDD.ID_DOC_DHCPA)
                              .DefaultIfEmpty()

                              from VCD in _dataContext.vw_CONSULTAR_DIRECCION
                              .Where(VCD => DDDD.ID_OFICINA_DIRECCION_DESTINO == VCD.ID_OFICINA_DIRECCION)
                              .DefaultIfEmpty()

                              from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                              .Where(VCOF => VCD.ID_OFICINA == VCOF.ID_OFICINA)
                              .DefaultIfEmpty()

                              from VCSED in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                              .Where(VCSED => VCD.ID_SEDE == VCSED.ID_SEDE)
                              .DefaultIfEmpty()

                              where MDCHPA.ID_DOC_DHCPA==id_documento_dhcpa

                              select new DocumentoDhcpaResponse
                              {
                                  lugar_destino = DDDD.ID_OFICINA_DIRECCION_DESTINO==0 ? "---" : VCOF.NOMBRE + "(SEDE :" + VCSED.NOMBRE + ")",
                                  persona_destino = DDDD.NOMBRE_PERSONA_DESTINO
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              }).AsEnumerable();
                return result;
        }

        public IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_dhcpa()
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA


                          from MAESTDHC in _dataContext.MAE_ESTADO_SEGUIMIENTO_DHCPA
                          .Where(MAESTDHC => MSDHCPA.ESTADO == MAESTDHC.ID_ESTADO)
                          .DefaultIfEmpty()

                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                          .Where(VCPL => MSDHCPA.ID_PLANTA == VCPL.ID_PLANTA)
                          .DefaultIfEmpty()

                          from MSOLI in _dataContext.MAE_SOLICITUD_INSPECCION
                          .Where(MSOLI => MSDHCPA.ID_SEGUIMIENTO == MSOLI.ID_SEGUIMIENTO)

                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                          .DefaultIfEmpty()

                          from MEX in _dataContext.MAE_EXPEDIENTES
                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                          .DefaultIfEmpty()

                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                          .DefaultIfEmpty()

                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                          .DefaultIfEmpty()

                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                          .Where(VCEMB => MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                          .DefaultIfEmpty()

                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                          .DefaultIfEmpty()


                          from MTSEG in _dataContext.MAE_TIPO_SEGUIMIENTO
                          .Where(MTSEG => MSDHCPA.ID_TIPO_SEGUIMIENTO == MTSEG.ID_TIPO_SEGUIMIENTO)
                          .DefaultIfEmpty()

                          from MTUP in _dataContext.MAE_TUPA
                          .Where(MTUP => MSDHCPA.TUPA == MTUP.ID_TUPA)
                          .DefaultIfEmpty()

                          select new SeguimientoDhcpaResponse
                          {
                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                              tupa = MSDHCPA.TUPA,
                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                              fecha_fin = MSDHCPA.FECHA_FIN,
                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                              num_tupa = MTUP.NUMERO,
                              nom_oficina_ext = MSDHCPA.NOMBRE_EXTERNO,
                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                              nom_estado = MAESTDHC.NOMBRE,
                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,
                              nom_planta = MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + "-" + VCPL.NUMERO_PLANTA + "-" + VCPL.NOMBRE_PLANTA),
                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                              cod_habilitante = ((MSDHCPA.ID_TIPO_SEGUIMIENTO != 0 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 7 && MSDHCPA.ID_TIPO_SEGUIMIENTO != 8) ? MTSEG.NOMBRE + " : " + MSDHCPA.COD_HABILITANTE : ""),
                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                              num_solicitud_dhcpa = " Solicitud N° " + MSOLI.NUMERO_DOCUMENTO,
                              fecha_solicitud_dhcpa = MSOLI.FECHA_CREA
                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                          }).OrderByDescending(r => r.fecha_solicitud_dhcpa).Take(500).AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoSeguimientoResponse> Lista_Documento_OD_pendientes_x_recibir(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, string expediente)
        {
             
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.SP_CONSULTAR_EXPEDIENTES_X_DOCUMENTO_HABILITACIONES_OD(estado, indicador, asunto, externo, nom_doc, evaluador, id_tipo_documento, num_doc, expediente)
                          select new DocumentoSeguimientoResponse
                          {
                              id_documento_seg = MDS.ID_DOCUMENTO_SEG,
                              id_tipo_documento = MDS.ID_TIPO_DOCUMENTO,
                              fecha_crea = MDS.FECHA_CREA,
                              fecha_documento = MDS.FECHA_DOCUMENTO,
                              tipo_documento = new TipoDocumentoResponse
                              {
                                  nombre = MDS.NOMBRE_TIPO_DOCUMENTO
                              },
                              nom_externo = MDS.NOMBRE_EXTERNO,
                              asunto = MDS.ASUNTO,
                              num_documento = MDS.NUM_DOCUMENTO,
                              nom_documento = MDS.NOMBRE_DOCUMENTO,
                              evaluador = MDS.EVALUADOR,
                              group_expedientes = MDS.EXPEDIENTES,
                              ruta_pdf = MDS.RUTA_PDF
                          }).Distinct().OrderByDescending(r => r.id_documento_seg).Take(500).AsEnumerable();
            return result;
        }
        
        public IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento(int id_tipo_documento, int anno)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (id_tipo_documento != 0)
            {
                #region con tipo documento
                var result = from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA

                             from MPER in _dataContext.vw_CONSULTAR_DNI
                             .Where(MPER => MDCHPA.USUARIO_REGISTRO == ("20565429656 - " + MPER.persona_num_documento))
                             .DefaultIfEmpty()

                             from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                             .Where(MTDOC => MDCHPA.ID_TIPO_DOCUMENTO == MTDOC.ID_TIPO_DOCUMENTO)
                             .DefaultIfEmpty()

                             from MFILIAL in _dataContext.MAE_FILIAL_DHCPA
                             .Where(MFILIAL => MDCHPA.ID_FILIAL == MFILIAL.ID_FILIAL)
                             .DefaultIfEmpty()

                             where MDCHPA.ID_TIPO_DOCUMENTO == id_tipo_documento && MDCHPA.FECHA_DOC.Value.Year == anno
                             select new DocumentoDhcpaResponse
                             {
                                 id_doc_dhcpa = MDCHPA.ID_DOC_DHCPA,
                                 num_doc = MDCHPA.NUM_DOC,
                                 nom_doc = MDCHPA.NOM_DOC,
                                 nom_tipo_documento = MTDOC.NOMBRE,
                                 asunto = MDCHPA.ASUNTO,
                                 anexos = MDCHPA.ANEXOS,
                                 nom_filial = MFILIAL.NOMBRE,
                                 fecha_doc = MDCHPA.FECHA_DOC,
                                 usuario_registro = MPER.paterno + " " + MPER.materno + ", " + MPER.nombres,
                                 id_oficina_direccion = MDCHPA.ID_OFICINA_DIRECCION
                                 
                                 //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                             } into document
                             orderby document.nom_tipo_documento, document.num_doc ascending
                             select document;
                return result;
                #endregion
            }
            else
            {
                #region sin tipo documento
                var result = from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA

                              from MPER in _dataContext.vw_CONSULTAR_DNI
                              .Where(MPER => MDCHPA.USUARIO_REGISTRO == ("20565429656 - " + MPER.persona_num_documento))
                              .DefaultIfEmpty()

                              from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                              .Where(MTDOC => MDCHPA.ID_TIPO_DOCUMENTO == MTDOC.ID_TIPO_DOCUMENTO)
                              .DefaultIfEmpty()

                              from MFILIAL in _dataContext.MAE_FILIAL_DHCPA
                              .Where(MFILIAL => MDCHPA.ID_FILIAL == MFILIAL.ID_FILIAL)
                              .DefaultIfEmpty()

                              where MDCHPA.FECHA_DOC.Value.Year == anno

                              select new DocumentoDhcpaResponse
                              {
                                  id_doc_dhcpa = MDCHPA.ID_DOC_DHCPA,
                                  num_doc = MDCHPA.NUM_DOC,
                                  nom_doc = MDCHPA.NOM_DOC,
                                  nom_tipo_documento = MTDOC.NOMBRE,
                                  asunto = MDCHPA.ASUNTO,
                                  anexos = MDCHPA.ANEXOS,
                                  nom_filial = MFILIAL.NOMBRE,
                                  fecha_doc = MDCHPA.FECHA_DOC,
                                  usuario_registro = MPER.paterno + " " + MPER.materno + ", " + MPER.nombres
                                  
                                  //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                              } into document
                             orderby document.nom_tipo_documento, document.num_doc ascending
                             select document;
                return result;
                #endregion
            }
            
        }
         
        public IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_excel()
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                          from DDDSEG in _dataContext.DAT_DET_SEG_DOC
                          .Where(DDDSEG => MSDHCPA.ID_SEGUIMIENTO == DDDSEG.ID_SEGUIMIENTO)

                          from MDOCSEG in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO
                          .Where(MDOCSEG => DDDSEG.ID_DOCUMENTO_SEG == MDOCSEG.ID_DOCUMENTO_SEG && MDOCSEG.INDICADOR == "1")

                          from MSOLI in _dataContext.MAE_SOLICITUD_INSPECCION
                          .Where(MSOLI => MSDHCPA.ID_SEGUIMIENTO == MSOLI.ID_SEGUIMIENTO)
                          .DefaultIfEmpty()

                          from VCPL in _dataContext.vw_CONSULTAR_PLANTAS
                          .Where(VCPL => MSDHCPA.ID_TIPO_SEGUIMIENTO == 1 && MSDHCPA.ID_HABILITANTE == VCPL.ID_PLANTA)
                          .DefaultIfEmpty()

                          from VCTPL in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                          .Where(VCTPL => VCPL.ID_TIPO_PLANTA == VCTPL.ID_TIPO_PLANTA)
                          .DefaultIfEmpty()

                          from VCTACTV in _dataContext.vw_CONSULTAR_TIPO_ACTIVIDAD_PLANTA
                          .Where(VCTACTV => VCPL.ID_TIPO_ACTIVIDAD == VCTACTV.ID_TIPO_ACTIVIDAD)
                          .DefaultIfEmpty()

                          from MFIL in _dataContext.MAE_FILIAL_DHCPA
                          .Where(MFIL => MSOLI.ID_FILIAL == MFIL.ID_FILIAL)
                          .DefaultIfEmpty()

                          from MEX in _dataContext.MAE_EXPEDIENTES
                          .Where(MEX => MSDHCPA.ID_EXPEDIENTE == MEX.ID_EXPEDIENTE)
                          .DefaultIfEmpty()

                          from MTEX in _dataContext.MAE_TIPO_EXPEDIENTE
                          .Where(MTEX => MEX.ID_TIPO_EXPEDIENTE == MTEX.ID_TIPO_EXPEDIENTE)
                          .DefaultIfEmpty()

                          from MTPRO in _dataContext.MAE_TIPO_PROCEDIMIENTO
                          .Where(MTPRO => MSDHCPA.ID_TIPO_PROCEDIMIENTO == MTPRO.ID_TIPO_PROCEDIMIENTO)
                          .DefaultIfEmpty()

                          from VCODIR in _dataContext.vw_CONSULTAR_DIRECCION
                          .Where(VCODIR => MSDHCPA.ID_OFI_DIR == VCODIR.ID_OFICINA_DIRECCION)
                          .DefaultIfEmpty()

                          from VCOF in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(VCOF => VCODIR.ID_OFICINA == VCOF.ID_OFICINA)
                          .DefaultIfEmpty()

                          from VCSEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                          .Where(VCSEDE => VCODIR.ID_SEDE == VCSEDE.ID_SEDE)
                          .DefaultIfEmpty()

                          from VCDNI in _dataContext.vw_CONSULTAR_DNI
                          .Where(VCDNI => MSDHCPA.PERSONA_NUM_DOCUMENTO == VCDNI.persona_num_documento)
                          .DefaultIfEmpty()

                          from VCEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                          .Where(VCEMB => MSDHCPA.ID_TIPO_SEGUIMIENTO == 2 && MSDHCPA.ID_EMBARCACION == VCEMB.ID_EMBARCACION)
                          .DefaultIfEmpty()

                          from VCHABEMB in _dataContext.vw_CONSULTAR_COD_HAB_EMBARCACION
                          .Where(VCHABEMB => VCEMB.CODIGO_HABILITACION == VCHABEMB.ID_COD_HAB_EMB)
                          .DefaultIfEmpty()

                          from VCDESEMB in _dataContext.VW_DB_GENERAL_MAE_DESEMBARCADERO
                          .Where(VCDESEMB => MSDHCPA.ID_TIPO_SEGUIMIENTO == 3 && MSDHCPA.ID_HABILITANTE == VCDESEMB.ID_DESEMBARCADERO)
                          .DefaultIfEmpty()

                          from VCCONCE in _dataContext.VW_CONSULTAR_DB_GENERAL_MAE_CONCESION
                          .Where(VCCONCE => MSDHCPA.ID_TIPO_SEGUIMIENTO == 4 && MSDHCPA.ID_HABILITANTE == VCCONCE.ID_CONCESION)
                          .DefaultIfEmpty()

                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                          .Where(VCEV => MSDHCPA.EVALUADOR == VCEV.persona_num_documento)
                          .DefaultIfEmpty()

                          from V_TRA in _dataContext.VISTA_DB_GENERAL_MAE_TRANSPORTE
                          .Where(V_TRA => MSDHCPA.ID_TIPO_SEGUIMIENTO == 5 && MSDHCPA.ID_HABILITANTE == V_TRA.ID_TRANSPORTE)
                          .DefaultIfEmpty()

                          from VCALMA in _dataContext.vw_CONSULTAR_DB_GENERAL_MAE_ALMACEN_SEDE
                          .Where(VCALMA => MSDHCPA.ID_TIPO_SEGUIMIENTO == 6 && MSDHCPA.ID_HABILITANTE == VCALMA.ID_ALMACEN)
                          .DefaultIfEmpty()

                          from MTUPA in _dataContext.MAE_TUPA
                          .Where(MTUPA => MSDHCPA.TUPA == MTUPA.ID_TUPA)
                          .DefaultIfEmpty()

                          select new SeguimientoDhcpaResponse
                          {
                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                              id_expediente = MSDHCPA.ID_EXPEDIENTE == null ? 0 : MSDHCPA.ID_EXPEDIENTE,
                              Expediente = MSDHCPA.ID_EXPEDIENTE == null ? "" : (MEX.NOM_EXPEDIENTE + "." + MTEX.NOMBRE),
                              asunto =MDOCSEG.ASUNTO,
                              fecha_recepcion_evaluador = MDOCSEG.FECHA_RECIBIDO_EVALUADOR,
                              tupa = MSDHCPA.TUPA,
                              id_tipo_procedimiento = MSDHCPA.ID_TIPO_PROCEDIMIENTO,
                              id_tipo_ser_hab = MSOLI.ID_TIPO_SER_HAB,
                              nom_tipo_procedimiento = MTPRO.NOMBRE,
                              fecha_inicio = MSDHCPA.FECHA_INICIO,
                              fecha_fin = MSDHCPA.FECHA_FIN,
                              id_ofi_dir = MSDHCPA.ID_OFI_DIR,
                              nom_oficina_ext = VCOF.NOMBRE == null ? VCDNI.paterno + " " + VCDNI.materno + " " + VCDNI.nombres : VCOF.NOMBRE + " - " + VCSEDE.NOMBRE,
                              id_embarcacion = MSDHCPA.ID_EMBARCACION,
                              nom_embarcacion = VCEMB.MATRICULA + " - " + VCEMB.NOMBRE,
                              matricula = MSDHCPA.ID_TIPO_SEGUIMIENTO == 5 ? V_TRA.PLACA : (MSDHCPA.ID_TIPO_SEGUIMIENTO == 2 ? VCEMB.MATRICULA : ""),
                              nom_estado = MSDHCPA.ESTADO == "0" ? "ENVIADO A LA SDHPA" : (MSDHCPA.ESTADO == "1" ? "POR ASIGNAR EVALUADOR" : (MSDHCPA.ESTADO == "2" ? "EN PROCESO" : "FINALIZADO")),
                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres,

                              codigo_habilitacion =
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 1 ?
                                            (MSDHCPA.ID_PLANTA == 0 ? "" : (VCTPL.SIGLAS + VCPL.NUMERO_PLANTA.ToString() + "-" + VCPL.NOMBRE_PLANTA))
                                            :
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 2 ?
                                            (VCEMB.CODIGO_HABILITACION == null ? "" : (VCHABEMB.CODIGO + VCEMB.NUM_COD_HABILITACION.ToString() + "-" + VCEMB.NOM_COD_HABILITACION))
                                            :
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 3 ?
                                            VCDESEMB.CODIGO_DESEMBARCADERO
                                            :
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 4 ?
                                            VCCONCE.CODIGO_HABILITACION
                                            :
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 5 ?
                                             V_TRA.COD_HABILITACION
                                            :
                                        MSDHCPA.ID_TIPO_SEGUIMIENTO == 6 ?
                                            VCALMA.CODIGO_HABILITANTE
                                            : ""
                                            ,
                              num_tupa_cadena = MSDHCPA.TUPA == null ? "" : MTUPA.NUMERO.ToString(),
                              nom_actividad = VCTACTV.NOMBRE,
                              nom_filial = MFIL.NOMBRE,
                              cond_expediente = MSDHCPA.ID_EXPEDIENTE == null ? false : (MSDHCPA.ESTADO == "3" ? false : true),
                              cond_finalizar = MSDHCPA.ESTADO == "3" ? false : true,
                              cond_planta = MSDHCPA.ID_PLANTA == 0 ? true : false,
                              duracion_sdhpa = MSDHCPA.DURACION_SDHPA == null ? 0 : MSDHCPA.DURACION_SDHPA,
                              duracion_tramite = MSDHCPA.DURACION_TRAMITE == null ? 0 : MSDHCPA.DURACION_TRAMITE,
                              observaciones = MSDHCPA.OBSERVACIONES == null ? "" : MSDHCPA.OBSERVACIONES,
                              num_solicitud_dhcpa = "" + MSOLI.NUMERO_DOCUMENTO,
                              fecha_solicitud_dhcpa = MSOLI.FECHA_CREA,
                              inspecto_designado = MSDHCPA.INSPECTO_DESIGNADO,
                              fecha_auditoria = MSDHCPA.FECHA_AUDITORIA,
                              fecha_envio_acta = MSDHCPA.FECHA_ENVIO_ACTA,
                              fecha_envio_oficio_sdhpa = MSDHCPA.FECHA_ENVIO_OFICIO_SDHPA,
                              con_proceso = MSDHCPA.CON_PROCESO
                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                          }).OrderByDescending(r => r.fecha_solicitud_dhcpa).AsEnumerable();
            return result;
        }
        public IEnumerable<Response.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO_Result> lista_transportes_con_protocolo_habilitado()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from sp in _dataContext.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO()
                          select new Response.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO_Result
                          {
                              externo = sp.EXTERNO,
                              placa = sp.PLACA,
                              cod_habilitacion = sp.COD_HABILITACION,
                              nombre = sp.NOMBRE,
                              nombre_carroceria = sp.NOMBRE_CARROCERIA,
                              nombre_furgon = sp.NOMBRE_FURGON,
                              fec_emi = sp.FEC_EMI,
                              fec_ini = sp.FEC_INI,
                              fec_fin = sp.FEC_FIN
                          });
            return result;
        }
        public IEnumerable<Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result> Lista_acta_info_pru_por_si(int id_sol_ins)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from sp in _dataContext.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI(id_sol_ins)
                          select new Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result
                          {
                              id_sol_ins = sp.ID_SOL_INS,
                              acta_id = sp.ACTA_ID,
                              acta_nombre = sp.ACTA_NOMBRE,
                              acta_fecha_carga = sp.ACTA_FECHA_CARGA,
                              acta_ruta_pdf = sp.ACTA_RUTA_PDF,
                              chkl_id = sp.CHKL_ID,
                              chkl_nombre = sp.CHKL_NOMBRE,
                              chkl_fecha_carga = sp.CHKL_FECHA_CARGA,
                              chkl_ruta_pdf = sp.CHKL_RUTA_PDF,
                              info_id = sp.INFO_ID,
                              info_nombre = sp.INFO_NOMBRE,
                              info_fecha_carga = sp.INFO_FECHA_CARGA,
                              info_ruta_pdf = sp.INFO_RUTA_PDF,
                              prue_cantidad = sp.PRUE_CANTIDAD
                          });
            return result;
        }

        public IEnumerable<ConsultarPlantasResponse> Lista_plantas_excel()
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from sp in _dataContext.SP_PLANTA_PROTOCOLO_EXCEL()

                          select new ConsultarPlantasResponse
                          {
                              excel_id_planta = sp.ID_PLANTA,
                              excel_filial = sp.FILIAL,
                              excel_razon_social = sp.RAZON_SOCIAL,
                              excel_codigo_planta = sp.CODIGO_PLANTA,
                              excel_actividad = sp.ACTIVIDAD,
                              excel_tch = sp.TIPO_CHD_CHI,
                              excel_licencia_operacion = sp.LICENCIA,
                              excel_direccion_planta = sp.DIRECCION,
                              excel_departamento_planta = sp.DEPARTAMENTO,
                              excel_provincia_planta = sp.PROVINCIA,
                              excel_distrito_planta = sp.DISTRITO,
                              excel_direccion_legal = sp.DIRECCION_LEGAL,
                              excel_departamento_legal = sp.DL___DEPARTAMENTO,
                              excel_provincia_legal = sp.DL___PROVINCIA,
                              excel_distrito_legal = sp.DL___DISTRITO
                          }).AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoDhcpaResponse> Lista_Destino_Documentos_x_tipo_documento(int id_doc_dhcpa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA
                          
                          from DDD in _dataContext.DAT_DOCUMENTO_DHCPA_DETALLE
                          .Where(DDD => MDCHPA.ID_DOC_DHCPA == DDD.ID_DOC_DHCPA)
                          .DefaultIfEmpty()

                          from MOFDIR in _dataContext.vw_CONSULTAR_DIRECCION
                          .Where(MOFDIR => DDD.ID_OFICINA_DIRECCION_DESTINO == MOFDIR.ID_OFICINA_DIRECCION)
                          .DefaultIfEmpty()

                          from MOFI in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(MOFI => MOFDIR.ID_OFICINA == MOFI.ID_OFICINA)
                          .DefaultIfEmpty()

                          from MOFI_PADRE in _dataContext.vw_CONSULTAR_OFICINA
                          .Where(MOFI_PADRE => MOFI.ID_OFI_PADRE == MOFI_PADRE.ID_OFICINA)
                          .DefaultIfEmpty()

                          where MDCHPA.ID_DOC_DHCPA == id_doc_dhcpa
                          select new DocumentoDhcpaResponse
                          {
                              id_doc_dhcpa = MDCHPA.ID_DOC_DHCPA,
                              lugar_destino = MOFI.RUC == "20565429656" ? MOFI.NOMBRE : MOFI_PADRE.NOMBRE,
                              persona_destino = DDD.NOMBRE_PERSONA_DESTINO
                              //nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento
                          }).OrderByDescending(r => r.id_doc_dhcpa).AsEnumerable();
            return result;
        }

        //procedimiento para excel_solicitud
        public SeguimientoDhcpaResponse Lista_protocolo_solicitud(int id_seguimiento)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from PLPRO in _dataContext.P_RELACION_PROTOCOLO_SOLICITUD(id_seguimiento)
                                                    
                          select new SeguimientoDhcpaResponse
                          {
                              excel_documento_resolutivo = PLPRO.DOCUMENTO_RESOLUTIVO,
                              excel_ini_vigencia = PLPRO.INICIO_VIGENCIA,
                              excel_fin_vigencia = PLPRO.FIN_VIGENCIA,
                              excel_fecha_emision = PLPRO.FECHA_EMISION
                          }).AsEnumerable().First();
            return result;
        }
        

        //procedimiento para excel_protocolo_planta
        public SeguimientoDhcpaResponse Lista_protocolo_seguimiento_planta(int id_planta)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from PLPRO in _dataContext.P_RELACION_PROTOCOLO_SEGUIMIENTO_PLANTA(id_planta)

                          select new SeguimientoDhcpaResponse
                          {
                              excel_documento_resolutivo = PLPRO.DOCUMENTO_RESOLUTIVO,
                              excel_ini_vigencia = PLPRO.INICIO_VIGENCIA,
                              excel_fin_vigencia = PLPRO.FIN_VIGENCIA,
                              excel_fecha_emision = PLPRO.FECHA_EMISION
                          }).AsEnumerable().First();
            return result;
        }
        public int Create_Persona_telefono(string persona_num_documento, string telefono, string usuario)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            return _dataContext.P_INSERT_PERSONA_TELEFONO(persona_num_documento,telefono,usuario).First().ID_PERSONA_TELEFONO;
        }

        public SeguimientoDhcpaResponse Lista_datos_evaluador(int id_seguimiento)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            SeguimientoDhcpaResponse seg_respo = new SeguimientoDhcpaResponse();

            var result = (from MSDHCPA in _dataContext.MAE_SEGUIMIENTO_DHCPA

                          from DDSEV in _dataContext.DAT_DET_SEG_EVALUADOR
                          .Where(DDSEV => MSDHCPA.ID_SEGUIMIENTO == DDSEV.ID_SEGUIMIENTO)

                          from VCEV in _dataContext.vw_CONSULTAR_DNI
                          .Where(VCEV => DDSEV.EVALUADOR == VCEV.persona_num_documento)
                          .DefaultIfEmpty()
                          
                          where MSDHCPA.ID_SEGUIMIENTO==id_seguimiento

                          select new SeguimientoDhcpaResponse
                          {
                              id_seguimiento = MSDHCPA.ID_SEGUIMIENTO,
                              fecha_recepcion_evaluador = DDSEV.FECHA_RECIBIDO,
                              nom_evaluador = VCEV.paterno + " " + VCEV.materno + " " + VCEV.nombres
                          }).AsEnumerable().OrderByDescending(X => X.id_seguimiento);

            if (result.Count() > 0)
            {
                seg_respo = result.First();
            }
            return seg_respo;
        }
    }
}
