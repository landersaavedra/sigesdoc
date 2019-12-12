using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGESDOC.Entidades;
using SIGESDOC.Contexto;
using SIGESDOC.Response;
using SIGESDOC.IRepositorio;

namespace SIGESDOC.Repositorio
{
    public partial class HojaTramiteRepositorio
    {

        public IEnumerable<Response.SP_CONSULTAR_REGISTRO_DE_USUARIO_Result> Consultar_registro_de_usuario(string usuario, int fechaini, int fechafin)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_CONSULTAR_REGISTRO_DE_USUARIO(usuario, fechaini, fechafin)
                          select new Response.SP_CONSULTAR_REGISTRO_DE_USUARIO_Result()
                          {
                              id = r.ID,
                              texto = r.TEXTO
                          });
            return result.ToList();
        }

        public IEnumerable<Response.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS_Result> Export_Excel_documentos_ht_archivadas_atendidas(int id_oficina)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS(id_oficina)
                          select new Response.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS_Result()
                          {
                              estado = r.ESTADO,
                              fecha_crea = r.FECHA_CREA,
                              fecha_recepcion = r.FECHA_RECEPCION,
                              hoja_tramite = r.HOJA_TRAMITE,
                              externo = r.EXTERNO,
                              documento = r.DOCUMENTO,
                              asunto = r.ASUNTO,
                              persona_asignada = r.PERSONA_ASIGNADA,
                              observacion = r.OBSERVACION,
                              fecha_fin = r.FECHA_FIN
                          }).OrderBy(x => x.fecha_recepcion);
            return result.ToList();
        }

        public IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_ATENDER_Result> Export_Excel_documentos_ht_pendientes_por_atender(int id_oficina)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_EXCEL_HT_PENDIENTES_POR_ATENDER(id_oficina)
                          select new Response.SP_EXCEL_HT_PENDIENTES_POR_ATENDER_Result()
                          {
                              estado = r.ESTADO,
                              fecha_crea = r.FECHA_CREA,
                              fecha_recepcion = r.FECHA_RECEPCION,
                              hoja_tramite = r.HOJA_TRAMITE,
                              externo = r.EXTERNO,
                              documento = r.DOCUMENTO,
                              asunto = r.ASUNTO,
                              persona_asignada = r.PERSONA_ASIGNADA,
                              observacion = r.OBSERVACION
                          }).OrderBy(x => x.fecha_recepcion);
            return result.ToList();
        }

        public IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR_Result> Export_Excel_documentos_ht_pendientes_por_recibir(int id_oficina)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR(id_oficina)
                          select new Response.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR_Result()
                          {
                              estado = r.ESTADO,
                              fecha_crea = r.FECHA_CREA,
                              hoja_tramite = r.HOJA_TRAMITE,
                              externo = r.EXTERNO,
                              documento = r.DOCUMENTO,
                              asunto = r.ASUNTO,
                              persona_asignada = r.PERSONA_ASIGNADA
                          }).OrderBy(x => x.fecha_crea);
            return result.ToList();
        }

        public IEnumerable<Response.SP_EXCEL_HT_ENVIADAS_Result> Export_Excel_documentos_ht_enviadas(int id_oficina)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_EXCEL_HT_ENVIADAS(id_oficina)
                          select new Response.SP_EXCEL_HT_ENVIADAS_Result()
                          {
                              estado = r.ESTADO,
                              fecha_crea = r.FECHA_CREA,
                              fecha_recibido = r.FECHA_RECIBIDO,
                              hoja_tramite = r.HOJA_TRAMITE,
                              externo = r.EXTERNO,
                              documento = r.DOCUMENTO,
                              asunto = r.ASUNTO,
                              persona_destino = r.PERSONA_DESTINO,
                              observacion = r.OBSERVACION,
                              indicadores = r.INDICADORES
                          }).OrderBy(x => x.fecha_crea);
            return result.ToList();
        }

        public IEnumerable<ExpedientesResponse> GetallRecupera_expediente()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MAE in _dataContext.MAE_EXPEDIENTES

                          from TEX in _dataContext.MAE_TIPO_EXPEDIENTE
                               .Where(TEX => MAE.ID_TIPO_EXPEDIENTE == TEX.ID_TIPO_EXPEDIENTE)
                               .DefaultIfEmpty() // <== makes join left join

                          select new ExpedientesResponse
                          {
                              id_expediente = MAE.ID_EXPEDIENTE,
                              numero_expediente = MAE.NUMERO_EXPEDIENTE,
                              tipo_expediente = new TipoExpedienteResponse
                              {
                                  id_tipo_expediente = TEX.ID_TIPO_EXPEDIENTE,
                                  nombre = TEX.NOMBRE
                              }
                          }).OrderByDescending(r => r.id_expediente).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<DocumentoResponse> GetAllHT(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_CONSULTAR_HT_GENERAL_X_OFICINA_DESTINO_X_TUPA(id_oficina_logeo.ToString(), ival_txtfechainicio, ival_txtfechafin, HT, asunto, Empresa, num_documento, cmbtipo_documento, nom_documento, id_tupa)
                          select new DocumentoResponse()
                          {
                              nom_doc = r.DOCUMENTO,
                              hoja_tramite = new HojaTramiteResponse
                              {
                                 // nombre_tipo_documento = r.NOMBRE_TIPO_DOCUMENTO,
                                  numero = r.NUMERO,
                                  nombre_tipo_tramite = r.TIPO_TRAMITE,
                                  asunto = r.ASUNTO,
                                  hoja_tramite = r.HOJA_TRAMITE,
                                  nombre_oficina = r.EXTERNO,
                                  fecha_emision = r.FECHA_EMISION,
                                  ver_pdf = r.VER_PDF != 0 ? true : false,
                                  editar = r.EDITAR,
                                  clave = r.CLAVE,
                                  nom_tupa = r.TUPA,
                                  nom_estado = r.ESTADO
                              }
                          }).OrderByDescending(r => r.hoja_tramite.numero).AsEnumerable(); // ordenado por documento enviado

            return result;

        }


        public IEnumerable<DocumentoResponse> GetmisHT(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_CONSULTAR_HT_GENERAL(id_oficina_logeo.ToString(), ival_txtfechainicio, ival_txtfechafin, HT, asunto, Empresa, num_documento, cmbtipo_documento, nom_documento, id_tupa)
                          select new DocumentoResponse()
                          {
                              nom_doc = r.DOCUMENTO,
                              hoja_tramite = new HojaTramiteResponse
                              {
                                  numero = r.NUMERO,
                                  nombre_tipo_tramite = r.TIPO_TRAMITE,
                                  asunto = r.ASUNTO,
                                  hoja_tramite = r.HOJA_TRAMITE,
                                  nombre_oficina = r.EXTERNO,
                                  fecha_emision = r.FECHA_EMISION,
                                  ver_pdf = r.VER_PDF != 0 ? true : false,
                                  ver_editar = r.EDITAR != "0" ? true : false,
                                  clave = r.CLAVE,
                                  nom_tupa = r.TUPA,
                                  nom_estado = r.ESTADO
                              }
                          }).OrderByDescending(r => r.hoja_tramite.numero).Take(500).AsEnumerable(); // ordenado por documento enviado

            return result;

        }

        public IEnumerable<DocumentoResponse> GetmisDoc(int id_oficina_logeo, string HT, string asunto, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa, string anexos, string Empresa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_CONSULTAR_DOC_X_OFICINA_X_TUPA(id_oficina_logeo.ToString(), ival_txtfechainicio, ival_txtfechafin, HT, asunto, Empresa, num_documento, cmbtipo_documento, nom_documento, id_tupa, anexos)
                          select new DocumentoResponse()
                          {

                              //nombre_tipo_documento = r.NOMBRE_TIPO_DOCUMENTO,
                              id_documento = r.ID_DOCUMENTO,
                              nom_doc = r.DOCUMENTO,
                              anexos = r.ANEXOS,
                              fecha_envio = r.FECHA_ENVIO,
                              ruta_pdf = r.RUTA_PDF,
                              hoja_tramite = new HojaTramiteResponse
                              {
                                 // nombre_tipo_documento = r.NOMBRE_TIPO_DOCUMENTO,
                                  numero = r.NUMERO,
                                  nombre_tipo_tramite = r.TIPO_TRAMITE,
                                  asunto = r.ASUNTO,
                                  hoja_tramite = r.HOJA_TRAMITE,
                                  nombre_oficina = r.EXTERNO,
                                  fecha_emision = r.FECHA_EMISION,
                                  ver_pdf = r.VER_PDF != 0 ? true : false,
                                  ver_editar = r.EDITAR != "0" ? true : false,
                                  clave = r.CLAVE
                              }
                          }).OrderByDescending(r => r.fecha_envio).Take(500).AsEnumerable(); // ordenado por documento enviado

            return result;

        }

        public IEnumerable<DocumentoResponse> GetAllDocumento_lista_resp_x_ht(int numero_ht)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDOC in _dataContext.MAE_DOCUMENTO

                          from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                .Where(MTDOC => MTDOC.ID_TIPO_DOCUMENTO == MDOC.ID_TIPO_DOCUMENTO)

                          select new DocumentoResponse()
                          {
                              id_documento = MDOC.ID_DOCUMENTO,
                              numero = MDOC.NUMERO,
                              id_tipo_documento = MDOC.ID_TIPO_DOCUMENTO,
                              tipo_documento = new TipoDocumentoResponse
                              {
                                  nombre = MTDOC.NOMBRE
                              },
                              numero_documento = MDOC.NUMERO_DOCUMENTO,
                              anexos = MDOC.ANEXOS == null ? "" : MDOC.ANEXOS,
                              folios = MDOC.FOLIOS,
                              oficina_crea = MDOC.OFICINA_CREA,
                              fecha_envio = MDOC.FECHA_ENVIO,
                              usuario_crea = MDOC.USUARIO_CREA,
                              nom_doc = MDOC.NOM_DOC,
                              persona_crea = MDOC.PERSONA_CREA,
                              id_indicador_documento = MDOC.ID_INDICADOR_DOCUMENTO,
                              ruta_pdf = MDOC.RUTA_PDF,
                              num_ext = MDOC.NUM_EXT,
                              documento_completo = MTDOC.NOMBRE + " " + (MDOC.NUMERO_DOCUMENTO == null ? "" : (MDOC.NUMERO_DOCUMENTO == 0 ? "" : " N. " + MDOC.NUMERO_DOCUMENTO.ToString())) + MDOC.NOM_DOC,
                              fecha_texto_corto = (MDOC.FECHA_ENVIO.Day > 9 ? MDOC.FECHA_ENVIO.Day.ToString() : "0" + MDOC.FECHA_ENVIO.Day.ToString()) + "/" + (MDOC.FECHA_ENVIO.Month > 9 ? MDOC.FECHA_ENVIO.Month.ToString() : "0" + MDOC.FECHA_ENVIO.Month.ToString()) + "/" + MDOC.FECHA_ENVIO.Year.ToString()
                          }).Where(zp => zp.numero == numero_ht).OrderByDescending(r => r.fecha_envio).AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoDetalleResponse> GetmisHT_archivados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && DDD.ID_EST_TRAMITE == 4

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento) && DDD.ID_EST_TRAMITE == 4
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 4

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 4
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public int CountmisHt_archivados(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && DDD.ID_EST_TRAMITE == 4

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join


                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento) && DDD.ID_EST_TRAMITE == 4
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 4

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 4
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public IEnumerable<DocumentoDetalleResponse> GetmisHT_finalizados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3) && DDD.OFICINA_DESTINO == id_oficina_logeo

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO == null ? DDD.FECHA_ATENDIDO : DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.FECHA_ARCHIVO == null ? DDD.OBSERVACION_ATENDIDO : DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento)
                                  && MDO.NOM_DOC.Contains(nom_documento) && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO == null ? DDD.FECHA_ATENDIDO : DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.FECHA_ARCHIVO == null ? DDD.OBSERVACION_ATENDIDO : DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO == null ? DDD.FECHA_ATENDIDO : DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.FECHA_ARCHIVO == null ? DDD.OBSERVACION_ATENDIDO : DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO == null ? DDD.FECHA_ATENDIDO : DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.FECHA_ARCHIVO == null ? DDD.OBSERVACION_ATENDIDO : DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public int CountmisHt_finalizados(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join


                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && (DDD.ID_EST_TRAMITE == 4 || DDD.ID_EST_TRAMITE == 3)
                                  && DDD.OFICINA_DESTINO == id_oficina_logeo
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_archivo = DDD.FECHA_ARCHIVO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public IEnumerable<DocumentoDetalleResponse> GetmisHT_atendidos(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && DDD.ID_EST_TRAMITE == 3

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento) && DDD.ID_EST_TRAMITE == 3
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 3

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 3
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public int CountmisHt_atendidos(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (num_documento != "")
            {
                num_documento = Convert.ToInt32(num_documento).ToString();
            }

            if (cmbtipo_documento == "")
            {
                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                  .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && DDD.ID_EST_TRAMITE == 3

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento) && DDD.ID_EST_TRAMITE == 3
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/


                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
            else
            {
                int var_id_tipo_doc = Convert.ToInt32(cmbtipo_documento);

                #region POR EMPRESA

                if (Empresa.Trim() != "")
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto) && MHT.NOMBRE_EXTERNO.Contains(Empresa) && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 3

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                else
                {
                    var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                                  from MTT in _dataContext.MAE_TIPO_TRAMITE
                                       .Where(MTT => MHT.ID_TIPO_TRAMITE == MTT.ID_TIPO_TRAMITE)

                                  from MDO in _dataContext.MAE_DOCUMENTO
                                       .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                                  from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                        .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)

                                  from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                        .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)

                                  //OFICINA_ARCHIVO
                                  from DDD_DIRECCION in _dataContext.vw_CONSULTAR_DIRECCION
                                  .Where(DDD_DIRECCION => DDD.OFICINA_DESTINO == DDD_DIRECCION.ID_OFICINA_DIRECCION)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_OFICINA in _dataContext.vw_CONSULTAR_OFICINA
                                  .Where(DDD_OFICINA => DDD_DIRECCION.ID_OFICINA == DDD_OFICINA.ID_OFICINA)
                                  .DefaultIfEmpty() // <== makes join left join

                                  from DDD_SEDE in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                                  .Where(DDD_SEDE => DDD_DIRECCION.ID_SEDE == DDD_SEDE.ID_SEDE)
                                  .DefaultIfEmpty() // <== makes join left join

                                  where MHT.HOJA_TRAMITE.Contains(HT) && MHT.ASUNTO.Contains(asunto)
                                  && MDO.NUMERO_DOCUMENTO.ToString().Contains(num_documento) && MDO.NOM_DOC.Contains(nom_documento)
                                  && MDO.ID_TIPO_DOCUMENTO == var_id_tipo_doc && DDD.ID_EST_TRAMITE == 3
                                  /*&& (OF_CAB.NOMBRE.Contains(Empresa) || OF_DET.NOMBRE.Contains(Empresa))*/

                                  select new DocumentoDetalleResponse
                                  {
                                      id_det_documento = DDD.ID_DET_DOCUMENTO,
                                      fecha_atendido = DDD.FECHA_ATENDIDO,
                                      nombre_oficina_destino = DDD_OFICINA.SIGLAS + " - " + DDD_SEDE.NOMBRE,
                                      observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                      documento = new DocumentoResponse
                                      {
                                          id_documento = MDO.ID_DOCUMENTO,
                                          nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N. " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                          hoja_tramite = new HojaTramiteResponse
                                          {
                                              numero = MHT.NUMERO, //numero de la hoja de tramite 
                                              nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                              asunto = MHT.ASUNTO, // asunto del documento
                                              hoja_tramite = MHT.HOJA_TRAMITE,
                                              nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                              fecha_emision = MHT.FECHA_EMISION,
                                              ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                              ver_editar = MHT.EDITAR != "0" ? true : false
                                          }
                                      }
                                  }).OrderByDescending(r => r.id_det_documento).AsEnumerable().Count(); // ordenado por detalle archivado
                    return result;
                }
                #endregion
            }
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllHoja_Tramite_x_PEDIDO_SIGA(int id_tipo_pedido_siga, int pedido_siga, int anno_siga, int id_oficina_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            var result = (from sp in _dataContext.SP_CONSULTA_HOJA_TRAMITE_X_PEDIDO(id_tipo_pedido_siga, pedido_siga, anno_siga, id_oficina_dir)
                          select new DocumentoDetalleResponse
                          {
                              documento = new DocumentoResponse
                              {
                                  hoja_tramite = new HojaTramiteResponse
                                  {
                                      numero = sp.NUMERO,
                                      anno_siga = sp.ANNO_SIGA,
                                      siga_asunto = sp.ASUNTO_SIGA,
                                      hoja_tramite = sp.HOJA_TRAMITE,
                                      asunto = sp.ASUNTO,
                                      siga_centro_costo = sp.CENTRO_COSTO
                                  }
                              },
                              nombre_encargado = sp.PERSONA_ASIGNADA,
                              fecha_crea = sp.FECHA_ENVIO,
                              fecha_recepcion = sp.FECHA_RECEPCION,
                              nombre_oficina_destino = sp.OFICINA,
                              estado_tramite = new EstadoTramiteResponse
                              {
                                  nombre = sp.ESTADO
                              }
                          }).OrderByDescending(r => r.fecha_crea).AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos(int id_oficina_logeo, string HT, string Asunto, string empresa, int id_ofi_crea, string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            //if (empresa.Trim() == "") { empresa = null; }
            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                    .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                    .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                    .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                    .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join


                              where DDD.ID_EST_TRAMITE == 1 && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                                    && MHT.ASUNTO.Contains(Asunto) && (empresa.Trim() == "" || (empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(empresa.Trim()))) && (id_ofi_crea == 0 || (id_ofi_crea != 0 && (DDD.OFICINA_CREA == id_ofi_crea)))

                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      anexos = (MDO.ANEXOS == null ? "" : MDO.ANEXOS.ToString()),
                                      nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : MDO.NUMERO_DOCUMENTO.ToString()) + " " + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDDCAB.FECHA_DERIVADO,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado
                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                    .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                    .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                    .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                    .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join


                              where DDD.ID_EST_TRAMITE == 1 && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                                    && MHT.ASUNTO.Contains(Asunto) && (empresa.Trim() == "" || (empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(empresa.Trim()))) && (id_ofi_crea == 0 || (id_ofi_crea != 0 && (DDD.OFICINA_CREA == id_ofi_crea)))
                                    && MHT.ID_TUPA.ToString() == cmbtupa
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      anexos = (MDO.ANEXOS == null ? "" : MDO.ANEXOS.ToString()),
                                      nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : MDO.NUMERO_DOCUMENTO.ToString()) + " " + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDDCAB.FECHA_DERIVADO,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado
                return result;
            }

        }



        public IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int id_ofi_crea, string persona_num_documento, string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            //if (empresa.Trim() == "") { empresa = null; }
            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                    .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                    .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                    .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                    .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join


                              where DDD.persona_num_documento == persona_num_documento && DDD.ID_EST_TRAMITE == 1 && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                                    && MHT.ASUNTO.Contains(Asunto) && (Empresa.Trim() == "" || (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa.Trim()))) && (id_ofi_crea == 0 || (id_ofi_crea != 0 && (DDD.OFICINA_CREA == id_ofi_crea)))

                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      anexos = (MDO.ANEXOS == null ? "" : MDO.ANEXOS.ToString()),
                                      nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : MDO.NUMERO_DOCUMENTO.ToString()) + " " + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDDCAB.FECHA_DERIVADO,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado
                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TDOC in _dataContext.MAE_TIPO_DOCUMENTO
                                    .Where(TDOC => MDO.ID_TIPO_DOCUMENTO == TDOC.ID_TIPO_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                    .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                    .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                    .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                    .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                    .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join


                              where DDD.persona_num_documento == persona_num_documento && DDD.ID_EST_TRAMITE == 1 && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                                    && MHT.ASUNTO.Contains(Asunto) && (Empresa.Trim() == "" || (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa.Trim()))) && (id_ofi_crea == 0 || (id_ofi_crea != 0 && (DDD.OFICINA_CREA == id_ofi_crea)))
                                    && MHT.ID_TUPA.ToString() == cmbtupa
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      anexos = (MDO.ANEXOS == null ? "" : MDO.ANEXOS.ToString()),
                                      nom_doc = TDOC.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : MDO.NUMERO_DOCUMENTO.ToString()) + " " + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDDCAB.FECHA_DERIVADO,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado
                return result;
            }

        }

        public IEnumerable<ConsultarDniResponse> GetAllPersona_Natural(int pageIndex, int pageSize, string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCPER in _dataContext.vw_CONSULTAR_DNI

                          from TDI in _dataContext.vw_CONSULTAR_TIPO_DOCUMENTO_IDENTIDAD
                               .Where(TDI => VCPER.tipo_doc_iden == TDI.TIPO_DOC_IDEN)
                               .DefaultIfEmpty() // <== makes join left join

                          where (VCPER.persona_num_documento).ToString().Contains(persona_num_documento) && VCPER.nombres.Contains(NOMBRE) && VCPER.paterno.Contains(PATERNO) && VCPER.materno.Contains(MATERNO)
                          select new ConsultarDniResponse
                          {
                              nom_tipo_doc = TDI.SIGLAS,
                              persona_num_documento = VCPER.persona_num_documento,
                              nombres = VCPER.paterno + " " + VCPER.materno + " " + VCPER.nombres,
                              sexo = VCPER.sexo == "F" ? "FEMENINO" : "MASCULINO",
                              direccion = VCPER.direccion,
                              fecha_nacimiento = VCPER.fecha_nacimiento
                          }).OrderByDescending(r => r.persona_num_documento).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public int CountPersona_Natural(string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCPER in _dataContext.vw_CONSULTAR_DNI

                          from TDI in _dataContext.vw_CONSULTAR_TIPO_DOCUMENTO_IDENTIDAD
                               .Where(TDI => VCPER.tipo_doc_iden == TDI.TIPO_DOC_IDEN)
                               .DefaultIfEmpty() // <== makes join left join

                          where (VCPER.persona_num_documento).ToString().Contains(persona_num_documento) && VCPER.nombres.Contains(NOMBRE) && VCPER.paterno.Contains(PATERNO) && VCPER.materno.Contains(MATERNO)
                          select new ConsultarDniResponse
                          {
                              nom_tipo_doc = TDI.SIGLAS,
                              persona_num_documento = VCPER.persona_num_documento,
                              nombres = VCPER.paterno + " " + VCPER.materno + " " + VCPER.nombres,
                              sexo = VCPER.sexo == "F" ? "FEMENINO" : "MASCULINO",
                              direccion = VCPER.direccion,
                              fecha_nacimiento = VCPER.fecha_nacimiento
                          }).OrderByDescending(r => r.persona_num_documento).AsEnumerable(); // ordenado por documento enviado

            return result.Count();
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllRecibidos(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                   .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from MEST in _dataContext.MAE_ESTADO_TRAMITE
                                .Where(MEST => DDD.ID_EST_TRAMITE == MEST.ID_EST_TRAMITE)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.ID_EST_TRAMITE == Estado && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                                (Empresa.Trim() == "" ||
                                    (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                    )
                                )

                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = MDO.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? MDO.NOM_DOC : MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  observacion = DDD.OBSERVACION,
                                  observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                  fecha_archivo = DDD.FECHA_ARCHIVO,
                                  fecha_atendido = DDD.FECHA_ATENDIDO,
                                  observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                  indicadores = DDD.INDICADORES,
                                  estado_tramite = new EstadoTramiteResponse
                                  {
                                      nombre = MEST.NOMBRE
                                  }
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                   .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from MEST in _dataContext.MAE_ESTADO_TRAMITE
                                .Where(MEST => DDD.ID_EST_TRAMITE == MEST.ID_EST_TRAMITE)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.ID_EST_TRAMITE == Estado && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                                (Empresa.Trim() == "" ||
                                    (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                    )
                                )
                            && MHT.ID_TUPA.ToString() == cmbtupa
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = MDO.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? MDO.NOM_DOC : MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  observacion = DDD.OBSERVACION,
                                  observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                  fecha_archivo = DDD.FECHA_ARCHIVO,
                                  fecha_atendido = DDD.FECHA_ATENDIDO,
                                  observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                  indicadores = DDD.INDICADORES,
                                  estado_tramite = new EstadoTramiteResponse
                                  {
                                      nombre = MEST.NOMBRE
                                  }
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }


        }

        public IEnumerable<DocumentoDetalleResponse> GetAllRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string persona_num_documento, string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                   .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from MEST in _dataContext.MAE_ESTADO_TRAMITE
                                .Where(MEST => DDD.ID_EST_TRAMITE == MEST.ID_EST_TRAMITE)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.persona_num_documento == persona_num_documento && DDD.ID_EST_TRAMITE == Estado && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                                (Empresa.Trim() == "" || (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa)))

                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? MDO.NOM_DOC : MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  observacion = DDD.OBSERVACION,
                                  observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                  fecha_archivo = DDD.FECHA_ARCHIVO,
                                  fecha_atendido = DDD.FECHA_ATENDIDO,
                                  observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                  indicadores = DDD.INDICADORES,
                                  estado_tramite = new EstadoTramiteResponse
                                  {
                                      nombre = MEST.NOMBRE
                                  }
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE

                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                   .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from MEST in _dataContext.MAE_ESTADO_TRAMITE
                                .Where(MEST => DDD.ID_EST_TRAMITE == MEST.ID_EST_TRAMITE)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.persona_num_documento == persona_num_documento && DDD.ID_EST_TRAMITE == Estado && DDD.OFICINA_DESTINO == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                                (Empresa.Trim() == "" || (Empresa.Trim() != "" && MHT.NOMBRE_EXTERNO.Contains(Empresa)))

                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? MDO.NOM_DOC : MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  observacion = DDD.OBSERVACION,
                                  observacion_archivo = DDD.OBSERVACION_ARCHIVO,
                                  fecha_archivo = DDD.FECHA_ARCHIVO,
                                  fecha_atendido = DDD.FECHA_ATENDIDO,
                                  observacion_atendido = DDD.OBSERVACION_ATENDIDO,
                                  indicadores = DDD.INDICADORES,
                                  estado_tramite = new EstadoTramiteResponse
                                  {
                                      nombre = MEST.NOMBRE
                                  }
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }


        }

        public IEnumerable<DocumentoDetalleResponse> GetAllDerivadas_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, string persona_num_documento,string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE


                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                     .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.USUARIO_CREA == "20565429656 - " + persona_num_documento && (DDD.ID_EST_TRAMITE == 1 || DDD.ID_EST_TRAMITE == 5) && DDD.OFICINA_CREA == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                              (Empresa == "" ||
                                  (Empresa != "" &&
                                    MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  )
                              )
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = DDD.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO == null ? 0 : DDD.ID_CAB_DET_DOCUMENTO,
                                  id_est_tramite = DDD.ID_EST_TRAMITE,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N° " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  fecha_crea = DDD.FECHA_CREA,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE


                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                     .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.USUARIO_CREA == "20565429656 - " + persona_num_documento && (DDD.ID_EST_TRAMITE == 1 || DDD.ID_EST_TRAMITE == 5) && DDD.OFICINA_CREA == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                              (Empresa == "" ||
                                  (Empresa != "" &&
                                    MHT.NOMBRE_EXTERNO.Contains(Empresa)
                                  )
                              )
                              && MHT.ID_TUPA.ToString()==cmbtupa
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = DDD.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO == null ? 0 : DDD.ID_CAB_DET_DOCUMENTO,
                                  id_est_tramite = DDD.ID_EST_TRAMITE,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N° " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  fecha_crea = DDD.FECHA_CREA,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }

           


        }

        public IEnumerable<DocumentoDetalleResponse> GetAllDerivadas(int id_oficina_logeo, string HT, string Asunto, string empresa,string cmbtupa)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            if (cmbtupa == "")
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE


                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                     .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.ID_EST_TRAMITE != 6 && DDD.OFICINA_CREA == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                              (empresa == "" ||
                                  (empresa != "" &&
                                    MHT.NOMBRE_EXTERNO.Contains(empresa)
                                  )
                              )
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = DDD.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO == null ? 0 : DDD.ID_CAB_DET_DOCUMENTO,
                                  id_est_tramite = DDD.ID_EST_TRAMITE,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N° " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  fecha_crea = DDD.FECHA_CREA,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }
            else
            {
                var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE


                              from MDO in _dataContext.MAE_DOCUMENTO
                                   .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                                     .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)

                              from DDD in _dataContext.DAT_DOCUMENTO_DETALLE
                                   .Where(DDD => MDO.ID_DOCUMENTO == DDD.ID_DOCUMENTO)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCDDER in _dataContext.vw_CONSULTAR_DIRECCION
                                   .Where(VCDDER => MDO.OFICINA_CREA == VCDDER.ID_OFICINA_DIRECCION)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCODER in _dataContext.vw_CONSULTAR_OFICINA
                                   .Where(VCODER => VCDDER.ID_OFICINA == VCODER.ID_OFICINA)
                                   .DefaultIfEmpty() // <== makes join left join

                              from VCP in _dataContext.vw_CONSULTAR_DNI
                                   .Where(VCP => DDD.persona_num_documento == VCP.persona_num_documento)
                                   .DefaultIfEmpty() // <== makes join left join

                              from DDDCAB in _dataContext.DAT_DOCUMENTO_DETALLE
                                                             .Where(DDDCAB => DDD.ID_CAB_DET_DOCUMENTO == DDDCAB.ID_DET_DOCUMENTO)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCDCAB in _dataContext.vw_CONSULTAR_DIRECCION
                                                             .Where(VCDCAB => DDDCAB.OFICINA_DESTINO == VCDCAB.ID_OFICINA_DIRECCION)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from VCOCAB in _dataContext.vw_CONSULTAR_OFICINA
                                                             .Where(VCOCAB => VCDCAB.ID_OFICINA == VCOCAB.ID_OFICINA)
                                                             .DefaultIfEmpty() // <== makes join left join

                              from MTUPA in _dataContext.MAE_TUPA
                                    .Where(MTUPA => MTUPA.ID_TUPA == MHT.ID_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              from TTUPA in _dataContext.MAE_TIPO_TUPA
                                    .Where(TTUPA => TTUPA.ID_TIPO_TUPA == MTUPA.ID_TIPO_TUPA)
                                    .DefaultIfEmpty() // <== makes join left join

                              where DDD.ID_EST_TRAMITE != 6 && DDD.OFICINA_CREA == id_oficina_logeo && (MHT.HOJA_TRAMITE).ToString().Contains(HT)
                              && MHT.ASUNTO.Contains(Asunto) &&
                              (empresa == "" ||
                                  (empresa != "" &&
                                    MHT.NOMBRE_EXTERNO.Contains(empresa)
                                  )
                              )
                              && MHT.ID_TUPA.ToString()==cmbtupa
                              select new DocumentoDetalleResponse
                              {
                                  id_det_documento = DDD.ID_DET_DOCUMENTO, // id del detalle
                                  id_documento = DDD.ID_DOCUMENTO,
                                  nombre_encargado = VCP.nombres + " " + VCP.paterno + " " + VCP.materno,
                                  id_cab_det_documento = DDD.ID_CAB_DET_DOCUMENTO == null ? 0 : DDD.ID_CAB_DET_DOCUMENTO,
                                  id_est_tramite = DDD.ID_EST_TRAMITE,
                                  documento = new DocumentoResponse
                                  {
                                      hoja_tramite = new HojaTramiteResponse
                                      {
                                          numero = MHT.NUMERO, //numero de la hoja de tramite 
                                          nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "Interno" : "Externo", // si es externa o interna
                                          asunto = MHT.ASUNTO, // asunto del documento
                                          nombre_oficina = MHT.NOMBRE_EXTERNO, // a quien pertenece el documento
                                          hoja_tramite = MHT.HOJA_TRAMITE,
                                          ver_pdf = MHT.RUTA_PDF != null ? true : false,
                                          editar = MHT.EDITAR,
                                          nom_tupa = MHT.ID_TUPA == null ? "" : TTUPA.NOMBRE + " " + MTUPA.NUMERO.ToString() + " : " + MTUPA.ASUNTO
                                      },
                                      id_documento = MDO.ID_DOCUMENTO,
                                      id_tipo_documento = MDO.ID_TIPO_DOCUMENTO,
                                      ruta_pdf = MDO.RUTA_PDF,
                                      nom_doc = MTD.NOMBRE + " " + (MDO.NUMERO_DOCUMENTO == null ? "" : " N° " + MDO.NUMERO_DOCUMENTO.ToString()) + MDO.NOM_DOC, // documento
                                      fecha_envio = MDO.FECHA_ENVIO, // cuando te enviaron el documento
                                      //fecha_derivado = DDD.ID_CAB_DET_DOCUMENTO == null ? MDO.FECHA_ENVIO : DDD.FECHA_DERIVADO
                                      siglas_oficina = DDD.ID_CAB_DET_DOCUMENTO == null ? VCODER.SIGLAS : VCOCAB.SIGLAS, // quien te envio el documento
                                      folios = MDO.FOLIOS // cuantos folios tiene el documento que te envio
                                  },
                                  fecha_recepcion = DDD.FECHA_RECEPCION,
                                  fecha_crea = DDD.FECHA_CREA,
                                  observacion = DDD.OBSERVACION,
                                  indicadores = DDD.INDICADORES
                              }).Distinct().OrderByDescending(r => r.id_det_documento).Take(500).AsEnumerable(); // ordenado por documento enviado

                return result;
            }
            


        }

        public DocumentoResponse Consultar_HT(string HT)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MHT in _dataContext.MAE_HOJA_TRAMITE


                          from MDO in _dataContext.MAE_DOCUMENTO
                               .Where(MDO => MHT.NUMERO == MDO.NUMERO)

                          from MTD in _dataContext.MAE_TIPO_DOCUMENTO
                               .Where(MTD => MDO.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => MHT.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          where MHT.HOJA_TRAMITE == HT
                          select new DocumentoResponse
                          {
                              id_documento = MDO.ID_DOCUMENTO,
                              nom_doc = (MDO.NUMERO_DOCUMENTO == null ? "" : MDO.NUMERO_DOCUMENTO.ToString()) + " " + MDO.NOM_DOC,
                              numero = MHT.NUMERO,
                              tipo_documento = new TipoDocumentoResponse
                              {
                                  nombre = MTD.NOMBRE
                              },
                              hoja_tramite = new HojaTramiteResponse
                              {
                                  numero = MHT.NUMERO,
                                  asunto = MHT.ASUNTO,
                                  nombre_tipo_tramite = MHT.ID_TIPO_TRAMITE != 1 ? "INTERNO" : "EXTERNO",
                                  nombre_oficina = VCO.NOMBRE,
                                  hoja_tramite = MHT.HOJA_TRAMITE
                              }
                          }).Distinct().OrderBy(r => r.id_documento).First(); // ordenado por documento
            return result;
        }

        public bool Crear_Empresa(string ruc, string nombre, string siglas, string usuario)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = from r in _dataContext.P_GUARDAR_OFICINA(ruc, nombre, siglas, usuario)
                         select new
                         {
                             ruc = r.RUC,
                             nombre = r.NOMBRE
                         };

            if (result.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public IEnumerable<ConsultarOficinaResponse> GetallOficina_x_sede(int sede)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VSO => VCD.ID_SEDE == VSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          where VSO.ID_SEDE == sede && VCO.ID_OFI_PADRE != null
                          select new ConsultarOficinaResponse
                          {
                              id_oficina = VCD.ID_OFICINA_DIRECCION,
                              nombre = VCO.NOMBRE
                          }).OrderBy(r => r.nombre).ToList();

            return result;
        }

        public IEnumerable<ConsultarDireccionResponse> GetAllEmpresa_RUC(string CONSUL_RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MODIR in _dataContext.vw_CONSULTAR_DIRECCION

                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOFIC => MODIR.ID_OFICINA == MOFIC.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from MSOFI in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(MSOFI => MODIR.ID_SEDE == MSOFI.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from MOREC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOREC => MOFIC.ID_OFI_PADRE == MOREC.ID_OFICINA && MOREC.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCU in _dataContext.vw_CONSULTAR_UBIGEO
                               .Where(VCU => MSOFI.UBIGEO == VCU.UBIGEO)
                               .DefaultIfEmpty() // <== makes join left join

                          where MOFIC.RUC == CONSUL_RUC && MOFIC.ID_OFI_PADRE == null

                          select new ConsultarDireccionResponse
                          {
                              id_oficina_direccion = MODIR.ID_OFICINA_DIRECCION,
                              id_oficina = MOFIC.ID_OFICINA,
                              nom_sede = MSOFI.NOMBRE,
                              nom_oficina = MOFIC.NOMBRE, // quien te envio el documento
                              var_id_ofi_padre = MOFIC.ID_OFI_PADRE,
                              siglas = MOFIC.SIGLAS,
                              ruc = MOFIC.RUC,
                              direccion = MSOFI.DIRECCION,
                              referencia = MSOFI.REFERENCIA,
                              nom_ubigeo = VCU.DEPARTAMENTO + "-" + VCU.PROVINCIA + "-" + VCU.DISTRITO,
                              codigo_ubigeo = VCU.UBIGEO
                          }).Distinct().OrderByDescending(r => r.id_oficina_direccion).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<ConsultarDireccionResponse> GetAll_Empresas_con_Oficinas()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MODIR in _dataContext.vw_CONSULTAR_DIRECCION

                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOFIC => MODIR.ID_OFICINA == MOFIC.ID_OFICINA && MOFIC.ID_OFI_PADRE == null)

                          from MSOFI in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(MSOFI => MODIR.ID_SEDE == MSOFI.ID_SEDE)
                          where MOFIC.RUC != "20565429656"
                          select new ConsultarDireccionResponse
                          {
                              id_oficina = MOFIC.ID_OFICINA,
                              nom_oficina = MOFIC.NOMBRE,
                              ruc = MOFIC.RUC
                          }).Distinct().OrderBy(r => r.nom_oficina).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<ConsultarDireccionResponse> GetAll_Oficinas_Direcciones(string RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MODIR in _dataContext.vw_CONSULTAR_DIRECCION

                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOFIC => MODIR.ID_OFICINA == MOFIC.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from MSOFI in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(MSOFI => MODIR.ID_SEDE == MSOFI.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from MOREC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOREC => MOFIC.ID_OFI_PADRE == MOREC.ID_OFICINA && MOREC.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          where MOFIC.RUC != "20565429656" && MOFIC.RUC.Contains(RUC)

                          select new ConsultarDireccionResponse
                          {
                              id_oficina_direccion = MODIR.ID_OFICINA_DIRECCION,
                              id_oficina = MOFIC.ID_OFICINA,
                              nom_sede = MSOFI.NOMBRE,
                              nom_oficina = MOFIC.ID_OFI_PADRE == null ? MOFIC.NOMBRE : MOREC.NOMBRE + " - " + MOFIC.NOMBRE, // quien te envio el documento
                              var_id_ofi_padre = MOFIC.ID_OFI_PADRE,
                              siglas = MOFIC.SIGLAS,
                              ruc = MOFIC.RUC,
                              direccion = MSOFI.DIRECCION,
                              referencia = MSOFI.REFERENCIA
                          }).Distinct().OrderByDescending(r => r.id_oficina_direccion).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<ConsultarOficinaResponse> GetAll_Oficinas_Direcciones_X_NOM(string NOM)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (/*from MODIR in _dataContext.vw_CONSULTAR_DIRECCION
                */
                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                          /*.Where(MOFIC => MODIR.ID_OFICINA == MOFIC.ID_OFICINA)
                          .DefaultIfEmpty() // <== makes join left join
                               
                     from MSOFI in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                          .Where(MSOFI => MODIR.ID_SEDE == MSOFI.ID_SEDE)
                          .DefaultIfEmpty() // <== makes join left join
                          */
                          from MORUC in _dataContext.VW_CONSULTAR_RUC
                               .Where(MORUC => MOFIC.RUC == MORUC.RUC)
                               .DefaultIfEmpty() // <== makes join left join

                          where MOFIC.RUC != "20565429656" && (((MORUC.RAZON_SOCIAL + " - " + MOFIC.NOMBRE).Contains(NOM) && MOFIC.RUC == "99999999999") ||
                          ((MORUC.RAZON_SOCIAL + " - " + MOFIC.NOMBRE).Contains(NOM) && MOFIC.RUC == "99999999998") ||
                          ((MORUC.RAZON_SOCIAL).Contains(NOM) && MOFIC.ID_OFI_PADRE == null))

                          select new ConsultarOficinaResponse
                          {
                              //id_oficina_direccion = MODIR.ID_OFICINA_DIRECCION,
                              id_oficina = MOFIC.ID_OFICINA,
                              //nom_sede = MSOFI.NOMBRE,
                              nombre = MOFIC.ID_OFI_PADRE == null ? MORUC.RAZON_SOCIAL : MORUC.RAZON_SOCIAL + " - " + MOFIC.NOMBRE, // quien te envio el documento
                              id_ofi_padre = MOFIC.ID_OFI_PADRE,
                              siglas = MOFIC.SIGLAS,
                              ruc = MOFIC.RUC
                              /*,
                              direccion = MSOFI.DIRECCION,
                              referencia = MSOFI.REFERENCIA*/
                          }).Distinct().AsEnumerable(); // ordenado por documento enviado

            return result.OrderBy(x => x.nombre).ThenBy(z => z.id_oficina);
        }

        public IEnumerable<ConsultarDireccionResponse> Getall_Direccion_x_Oficina(int ID_OFICINA)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MODIR in _dataContext.vw_CONSULTAR_DIRECCION

                          from MOFIC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOFIC => MODIR.ID_OFICINA == MOFIC.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from MSOFI in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(MSOFI => MODIR.ID_SEDE == MSOFI.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from MOREC in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(MOREC => MOFIC.ID_OFI_PADRE == MOREC.ID_OFICINA && MOREC.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          where MOFIC.RUC != "20565429656" && MOFIC.ID_OFICINA == ID_OFICINA

                          select new ConsultarDireccionResponse
                          {
                              id_oficina_direccion = MODIR.ID_OFICINA_DIRECCION,
                              id_oficina = MOFIC.ID_OFICINA,
                              nom_sede = MSOFI.NOMBRE,
                              nom_oficina = MOFIC.ID_OFI_PADRE == null ? MOFIC.NOMBRE : MOREC.NOMBRE + " - " + MOFIC.NOMBRE, // quien te envio el documento
                              var_id_ofi_padre = MOFIC.ID_OFI_PADRE,
                              siglas = MOFIC.SIGLAS,
                              ruc = MOFIC.RUC,
                              direccion = MSOFI.DIRECCION,
                              referencia = MSOFI.REFERENCIA
                          }).Distinct().OrderByDescending(r => r.id_oficina_direccion).AsEnumerable(); // ordenado por documento enviado

            return result;
        }




        public IEnumerable<ConsultarOficinaResponse> OF_GetallOficina_x_RUC_NOMBRE(int pageIndex, int pageSize, string CONS_RUC, string CONS_NOMBRE)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCO_OFI in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO_OFI => VCO.RUC == VCO_OFI.RUC && VCO_OFI.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VCSO => VCD.ID_SEDE == VCSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join


                          where VCO.RUC.Contains(CONS_RUC) && VCO.NOMBRE.Contains(CONS_NOMBRE)
                          select new ConsultarOficinaResponse
                          {
                              ruc = VCO.RUC,
                              nombre = VCO.ID_OFI_PADRE == null ? VCO.NOMBRE : VCO_OFI.SIGLAS + " - " + VCO.NOMBRE + (VCSO.NOMBRE.ToString().Trim() == "" ? " " : " - " + VCSO.NOMBRE),
                              siglas = VCO.SIGLAS,
                              activo_direccion = VCD.ACTIVO,
                              nombre_direccion = VCSO.NOMBRE.ToString().Trim() == "" ? VCSO.DIRECCION : VCSO.DIRECCION
                          }).OrderByDescending(r => r.ruc).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();
            return result;
        }

        public int OF_CountOficina_x_RUC_NOMBRE(string CONS_RUC, string CONS_NOMBRE)
        {

            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCO_OFI in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO_OFI => VCO.RUC == VCO_OFI.RUC && VCO_OFI.ID_OFI_PADRE == null)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VCSO => VCD.ID_SEDE == VCSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC.Contains(CONS_RUC) && VCO.NOMBRE.Contains(CONS_NOMBRE)
                          select new ConsultarOficinaResponse
                          {
                              ruc = VCO.RUC,
                              nombre = VCO.ID_OFI_PADRE == null ? VCO.NOMBRE : VCO_OFI.SIGLAS + " - " + VCO.NOMBRE,
                              siglas = VCO.SIGLAS,
                              activo_direccion = VCD.ACTIVO,
                              nombre_direccion = VCSO.NOMBRE.ToString().Trim() == "" ? VCSO.DIRECCION : VCSO.NOMBRE + " - " + VCSO.DIRECCION
                          }).OrderByDescending(r => r.ruc).AsEnumerable();
            return result.Count();
        }

        public IEnumerable<Response.ConsultarDireccionResponse> OF_GetallOficina_DIR_x_RUC(int pageIndex, int pageSize, string CONS_RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VSO => VCD.ID_SEDE == VSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCU in _dataContext.vw_CONSULTAR_UBIGEO
                               .Where(VCU => VSO.UBIGEO == VCU.UBIGEO)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC == CONS_RUC && VCO.ID_OFI_PADRE != null
                          select new ConsultarDireccionResponse()
                          {
                              id_oficina_direccion = VCD.ID_OFICINA_DIRECCION,
                              nom_oficina = VSO.NOMBRE.ToString().Trim() == "" ? VCO.NOMBRE : VCO.NOMBRE + "-" + VSO.NOMBRE,
                              direccion = VSO.DIRECCION,
                              nom_ubigeo = VCU.DEPARTAMENTO + "-" + VCU.PROVINCIA + "-" + VCU.DISTRITO,
                              activo = VCD.ACTIVO
                          }).OrderByDescending(r => r.id_oficina_direccion).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();

            return result;
        }

        public int OF_CountOficina_DIR_x_RUC(string CONS_RUC)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from VCD in _dataContext.vw_CONSULTAR_DIRECCION

                          from VCO in _dataContext.vw_CONSULTAR_OFICINA
                               .Where(VCO => VCD.ID_OFICINA == VCO.ID_OFICINA)
                               .DefaultIfEmpty() // <== makes join left join

                          from VSO in _dataContext.vw_CONSULTAR_SEDE_OFICINA
                               .Where(VSO => VCD.ID_SEDE == VSO.ID_SEDE)
                               .DefaultIfEmpty() // <== makes join left join

                          from VCU in _dataContext.vw_CONSULTAR_UBIGEO
                               .Where(VCU => VSO.UBIGEO == VCU.UBIGEO)
                               .DefaultIfEmpty() // <== makes join left join

                          where VCO.RUC == CONS_RUC && VCO.ID_OFI_PADRE != null
                          select new ConsultarDireccionResponse()
                          {
                              id_oficina_direccion = VCD.ID_OFICINA_DIRECCION,
                              nom_oficina = VSO.NOMBRE.ToString().Trim() == "" ? VCO.NOMBRE : VCO.NOMBRE + "-" + VSO.NOMBRE,
                              direccion = VSO.DIRECCION,
                              nom_ubigeo = VCU.DEPARTAMENTO + "-" + VCU.PROVINCIA + "-" + VCU.DISTRITO,
                              activo = VCD.ACTIVO
                          }).OrderByDescending(r => r.id_oficina_direccion).AsEnumerable();

            return result.Count();
        }
        public IEnumerable<DocDetObservacionesResponse> Listar_Observacion_x_det_documento(int id_det_documento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            var result = (
                            from DDOBS in _dataContext.DAT_DOC_DET_OBSERVACIONES

                            from VCDNI in _dataContext.vw_CONSULTAR_DNI
                                .Where(VCDNI => VCDNI.persona_num_documento == DDOBS.USUARIO_CREA)
                                .DefaultIfEmpty()

                            where DDOBS.ACTIVO == "1" && DDOBS.ID_DET_DOCUMENTO == id_det_documento
                            select new DocDetObservacionesResponse
                            {
                                id_det_doc_observacion = DDOBS.ID_DET_DOC_OBSERVACION,
                                id_det_documento = DDOBS.ID_DET_DOCUMENTO,
                                observacion = DDOBS.OBSERVACION,
                                usuario_crea = DDOBS.USUARIO_CREA,
                                fecha_crea = DDOBS.FECHA_CREA,
                                activo = DDOBS.ACTIVO,
                                nombre_usuario = VCDNI.nombres + " " + VCDNI.paterno + " " + VCDNI.materno
                            });
            return result;
        }
        public Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_Result Consultar_documentos_pendientes(string documento, int id_ofi_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MAE in _dataContext.SP_CONSULTAR_DOCUMENTOS_PENDIENTES(documento, id_ofi_dir)
                          select new Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_Result
                          {
                              contador = MAE.CONTADOR,
                              oficina_destino = MAE.OFICINA_DESTINO
                          }).First(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<Response.SP_CONSULTA_HISTORIAL_HT_Result> recupera_historial_ht(int numero)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from p in _dataContext.SP_CONSULTA_HISTORIAL_HT(numero)
                          select new Response.SP_CONSULTA_HISTORIAL_HT_Result
                          {
                              est_tramite = p.EST_TRAMITE,
                              id_documento = p.ID_DOCUMENTO,
                              id_det_documento = p.ID_DET_DOCUMENTO,
                              id_cab_det_documento = p.ID_CAB_DET_DOCUMENTO,
                              fecha_crea = p.FECHA_CREA,
                              fecha_recepcion = p.FECHA_RECEPCION,
                              documento = p.DOCUMENTO,
                              ruta_pdf = p.RUTA_PDF,
                              designado = p.DESIGNADO,
                              nom_sede = p.NOM_SEDE,
                              nom_oficina = p.NOM_OFICINA,
                              observacion = p.OBSERVACION,
                              observacion_fin = p.OBSERVACION_FIN,
                              fecha_fin = p.FECHA_FIN
                          }); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result> Consultar_documentos_pendientes_detalle(string documento, int id_ofi_dir)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MAE in _dataContext.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE(documento, id_ofi_dir)
                          select new Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result
                          {
                              diferencia = MAE.DIFERENCIA,
                              fecha = MAE.FECHA,
                              cantidad = MAE.CANTIDAD
                          }).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result> Consultar_documentos_pendientes_detalle_desagregado(string documento, int id_ofi_dir, string fecha)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MAE in _dataContext.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO(documento, id_ofi_dir, fecha)
                          select new Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result
                          {
                              id_det_documento = MAE.ID_DET_DOCUMENTO,
                              numero = MAE.NUMERO,
                              fecha_derivada = MAE.FECHA_DERIVADA,
                              fecha_recibida = MAE.FECHA_RECIBIDA,
                              hoja_tramite = MAE.HOJA_TRAMITE,
                              asunto = MAE.ASUNTO,
                              documento = MAE.DOCUMENTO,
                              externo = MAE.EXTERNO,
                              oficina_deriva = MAE.OFICINA_DERIVA,
                              servidor_publico = MAE.SERVIDOR_PUBLICO
                          }).AsEnumerable(); // ordenado por documento enviado

            return result;
        }

        public string genera_clave_documento_externo()
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from r in _dataContext.SP_GENERA_CLAVE()
                          select new Response.SP_GENERA_CLAVE_Result()
                          {
                              clave = r.CLAVE
                          }).AsEnumerable();
            return result.First().clave;
        }

    }
}
