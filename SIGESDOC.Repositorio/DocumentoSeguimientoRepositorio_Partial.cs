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
    public partial class DocumentoSeguimientoRepositorio : IDocumentoSeguimientoRepositorio
    {
        
        public IEnumerable<DocumentoSeguimientoResponse>  GetAllDocumentos(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea,string expediente)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            /*            if(evaluador=="")            {                var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO                              from MTD in _dataContext.MAE_TIPO_DOCUMENTO                                   .Where(MTD => MDS.ID_TIPO_DOCUMENTO == MTD.ID_TIPO_DOCUMENTO)                              from VPER in _dataContext.vw_CONSULTAR_DNI                                .Where(VPER => MDS.EVALUADOR == VPER.persona_num_documento)                             .DefaultIfEmpty() // <== makes join left join                             from DDOSEG in _dataContext.DAT_DET_SEG_DOC                                .Where(DDOSEG => MDS.ID_DOCUMENTO_SEG == DDOSEG.ID_DOCUMENTO_SEG)                             .DefaultIfEmpty() // <== makes join left join                              from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA                                 .Where(MSEG => DDOSEG.ID_SEGUIMIENTO == MSEG.ID_SEGUIMIENTO)                              .DefaultIfEmpty() // <== makes join left join                              from VCDIR in _dataContext.vw_CONSULTAR_DIRECCION                              .Where(VCDIR => MSEG.ID_OFI_DIR == VCDIR.ID_OFICINA_DIRECCION)                              .DefaultIfEmpty() // <== makes join left join                              from VCOF in _dataContext.vw_CONSULTAR_OFICINA                              .Where(VCOF => VCDIR.ID_OFICINA == VCOF.ID_OFICINA)                              .DefaultIfEmpty() // <== makes join left join                              from VPER_EXTER in _dataContext.vw_CONSULTAR_DNI                                .Where(VPER_EXTER => MSEG.PERSONA_NUM_DOCUMENTO == VPER_EXTER.persona_num_documento)                             .DefaultIfEmpty() // <== makes join left join                              where MDS.ESTADO.Contains(estado) && MDS.INDICADOR.Contains(indicador)                              select new DocumentoSeguimientoResponse                              {                                  id_documento_seg = MDS.ID_DOCUMENTO_SEG,                                  id_tipo_documento = MDS.ID_TIPO_DOCUMENTO,                                  fecha_crea = MDS.FECHA_CREA,                                  fecha_documento = MDS.FECHA_DOCUMENTO,                                  tipo_documento = new TipoDocumentoResponse                                  {                                      nombre = MTD.NOMBRE                                  },                                  nom_externo = MSEG.PERSONA_NUM_DOCUMENTO == null ? VCOF.RUC + " - " + VCOF.NOMBRE : VPER_EXTER.paterno + " " + VPER_EXTER.materno + " " + VPER_EXTER.nombres,                                  asunto = MDS.ASUNTO,                                  num_documento = MDS.NUM_DOCUMENTO,                                  nom_documento = MTD.NOMBRE + " " + (MDS.NUM_DOCUMENTO == null ? "" : " N° " + MDS.NUM_DOCUMENTO.ToString()) + " " + MDS.NOM_DOCUMENTO, // documento                                  evaluador = VPER.persona_num_documento + " - " + VPER.paterno + " " + VPER.materno + " " + VPER.nombres                              }).Distinct().OrderByDescending(r => r.id_documento_seg).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable();                return result;            }            else            {*/
            var data = _dataContext.SP_CONSULTAR_EXPEDIENTES_X_DOCUMENTO_HABILITACIONES(estado, indicador, asunto, externo, nom_doc, evaluador, id_tipo_documento, num_doc, oficina_crea,expediente);
            var result = (from MDS in data
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
                              documento_codigo_habilitacion =MDS.CODIGO_HABILITANTE,
                              nom_externo = MDS.NOMBRE_EXTERNO,
                              asunto = MDS.ASUNTO,
                              num_documento = MDS.NUM_DOCUMENTO,
                              nom_documento = MDS.NOMBRE_DOCUMENTO,
                              evaluador = MDS.EVALUADOR,
                              group_expedientes = MDS.EXPEDIENTES,
                              fecha_od = MDS.FECHA_OD,
                              ruta_pdf =MDS.RUTA_PDF,
                              nom_ofi_crea = MDS.NOM_OFICINA_CREA,
                              usu_crea = MDS.USU_CREA
                          }).Distinct().OrderByDescending(r => r.id_documento_seg).AsEnumerable();
                return result;
        /*}*/
        }
         
        public IEnumerable<ExpedientesResponse> GetAllExpediente_x_Documento(int id_documento_seg)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO

                          from DDE in _dataContext.DAT_DET_SEG_DOC
                               .Where(DDE => MDS.ID_DOCUMENTO_SEG == DDE.ID_DOCUMENTO_SEG)

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                          .Where(MSEG => DDE.ID_SEGUIMIENTO == MSEG.ID_SEGUIMIENTO)

                          from MEXP in _dataContext.MAE_EXPEDIENTES
                          .Where(MEXP => MSEG.ID_EXPEDIENTE == MEXP.ID_EXPEDIENTE)

                          from MTEXP in _dataContext.MAE_TIPO_EXPEDIENTE
                          .Where(MTEXP => MEXP.ID_TIPO_EXPEDIENTE == MTEXP.ID_TIPO_EXPEDIENTE)

                          where MDS.ID_DOCUMENTO_SEG == id_documento_seg

                          select new ExpedientesResponse
                          {
                              id_expediente = MEXP.ID_EXPEDIENTE,
                              id_tipo_expediente = MEXP.ID_TIPO_EXPEDIENTE,
                              numero_expediente = MEXP.NUMERO_EXPEDIENTE,
                              nom_expediente = MEXP.NOM_EXPEDIENTE,
                              tipo_expediente = new TipoExpedienteResponse
                              {
                                  nombre = MTEXP.NOMBRE
                              }
                          }).AsEnumerable();
            return result;
        }

        public IEnumerable<ConsultaFacturasResponse> GetAllfacturas_x_Documento(int id_documento_seg)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO

                          from DDF in _dataContext.DAT_DET_DOC_FACT
                               .Where(DDF => MDS.ID_DOCUMENTO_SEG == DDF.ID_DOCUMENTO_SEG)

                          from VFAC in _dataContext.VW_CONSULTA_FACTURAS
                          .Where(VFAC => DDF.ID_FACTURA == VFAC.ID_FACTURA)

                          where MDS.ID_DOCUMENTO_SEG == id_documento_seg

                          select new ConsultaFacturasResponse
                          {
                              id_factura = VFAC.ID_FACTURA,
                              num1_fact = VFAC.NUM1_FACT,
                              num2_fact = VFAC.NUM2_FACT,
                              fecha_fact = VFAC.FECHA_FACT,
                              importe_total = VFAC.IMPORTE_TOTAL
                          }).AsEnumerable();
            return result;
        }

        public IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcacion_x_documento(int id_documento_seg)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO

                          from DDE in _dataContext.DAT_DET_SEG_DOC
                               .Where(DDE => MDS.ID_DOCUMENTO_SEG == DDE.ID_DOCUMENTO_SEG)

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                          .Where(MSEG => DDE.ID_SEGUIMIENTO == MSEG.ID_SEGUIMIENTO)

                          from MEMB in _dataContext.VW_CONSULTA_EMBARCACIONES
                          .Where(MEMB => MSEG.ID_EMBARCACION == MEMB.ID_EMBARCACION)

                          where MDS.ID_DOCUMENTO_SEG == id_documento_seg

                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = MEMB.ID_EMBARCACION,
                              matricula = MEMB.MATRICULA,
                              nombre = MEMB.NOMBRE
                          }).AsEnumerable();
            return result;
        }
        public IEnumerable<ConsultarPlantasResponse> GetAllPlanta_x_seguimiento(int id_documento_seg)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO

                          from DDE in _dataContext.DAT_DET_SEG_DOC
                               .Where(DDE => MDS.ID_DOCUMENTO_SEG == DDE.ID_DOCUMENTO_SEG)

                          from MSEG in _dataContext.MAE_SEGUIMIENTO_DHCPA
                          .Where(MSEG => DDE.ID_SEGUIMIENTO == MSEG.ID_SEGUIMIENTO)
                                  .DefaultIfEmpty() // <== makes join left join

                          from MPLA in _dataContext.vw_CONSULTAR_PLANTAS
                          .Where(MPLA => MSEG.ID_PLANTA == MPLA.ID_PLANTA)
                                  .DefaultIfEmpty() // <== makes join left join

                          from MTPLA in _dataContext.vw_CONSULTAR_TIPO_PLANTA
                          .Where(MTPLA => MPLA.ID_TIPO_PLANTA == MTPLA.ID_TIPO_PLANTA)
                                  .DefaultIfEmpty() // <== makes join left join

                          where MDS.ID_DOCUMENTO_SEG == id_documento_seg

                          select new ConsultarPlantasResponse
                          {
                              numero_planta=MPLA.NUMERO_PLANTA,
                              nombre_planta = MPLA.NOMBRE_PLANTA,
                              siglas_tipo_planta = MTPLA.SIGLAS
                          }).Distinct().AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoSeguimientoResponse> lista_documentos_recibidos_x_seguimiento(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDS in _dataContext.MAE_DOCUMENTO_SEGUIMIENTO

                          from DDE in _dataContext.DAT_DET_SEG_DOC
                               .Where(DDE => MDS.ID_DOCUMENTO_SEG == DDE.ID_DOCUMENTO_SEG && DDE.ACTIVO=="1")

                          from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                          .Where(MTDOC => MDS.ID_TIPO_DOCUMENTO==MTDOC.ID_TIPO_DOCUMENTO)

                          where DDE.ID_SEGUIMIENTO == id_seguimiento

                          select new DocumentoSeguimientoResponse
                          {
                              id_documento_seg = MDS.ID_DOCUMENTO_SEG,
                              estado = MDS.ESTADO,
                              fecha_crea = MDS.FECHA_CREA,
                              fecha_recepcion_sdhpa = MDS.FECHA_RECEPCION_SDHPA,
                              fecha_od = MDS.FECHA_OD,
                              nom_tipo_documento = MTDOC.NOMBRE,
                              num_documento = MDS.NUM_DOCUMENTO,
                              nom_documento = MDS.NOM_DOCUMENTO,
                              asunto = MDS.ASUNTO,
                              fecha_documento = MDS.FECHA_DOCUMENTO,
                              ruta_pdf = MDS.RUTA_PDF
                          }).Distinct().AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoDhcpaResponse> lista_documentos_emitidos_dhcpa_x_seguimiento(int id_seguimiento)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;

            var result = (from MDCHPA in _dataContext.MAE_DOCUMENTO_DHCPA

                          from DDE in _dataContext.DAT_DET_SEG_DOC_DHCPA
                               .Where(DDE => MDCHPA.ID_DOC_DHCPA == DDE.ID_DOC_DHCPA)

                          from MTDOC in _dataContext.MAE_TIPO_DOCUMENTO
                          .Where(MTDOC => MDCHPA.ID_TIPO_DOCUMENTO == MTDOC.ID_TIPO_DOCUMENTO)

                          where DDE.ID_SEGUIMIENTO == id_seguimiento

                          select new DocumentoDhcpaResponse
                          {
                              id_doc_dhcpa = MDCHPA.ID_DOC_DHCPA,
                              asunto = MDCHPA.ASUNTO,
                              nom_tipo_documento = MTDOC.NOMBRE,
                              num_doc = MDCHPA.NUM_DOC,
                              nom_doc = MDCHPA.NOM_DOC,
                              fecha_doc = MDCHPA.FECHA_DOC,
                              pdf = MDCHPA.PDF,
                              id_oficina_direccion = MDCHPA.ID_OFICINA_DIRECCION

                          }).Distinct().AsEnumerable();
            return result;
        }

        public IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos_x_rec(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente)
        {
            DB_GESDOCEntities _dataContext = base.Context.GetContext() as DB_GESDOCEntities;
            
            var data = _dataContext.SP_CONSULTAR_EXPEDIENTES_X_DOCUMENTO_HABILITACIONES_RECEP(estado, indicador, asunto, externo, nom_doc, evaluador, id_tipo_documento, num_doc, oficina_crea, expediente);
            var result = (from MDS in data
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
                              documento_codigo_habilitacion =MDS.CODIGO_HABILITANTE,
                              nom_externo = MDS.NOMBRE_EXTERNO,
                              asunto = MDS.ASUNTO,
                              num_documento = MDS.NUM_DOCUMENTO,
                              nom_documento = MDS.NOMBRE_DOCUMENTO,
                              evaluador = MDS.EVALUADOR,
                              group_expedientes = MDS.EXPEDIENTES,
                              fecha_od = MDS.FECHA_OD,
                              ruta_pdf =MDS.RUTA_PDF
                          }).Distinct().OrderByDescending(r => r.id_documento_seg).Take(500).AsEnumerable();
                return result;
        /*}*/
        }

    }
}
