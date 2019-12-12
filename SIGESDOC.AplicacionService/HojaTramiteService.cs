using SIGESDOC.Entidades;
using SIGESDOC.IAplicacionService;
using SIGESDOC.IRepositorio;
using SIGESDOC.Request;
using SIGESDOC.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace SIGESDOC.AplicacionService
{
    public class HojaTramiteService : IHojaTramiteService
    {
        /*01*/
        private readonly IHojaTramiteRepositorio _hojatramiteRepositorio;
        /*02*/
        private readonly IDocumentoRepositorio _DocumentoRepositorio;
        /*03*/
        private readonly IDocumentoDetalleRepositorio _DocumentoDetalleRepositorio;
        /*04*/
        private readonly ITipoDocumentoRepositorio _TipoDocumentoRepositorio;
        /*05*/
        private readonly IConsultarDniRepositorio _ConsultarDniRepositorio;
        /*06*/
        private readonly IConsultarOficinaRepositorio _ConsultarOficinaRepositorio;
        /*07*/
        private readonly IConsultarSedeOficinaRepositorio _ConsultarSedeOficinaRepositorio;
        /*08*/
        private readonly IHtExternoRepositorio _HtExternoRepositorio;
        /*09*/
        private readonly IHtInternoRepositorio _HtInternoRepositorio;
        /*10*/
        private readonly IExpedientesRepositorio _ExpedientesRepositorio;
        /*11*/
        private readonly IUnitOfWork _unitOfWork;
        /*12*/
        private readonly ILogDesarchivoDesatendidoRepositorio _LogDesarchivoDesatendidoRepositorio;
        /*13*/
        private readonly IConsultarPersonalRepositorio _ConsultarPersonalRepositorio;
        private readonly IDocDetObservacionesRepositorio _DocDetObservacionesRepositorio;
        private readonly IExpedienteUnicoRepositorio _ExpedienteUnicoRepositorio;
        private readonly IConsultarPendientesHtParaAdjuntar20190104Repositorio _IConsultarPendientesHtParaAdjuntar20190104Repositorio;
        private readonly IVerPedientesGesdocRepositorio _VerPedientesGesdocRepositorio;
        private readonly IConsultaPendientesSanipesDetalleRepositorio _ConsultaPendientesSanipesDetalleRepositorio;
        private readonly IEstadoTramiteRepositorio _EstadoTramiteRepositorio;
        private readonly IDocumentoAnexoRepositorio _DocumentoAnexoRepositorio;
        private readonly IDetalleMaeDocumentoRepositorio _DetalleMaeDocumentoRepositorio;
        

        public HojaTramiteService(
            /*01*/  IHojaTramiteRepositorio hojatramiteRepositorio,
            /*02*/  IDocumentoRepositorio DocumentoRepositorio,
            /*03*/  IDocumentoDetalleRepositorio DocumentoDetalleRepositorio,
            /*04*/  ITipoDocumentoRepositorio TipoDocumentoRepositorio,
            /*05*/  IConsultarDniRepositorio ConsultarDniRepositorio,
            /*06*/  IConsultarOficinaRepositorio ConsultarOficinaRepositorio,
            /*07*/  IConsultarSedeOficinaRepositorio ConsultarSedeOficinaRepositorio,
            /*08*/  IHtExternoRepositorio HtExternoRepositorio,
            /*09*/  IHtInternoRepositorio HtInternoRepositorio,
            /*10*/  IExpedientesRepositorio ExpedientesRepositorio,
            /*11*/  IUnitOfWork unitOfWork,
            /*12*/  ILogDesarchivoDesatendidoRepositorio LogDesarchivoDesatendidoRepositorio,
            /*13*/  IConsultarPersonalRepositorio ConsultarPersonalRepositorio,
            IDocDetObservacionesRepositorio DocDetObservacionesRepositorio,
            IExpedienteUnicoRepositorio ExpedienteUnicoRepositorio,
            IConsultarPendientesHtParaAdjuntar20190104Repositorio IConsultarPendientesHtParaAdjuntar20190104Repositorio,
            IVerPedientesGesdocRepositorio VerPedientesGesdocRepositorio,
            IConsultaPendientesSanipesDetalleRepositorio ConsultaPendientesSanipesDetalleRepositorio,
            IEstadoTramiteRepositorio EstadoTramiteRepositorio,
            IDocumentoAnexoRepositorio DocumentoAnexoRepositorio,
        IDetalleMaeDocumentoRepositorio DetalleMaeDocumentoRepositorio

            )
        {
            /*01*/
            _hojatramiteRepositorio = hojatramiteRepositorio;
            /*02*/
            _DocumentoRepositorio = DocumentoRepositorio;
            /*03*/
            _DocumentoDetalleRepositorio = DocumentoDetalleRepositorio;
            /*04*/
            _TipoDocumentoRepositorio = TipoDocumentoRepositorio;
            /*05*/
            _ConsultarDniRepositorio = ConsultarDniRepositorio;
            /*06*/
            _ConsultarOficinaRepositorio = ConsultarOficinaRepositorio;
            /*07*/
            _ConsultarSedeOficinaRepositorio = ConsultarSedeOficinaRepositorio;
            /*08*/
            _HtExternoRepositorio = HtExternoRepositorio;
            /*09*/
            _HtInternoRepositorio = HtInternoRepositorio;
            /*10*/
            _ExpedientesRepositorio = ExpedientesRepositorio;
            /*11*/
            _unitOfWork = unitOfWork;
            /*12*/
            _LogDesarchivoDesatendidoRepositorio = LogDesarchivoDesatendidoRepositorio;
            /*13*/
            _ConsultarPersonalRepositorio = ConsultarPersonalRepositorio;
            _DocDetObservacionesRepositorio = DocDetObservacionesRepositorio;
            _ExpedienteUnicoRepositorio = ExpedienteUnicoRepositorio;
            _IConsultarPendientesHtParaAdjuntar20190104Repositorio = IConsultarPendientesHtParaAdjuntar20190104Repositorio;
            _VerPedientesGesdocRepositorio = VerPedientesGesdocRepositorio;
            _ConsultaPendientesSanipesDetalleRepositorio = ConsultaPendientesSanipesDetalleRepositorio;
            _EstadoTramiteRepositorio = EstadoTramiteRepositorio;
            _DocumentoAnexoRepositorio = DocumentoAnexoRepositorio;

            _DetalleMaeDocumentoRepositorio = DetalleMaeDocumentoRepositorio;
        }


        /*01*/
        public bool Crear_Empresa(string ruc, string nombre, string siglas, string usuario)
        {
            try
            {
                return _hojatramiteRepositorio.Crear_Empresa(ruc, nombre, siglas, usuario);
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*02*/
        public int Create(HojaTramiteRequest request)
        {
            MAE_HOJA_TRAMITE entity = RequestToEntidad.HojaTramite(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _hojatramiteRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.NUMERO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*03*/
        public bool Update(HojaTramiteRequest request)
        {
            MAE_HOJA_TRAMITE entity = RequestToEntidad.HojaTramite(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _hojatramiteRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*04*/
        public int Documento_Create(DocumentoRequest request)
        {
            MAE_DOCUMENTO entity = RequestToEntidad.Documento(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DOCUMENTO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*05*/
        public bool Documento_Update(DocumentoRequest request)
        {
            MAE_DOCUMENTO entity = RequestToEntidad.Documento(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        
        public bool Documento_anexo_Update(DocumentoAnexoRequest request)
        {
            MAE_DOCUMENTO_ANEXO entity = RequestToEntidad.Documentoanexo(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoAnexoRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
                return true;
                
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public int Documento_anexo_Insertar(DocumentoAnexoRequest request)
        {
            MAE_DOCUMENTO_ANEXO entity = RequestToEntidad.Documentoanexo(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoAnexoRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                 return entity.ID_DOCUMENTO_ANEXO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*06*/
        public int Create_Expediente(ExpedientesRequest request)
        {
            MAE_EXPEDIENTES entity = RequestToEntidad.expedientes(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ExpedientesRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_EXPEDIENTE;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*07*/
        public string Create_numero(int tipo_ht)
        {
            tipo_ht = 3;
            COUNT_EXPEDIENTE_UNICO entity_ext = new COUNT_EXPEDIENTE_UNICO();
            entity_ext.fechaRegistro = DateTime.Now;
            entity_ext.AÑO = DateTime.Now.Year;
            entity_ext.FUENTE_SISTEMA = "SIGESDOC";

            using (TransactionScope scope = new TransactionScope())
            {
                _ExpedienteUnicoRepositorio.Insertar(entity_ext);
                _unitOfWork.Guardar();
                scope.Complete();
            }

            return (entity_ext.ID_EXPEDIENTE.ToString() + DateTime.Now.Year.ToString());

            /*
            if (tipo_ht == 1)
            {
                COUNT_HT_EXTERNO entity_ext = new COUNT_HT_EXTERNO();
                entity_ext.ACTIVO = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    _HtExternoRepositorio.Insertar(entity_ext);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
                return ("E" + entity_ext.HT_EXTERNO.ToString());
            }
            else
            {
                COUNT_HT_INTERNO entity_int = new COUNT_HT_INTERNO();
                entity_int.ACTIVO = true;

                using (TransactionScope scope = new TransactionScope())
                {
                    _HtInternoRepositorio.Insertar(entity_int);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
                return ("I" + entity_int.HT_INTERNO.ToString());
            }
             */

        }
        /*08*/
        public int Documento_detalle_Create(DocumentoDetalleRequest request)
        {
            DAT_DOCUMENTO_DETALLE entity = RequestToEntidad.DocumentoDetalle(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_DOCUMENTO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*09*/
        public bool Documento_detalle_Update(DocumentoDetalleRequest request)
        {

            DAT_DOCUMENTO_DETALLE entity = RequestToEntidad.DocumentoDetalle(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*10*/
        public IEnumerable<DocumentoResponse> GetAllDocumento(int id_documento)
        {
            var result = from zp in _DocumentoRepositorio.Listar(x => x.ID_DOCUMENTO == id_documento)
                         select new DocumentoResponse
                         {
                             id_documento = zp.ID_DOCUMENTO,
                             numero = zp.NUMERO,
                             id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                             numero_documento = zp.NUMERO_DOCUMENTO,
                             anexos = zp.ANEXOS,
                             folios = zp.FOLIOS,
                             oficina_crea = zp.OFICINA_CREA,
                             fecha_envio = zp.FECHA_ENVIO,
                             usuario_crea = zp.USUARIO_CREA,
                             nom_doc = zp.NOM_DOC,
                             persona_crea = zp.PERSONA_CREA,
                             id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                             ruta_pdf = zp.RUTA_PDF,
                             num_ext = zp.NUM_EXT,
                             nom_oficina_crea = zp.NOM_OFICINA_CREA
                         };

            return result.ToList();
        }


        public DocumentoResponse GetAllDocumento_resp(int id_documento)
        {
            var result = (from zp in _DocumentoRepositorio.Listar(x => x.ID_DOCUMENTO == id_documento)
                          select new DocumentoResponse
                          {
                              id_documento = zp.ID_DOCUMENTO,
                              numero = zp.NUMERO,
                              id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              anexos = zp.ANEXOS,
                              folios = zp.FOLIOS,
                              oficina_crea = zp.OFICINA_CREA,
                              fecha_envio = zp.FECHA_ENVIO,
                              usuario_crea = zp.USUARIO_CREA,
                              nom_doc = zp.NOM_DOC,
                              persona_crea = zp.PERSONA_CREA,
                              id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                              ruta_pdf = zp.RUTA_PDF,
                              num_ext = zp.NUM_EXT,
                              nom_oficina_crea = zp.NOM_OFICINA_CREA
                          }).First();

            return result;
        }


        public IEnumerable<DocumentoResponse> GetAllDocumento_lista_resp_x_ht(int numero_ht)
        {
            return _hojatramiteRepositorio.GetAllDocumento_lista_resp_x_ht(numero_ht);
        }

        public DocumentoRequest GetAllDocumento_req(int id_documento)
        {
            var result = (from zp in _DocumentoRepositorio.Listar(x => x.ID_DOCUMENTO == id_documento)
                          select new DocumentoRequest
                          {
                              id_documento = zp.ID_DOCUMENTO,
                              numero = zp.NUMERO,
                              id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              anexos = zp.ANEXOS,
                              folios = zp.FOLIOS,
                              oficina_crea = zp.OFICINA_CREA,
                              fecha_envio = zp.FECHA_ENVIO,
                              usuario_crea = zp.USUARIO_CREA,
                              nom_doc = zp.NOM_DOC,
                              persona_crea = zp.PERSONA_CREA,
                              id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                              ruta_pdf = zp.RUTA_PDF,
                              num_ext = zp.NUM_EXT,
                              nom_oficina_crea = zp.NOM_OFICINA_CREA
                          }).First();

            return result;
        }

        public IEnumerable<DocumentoAnexoResponse> Lista_Documentos_anexos(int id_documento)
        {
            var result = from zp in _DocumentoAnexoRepositorio.Listar(x => x.ID_DOCUMENTO == id_documento && x.ACTIVO == "1")
                         select new DocumentoAnexoResponse
                         {
                             id_documento_anexo = zp.ID_DOCUMENTO_ANEXO,
                             id_documento = zp.ID_DOCUMENTO,
                             ruta = zp.RUTA,
                             descripcion = zp.DESCRIPCION,
                             extension = zp.EXTENSION
                         };

            return result;
        }

        public DocumentoAnexoResponse Documento_Anexo_HT(int id_documento_anexo)
        {
            var result = (from zp in _DocumentoAnexoRepositorio.Listar(x => x.ID_DOCUMENTO_ANEXO == id_documento_anexo)
                         select new DocumentoAnexoResponse
                         {
                             id_documento = zp.ID_DOCUMENTO,
                             ruta = zp.RUTA,
                             descripcion = zp.DESCRIPCION,
                             extension = zp.EXTENSION
                         }).First();

            return result;
        }

        public DocumentoRequest GetAllDocumento_req_x_ht(int numero_ht)
        {
            var result = (from zp in _DocumentoRepositorio.Listar(x => x.NUMERO == numero_ht && x.ID_INDICADOR_DOCUMENTO == 1)
                          select new DocumentoRequest
                          {
                              id_documento = zp.ID_DOCUMENTO,
                              numero = zp.NUMERO,
                              id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              anexos = zp.ANEXOS,
                              folios = zp.FOLIOS,
                              oficina_crea = zp.OFICINA_CREA,
                              fecha_envio = zp.FECHA_ENVIO,
                              usuario_crea = zp.USUARIO_CREA,
                              nom_doc = zp.NOM_DOC,
                              persona_crea = zp.PERSONA_CREA,
                              id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                              ruta_pdf = zp.RUTA_PDF,
                              num_ext = zp.NUM_EXT,
                              nom_oficina_crea = zp.NOM_OFICINA_CREA
                          }).First();

            return result;
        }

        /*11*/
        public IEnumerable<DocumentoRequest> GetAllDocumento_x_Numero_HT_request(int numero)
        {
            var result = from zp in _DocumentoRepositorio.Listar(x => x.NUMERO == numero)
                         select new DocumentoRequest
                         {
                             id_documento = zp.ID_DOCUMENTO,
                             numero = zp.NUMERO,
                             id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                             numero_documento = zp.NUMERO_DOCUMENTO,
                             anexos = zp.ANEXOS,
                             folios = zp.FOLIOS,
                             oficina_crea = zp.OFICINA_CREA,
                             fecha_envio = zp.FECHA_ENVIO,
                             usuario_crea = zp.USUARIO_CREA,
                             nom_doc = zp.NOM_DOC,
                             persona_crea = zp.PERSONA_CREA,
                             id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                             num_ext = zp.NUM_EXT,
                             ruta_pdf = zp.RUTA_PDF,
                             nom_oficina_crea = zp.NOM_OFICINA_CREA
                         };

            return result.ToList();
        }
        /*12*/
        public IEnumerable<DocumentoResponse> GetAllDocumento_x_Numero_HT(int numero)
        {
            var result = from zp in _DocumentoRepositorio.Listar(x => x.NUMERO == numero)
                         select new DocumentoResponse
                         {
                             id_documento = zp.ID_DOCUMENTO,
                             numero = zp.NUMERO,
                             id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                             numero_documento = zp.NUMERO_DOCUMENTO,
                             anexos = zp.ANEXOS,
                             folios = zp.FOLIOS,
                             nom_doc = zp.NOM_DOC,
                             persona_crea = zp.PERSONA_CREA,
                             id_indicador_documento = zp.ID_INDICADOR_DOCUMENTO,
                             nom_oficina_crea = zp.NOM_OFICINA_CREA
                         };

            return result.ToList();
        }
        /*13*/
        public string Consult_tipo_docuemnto(int id_tipo_documento)
        {
            var result = (from zp in _TipoDocumentoRepositorio.Listar(x => x.ID_TIPO_DOCUMENTO == id_tipo_documento)
                          select new TipoDocumentoResponse
                          {
                              nombre = zp.NOMBRE
                          }).OrderBy(r => r.nombre);

            return result.ToList().First().nombre;
        }
        /*14*/
        public HojaTramiteRequest GetAllHT_x_Numero_request(int numero)
        {
            var result = from zp in _hojatramiteRepositorio.Listar(x => x.NUMERO == numero)
                         select new HojaTramiteRequest
                         {
                             numero = zp.NUMERO,
                             id_tipo_tramite = zp.ID_TIPO_TRAMITE,
                             id_oficina = zp.ID_OFICINA,
                             fecha_emision = zp.FECHA_EMISION,
                             usuario_emision = zp.USUARIO_EMISION,
                             asunto = zp.ASUNTO,
                             persona_num_documento = zp.persona_num_documento,
                             tipo_per = zp.TIPO_PER,
                             hoja_tramite = zp.HOJA_TRAMITE,
                             id_expediente = zp.ID_EXPEDIENTE,
                             numero_padre = zp.NUMERO_PADRE,
                             ruta_pdf = zp.RUTA_PDF,
                             referencia = zp.REFERENCIA,
                             editar = zp.EDITAR,
                             pedido_siga = zp.PEDIDO_SIGA,
                             id_tipo_pedido_siga = zp.ID_TIPO_PEDIDO_SIGA,
                             anno_siga = zp.ANNO_SIGA,
                             clave = zp.CLAVE,
                             id_tupa = zp.ID_TUPA,
                             nombre_externo = zp.NOMBRE_EXTERNO
                         };

            return result.ToList().First();
        }
        /*15*/
        public IEnumerable<DocumentoDetalleResponse> GetAllDocumentoDetalle(int id_det_documento)
        {

            var result = from zp in _DocumentoDetalleRepositorio.Listar(x => x.ID_DET_DOCUMENTO == id_det_documento)
                         select new DocumentoDetalleResponse
                         {
                             id_documento = zp.ID_DOCUMENTO,
                             id_est_tramite = zp.ID_EST_TRAMITE
                         };

            return result.ToList();
        }
        /*16*/
        public IEnumerable<HojaTramiteResponse> GetAllHT_x_HojaTramite(string HT)
        {

            var result = from zp in _hojatramiteRepositorio.Listar(x => x.HOJA_TRAMITE == HT.Trim())
                         select new HojaTramiteResponse
                         {
                             numero = zp.NUMERO,
                             id_tipo_tramite = zp.ID_TIPO_TRAMITE,
                             id_oficina = zp.ID_OFICINA,
                             fecha_emision = zp.FECHA_EMISION,
                             usuario_emision = zp.USUARIO_EMISION,
                             asunto = zp.ASUNTO,
                             referencia = zp.REFERENCIA,
                             persona_num_documento = zp.persona_num_documento,
                             tipo_per = zp.TIPO_PER,
                             hoja_tramite = zp.HOJA_TRAMITE,
                             id_expediente = zp.ID_EXPEDIENTE,
                             numero_padre = zp.NUMERO_PADRE,
                             ruta_pdf = zp.RUTA_PDF,
                             editar = zp.EDITAR,
                             pedido_siga = zp.PEDIDO_SIGA,
                             id_tipo_pedido_siga = zp.ID_TIPO_PEDIDO_SIGA,
                             anno_siga = zp.ANNO_SIGA,
                             clave = zp.CLAVE,
                             id_tupa = zp.ID_TUPA,
                             nombre_externo = zp.NOMBRE_EXTERNO
                         };

            return result.ToList();
        }
        /*17*/
        public IEnumerable<DocumentoDetalleResponse> Consultar_Doc_detalle(int id_det_documento)
        {

            var result = from zp in _DocumentoDetalleRepositorio.Listar(x => x.ID_DET_DOCUMENTO == id_det_documento)
                         select new DocumentoDetalleResponse
                         {
                             id_det_documento = zp.ID_DET_DOCUMENTO,
                             id_documento = zp.ID_DOCUMENTO,
                             id_cab_det_documento = zp.ID_CAB_DET_DOCUMENTO,
                             oficina_destino = zp.OFICINA_DESTINO,
                             observacion = zp.OBSERVACION,
                             id_est_tramite = zp.ID_EST_TRAMITE,
                             persona_num_documento = zp.persona_num_documento,
                             ind_01 = zp.IND_01,
                             ind_02 = zp.IND_02,
                             ind_03 = zp.IND_03,
                             ind_04 = zp.IND_04,
                             ind_05 = zp.IND_05,
                             ind_06 = zp.IND_06,
                             ind_07 = zp.IND_07,
                             ind_08 = zp.IND_08,
                             ind_09 = zp.IND_09,
                             ind_10 = zp.IND_10,
                             ind_11 = zp.IND_11,
                             indicadores = zp.INDICADORES,
                             fecha_recepcion = zp.FECHA_RECEPCION,
                             usuario_recepcion = zp.USUARIO_RECEPCION,
                             fecha_atendido = zp.FECHA_ATENDIDO,
                             usuario_atendido = zp.USUARIO_ATENDIDO,
                             fecha_archivo = zp.FECHA_ARCHIVO,
                             usuario_archivo = zp.USUARIO_ARCHIVO,
                             fecha_derivado = zp.FECHA_DERIVADO,
                             usuario_derivado = zp.USUARIO_DERIVADO,
                             usuario_crea = zp.USUARIO_CREA,
                             fecha_crea = zp.FECHA_CREA,
                             oficina_crea = zp.OFICINA_CREA,
                             fecha_cancelar = zp.FECHA_CANCELAR,
                             usuario_cancelar = zp.USUARIO_CANCELAR,
                             nom_oficina_crea = zp.NOM_OFICINA_CREA,
                             nom_oficina_destino = zp.NOM_OFICINA_DESTINO
                         };

            return result.ToList();
        }
        /*18*/
        public IEnumerable<HojaTramiteResponse> GetAllHojaTramite_Padre()
        {

            var result = from zp in _hojatramiteRepositorio.Listar(x => x.NUMERO_PADRE == null)
                         select new HojaTramiteResponse
                         {
                             numero = zp.NUMERO,
                             hoja_tramite = zp.HOJA_TRAMITE,
                             asunto = zp.ASUNTO,
                             fecha_emision = zp.FECHA_EMISION,
                             editar = zp.EDITAR
                         };

            return result.ToList();
        }
        public IEnumerable<Response.SP_CONSULTA_HISTORIAL_HT_Result> recupera_historial_ht(int numero)
        {
            return _hojatramiteRepositorio.recupera_historial_ht(numero);
        }
        /*19*/
        public IEnumerable<ConsultarDniResponse> Consultar_DNI(string DNI)
        {

            var result = from zp in _ConsultarDniRepositorio.Listar(x => x.persona_num_documento == DNI)
                         select new ConsultarDniResponse
                         {
                             persona_num_documento = zp.persona_num_documento,
                             paterno = zp.paterno,
                             materno = zp.materno,
                             nombres = zp.nombres,
                             direccion = zp.direccion
                         };
            return result.ToList();
        }
        /*20*/
        public IEnumerable<ConsultarOficinaResponse> Consultar_RUC(string RUC)
        {
            var result_dir = from zp in _hojatramiteRepositorio.GetAll_Oficinas_Direcciones(RUC)
                             select new ConsultarOficinaResponse
                             {
                                 id_oficina = zp.id_oficina,
                                 nombre = zp.nom_oficina,
                                 ruc = zp.ruc
                             };

            return result_dir.ToList();
        }
        /*21*/
        public IEnumerable<ConsultarOficinaResponse> Consultar_RUC_X_NOM(string NOM)
        {
            /*
            var result_dir = (from zp in _hojatramiteRepositorio.GetAll_Oficinas_Direcciones_X_NOM(NOM)
                             select new ConsultarOficinaResponse
                             {
                                 id_oficina = zp.id_oficina,
                                 nombre = zp.nom_oficina,
                                 ruc = zp.ruc
                             }).Distinct();

            return result_dir.ToList();*/
            var result_dir = from zp in _hojatramiteRepositorio.GetAll_Oficinas_Direcciones_X_NOM(NOM)
                             select new ConsultarOficinaResponse
                             {
                                 id_oficina = zp.id_oficina,
                                 nombre = zp.nombre,
                                 ruc = zp.ruc
                             };
            return result_dir.ToList();
        }
        /*22*/
        public IEnumerable<ConsultarDireccionResponse> Consultar_DIRECCION(int ID_OFICINA)
        {


            var result_dir = from zp in _hojatramiteRepositorio.Getall_Direccion_x_Oficina(ID_OFICINA)
                             select new ConsultarDireccionResponse
                             {
                                 id_oficina_direccion = zp.id_oficina_direccion,
                                 direccion = zp.direccion
                             };

            return result_dir.ToList();
        }
        /*23*/
        public IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos(int id_oficina_logeo, string HT, string Asunto, string empresa, int id_ofi_crea, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllNoRecibidos(id_oficina_logeo, HT, Asunto, empresa, id_ofi_crea, cmbtupa);
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllNoRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int id_ofi_crea, string persona_num_documento, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllNoRecibidos_x_persona(id_oficina_logeo, HT, Asunto, Empresa, id_ofi_crea, persona_num_documento, cmbtupa);
        }
        /*23*/
        public IEnumerable<DocumentoDetalleResponse> GetAllHoja_Tramite_x_PEDIDO_SIGA(int id_tipo_pedido_siga, int pedido_siga, int anno_siga, int id_oficina_dir)
        {
            return _hojatramiteRepositorio.GetAllHoja_Tramite_x_PEDIDO_SIGA(id_tipo_pedido_siga, pedido_siga, anno_siga, id_oficina_dir);
        }
        /*24*/
        public IEnumerable<DocumentoResponse> GetAllHT(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa)
        {
            return _hojatramiteRepositorio.GetAllHT(id_oficina_logeo, HT, asunto, empresa, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, id_tupa);
        }
        /*26*/
        public IEnumerable<DocumentoResponse> GetmisHT(int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa)
        {
            return _hojatramiteRepositorio.GetmisHT(id_oficina_logeo, HT, asunto, Empresa, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, id_tupa);
        }

        /*26*/
        public IEnumerable<DocumentoResponse> GetmisDoc(int id_oficina_logeo, string HT, string asunto, string cmbtipo_documento, string num_documento, string nom_documento, int ival_txtfechainicio, int ival_txtfechafin, string id_tupa, string anexos, string Empresa)
        {
            return _hojatramiteRepositorio.GetmisDoc(id_oficina_logeo, HT, asunto, cmbtipo_documento, num_documento, nom_documento, ival_txtfechainicio, ival_txtfechafin, id_tupa, anexos, Empresa);
        }
        public IEnumerable<DocumentoResponse> Recupera_Documento(int id_oficina_logeo, int id_tipo_documento, int anio_doc)
        {
            var res = (from r in _DocumentoRepositorio.Listar(x => x.OFICINA_CREA == id_oficina_logeo && x.ID_TIPO_DOCUMENTO == id_tipo_documento && x.FECHA_ENVIO.Year == anio_doc)
                       select new DocumentoResponse()
                       {
                           id_documento = r.ID_DOCUMENTO,
                           numero_documento = r.NUMERO_DOCUMENTO,
                           id_tipo_documento = r.ID_TIPO_DOCUMENTO,
                           fecha_envio = r.FECHA_ENVIO
                       }).OrderByDescending(x => x.fecha_envio);
            return res;
        }
        /*28*/
        public IEnumerable<ConsultarDniResponse> GetAllPersona_Natural(int pageIndex, int pageSize, string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE)
        {
            return _hojatramiteRepositorio.GetAllPersona_Natural(pageIndex, pageSize, persona_num_documento, PATERNO, MATERNO, NOMBRE);
        }
        /*29*/
        public int CountPersona_Natural(string persona_num_documento, string PATERNO, string MATERNO, string NOMBRE)
        {
            return _hojatramiteRepositorio.CountPersona_Natural(persona_num_documento, PATERNO, MATERNO, NOMBRE);
        }
        /*30*/
        public int CountHT()
        {
            return _hojatramiteRepositorio.Contar(x => x.FECHA_EMISION.Year == DateTime.Now.Year);
        }
        /*32*/
        public DocumentoResponse Consultar_HT(string HT)
        {
            return _hojatramiteRepositorio.Consultar_HT(HT);
        }
        /*33*/
        public IEnumerable<DocumentoDetalleResponse> GetAllRecibidos(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllRecibidos(id_oficina_logeo, HT, Asunto, Empresa, Estado, cmbtupa);
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllRecibidos_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, int Estado, string persona_num_documento, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllRecibidos_x_persona(id_oficina_logeo, HT, Asunto, Empresa, Estado, persona_num_documento, cmbtupa);
        }

        /*35*/
        public IEnumerable<DocumentoDetalleResponse> GetAllDerivadas(int id_oficina_logeo, string HT, string Asunto, string Empresa, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllDerivadas(id_oficina_logeo, HT, Asunto, Empresa, cmbtupa);
        }

        public IEnumerable<DocumentoDetalleResponse> GetAllDerivadas_x_persona(int id_oficina_logeo, string HT, string Asunto, string Empresa, string persona_num_documento, string cmbtupa)
        {
            return _hojatramiteRepositorio.GetAllDerivadas_x_persona(id_oficina_logeo, HT, Asunto, Empresa, persona_num_documento, cmbtupa);
        }
        /*37*/
        public bool Recibir_ht(int id, string usuario_logeo)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.FECHA_RECEPCION = DateTime.Now;
            entity.USUARIO_RECEPCION = usuario_logeo;
            entity.ID_EST_TRAMITE = 2;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*38*/
        public bool Archivar_ht(int id, string usuario_logeo, string observacion)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.FECHA_ARCHIVO = DateTime.Now;
            entity.USUARIO_ARCHIVO = usuario_logeo;
            entity.OBSERVACION_ARCHIVO = observacion;
            entity.ID_EST_TRAMITE = 4;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }



        /*38*/
        public bool cancelar_recepcion_ht(int id)
        {

            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.USUARIO_RECEPCION = null;
            entity.FECHA_RECEPCION = null;
            entity.ID_EST_TRAMITE = 1;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*39*/
        public bool Cancelar_Ht(int id, string usuario_logeo)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.FECHA_CANCELAR = DateTime.Now;
            entity.USUARIO_CANCELAR = usuario_logeo;
            entity.ID_EST_TRAMITE = 6;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                if (entity.ID_CAB_DET_DOCUMENTO != null)
                {
                    Retornar_Estado_En_Proceso(entity.ID_CAB_DET_DOCUMENTO, entity.OFICINA_CREA);
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }


        }
        /*40*/
        public bool Atender_ht(int id, string usuario_logeo, string observacion)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.FECHA_ATENDIDO = DateTime.Now;
            entity.USUARIO_ATENDIDO = usuario_logeo;
            entity.OBSERVACION_ATENDIDO = observacion;
            entity.ID_EST_TRAMITE = 3;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*40*/
        public bool Editar_Observacion_Detalle(int id, string usuario_logeo, string observacion)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            if (entity.ID_EST_TRAMITE == 3)
            {
                entity.USUARIO_ATENDIDO = usuario_logeo;
                entity.OBSERVACION_ATENDIDO = observacion;
            }
            else
            {
                entity.USUARIO_ARCHIVO = usuario_logeo;
                entity.OBSERVACION_ARCHIVO = observacion;
            }
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*41*/
        public bool Derivar_HT(int id, string usuario_logeo)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.FECHA_DERIVADO = DateTime.Now;
            entity.USUARIO_DERIVADO = usuario_logeo;
            entity.ID_EST_TRAMITE = 5;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*42*/
        public bool Asignar_HT(int id, string persona_num_documento)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);
            entity.persona_num_documento = persona_num_documento;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*43*/
        public IEnumerable<DocumentoDetalleResponse> GetmisHT_archivados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.GetmisHT_archivados(pageIndex, pageSize, id_oficina_logeo, HT, asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
        }
        /*44*/
        public int CountmisHt_archivados(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.CountmisHt_archivados(id_oficina_logeo, HT, asunto, empresa, cmbtipo_documento, num_documento, nom_documento);
        }
        /*43*/
        public IEnumerable<DocumentoDetalleResponse> GetmisHT_finalizados(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.GetmisHT_finalizados(pageIndex, pageSize, id_oficina_logeo, HT, asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
        }
        /*44*/
        public int CountmisHt_finalizados(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.CountmisHt_finalizados(id_oficina_logeo, HT, asunto, empresa, cmbtipo_documento, num_documento, nom_documento);
        }
        /*45*/
        public bool Quitar_Archivo_Atendido_ht(int id, string usuario_logeo, string observacion)
        {
            DAT_DOCUMENTO_DETALLE entity;

            entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == id);

            DAT_LOG_DESARCHIVO_DESATENDIDO entity_desactivo = new DAT_LOG_DESARCHIVO_DESATENDIDO();

            entity_desactivo.ID_DET_DOCUMENTO = id;
            entity_desactivo.OLD_ID_EST_TRAMITE = entity.ID_EST_TRAMITE;
            entity_desactivo.OLD_FECHA = entity.FECHA_ARCHIVO;
            entity_desactivo.OLD_USUARIO = entity.USUARIO_ARCHIVO;
            entity_desactivo.OLD_OBSERVACION = entity.OBSERVACION_ARCHIVO;
            entity_desactivo.FECHA_DESACTIVO = DateTime.Now;
            entity_desactivo.USUARIO_DESACTIVO = usuario_logeo;
            entity_desactivo.OBSERVACION = observacion;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _LogDesarchivoDesatendidoRepositorio.Insertar(entity_desactivo);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }

            entity.OBSERVACION_ARCHIVO = "";
            entity.FECHA_ARCHIVO = null;
            entity.USUARIO_ARCHIVO = null;
            entity.OBSERVACION_ATENDIDO = "";
            entity.FECHA_ATENDIDO = null;
            entity.USUARIO_ATENDIDO = null;
            entity.ID_EST_TRAMITE = 2;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDetalleRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return true;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*46*/
        public IEnumerable<DocumentoDetalleResponse> GetmisHT_atendidos(int pageIndex, int pageSize, int id_oficina_logeo, string HT, string asunto, string Empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.GetmisHT_atendidos(pageIndex, pageSize, id_oficina_logeo, HT, asunto, Empresa, cmbtipo_documento, num_documento, nom_documento);
        }
        /*47*/
        public int CountmisHt_atendidos(int id_oficina_logeo, string HT, string asunto, string empresa, string cmbtipo_documento, string num_documento, string nom_documento)
        {
            return _hojatramiteRepositorio.CountmisHt_atendidos(id_oficina_logeo, HT, asunto, empresa, cmbtipo_documento, num_documento, nom_documento);
        }

        public IEnumerable<Response.SP_EDITA_DB_SEGURIDAD_PERSONA_Result> editar_persona(string persona_num_documento, string paterno, string materno, string nombres, string direccion, string ubigeo)
        {
            return _ConsultarDniRepositorio.editar_persona(persona_num_documento, paterno, materno, nombres, direccion, ubigeo);
        }

        /*48*/
        public IEnumerable<ConsultarDniResponse> Consultar_DNI_x_NOM(string NOM, string TIPO)
        {

            var result = from zp in _ConsultarDniRepositorio.Listar(x => (x.paterno + " " + x.materno + " " + x.nombres).Contains(NOM) && x.tipo_doc_iden.ToString() == TIPO)
                         select new ConsultarDniResponse
                         {
                             persona_num_documento = zp.persona_num_documento,
                             paterno = zp.paterno,
                             materno = zp.materno,
                             nombres = zp.nombres,
                             direccion = zp.direccion
                         };
            return result.ToList();
        }

        public IEnumerable<EstadoTramiteResponse> lista_estado_tramite()
        {

            var result = from zp in _EstadoTramiteRepositorio.Listar()
                         select new EstadoTramiteResponse
                         {
                             id_est_tramite = zp.ID_EST_TRAMITE,
                             nombre = zp.NOMBRE
                         };
            return result.ToList();
        }

        /*49*/
        public ConsultarDniResponse Recupera_persona_x_documento(string persona_num_doc)
        {

            var result = (from zp in _ConsultarDniRepositorio.Listar(x => x.persona_num_documento == persona_num_doc)
                          select new ConsultarDniResponse
                          {
                              persona_num_documento = zp.persona_num_documento,
                              tipo_doc_iden = zp.tipo_doc_iden,
                              nom_tipo_doc = zp.NOM_TIPO_DOC,
                              paterno = zp.paterno,
                              materno = zp.materno,
                              nombres = zp.nombres,
                              direccion = zp.direccion,
                              ubigeo = zp.ubigeo
                          }).ToList().First();
            return result;
        }

        /*50*/
        public IEnumerable<ConsultarPersonalResponse> Recupera_oficina_x_persona(int pageIndex, int pageSize, string persona_num_doc)
        {

            var result = (from zp in _ConsultarPersonalRepositorio.Listar(x => x.persona_num_documento == persona_num_doc && x.ACTIVO == true && x.RUC != "20565429656")
                          select new ConsultarPersonalResponse
                          {
                              id_per_empresa = zp.ID_PER_EMPRESA,
                              ruc = zp.RUC,
                              razon_social = zp.RAZON_SOCIAL,
                              nom_ofi = zp.NOM_OFI,
                              nom_sede = zp.NOM_SEDE
                          }).OrderByDescending(r => r.razon_social).Skip((pageIndex - 1) * pageSize).Take(pageSize).AsEnumerable(); // ordenado por documento enviado
            return result;
        }
        /*51*/
        public int Count_oficina_x_persona(string persona_num_doc)
        {

            var result = (from zp in _ConsultarPersonalRepositorio.Listar(x => x.persona_num_documento == persona_num_doc && x.ACTIVO == true && x.RUC != "20565429656")
                          select new ConsultarPersonalResponse
                          {
                              id_per_empresa = zp.ID_PER_EMPRESA,
                              ruc = zp.RUC,
                              razon_social = zp.RAZON_SOCIAL,
                              nom_ofi = zp.NOM_OFI,
                              nom_sede = zp.NOM_SEDE
                          }).AsEnumerable().Count(); // ordenado por documento enviado
            return result;
        }
        /*52*/
        public IEnumerable<ConsultarOficinaResponse> Consulta_Empresas()
        {
            var result_dir = from zp in _hojatramiteRepositorio.GetAll_Empresas_con_Oficinas()
                             select new ConsultarOficinaResponse
                             {
                                 id_oficina = zp.id_oficina,
                                 nombre = zp.nom_oficina,
                                 ruc = zp.ruc
                             };

            return result_dir.ToList();
        }

        /*53*/
        public IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_ATENDER_Result> Export_Excel_documentos_ht_pendientes_por_atender(int id_oficina)
        {
            return _hojatramiteRepositorio.Export_Excel_documentos_ht_pendientes_por_atender(id_oficina);
        }

        public IEnumerable<Response.SP_CONSULTAR_REGISTRO_DE_USUARIO_Result> Consultar_registro_de_usuario(string usuario, int fechaini, int fechafin)
        {
            return _hojatramiteRepositorio.Consultar_registro_de_usuario(usuario, fechaini, fechafin);
        }

        public IEnumerable<Response.SP_EXCEL_HT_ARCHIVADOS_ATENDIDOS_Result> Export_Excel_documentos_ht_archivadas_atendidas(int id_oficina)
        {
            return _hojatramiteRepositorio.Export_Excel_documentos_ht_archivadas_atendidas(id_oficina);
        }
        /*53*/
        public IEnumerable<Response.SP_EXCEL_HT_PENDIENTES_POR_RECIBIR_Result> Export_Excel_documentos_ht_pendientes_por_recibir(int id_oficina)
        {
            return _hojatramiteRepositorio.Export_Excel_documentos_ht_pendientes_por_recibir(id_oficina);
        }


        /*53*/
        public IEnumerable<Response.SP_EXCEL_HT_ENVIADAS_Result> Export_Excel_documentos_ht_enviadas(int id_oficina)
        {
            return _hojatramiteRepositorio.Export_Excel_documentos_ht_enviadas(id_oficina);
        }
        public IEnumerable<VerPedientesGesdocResponse> lista_pendientes_sigesdoc(int aniodesde, int aniohasta)
        {
            var result = from zp in _VerPedientesGesdocRepositorio.Listar().Where(x => x.ANIO >= aniodesde && x.ANIO <= aniohasta)
                         select new VerPedientesGesdocResponse
                         {
                             anio = zp.ANIO,
                             cant = zp.CANT,
                             id_oficina = zp.ID_OFICINA,
                             tupa = zp.TUPA
                         };
            return result.ToList();
        }

        public IEnumerable<ConsultaPendientesSanipesDetalleResponse> lista_pendientes_sigesdoc_det(int aniodesde, int aniohasta)
        {
            var result = from zp in _ConsultaPendientesSanipesDetalleRepositorio.Listar().Where(x => x.FECHA_ENVIO.Year >= aniodesde && x.FECHA_ENVIO.Year <= aniohasta)
                         select new ConsultaPendientesSanipesDetalleResponse
                         {
                             hoja_tramite = zp.HOJA_TRAMITE,
                             fecha_emision = zp.FECHA_EMISION,
                             tupa = zp.TUPA,
                             asunto_tupa = zp.ASUNTO_TUPA,
                             estado = zp.ESTADO,
                             fecha_envio = zp.FECHA_ENVIO,
                             fecha_recepcion = zp.FECHA_RECEPCION,
                             oficina = zp.OFICINA,
                             asunto = zp.ASUNTO,
                             destino = zp.DESTINO,
                             persona_envia = zp.PERSONA_ENVIA
                         };
            return result.ToList();
        }
        /*53*/
        public IEnumerable<ConsultarDniResponse> Consultar_DNI_total()
        {

            var result = (from zp in _ConsultarDniRepositorio.Listar()
                          select new ConsultarDniResponse
                          {
                              persona_num_documento = zp.persona_num_documento,
                              paterno = zp.paterno,
                              materno = zp.materno,
                              nombres = zp.nombres,
                              direccion = zp.direccion
                          }).OrderBy(x => x.paterno);
            return result.ToList();
        }

        public IEnumerable<ConsultarPendientesHtParaAdjuntar20190104Response> GetAllpendienteshtadjuntar(string expediente)
        {
            var result = (from zp in _IConsultarPendientesHtParaAdjuntar20190104Repositorio.Listar().Where(z => z.HOJA_TRAMITE == expediente)
                          select new ConsultarPendientesHtParaAdjuntar20190104Response
                          {
                              oficina_destino = zp.OFICINA_DESTINO,
                              oficina = zp.OFICINA,
                              hoja_tramite = zp.HOJA_TRAMITE,
                              nombre = zp.NOMBRE,
                              persona_num_documento = zp.persona_num_documento
                          }).OrderBy(x => x.oficina).ThenBy(y => y.nombre);
            return result.ToList();
        }

        public string genera_clave_documento_externo()
        {
            return _hojatramiteRepositorio.genera_clave_documento_externo();
        }
        void Retornar_Estado_En_Proceso(int? var_id_cab_documento, int var_id_oficina_crea)
        {
            IEnumerable<DocumentoDetalleResponse> doc_det_resp = new List<DocumentoDetalleResponse>();

            var result = from zp in _DocumentoDetalleRepositorio.Listar()
                         where zp.ID_CAB_DET_DOCUMENTO == var_id_cab_documento && zp.ID_EST_TRAMITE == 1 && zp.OFICINA_CREA == var_id_oficina_crea
                         select new DocumentoDetalleResponse
                         {
                             id_det_documento = zp.ID_DET_DOCUMENTO
                         };

            if (result.Count() == 0)
            {
                DAT_DOCUMENTO_DETALLE entity;
                entity = _DocumentoDetalleRepositorio.ListarUno(x => x.ID_DET_DOCUMENTO == var_id_cab_documento);
                entity.FECHA_DERIVADO = null;
                entity.USUARIO_DERIVADO = null;
                entity.ID_EST_TRAMITE = 2;
                try
                {
                    using (TransactionScope scope = new TransactionScope())
                    {
                        _DocumentoDetalleRepositorio.Actualizar(entity);
                        _unitOfWork.Guardar();
                        scope.Complete();
                    }
                }
                catch
                {
                    throw new InvalidOperationException();
                }
            }
        }

        public int Grabar_DocDetObservaciones(DocDetObservacionesRequest request)
        {
            DAT_DOC_DET_OBSERVACIONES entity = RequestToEntidad.doc_det_observaciones(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocDetObservacionesRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_DOC_OBSERVACION;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        public IEnumerable<DocDetObservacionesResponse> Listar_Observacion_x_det_documento(int id_det_documento)
        {
            return _hojatramiteRepositorio.Listar_Observacion_x_det_documento(id_det_documento);
        }


        public int Get_num_ext_Documento(int id_ht)
        {
            int rr = 0;
            var result = (from zp in _DocumentoRepositorio.Listar().Where(x => x.NUMERO == id_ht)
                          select new DocumentoResponse
                          {
                              num_ext = zp.NUM_EXT
                          }).OrderByDescending(x => x.num_ext);
            if (result.Count() > 0)
            {
                rr = result.ToList().First().num_ext ?? 0;
            }
            return rr;
        }

        public IEnumerable<DetalleMaeDocumentoResponse> Listar_Detalle_Documento_Interno(int id_documento)
        {
            var result = (from x in _DetalleMaeDocumentoRepositorio.Listar()
                          where x.ID_DOCUMENTO == id_documento
                          select new DetalleMaeDocumentoResponse
                          {
                            nombres = x.NOMBRES,
                            asunto = x.ASUNTO,
                            nom_doc = x.NUMERO_DOCUMENTO + x.NOM_DOC,
                            flag_destino_principal = x.FLAG_DESTINO_PRINCIPAL

                          }).OrderBy(z => z.flag_destino_principal);
            return result.ToList();
        }
    }
}
