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
    public class HabilitacionesService : IHabilitacionesService
    {
        /*01*/  private readonly IDocumentoSeguimientoRepositorio _DocumentoSeguimientoRepositorio;
        /*02*/  private readonly IUnitOfWork _unitOfWork;
        /*03*/  private readonly IDetDocFactRepositorio _DetDocFactRepositorio;
        /*04*/  private readonly ISeguimientoDhcpaRepositorio _SeguimientoDhcpaRepositorio;
        /*05*/  private readonly IDetSegDocRepositorio _DetSegDocRepositorio;
        /*06*/  private readonly IExpedientesRepositorio _ExpedientesRepositorio;
        /*07*/  private readonly IDetSegEvaluadorRepositorio _DetSegEvaluadorRepositorio;
        /*08*/  private readonly IConsultaEmbarcacionesRepositorio _ConsultaEmbarcacionesRepositorio;
        /*09*/  private readonly IArchivadorDhcpaRepositorio _ArchivadorDhcpaRepositorio;
        /*10*/  private readonly IFilialDhcpaRepositorio _FilialDhcpaRepositorio;
        /*11*/  private readonly IDocumentoDhcpaRepositorio _DocumentoDhcpaRepositorio;
        /*12*/  private readonly IDocumentoDhcpaDetalleRepositorio _DocumentoDhcpaDetalleRepositorio;
        /*13*/  private readonly IDetSegDocDhcpaRepositorio _DetSegDocDhcpaRepositorio;
        /*14*/  private readonly IProtocoloRepositorio _ProtocoloRepositorio;
        /*15*/  private readonly ISolicitudInspeccionRepositorio _SolicitudInspeccionRepositorio;
        /*16*/  private readonly IInformeTecnicoEvalRepositorio _InformeTecnicoEvalRepositorio;
        /*17*/  private readonly IConsultarPlantasRepositorio _ConsultarPlantasRepositorio;
        /*18*/  private readonly IHojaTramiteRepositorio _HojaTramiteRepositorio;
        /*19*/  private readonly IEstadoSeguimientoDhcpaRepositorio _EstadoSeguimientoDhcpaRepositorio;
        /*20*/  private readonly IProtocoloPlantaRepositorio _ProtocoloPlantaRepositorio;
        /*21*/  private readonly IProtocoloEmbarcacionRepositorio _ProtocoloEmbarcacionRepositorio;
        /*22*/  private readonly IIndicadorProtocoloEspecieRepositorio _IndicadorProtocoloEspecieRepositorio;
        /*23*/  private readonly IEspeciesHabilitacionesRepositorio _EspeciesHabilitacionesRepositorio;
        /*24*/  private readonly IProtocoloEspecieRepositorio _ProtocoloEspecieRepositorio;
        /*25*/  private readonly IProtocoloAlmacenRepositorio _ProtocoloAlmacenRepositorio;
        /*26*/  private readonly IConsultarDbGeneralMaeAlmacenSedeRepositorio _ConsultarDbGeneralMaeAlmacenSedeRepositorio;
        /*27*/  private readonly IProtocoloConcesionRepositorio _ProtocoloConcesionRepositorio;
        /*28*/  private readonly IConstanciaHaccpRepositorio _ConstanciaHaccpRepositorio;
        /*29*/  private readonly IConsultarDbGeneralMaeConcesionRepositorio _ConsultarDbGeneralMaeConcesionRepositorio;
        /*30*/  private readonly IVersionSolicitudRepositorio _VersionSolicitudRepositorio;
        /*31*/  private readonly IConsultarPersonaTelefonoRepositorio _ConsultarPersonaTelefonoRepositorio;
        /*32*/  private readonly IProtocoloDesembarcaderoRepositorio _ProtocoloDesembarcaderoRepositorio;
        /*33*/  private readonly IDbGeneralMaeDesembarcaderoRepositorio _DbGeneralMaeDesembarcaderoRepositorio;
        /*34*/  private readonly ITipoAutorizacionInstalacionRepositorio _TipoAutorizacionInstalacionRepositorio;
        /*35*/  private readonly IProtocoloAutorizacionInstalacionRepositorio _ProtocoloAutorizacionInstalacionRepositorio;
        /*36*/  private readonly ITipoLicenciaOperacionRepositorio _TipoLicenciaOperacionRepositorio;
        /*37*/  private readonly IProtocoloLicenciaOperacionRepositorio _ProtocoloLicenciaOperacionRepositorio;
        /*38*/  private readonly ISeguimientoDhcpaObservacionesRepositorio _SeguimientoDhcpaObservacionesRepositorio;
        /*39*/  private readonly ITipoDocumentoSeguimientoAdjuntoRepositorio _TipoDocumentoSeguimientoAdjuntoRepositorio;
        /*40*/  private readonly IDocumentoSeguimientoAdjuntoRepositorio _DocumentoSeguimientoAdjuntoRepositorio;
        /*40*/  private readonly IConsultarTipoFurgonTransporteRepositorio _ConsultarTipoFurgonTransporteRepositorio;
        
        /*40*/
        private readonly IDbGeneralMaeTransporteRepositorio _DbGeneralMaeTransporteRepositorio;
        /*40*/
        private readonly ITipoCamaraTransporteRepositorio _TipoCamaraTransporteRepositorio;
        private readonly IDbGeneralMaeUnidadMedidaRepositorio _DbGeneralMaeUnidadMedidaRepositorio;
        private readonly IDbGeneralMaeTipoCarroceriaRepositorio _DbGeneralMaeTipoCarroceriaRepositorio;
        private readonly IProtocoloTransporteRepositorio _ProtocoloTransporteRepositorio;
        private readonly ITipoServicioHabilitacionRepositorio _TipoServicioHabilitacionRepositorio;
        private readonly IActividadProtocoloRepositorio _ActividadProtocoloRepositorio;
        private readonly IFirmasSdhpaRepositorio _FirmasSdhpaRepositorio;
        private readonly IActaInspeccionDsfpaRepositorio _ActaInspeccionDsfpaRepositorio;
        private readonly IInformeInspeccionDsfpaRepositorio _InformeInspeccionDsfpaRepositorio;
        private readonly IPruebaInspeccionDsfpaRepositorio _PruebaInspeccionDsfpaRepositorio;
        private readonly ICheckListInspeccionDsfpaRepositorio _CheckListInspeccionDsfpaRepositorio;
        private readonly IPtocoloTransporteXIdTransporte2018V1Repositorio _PtocoloTransporteXIdTransporte2018V1Repositorio;
        private readonly ITipoAtencionInspeccionRepositorio _TipoAtencionInspeccionRepositorio;
        private readonly IConsultaProtocolosAiRepositorio _ConsultaProtocolosAiRepositorio;
        private readonly IConsultaProtocolosLoRepositorio _ConsultaProtocolosLoRepositorio;
        private readonly IConsultarOficinaRepositorio _ConsultarOficinaRepositorio;
        private readonly IConsultaExpedienteXExpedienteRepositorio _ConsultaExpedienteXExpedienteRepositorio;





        public HabilitacionesService(
            /*01*/  IDocumentoSeguimientoRepositorio DocumentoSeguimientoRepositorio,
            /*02*/  IUnitOfWork unitOfWork,
            /*03*/  IDetDocFactRepositorio DetDocFactRepositorio,
            /*04*/  ISeguimientoDhcpaRepositorio SeguimientoDhcpaRepositorio,
            /*05*/  IDetSegDocRepositorio DetSegDocRepositorio,
            /*06*/  IExpedientesRepositorio ExpedientesRepositorio,
            /*07*/  IDetSegEvaluadorRepositorio DetSegEvaluadorRepositorio,
            /*08*/  IConsultaEmbarcacionesRepositorio ConsultaEmbarcacionesRepositorio,
            /*09*/  IArchivadorDhcpaRepositorio ArchivadorDhcpaRepositorio,
            /*10*/  IFilialDhcpaRepositorio FilialDhcpaRepositorio,
            /*11*/  IDocumentoDhcpaRepositorio DocumentoDhcpaRepositorio,
            /*12*/  IDocumentoDhcpaDetalleRepositorio DocumentoDhcpaDetalleRepositorio,
            /*13*/  IDetSegDocDhcpaRepositorio DetSegDocDhcpaRepositorio,
            /*14*/  IProtocoloRepositorio ProtocoloRepositorio,
            /*15*/  ISolicitudInspeccionRepositorio SolicitudInspeccionRepositorio,
            /*16*/  IInformeTecnicoEvalRepositorio InformeTecnicoEvalRepositorio,
            /*17*/  IConsultarPlantasRepositorio ConsultarPlantasRepositorio,
            /*18*/  IHojaTramiteRepositorio HojaTramiteRepositorio,
            /*19*/  IEstadoSeguimientoDhcpaRepositorio EstadoSeguimientoDhcpaRepositorio,
            /*20*/  IProtocoloPlantaRepositorio ProtocoloPlantaRepositorio,
            /*21*/  IProtocoloEmbarcacionRepositorio ProtocoloEmbarcacionRepositorio,
            /*22*/  IIndicadorProtocoloEspecieRepositorio IndicadorProtocoloEspecieRepositorio,
            /*23*/  IEspeciesHabilitacionesRepositorio EspeciesHabilitacionesRepositorio,
            /*24*/  IProtocoloEspecieRepositorio ProtocoloEspecieRepositorio,
            /*25*/  IProtocoloAlmacenRepositorio ProtocoloAlmacenRepositorio,
            /*26*/  IConsultarDbGeneralMaeAlmacenSedeRepositorio ConsultarDbGeneralMaeAlmacenSedeRepositorio,
            /*27*/  IProtocoloConcesionRepositorio ProtocoloConcesionRepositorio,
            /*28*/  IConstanciaHaccpRepositorio ConstanciaHaccpRepositorio,
            /*29*/  IConsultarDbGeneralMaeConcesionRepositorio ConsultarDbGeneralMaeConcesionRepositorio,
            /*30*/  IVersionSolicitudRepositorio VersionSolicitudRepositorio,
            /*32*/  IConsultarPersonaTelefonoRepositorio ConsultarPersonaTelefonoRepositorio,
            /*32*/  IProtocoloDesembarcaderoRepositorio ProtocoloDesembarcaderoRepositorio,
            /*33*/  IDbGeneralMaeDesembarcaderoRepositorio DbGeneralMaeDesembarcaderoRepositorio,
            /*34*/  ITipoAutorizacionInstalacionRepositorio TipoAutorizacionInstalacionRepositorio,
            /*35*/  IProtocoloAutorizacionInstalacionRepositorio ProtocoloAutorizacionInstalacionRepositorio,
            /*36*/  ITipoLicenciaOperacionRepositorio TipoLicenciaOperacionRepositorio,
            /*37*/  IProtocoloLicenciaOperacionRepositorio ProtocoloLicenciaOperacionRepositorio,
            /*37*/  ISeguimientoDhcpaObservacionesRepositorio SeguimientoDhcpaObservacionesRepositorio,
            /*39*/  ITipoDocumentoSeguimientoAdjuntoRepositorio TipoDocumentoSeguimientoAdjuntoRepositorio,
            /*40*/  IDocumentoSeguimientoAdjuntoRepositorio DocumentoSeguimientoAdjuntoRepositorio,
            /*40*/  IDbGeneralMaeTransporteRepositorio DbGeneralMaeTransporteRepositorio,
            ITipoCamaraTransporteRepositorio TipoCamaraTransporteRepositorio,
            IDbGeneralMaeUnidadMedidaRepositorio DbGeneralMaeUnidadMedidaRepositorio,
            IDbGeneralMaeTipoCarroceriaRepositorio DbGeneralMaeTipoCarroceriaRepositorio,
            /*40*/  IConsultarTipoFurgonTransporteRepositorio ConsultarTipoFurgonTransporteRepositorio,
            IProtocoloTransporteRepositorio ProtocoloTransporteRepositorio,
            ITipoServicioHabilitacionRepositorio TipoServicioHabilitacionRepositorio,
            IActividadProtocoloRepositorio ActividadProtocoloRepositorio,
            IFirmasSdhpaRepositorio FirmasSdhpaRepositorio,
            IActaInspeccionDsfpaRepositorio ActaInspeccionDsfpaRepositorio,
            IInformeInspeccionDsfpaRepositorio InformeInspeccionDsfpaRepositorio,
            IPruebaInspeccionDsfpaRepositorio PruebaInspeccionDsfpaRepositorio,
            ICheckListInspeccionDsfpaRepositorio CheckListInspeccionDsfpaRepositorio,
            IPtocoloTransporteXIdTransporte2018V1Repositorio PtocoloTransporteXIdTransporte2018V1Repositorio,
            ITipoAtencionInspeccionRepositorio TipoAtencionInspeccionRepositorio,
            IConsultaProtocolosLoRepositorio ConsultaProtocolosLoRepositorio,
            IConsultaProtocolosAiRepositorio ConsultaProtocolosAiRepositorio,
            IConsultarOficinaRepositorio ConsultarOficinaRepositorio,
            IConsultaExpedienteXExpedienteRepositorio ConsultaExpedienteXExpedienteRepositorio
            )
        {
            /*01*/  _DocumentoSeguimientoRepositorio = DocumentoSeguimientoRepositorio;
            /*02*/  _unitOfWork = unitOfWork;
            /*03*/  _DetDocFactRepositorio = DetDocFactRepositorio;
            /*04*/  _SeguimientoDhcpaRepositorio = SeguimientoDhcpaRepositorio;
            /*05*/  _DetSegDocRepositorio = DetSegDocRepositorio;
            /*06*/  _ExpedientesRepositorio = ExpedientesRepositorio;
            /*07*/  _DetSegEvaluadorRepositorio = DetSegEvaluadorRepositorio;
            /*08*/  _ConsultaEmbarcacionesRepositorio = ConsultaEmbarcacionesRepositorio;
            /*09*/  _ArchivadorDhcpaRepositorio = ArchivadorDhcpaRepositorio;
            /*10*/  _FilialDhcpaRepositorio = FilialDhcpaRepositorio;
            /*11*/  _DocumentoDhcpaRepositorio = DocumentoDhcpaRepositorio;
            /*12*/  _DocumentoDhcpaDetalleRepositorio = DocumentoDhcpaDetalleRepositorio;
            /*13*/  _DetSegDocDhcpaRepositorio = DetSegDocDhcpaRepositorio;
            /*14*/  _ProtocoloRepositorio = ProtocoloRepositorio;
            /*15*/  _SolicitudInspeccionRepositorio =SolicitudInspeccionRepositorio;
            /*16*/  _InformeTecnicoEvalRepositorio = InformeTecnicoEvalRepositorio;
            /*17*/  _ConsultarPlantasRepositorio = ConsultarPlantasRepositorio;
            /*18*/  _HojaTramiteRepositorio = HojaTramiteRepositorio;
            /*19*/  _EstadoSeguimientoDhcpaRepositorio = EstadoSeguimientoDhcpaRepositorio;
            /*20*/  _ProtocoloPlantaRepositorio = ProtocoloPlantaRepositorio;
            /*21*/  _ProtocoloEmbarcacionRepositorio = ProtocoloEmbarcacionRepositorio;
            /*22*/  _IndicadorProtocoloEspecieRepositorio = IndicadorProtocoloEspecieRepositorio;
            /*23*/  _EspeciesHabilitacionesRepositorio = EspeciesHabilitacionesRepositorio;
            /*24*/  _ProtocoloEspecieRepositorio = ProtocoloEspecieRepositorio;
            /*25*/  _ProtocoloAlmacenRepositorio = ProtocoloAlmacenRepositorio;
            /*26*/  _ConsultarDbGeneralMaeAlmacenSedeRepositorio = ConsultarDbGeneralMaeAlmacenSedeRepositorio;
            /*27*/  _ProtocoloConcesionRepositorio = ProtocoloConcesionRepositorio;
            /*28*/  _ConstanciaHaccpRepositorio = ConstanciaHaccpRepositorio;
            /*29*/  _ConsultarDbGeneralMaeConcesionRepositorio = ConsultarDbGeneralMaeConcesionRepositorio;
            /*30*/  _VersionSolicitudRepositorio = VersionSolicitudRepositorio;
            /*31*/  _ConsultarPersonaTelefonoRepositorio = ConsultarPersonaTelefonoRepositorio;
            /*32*/  _ProtocoloDesembarcaderoRepositorio = ProtocoloDesembarcaderoRepositorio;
            /*33*/  _DbGeneralMaeDesembarcaderoRepositorio = DbGeneralMaeDesembarcaderoRepositorio;
            /*34*/  _TipoAutorizacionInstalacionRepositorio = TipoAutorizacionInstalacionRepositorio;
            /*35*/  _ProtocoloAutorizacionInstalacionRepositorio = ProtocoloAutorizacionInstalacionRepositorio;
            /*36*/  _TipoLicenciaOperacionRepositorio = TipoLicenciaOperacionRepositorio;
            /*37*/  _ProtocoloLicenciaOperacionRepositorio = ProtocoloLicenciaOperacionRepositorio;
            /*37*/  _SeguimientoDhcpaObservacionesRepositorio = SeguimientoDhcpaObservacionesRepositorio;
            /*39*/  _TipoDocumentoSeguimientoAdjuntoRepositorio = TipoDocumentoSeguimientoAdjuntoRepositorio;
            /*40*/  _DocumentoSeguimientoAdjuntoRepositorio = DocumentoSeguimientoAdjuntoRepositorio;
            /*40*/  _DbGeneralMaeTransporteRepositorio = DbGeneralMaeTransporteRepositorio;
            _TipoCamaraTransporteRepositorio = TipoCamaraTransporteRepositorio;
            _DbGeneralMaeUnidadMedidaRepositorio = DbGeneralMaeUnidadMedidaRepositorio;
        _DbGeneralMaeTipoCarroceriaRepositorio = DbGeneralMaeTipoCarroceriaRepositorio;
        _ConsultarTipoFurgonTransporteRepositorio = ConsultarTipoFurgonTransporteRepositorio;
            _ProtocoloTransporteRepositorio =ProtocoloTransporteRepositorio;
            _TipoServicioHabilitacionRepositorio = TipoServicioHabilitacionRepositorio;
            _ActividadProtocoloRepositorio = ActividadProtocoloRepositorio;
            _FirmasSdhpaRepositorio = FirmasSdhpaRepositorio;
            _ActaInspeccionDsfpaRepositorio =ActaInspeccionDsfpaRepositorio;
            _InformeInspeccionDsfpaRepositorio =InformeInspeccionDsfpaRepositorio;
            _PruebaInspeccionDsfpaRepositorio = PruebaInspeccionDsfpaRepositorio;
            _CheckListInspeccionDsfpaRepositorio = CheckListInspeccionDsfpaRepositorio;
            _PtocoloTransporteXIdTransporte2018V1Repositorio = PtocoloTransporteXIdTransporte2018V1Repositorio;
            _TipoAtencionInspeccionRepositorio = TipoAtencionInspeccionRepositorio;
            _ConsultaProtocolosLoRepositorio = ConsultaProtocolosLoRepositorio;
            _ConsultaProtocolosAiRepositorio = ConsultaProtocolosAiRepositorio;
            _ConsultarOficinaRepositorio = ConsultarOficinaRepositorio;
            _ConsultaExpedienteXExpedienteRepositorio = ConsultaExpedienteXExpedienteRepositorio;
        }

        public FirmasSdhpaResponse lista_firmas_sdhpa_activas(string persona_num_documento)
        {
            FirmasSdhpaResponse resp = new FirmasSdhpaResponse();

            var result = (from zp in _FirmasSdhpaRepositorio.Listar()
                         where zp.PERSONA_NUM_DOCUMENTO==persona_num_documento && zp.ACTIVO=="1"
                         select new FirmasSdhpaResponse
                         {
                             id_firma_sdhpa = zp.ID_FIRMA_SDHPA,
                             nombre_reporte = zp.NOMBRE_REPORTE
                         });

            if (result.Count() > 0)
            {
                resp = result.First();
            }

            return resp;
        }
        
        

        public ConsultaExpedienteXExpedienteResponse Consulta_expediente_x_expediente(string expediente)
        {
            ConsultaExpedienteXExpedienteResponse resp = new ConsultaExpedienteXExpedienteResponse();

            var result = (from zp in _ConsultaExpedienteXExpedienteRepositorio.Listar()
                          where zp.EXPEDIENTE == expediente
                          select new ConsultaExpedienteXExpedienteResponse
                          {
                              expediente = zp.EXPEDIENTE,
                              externo = zp.EXTERNO,
                              direccion = zp.DIRECCION,
                              evaluador =zp.EVALUADOR,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              id_expediente = zp.ID_EXPEDIENTE,
                              num_documento = zp.NUM_DOCUMENTO
                          });

            if (result.Count() > 0)
            {
                resp = result.First();
            }

            return resp;
        }

        public PtocoloTransporteXIdTransporte2018V1Response lista_PtocoloTransporteXIdTransporte2018V1Response_x_id(int id)
        {
            PtocoloTransporteXIdTransporte2018V1Response resp = new PtocoloTransporteXIdTransporte2018V1Response();

            var result = (from zp in _PtocoloTransporteXIdTransporte2018V1Repositorio.Listar()
                          where zp.ID_PROTOCOLO == id
                          select new PtocoloTransporteXIdTransporte2018V1Response
                          {
                              razon_social = zp.RAZON_SOCIAL,
                              direccion_legal = zp.DIRECCION_LEGAL,
                              representante_legal = zp.REPRESENTANTE_LEGAL,
                              infra_pesq = zp.INFRA_PESQ,
                              placa = zp.PLACA,
                              cod_habilitacion = zp.COD_HABILITACION,
                              nombre_carroceria = zp.NOMBRE_CARROCERIA,
                              siglas_um = zp.SIGLAS_UM,
                              carga_util = zp.CARGA_UTIL,
                              acta_inspeccion = zp.ACTA_INSPECCION,
                              informe_auditoria = zp.INFORME_AUDITORIA,
                              informe_tecnico_evaluacion = zp.INFORME_TECNICO_EVALUACION,
                              fecha_inicio = zp.FECHA_INICIO,
                              fecha_fin = zp.FECHA_FIN,
                              fecha_emision = zp.FECHA_EMISION,
                              nombre_protocolo = zp.NOMBRE_PROTOCOLO,
                              expediente = zp.EXPEDIENTE,
                              nombre_tipo_furgon = zp.NOMBRE_TIPO_FURGON,
                              informe_sdhpa =zp.INFORME_SDHPA
                          });

            if (result.Count() > 0)
            {
                resp = result.First();
            }

            return resp;
        }

        public IEnumerable<DbGeneralMaeTransporteResponse> Lista_db_general_mae_transporte(string placa = "", string cod_habilitacion = "", int id_tipo_carroceria = 0, int id_tipo_furgon = 0)
        {
            var result = (from p in _DbGeneralMaeTransporteRepositorio.Listar(x => 
                            (placa.Trim() == "" || (placa != "" && x.PLACA == placa)) &&
                            (cod_habilitacion.Trim() == "" || (cod_habilitacion.Trim() != "" && x.COD_HABILITACION == cod_habilitacion)) &&
                            (id_tipo_carroceria == 0 || (id_tipo_carroceria != 0 && x.ID_TIPO_CARROCERIA == id_tipo_carroceria)) &&
                            (id_tipo_furgon == 0 || (id_tipo_furgon != 0 && x.ID_TIPO_FURGON == id_tipo_furgon))
                            )
                         select new DbGeneralMaeTransporteResponse
                         {
                             id_transporte = p.ID_TRANSPORTE,
                             placa = p.PLACA,
                             cod_habilitacion = p.COD_HABILITACION,
                             nombre_carroceria = p.NOMBRE_CARROCERIA,
                             nombre_um = p.NOMBRE_UM,
                             siglas_um = p.SIGLAS_UM,
                             carga_util = p.CARGA_UTIL,
                             nombre_estado = p.NOMBRE_ESTADO,
                             nombre_furgon = p.NOMBRE_FURGON
                         }).OrderByDescending(x => x.id_transporte);

            return result;
        }

        /*01*/
        public int Create_documento_sdhcp(DocumentoSeguimientoRequest request)
        {
            MAE_DOCUMENTO_SEGUIMIENTO entity = RequestToEntidad.documento_seguimiento(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoSeguimientoRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DOCUMENTO_SEG;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*02*/
        public int Create_det_doc_fac(DetDocFactRequest request)
        {
            DAT_DET_DOC_FACT entity = RequestToEntidad.det_doc_fac(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DetDocFactRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_DOC_FACT;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public int Create_ActaInspeccionDsfpa(ActaInspeccionDsfpaRequest request)
        {
            MAE_ACTA_INSPECCION_DSFPA entity = RequestToEntidad.acta_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ActaInspeccionDsfpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_ACTA_INSP;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public int Create_InformeInspeccionDsfpa(InformeInspeccionDsfpaRequest request)
        {
            MAE_INFORME_INSPECCION_DSFPA entity = RequestToEntidad.informe_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _InformeInspeccionDsfpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_INFORME_INSP;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public int Create_ChecklistInspeccionDsfpa(CheckListInspeccionDsfpaRequest request)
        {
            MAE_CHECK_LIST_INSPECCION_DSFPA entity = RequestToEntidad.check_list_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _CheckListInspeccionDsfpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_CHK_LIST_INSP;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        public int Create_pruebaInspeccionDsfpa(PruebaInspeccionDsfpaRequest request)
        {
            MAE_PRUEBA_INSPECCION_DSFPA entity = RequestToEntidad.pruebas_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _PruebaInspeccionDsfpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_PRUEBA_INSP;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*03*/
        public int Create_Seguimiento(SeguimientoDhcpaRequest request)
        {
            MAE_SEGUIMIENTO_DHCPA entity = RequestToEntidad.Seguimiento_dhcpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _SeguimientoDhcpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_SEGUIMIENTO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*04*/
        public int Create_det_doc_seg(DetSegDocRequest request)
        {
            DAT_DET_SEG_DOC entity = RequestToEntidad.det_doc_seg(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DetSegDocRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_DOC;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*04*/
        public int Create_documento_seguimiento_adjunto(DocumentoSeguimientoAdjuntoRequest request)
        {
            MAE_DOCUMENTO_SEGUIMIENTO_ADJUNTO entity = RequestToEntidad.DocumentoSeguimientoAdjunto(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoSeguimientoAdjuntoRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DOC_SEG_ADJUNTO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*05*/
        public bool actualizar_protocolo(ProtocoloRequest request)
        {
            MAE_PROTOCOLO entity = RequestToEntidad.protocolo(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloRepositorio.Actualizar(entity);
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

        /*05*/
        public bool Update_mae_expediente(ExpedientesRequest request)
        {
            MAE_EXPEDIENTES entity = RequestToEntidad.expedientes(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ExpedientesRepositorio.Actualizar(entity);
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
   
        /*06*/
        public ExpedientesRequest GetExpediente(int id_expediente)
        {
            var result = from zp in _ExpedientesRepositorio.Listar(x => x.ID_EXPEDIENTE== id_expediente)
                         select new ExpedientesRequest
                         {
                           id_expediente = zp.ID_EXPEDIENTE,
                           id_tipo_expediente = zp.ID_TIPO_EXPEDIENTE,
                           numero_expediente=zp.NUMERO_EXPEDIENTE,
                           fecha_registro=zp.FECHA_REGISTRO,
                           usuario_registro=zp.USUARIO_REGISTRO,
                           fecha_modifico=zp.FECHA_MODIFICO,
                           usuario_modifico=zp.USUARIO_MODIFICO,
                           indicador_seguimiento = zp.INDICADOR_SEGUIMIENTO,
                           nom_expediente = zp.NOM_EXPEDIENTE,
                           año_crea = zp.AÑO_CREA
                         };
            return result.ToList().First();

        }


        /*06*/
        public ExpedientesResponse GetExpediente_x_id(int id_expediente)
        {
            return _ExpedientesRepositorio.GetExpediente_x_id(id_expediente);
        }

        /*07*/
        public IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente)
        {
            return _DocumentoSeguimientoRepositorio.GetAllDocumentos(estado, indicador, evaluador, asunto, externo, id_tipo_documento, num_doc, nom_doc, oficina_crea, expediente);
        }
          
        /*09*/
        public IEnumerable<ExpedientesResponse> GetAllExpediente_x_Documento(int id_documento_seg)
        {
            return _DocumentoSeguimientoRepositorio.GetAllExpediente_x_Documento(id_documento_seg);
        }
        /*10*/
        public IEnumerable<ConsultaFacturasResponse> GetAllfacturas_x_Documento(int id_documento_seg)
        {
            return _DocumentoSeguimientoRepositorio.GetAllfacturas_x_Documento(id_documento_seg);
        }
        /*11*/
        public IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcacion_x_documento(int id_documento_seg)
        {
            return _DocumentoSeguimientoRepositorio.GetAllEmbarcacion_x_documento(id_documento_seg);
        }
        /*12*/
        public int Create_det_seg_evaluador(DetSegEvaluadorRequest request)
        {
            DAT_DET_SEG_EVALUADOR entity = RequestToEntidad.det_seg_evaluador(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DetSegEvaluadorRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_EXP_EVA;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*13*/
        public DocumentoSeguimientoRequest GetAllDocumento_req(int id_documento_seg)
        {
            var result = from zp in _DocumentoSeguimientoRepositorio.Listar(x => x.ID_DOCUMENTO_SEG == id_documento_seg)
                         select new DocumentoSeguimientoRequest
                         {
                             id_documento_seg= zp.ID_DOCUMENTO_SEG,
                             id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                             num_documento = zp.NUM_DOCUMENTO,
                             nom_documento = zp.NOM_DOCUMENTO,
                             asunto = zp.ASUNTO,
                             fecha_crea = zp.FECHA_CREA,
                             evaluador = zp.EVALUADOR,
                             fecha_asignacion_evaluador = zp.FECHA_ASIGNACION_EVALUADOR,
                             fecha_recibido_evaluador = zp.FECHA_RECIBIDO_EVALUADOR,
                             estado = zp.ESTADO,
                             indicador = zp.INDICADOR,
                             fecha_documento = zp.FECHA_DOCUMENTO,
                             fecha_recepcion_sdhpa = zp.FECHA_RECEPCION_SDHPA,
                             usuario_recepcion_sdhpa = zp.USUARIO_RECEPCION_SDHPA,
                             oficina_crea = zp.OFICINA_CREA,
                             usuario_crea = zp.USUARIO_CREA,
                             id_servicio_dhcpa = zp.ID_SERVICIO_DHCPA,
                             fecha_od = zp.FECHA_OD,
                             usuario_od = zp.USUARIO_OD,
                             expedientes_relacion = zp.EXPEDIENTES_RELACION,
                             ruta_pdf = zp.RUTA_PDF,
                             nom_ofi_crea = zp.NOM_OFI_CREA
                         };
            return result.ToList().First();
        }
        public ActaInspeccionDsfpaRequest GetAllacta_inspeccion_req(int id_acta_insp)
        {
            var result = from zp in _ActaInspeccionDsfpaRepositorio.Listar(x => x.ID_ACTA_INSP == id_acta_insp)
                         select new ActaInspeccionDsfpaRequest
                         {
                             id_acta_insp = zp.ID_ACTA_INSP,
                             id_sol_ins = zp.ID_SOL_INS,
                             nombre_acta = zp.NOMBRE_ACTA,
                             usuario_carga = zp.USUARIO_CARGA,
                             usuario_oficina = zp.USUARIO_OFICINA,
                             inspector = zp.INSPECTOR,
                             fecha_carga = zp.FECHA_CARGA,
                             activo = zp.ACTIVO,
                             ruta_pdf = zp.RUTA_PDF
                         };

            return result.ToList().First();
        }

        public InformeInspeccionDsfpaRequest GetAllinforme_inspeccion_req(int id_informe_insp)
        {
            var result = from zp in _InformeInspeccionDsfpaRepositorio.Listar(x => x.ID_INFORME_INSP == id_informe_insp)
                         select new InformeInspeccionDsfpaRequest
                         {
                             id_informe_insp = zp.ID_INFORME_INSP,
                             id_sol_ins = zp.ID_SOL_INS,
                             nombre_informe = zp.NOMBRE_INFORME,
                             usuario_carga = zp.USUARIO_CARGA,
                             usuario_oficina = zp.USUARIO_OFICINA,
                             inspector = zp.INSPECTOR,
                             fecha_carga = zp.FECHA_CARGA,
                             activo = zp.ACTIVO,
                             ruta_pdf = zp.RUTA_PDF
                         };

            return result.ToList().First();
        }

        public CheckListInspeccionDsfpaRequest GetAllchk_list_inspeccion_req(int id_chck_list)
        {
            var result = from zp in _CheckListInspeccionDsfpaRepositorio.Listar(x => x.ID_CHK_LIST_INSP == id_chck_list)
                         select new CheckListInspeccionDsfpaRequest
                         {
                             id_chk_list_insp = zp.ID_CHK_LIST_INSP,
                             id_sol_ins = zp.ID_SOL_INS,
                             nombre_check_list = zp.NOMBRE_CHECK_LIST,
                             usuario_carga = zp.USUARIO_CARGA,
                             usuario_oficina = zp.USUARIO_OFICINA,
                             inspector = zp.INSPECTOR,
                             fecha_carga = zp.FECHA_CARGA,
                             activo = zp.ACTIVO,
                             ruta_pdf = zp.RUTA_PDF
                         };

            return result.ToList().First();
        }
        public PruebaInspeccionDsfpaRequest GetAllpruebas_inspeccion_req(int id_prueba_insp)
        {
            var result = from zp in _PruebaInspeccionDsfpaRepositorio.Listar(x => x.ID_PRUEBA_INSP == id_prueba_insp)
                         select new PruebaInspeccionDsfpaRequest
                         {
                             id_prueba_insp = zp.ID_PRUEBA_INSP,
                             id_sol_ins = zp.ID_SOL_INS,
                             usuario_carga = zp.USUARIO_CARGA,
                             usuario_oficina = zp.USUARIO_OFICINA,
                             inspector = zp.INSPECTOR,
                             fecha_carga = zp.FECHA_CARGA,
                             activo = zp.ACTIVO,
                             ruta_pdf = zp.RUTA_PDF
                         };

            return result.ToList().First();
        }
        public string enviar_correo_notificacion_solicitud_sdhpa(int id_solicitud, string destinos)
        {
            return _SeguimientoDhcpaRepositorio.enviar_correo_notificacion_solicitud_sdhpa(id_solicitud, destinos);
        }

        public IEnumerable<PruebaInspeccionDsfpaRequest> GetAllpruebas_inspeccion_req_x_id_sol_insp_sdhpa(int id_sol_ins)
        {
            var result = from zp in _PruebaInspeccionDsfpaRepositorio.Listar(x => x.ID_SOL_INS == id_sol_ins)
                         select new PruebaInspeccionDsfpaRequest
                         {
                             id_prueba_insp = zp.ID_PRUEBA_INSP,
                             id_sol_ins = zp.ID_SOL_INS,
                             usuario_carga = zp.USUARIO_CARGA,
                             usuario_oficina = zp.USUARIO_OFICINA,
                             inspector = zp.INSPECTOR,
                             fecha_carga = zp.FECHA_CARGA,
                             activo = zp.ACTIVO,
                             ruta_pdf = zp.RUTA_PDF
                         };

            return result.ToList();
        }
        /*14*/
        public bool Update_mae_documento_seg(DocumentoSeguimientoRequest request)
        {
            MAE_DOCUMENTO_SEGUIMIENTO entity = RequestToEntidad.documento_seguimiento(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoSeguimientoRepositorio.Actualizar(entity);
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

        public bool Update_acta_insp_dsfpa(ActaInspeccionDsfpaRequest request)
        {
            MAE_ACTA_INSPECCION_DSFPA entity = RequestToEntidad.acta_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ActaInspeccionDsfpaRepositorio.Actualizar(entity);
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

        public bool Update_informe_insp_dsfpa(InformeInspeccionDsfpaRequest request)
        {
            MAE_INFORME_INSPECCION_DSFPA entity = RequestToEntidad.informe_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _InformeInspeccionDsfpaRepositorio.Actualizar(entity);
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

        public bool Update_chk_list_insp_dsfpa(CheckListInspeccionDsfpaRequest request)
        {
            MAE_CHECK_LIST_INSPECCION_DSFPA entity = RequestToEntidad.check_list_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _CheckListInspeccionDsfpaRepositorio.Actualizar(entity);
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

        public bool Update_prueba_insp_dsfpa(PruebaInspeccionDsfpaRequest request)
        {
            MAE_PRUEBA_INSPECCION_DSFPA entity = RequestToEntidad.pruebas_inspeccion_dsfpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _PruebaInspeccionDsfpaRepositorio.Actualizar(entity);
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
        /*15*/
        public IEnumerable<DetSegDocResponse> GetAllDet_seg_doc(int id_documento_seg)
        {
            var result = from zp in _DetSegDocRepositorio.Listar(x => x.ID_DOCUMENTO_SEG == id_documento_seg)
                         select new DetSegDocResponse
                         {
                             id_det_doc = zp.ID_DET_DOC,
                             id_documento_seg = zp.ID_DOCUMENTO_SEG,
                             id_seguimiento = zp.ID_SEGUIMIENTO
                         };
            return result.ToList();
        }
        /*16*/
        public IEnumerable<DetSegEvaluadorRequest> GetAlldet_seg_evaluador(int id_seguimiento)
        {
            var result = from zp in _DetSegEvaluadorRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento && x.INDICADOR == "1")
                         select new DetSegEvaluadorRequest
                         {
                             id_det_exp_eva = zp.ID_DET_EXP_EVA,
                             id_seguimiento = zp.ID_SEGUIMIENTO,
                             evaluador = zp.EVALUADOR,
                             indicador = zp.INDICADOR,
                             fecha_recibido = zp.FECHA_RECIBIDO,
                             fecha_derivado = zp.FECHA_DERIVADO
                         };
            return result.ToList();
        }
        /*17*/
        public bool Update_det_seg_evalua(DetSegEvaluadorRequest request)
        {
            DAT_DET_SEG_EVALUADOR entity = RequestToEntidad.det_seg_evaluador(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DetSegEvaluadorRepositorio.Actualizar(entity);
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
        /*18*/
        public IEnumerable<ArchivadorDhcpaResponse> GetAll_Archivador()
        {
            var result = from zp in _ArchivadorDhcpaRepositorio.Listar()
                         select new ArchivadorDhcpaResponse
                         {
                             id_archivador = zp.ID_ARCHIVADOR,
                             nombre = zp.NOMBRE
                         };
            return result.ToList();
        }
        /*19*/
        public IEnumerable<FilialDhcpaResponse> GetAll_Filial()
        {
            var result = from zp in _FilialDhcpaRepositorio.Listar()
                         select new FilialDhcpaResponse
                         {
                             id_filial= zp.ID_FILIAL,
                             id_od_insp = zp.ID_OD_INSP,
                             nombre = zp.NOMBRE,
                             sol_insp = zp.SOL_INSP,
                             tp_e_i = zp.TP_E_I
                         };
            return result.ToList();
        }
        /*20*/
        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento(string persona_num_documento)
        {
            return _SeguimientoDhcpaRepositorio.Consulta_Seguimiento(persona_num_documento);
        }

        /*21*/

        public DocumentoSeguimientoAdjuntoResponse documento_seguimiento_x_tipo_documento_adjunto(int id_documento_seg, int id_tipo_documento_adjunto)
        {
            DocumentoSeguimientoAdjuntoResponse doc_adjun = new DocumentoSeguimientoAdjuntoResponse();

            var result2 = (from zp in _DocumentoSeguimientoAdjuntoRepositorio.Listar(x => x.ID_TIPO_DOC_SEG_ADJUNTO == id_tipo_documento_adjunto && x.ID_DOCUMENTO_SEG == id_documento_seg)
                           where zp.ACTIVO == "1"
                           select new DocumentoSeguimientoAdjuntoResponse
                           {
                               id_doc_seg_adjunto = zp.ID_DOC_SEG_ADJUNTO,
                               id_documento_seg = zp.ID_DOCUMENTO_SEG,
                               usuario_crea = zp.USUARIO_CREA,
                               fecha_crea = zp.FECHA_CREA,
                               activo= zp.ACTIVO
                           }).AsEnumerable();

            if (result2.Count() > 0)
            {
                return result2.First();
            }
            else
            {
                return doc_adjun;
            }

        }
                
        public IEnumerable<DocumentoSeguimientoAdjuntoResponse> lita_documento_seguimiento_x_documento_seg(int id_documento_seg)
        {

            return _SeguimientoDhcpaRepositorio.lita_documento_seguimiento_x_documento_seg(id_documento_seg);

        }
        public IEnumerable<TipoDocumentoSeguimientoAdjuntoResponse> Lista_tipo_documento_seguimiento_adjunto_x_tipo_seguimiento(int id_tipo_seguimiento)
        {

            var result2 = (from zp in _TipoDocumentoSeguimientoAdjuntoRepositorio.Listar(x => x.ID_TIPO_SEGUIMIENTO == id_tipo_seguimiento)
                           where zp.ACTIVO=="1"
                           select new TipoDocumentoSeguimientoAdjuntoResponse
                           {
                               id_tipo_doc_seg_adjunto = zp.ID_TIPO_DOC_SEG_ADJUNTO,
                               nombre = zp.NOMBRE
                           }).AsEnumerable();
            return result2;

        }
        public SeguimientoDhcpaResponse GetAllSeguimiento_x_id(int id_seguimiento)
        {
            return _SeguimientoDhcpaRepositorio.Consulta_Seguimiento_x_id_seguimiento(id_seguimiento);
        }
        /*21*/
        public int CountDocumentos_x_tipo(int id_tipo_documento)
        {

            var result2 = (from zp in _DocumentoDhcpaRepositorio.Listar(x => x.ID_TIPO_DOCUMENTO == id_tipo_documento && x.FECHA_REGISTRO.Value.Year == DateTime.Now.Year)
                           select new DocumentoDhcpaResponse
                           {
                               id_doc_dhcpa = zp.ID_DOC_DHCPA,
                               id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                               num_doc = zp.NUM_DOC
                           }).ToList().OrderByDescending(x => x.num_doc);

            int result_numero = result2.Count();

            if (result2.Count() > 0)
            {
                result_numero = result2.First().num_doc;
            }

            return result_numero;

        }
        /*22*/
        public int Create_documento_dhcpa(DocumentoDhcpaRequest request)
        {
            MAE_DOCUMENTO_DHCPA entity = RequestToEntidad.documento_dhcpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDhcpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DOC_DHCPA;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*22*/
        public bool Update_documento_dhcpa(DocumentoDhcpaRequest request)
        {
            MAE_DOCUMENTO_DHCPA entity = RequestToEntidad.documento_dhcpa(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDhcpaRepositorio.Actualizar(entity);
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
        /*23*/
        public int Create_documento_dhcpa_detalle(DocumentoDhcpaDetalleRequest request)
        {
            DAT_DOCUMENTO_DHCPA_DETALLE entity = RequestToEntidad.documento_dhcpa_detalle(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DocumentoDhcpaDetalleRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DOC_DHCPA_DET;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*24*/
        public int Create_documento_dhcpa_seguimiento(DetSegDocDhcpaRequest request)
        {
            DAT_DET_SEG_DOC_DHCPA entity = RequestToEntidad.documento_dhcpa_seguimiento(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _DetSegDocDhcpaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_DSDHCPA;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*25*//*
        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta(int pageIndex, int pageSize, string expediente, string evaluador,string externo, string matricula, string cmbestado)
        {
            return _SeguimientoDhcpaRepositorio.GetAllSeguimiento_Consulta(pageIndex, pageSize, expediente,evaluador,externo,matricula,cmbestado);
        }*/
        
        /*25*/
        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_sin_paginado(string expediente, string evaluador, string externo, string habilitante, string cmbestado, int id_oficina_filtro, int id_tupa)
        {
            return _SeguimientoDhcpaRepositorio.GetAllSeguimiento_Consulta_sin_paginado(expediente, evaluador, externo, habilitante, cmbestado, id_oficina_filtro, id_tupa);
        }
        
        /*26*//*
        public int CountSeguimiento_Consulta(string expediente, string evaluador, string externo,string matricula, string cmbestado)
        {
            return _SeguimientoDhcpaRepositorio.CountSeguimiento_Consulta(expediente,evaluador, externo,matricula,cmbestado);
        }*/
        /*27*/
        public int Create_Protocolo(ProtocoloRequest request)
        {
            MAE_PROTOCOLO entity = RequestToEntidad.protocolo(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_PROTOCOLO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }


        /*27*/
        public int Insertar_actividad_estado_protocolo(int estado, int id_protocolo)
        {
            MAE_ACTIVIDAD_PROTOCOLO entity_actividad = new MAE_ACTIVIDAD_PROTOCOLO();
            entity_actividad.FECHA_REGISTRO = DateTime.Now;
            entity_actividad.ID_EST_PRO = estado;

            try
            {
                using (TransactionScope scope_p = new TransactionScope())
                {
                    entity_actividad.ID_PROTOCOLO = id_protocolo;
                    _ActividadProtocoloRepositorio.Insertar(entity_actividad);
                    _unitOfWork.Guardar();
                    scope_p.Complete();
                }
                return entity_actividad.ID_ACTIVIDAD_PROTO;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        private void actividad_estado_protocolo(int estado, int id_protocolo)
        {
            MAE_ACTIVIDAD_PROTOCOLO entity_actividad = new MAE_ACTIVIDAD_PROTOCOLO();
            entity_actividad.FECHA_REGISTRO = DateTime.Now;
            entity_actividad.ID_EST_PRO = estado;

            using (TransactionScope scope_p = new TransactionScope())
            {
                entity_actividad.ID_PROTOCOLO = id_protocolo;
                _ActividadProtocoloRepositorio.Insertar(entity_actividad);
                _unitOfWork.Guardar();
                scope_p.Complete();
            }

            ProtocoloRequest proto_res = new ProtocoloRequest();
            proto_res = (from p in _ProtocoloRepositorio.Listar(x => x.ID_PROTOCOLO == id_protocolo)
                          select new ProtocoloRequest
                          {
                              id_protocolo = p.ID_PROTOCOLO,
                              id_seguimiento = p.ID_SEGUIMIENTO,
                              nombre = p.NOMBRE,
                              fecha_inicio = p.FECHA_INICIO,
                              fecha_fin = p.FECHA_FIN,
                              fecha_registro = p.FECHA_REGISTRO,
                              activo = p.ACTIVO,
                              id_ind_pro_esp = p.ID_IND_PRO_ESP,
                              evaluador = p.EVALUADOR,
                              id_est_pro = p.ID_EST_PRO,
                              id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                          }).AsEnumerable().First();

            MAE_PROTOCOLO entity = RequestToEntidad.protocolo(proto_res);
            entity.ID_EST_PRO = estado;

            try
            { 
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloRepositorio.Actualizar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
            }
            catch
            {
                throw new InvalidOperationException();
            }

        }

        /*27*/
        public int Create_Protocolo_Planta(ProtocoloPlantaRequest request)
        {
            DAT_PROTOCOLO_PLANTA entity = RequestToEntidad.dat_protocolo_planta(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloPlantaRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DAT_PRO_PLA;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        

        /*27*/
        public int Create_Protocolo_Desembarcadero(ProtocoloDesembarcaderoRequest request)
        {
            DAT_PROTOCOLO_DESEMBARCADERO entity = RequestToEntidad.dat_protocolo_desembarcadero(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloDesembarcaderoRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_PRO_DESEMB;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*27*/
        public int Create_Protocolo_Almacen(ProtocoloAlmacenRequest request)
        {
            DAT_PROTOCOLO_ALMACEN entity = RequestToEntidad.dat_protocolo_almacen(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloAlmacenRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DAT_PRO_ALMACEN;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        
        /*27*/
        public int Create_Protocolo_Concesion(ProtocoloConcesionRequest request)
        {
            DAT_PROTOCOLO_CONCESION entity = RequestToEntidad.dat_protocolo_concesion(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloConcesionRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_PRO_CONCE;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        
        /*27*/
        public int Create_Protocolo_Autorizacion_Instalacion(ProtocoloAutorizacionInstalacionRequest request)
        {
            DAT_PROTOCOLO_AUTORIZACION_INSTALACION entity = RequestToEntidad.dat_protocolo_autorizacion_instalacion(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloAutorizacionInstalacionRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_PRO_AUTORIZACION_INSTALACION;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        /*27*/
        public int Create_Protocolo_Licencia_Operacion(ProtocoloLicenciaOperacionRequest request)
        {
            DAT_PROTOCOLO_LICENCIA_OPERACION entity = RequestToEntidad.dat_protocolo_licencia_operacion(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloLicenciaOperacionRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_PRO_LICENCIA_OPERACION;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*27*/
        public int Create_Protocolo_Especie(ProtocoloEspecieRequest request)
        {
            DAT_PROTOCOLO_ESPECIE entity = RequestToEntidad.dat_protocolo_especie(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloEspecieRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_PRO_ESPE;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*28*/
        public SeguimientoDhcpaRequest recupera_todo_seguimiento_dhcpa(int id_seguimiento)
        {
            var result = (from zp in _SeguimientoDhcpaRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                          select new SeguimientoDhcpaRequest
                          {
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              id_expediente = zp.ID_EXPEDIENTE,
                              tupa = zp.TUPA,
                              id_tipo_procedimiento = zp.ID_TIPO_PROCEDIMIENTO,
                              fecha_inicio = zp.FECHA_INICIO,
                              fecha_fin = zp.FECHA_FIN,
                              id_ofi_dir = zp.ID_OFI_DIR,
                              persona_num_documento = zp.PERSONA_NUM_DOCUMENTO,
                              id_embarcacion = zp.ID_EMBARCACION,
                              estado = zp.ESTADO,
                              evaluador = zp.EVALUADOR,
                              oficina_crea = zp.OFICINA_CREA,
                              persona_crea = zp.PERSONA_CREA,
                              id_planta = zp.ID_PLANTA,
                              duracion_sdhpa = zp.DURACION_SDHPA,
                              duracion_tramite = zp.DURACION_TRAMITE,
                              observaciones = zp.OBSERVACIONES,
                              inspecto_designado = zp.INSPECTO_DESIGNADO,
                              fecha_auditoria = zp.FECHA_AUDITORIA,
                              fecha_envio_acta = zp.FECHA_ENVIO_ACTA,
                              fecha_envio_oficio_sdhpa = zp.FECHA_ENVIO_OFICIO_SDHPA,
                              con_proceso = zp.CON_PROCESO,
                              id_tipo_seguimiento = zp.ID_TIPO_SEGUIMIENTO,
                              id_habilitante = zp.ID_HABILITANTE,
                              cod_habilitante = zp.COD_HABILITANTE,
                              nombre_externo = zp.NOMBRE_EXTERNO,
                              nom_oficina_crea = zp.NOM_OFICINA_CREA
                          }).ToList().First();
            return result;
        }
        /*29*/
        public bool Update_seguimiento_dhcpa(SeguimientoDhcpaRequest request)
        {
            MAE_SEGUIMIENTO_DHCPA entity = RequestToEntidad.Seguimiento_dhcpa(request);

            try
            {
                using (TransactionScope     scope = new TransactionScope())
                {
                    _SeguimientoDhcpaRepositorio.Actualizar(entity);
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

        /*29*/
        public bool insert_constancia(ConstanciaHaccpRequest request)
        {
            MAE_CONSTANCIA_HACCP entity = RequestToEntidad.constancia_haccp(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ConstanciaHaccpRepositorio.Insertar(entity);
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
        /*29*/
        public bool Guardar_Observacion_seguimiento(SeguimientoDhcpaObservacionesRequest request)
        {
            MAE_SEGUIMIENTO_DHCPA_OBSERVACIONES entity = RequestToEntidad.Seguimiento_dhcpa_observaciones(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _SeguimientoDhcpaObservacionesRepositorio.Insertar(entity);
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
        /*30*/
        public IEnumerable<ConsultarPlantasResponse> GetAllPlanta_x_seguimiento(int id_documento_seg)
        {
            return _DocumentoSeguimientoRepositorio.GetAllPlanta_x_seguimiento(id_documento_seg);
        }
        /*31*/
        public IEnumerable<SeguimientoDhcpaRequest> Recupera_seguimiento_x_id(int id_seguimiento)
        {
            var result = from zp in _SeguimientoDhcpaRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                         select new SeguimientoDhcpaRequest
                         {
                             id_seguimiento = zp.ID_SEGUIMIENTO,
                             id_expediente = zp.ID_EXPEDIENTE,
                             tupa = zp.TUPA,
                             id_tipo_procedimiento = zp.ID_TIPO_PROCEDIMIENTO,
                             fecha_inicio = zp.FECHA_INICIO,
                             fecha_fin = zp.FECHA_FIN,
                             id_ofi_dir = zp.ID_OFI_DIR,
                             persona_num_documento = zp.PERSONA_NUM_DOCUMENTO,
                             id_embarcacion = zp.ID_EMBARCACION,
                             estado = zp.ESTADO,
                             evaluador = zp.EVALUADOR,
                             oficina_crea = zp.OFICINA_CREA,
                             persona_crea = zp.PERSONA_CREA,
                             id_planta = zp.ID_PLANTA,
                             duracion_sdhpa = zp.DURACION_SDHPA,
                             duracion_tramite = zp.DURACION_TRAMITE,
                             observaciones = zp.OBSERVACIONES,
                             inspecto_designado = zp.INSPECTO_DESIGNADO,
                             fecha_auditoria = zp.FECHA_AUDITORIA,
                             fecha_envio_acta =zp.FECHA_ENVIO_ACTA,
                             fecha_envio_oficio_sdhpa = zp.FECHA_ENVIO_OFICIO_SDHPA,
                             con_proceso = zp.CON_PROCESO,
                             id_tipo_seguimiento = zp.ID_TIPO_SEGUIMIENTO,
                             id_habilitante = zp.ID_HABILITANTE,
                             cod_habilitante = zp.COD_HABILITANTE,
                             nom_oficina_crea = zp.NOM_OFICINA_CREA,
                             nombre_externo = zp.NOMBRE_EXTERNO
                         };
            return result;
        }
        /*32*/
        public int recupera_cantidad_solicitud_inspeccion(int var_oficina_crea, int var_año)
        {
            var result = (from zp in _SolicitudInspeccionRepositorio.Listar(x => x.OFICINA_CREA == var_oficina_crea && x.AÑO_CREA == var_año)
                          select new SolicitudInspeccionResponse
                          {
                              id_sol_ins = zp.ID_SOL_INS,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              año_crea = zp.AÑO_CREA,
                              fecha_crea = zp.FECHA_CREA,
                              id_version_solicitud = zp.ID_VERSION_SOLICITUD
                          }).OrderByDescending(x => x.id_sol_ins).AsEnumerable();

            var result_final = result.Count();

            if (result.Count() > 0)
            {
                    result_final = Convert.ToInt32(result.First().numero_documento);
            }

            return result_final;
        }
        /*33*/
        public int Create_solicitud_inspeccion(SolicitudInspeccionRequest request)
        {
            int id_ver_sol = _VersionSolicitudRepositorio.Listar(x => x.ACTIVO == "1").First().ID_VERSION_SOL;
            request.id_version_solicitud = id_ver_sol;

            MAE_SOLICITUD_INSPECCION entity = RequestToEntidad.solicitud_inspeccion(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _SolicitudInspeccionRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }
                return entity.ID_SOL_INS;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        /*34*/
        public int recupera_cantidad_informe_tecnico(int var_oficina_crea, int var_año)
        {
            var result = (from zp in _InformeTecnicoEvalRepositorio.Listar(x => x.OFICINA_CREA == var_oficina_crea && x.AÑO_CREA == var_año)
                          select new InformeTecnicoEvalResponse
                          {
                              id_inf_tec_eval = zp.ID_INF_TEC_EVAL,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              observaciones = zp.OBSERVACIONES,
                              año_crea = zp.AÑO_CREA,
                              fecha_crea = zp.FECHA_CREA
                          }).OrderByDescending(x => x.numero_documento).AsEnumerable();

            var result_final = result.Count();

            if (result.Count() > 0)
            {
                result_final = result.First().numero_documento;
            }

            return result_final;
        }
        /*35*/
        public bool Create_informe_tecnico(InformeTecnicoEvalRequest request)
        {
            MAE_INFORME_TECNICO_EVAL entity = RequestToEntidad.informe_tecnico(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _InformeTecnicoEvalRepositorio.Insertar(entity);
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
        /*36*/
        public ConsultarPlantasResponse Recupera_Planta(int id_seguimiento, int id_planta)
        {
            return _ConsultarPlantasRepositorio.Recupera_Planta(id_seguimiento, id_planta);
        }
        
        /*37*/
        public bool Actualiza_habilitacion_planta(DateTime fecha_habilitacion_final, int id_planta)
        {
            return _ConsultarPlantasRepositorio.Actualiza_habilitacion_planta(fecha_habilitacion_final, id_planta);
        }
        /*38*/
        public IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_dhcpa(string evaluador, int tipo_doc_dhcpa, string asunto, int anno)
        {
            return _SeguimientoDhcpaRepositorio.Lista_Documentos_dhcpa(evaluador, tipo_doc_dhcpa,asunto, anno);
        }

        public DocumentoDhcpaResponse Lista_Documento_dhcpa_x_id_rs(int id_doc_dhcpa)
        {
            MAE_DOCUMENTO_DHCPA m_doc_dhcpa = new MAE_DOCUMENTO_DHCPA();
            m_doc_dhcpa = _DocumentoDhcpaRepositorio.ListarUno(x => x.ID_DOC_DHCPA == id_doc_dhcpa);

            return EntidadToResponse.documentodhcpa(m_doc_dhcpa);
        }

        public DocumentoDhcpaRequest Lista_Documento_dhcpa_x_id_rq(int id_doc_dhcpa)
        {
            MAE_DOCUMENTO_DHCPA m_doc_dhcpa = new MAE_DOCUMENTO_DHCPA();
            m_doc_dhcpa = _DocumentoDhcpaRepositorio.ListarUno(x => x.ID_DOC_DHCPA == id_doc_dhcpa);

            return EntidadToRequest.documentodhcpa(m_doc_dhcpa);
        }
        /*40*/
        public IEnumerable<DocumentoDhcpaResponse> Lista_destino_documentos_dhcpa(int id_documento_dhcpa)
        {
            return _SeguimientoDhcpaRepositorio.Lista_destino_documentos_dhcpa(id_documento_dhcpa);
        }
        /*41*/
        public IEnumerable<SolicitudInspeccionResponse> Lista_solicitud_seguimiento(int id_seguimiento)
        {

            var result = (from zp in _SolicitudInspeccionRepositorio.Listar(x => x.ID_SEGUIMIENTO==id_seguimiento)
                          select new SolicitudInspeccionResponse
                          {
                              id_sol_ins = zp.ID_SOL_INS,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              año_crea = zp.AÑO_CREA,
                              fecha_crea = zp.FECHA_CREA,
                              id_estado = zp.ID_ESTADO
                          }).OrderByDescending(x => x.numero_documento).AsEnumerable();
            return result;
        }
        public SolicitudInspeccionResponse Lista_solicitud_seguimiento_x_id_solicitud(int id_solicitud)
        {

            var result = (from zp in _SolicitudInspeccionRepositorio.Listar(x => x.ID_SOL_INS == id_solicitud)
                          select new SolicitudInspeccionResponse
                          {
                              id_sol_ins = zp.ID_SOL_INS,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              año_crea = zp.AÑO_CREA,
                              fecha_crea = zp.FECHA_CREA,
                              usuario_crea= zp.USUARIO_CREA
                          }).First();
            return result;
        }
        /*42*/
        public IEnumerable<InformeTecnicoEvalResponse> Lista_informe_tecnico_seguimiento(int id_seguimiento)
        {

            var result = (from zp in _InformeTecnicoEvalRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                          select new InformeTecnicoEvalResponse
                          {
                              id_inf_tec_eval = zp.ID_INF_TEC_EVAL,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              numero_documento = zp.NUMERO_DOCUMENTO,
                              año_crea = zp.AÑO_CREA,
                              observaciones = zp.OBSERVACIONES,
                              fecha_crea = zp.FECHA_CREA
                          }).OrderByDescending(x => x.numero_documento).AsEnumerable();
            return result;
        }
        
        /*42*/
        public IEnumerable<Response.SP_CONSULTAR_CORREO_OD_POR_FILIAL_DHCPA_Result> consulta_correo_x_solicitud(int id_solicitud)
        {
            return _SeguimientoDhcpaRepositorio.consulta_correo_x_solicitud(id_solicitud);
        }
        /*43*/
        public IEnumerable<DocumentoSeguimientoResponse> lista_documentos_recibidos_x_seguimiento(int id_seguimiento)
        {
            return _DocumentoSeguimientoRepositorio.lista_documentos_recibidos_x_seguimiento(id_seguimiento);
        }
        /*43*/
        public IEnumerable<DocumentoDhcpaResponse> lista_documentos_emitidos_dhcpa_x_seguimiento(int id_seguimiento)
        {
            return _DocumentoSeguimientoRepositorio.lista_documentos_emitidos_dhcpa_x_seguimiento(id_seguimiento);
        }
        
        /*44*/
        public IEnumerable<ConstanciaHaccpResponse> lista_haccp_x_seguimiento(int id_seguimiento)
        {
            var result = (from p in _ConstanciaHaccpRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                          where p.ACTIVO=="1"
                          select new ConstanciaHaccpResponse
                          {
                              id_constancia_haccp = p.ID_CONSTANCIA_HACCP,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result.ToList();
        }

        public IEnumerable<SeguimientoDhcpaObservacionesResponse> Listar_Observacion_x_seguimiento(int id_seguimiento)
        {
            return _SeguimientoDhcpaRepositorio.Listar_Observacion_x_seguimiento(id_seguimiento);
        }


        public IEnumerable<Response.SP_CONSULTA_HISTORIAL_EVALUADOR_Result> CONSULTA_HISTORIAL_EVALUADOR(int id_seguimiento)
        {
            return _SeguimientoDhcpaRepositorio.CONSULTA_HISTORIAL_EVALUADOR(id_seguimiento);
        }
        /*44*/
        public IEnumerable<ProtocoloResponse> lista_protocolo_x_id_transporte(int id_transporte)
        {
            return _SeguimientoDhcpaRepositorio.lista_protocolo_x_id_transporte(id_transporte);
        }

        /*44*/
        public IEnumerable<ProtocoloResponse> lista_protocolo_x_seguimiento(int id_seguimiento)
        {
            var result = (from p in _ProtocoloRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                         select new ProtocoloResponse
                         {
                             id_protocolo = p.ID_PROTOCOLO,
                             id_seguimiento = p.ID_SEGUIMIENTO,
                             nombre = p.NOMBRE,
                             fecha_inicio = p.FECHA_INICIO,
                             fecha_fin = p.FECHA_FIN,
                             activo = p.ACTIVO,
                             id_ind_pro_esp = p.ID_IND_PRO_ESP,
                             id_est_pro = p.ID_EST_PRO,
                             id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                         }).AsEnumerable();
            return result.ToList();
        }
        /*44*/
        public IEnumerable<ConsultaProtocolosAiResponse> lista_protocolo_ai_x_seguimiento(int id_seguimiento)
        {
            var result = (from p in _ConsultaProtocolosAiRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                          select new ConsultaProtocolosAiResponse
                          {
                              id_protocolo = p.ID_PROTOCOLO,
                              id_seguimiento = p.ID_SEGUIMIENTO,
                              nombre = p.NOMBRE,
                              fecha_inicio = p.FECHA_INICIO,
                              fecha_fin = p.FECHA_FIN,
                              activo = p.ACTIVO,
                              ruta_pdf = p.RUTA_PDF,
                              id_est_pro = p.ID_EST_PRO,
                              id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                          }).AsEnumerable();
            return result.ToList();
        }
        /*44*/
        public IEnumerable<ConsultaProtocolosLoResponse> lista_protocolo_lo_x_seguimiento(int id_seguimiento)
        {
            var result = (from p in _ConsultaProtocolosLoRepositorio.Listar(x => x.ID_SEGUIMIENTO == id_seguimiento)
                          select new ConsultaProtocolosLoResponse
                          {
                              id_protocolo = p.ID_PROTOCOLO,
                              id_seguimiento = p.ID_SEGUIMIENTO,
                              nombre = p.NOMBRE,
                              fecha_inicio = p.FECHA_INICIO,
                              fecha_fin = p.FECHA_FIN,
                              activo = p.ACTIVO,
                              ruta_pdf = p.RUTA_PDF,
                              id_est_pro = p.ID_EST_PRO,
                              id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                          }).AsEnumerable();
            return result.ToList();
        }
        /*45*/
        public IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_dhcpa()
        {
            return _SeguimientoDhcpaRepositorio.Lista_Solicitudes_dhcpa();
        }
        /*48*/
        public IEnumerable<DocumentoSeguimientoResponse> Lista_Documento_OD_pendientes_x_recibir(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc,string expediente)
        {
            return _SeguimientoDhcpaRepositorio.Lista_Documento_OD_pendientes_x_recibir( estado,  indicador,  evaluador,  asunto,  externo,  id_tipo_documento,  num_doc,  nom_doc, expediente);
        }
         
        /*49*/
        public IEnumerable<ExpedientesResponse> Lista_expediente_sin_seguimiento()
        {
            return _ExpedientesRepositorio.Lista_expediente_sin_seguimiento();
        }
        /*43*/
        public IEnumerable<DocumentoDhcpaResponse> Lista_Documentos_x_tipo_documento(int id_tipo_documento, int anno)
        {
            return _SeguimientoDhcpaRepositorio.Lista_Documentos_x_tipo_documento(id_tipo_documento, anno);
        }
        /*45*/
        public IEnumerable<SeguimientoDhcpaResponse> Lista_Solicitudes_excel()
        {
            return _SeguimientoDhcpaRepositorio.Lista_Solicitudes_excel();
        }
        public IEnumerable<Response.SP_CONSULTAR_TRANSPORTES_CON_PROTOCOLO_HABILITADO_Result> lista_transportes_con_protocolo_habilitado()
        {
            return _SeguimientoDhcpaRepositorio.lista_transportes_con_protocolo_habilitado();
        }
        public IEnumerable<ProtocoloResponse> Lista_mae_protocolo(string nombre_protocolo)
        {
            var reuslt = (from p in _ProtocoloRepositorio.Listar(x => x.NOMBRE.Contains(nombre_protocolo)).Take(500)
                         select new ProtocoloResponse
                         {
                             id_protocolo = p.ID_PROTOCOLO,
                             nombre =p.NOMBRE,
                             fecha_registro = p.FECHA_REGISTRO,
                             fecha_inicio = p.FECHA_INICIO,
                             fecha_fin = p.FECHA_FIN,
                             activo = p.ACTIVO,
                             id_est_pro = p.ID_EST_PRO,
                             id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                         }).OrderByDescending(x => x.id_protocolo);
            return reuslt;
        }
        
        /*45*/
        public IEnumerable<ConsultarPlantasResponse> Lista_plantas_excel()
        {
            return _SeguimientoDhcpaRepositorio.Lista_plantas_excel();
        }

        /*46*/
        public IEnumerable<ConsultarOficinaResponse> Consultar_RUC_X_NOM_Seguimiento(string NOM)
        {
            //var result_dir = (from zp in _ConsultarOficinaRepositorio.Listar(x => x.ID_OFI_PADRE == null && x.NOMBRE.Contains(NOM))
            var result_dir = (from zp in _HojaTramiteRepositorio.GetAll_Oficinas_Direcciones_X_NOM(NOM)
                              select new ConsultarOficinaResponse
                              {
                                  id_oficina = zp.id_oficina,
                                  nombre = zp.nombre,
                                  ruc = zp.ruc
                              }).OrderBy(x => x.nombre);
            return result_dir.ToList();
        }

        /*47*/
        public IEnumerable<ConsultarOficinaResponse> Consultar_RUC_seguimiento(string RUC)
        {
            var result_dir = from zp in _HojaTramiteRepositorio.GetAll_Oficinas_Direcciones(RUC)
                             where zp.var_id_ofi_padre==null
                             select new ConsultarOficinaResponse
                             {
                                 id_oficina = zp.id_oficina,
                                 nombre = zp.nom_oficina,
                                 ruc = zp.ruc
                             };

            return result_dir.ToList();
        }
        /*48*/
        public IEnumerable<DocumentoDhcpaResponse> Lista_Destino_Documentos_x_tipo_documento(int id_doc_dhcpa)
        {
            return _SeguimientoDhcpaRepositorio.Lista_Destino_Documentos_x_tipo_documento(id_doc_dhcpa);
        }
        /*49*/
        public IEnumerable<EstadoSeguimientoDhcpaResponse> Lista_estado_seguimiento_dhcpa()
        {
            var result_list = from zp in _EstadoSeguimientoDhcpaRepositorio.Listar()
                             select new EstadoSeguimientoDhcpaResponse
                             {
                                 id_estado = zp.ID_ESTADO,
                                 nombre = zp.NOMBRE
                             };

            return result_list.ToList();
        }

        public IEnumerable<TipoServicioHabilitacionResponse> Lista_tipo_servicio_habilitaciones()
        {
            var result_list = from zp in _TipoServicioHabilitacionRepositorio.Listar()
                              select new TipoServicioHabilitacionResponse
                              {
                                  id_tipo_ser_hab = zp.ID_TIPO_SER_HAB,
                                  nombre = zp.NOMBRE
                              };
            return result_list.ToList();
        }
        /*50*/
        public IEnumerable<SeguimientoDhcpaResponse> GetAllSeguimiento_Consulta_excel(int id_oficina) 
        {
            return _SeguimientoDhcpaRepositorio.GetAllSeguimiento_Consulta_excel(id_oficina);
        }
        /*51*/
        public SeguimientoDhcpaResponse Lista_protocolo_solicitud(int id_seguimiento)
        {
            return _SeguimientoDhcpaRepositorio.Lista_protocolo_solicitud(id_seguimiento);
        }
        /*51*/
        public SeguimientoDhcpaResponse Lista_protocolo_seguimiento_planta(int id_planta)
        {
            return _SeguimientoDhcpaRepositorio.Lista_protocolo_seguimiento_planta(id_planta);
        }
        
        /*52*/
        public SeguimientoDhcpaResponse Lista_datos_evaluador(int id_seguimiento)
        {
            return _SeguimientoDhcpaRepositorio.Lista_datos_evaluador(id_seguimiento);
        }
        
        /*27*/
        public int Create_Protocolo_Embarcacion(ProtocoloEmbarcacionRequest request)
        {
            DAT_PROTOCOLO_EMBARCACION entity = RequestToEntidad.dat_protocolo_embarcacion(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloEmbarcacionRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DET_PRO_HAB;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }
        
        /*27*/
        public int Create_Persona_telefono(string persona_num_documento, string telefono, string usuario)
        {
            return _SeguimientoDhcpaRepositorio.Create_Persona_telefono(persona_num_documento, telefono, usuario);
        }
        /*36*/
        public ConsultaEmbarcacionesResponse Recupera_Embarcacion(int id_seguimiento, int id_embarcacion)
        {
            return _ConsultaEmbarcacionesRepositorio.Recupera_Embarcacion(id_seguimiento, id_embarcacion);
        }
        
        /*49*/
        public IEnumerable<IndicadorProtocoloEspecieResponse> Lista_indicadorprotocoloespecie()
        {
            var result_list = from zp in _IndicadorProtocoloEspecieRepositorio.Listar()
                              select new IndicadorProtocoloEspecieResponse
                              {
                                  id_ind_pro_esp = zp.ID_IND_PRO_ESP,
                                  nombre = zp.NOMBRE
                              };

            return result_list.ToList();
        }
        
        /*49*/
        public IEnumerable<TipoAutorizacionInstalacionResponse> Lista_tipo_autorizacion(int id_tipo_ai)
        {
            var result_list = from zp in _TipoAutorizacionInstalacionRepositorio.Listar(x => (id_tipo_ai == 0 || (id_tipo_ai != 0 && x.ID_TIPO_AUTORIZACION_INSTALACION==id_tipo_ai)))
                              select new TipoAutorizacionInstalacionResponse
                              {
                                  id_tipo_autorizacion_instalacion = zp.ID_TIPO_AUTORIZACION_INSTALACION,
                                  ruta_pdf = zp.RUTA_PDF,
                                  nombre = zp.NOMBRE
                              };

            return result_list.ToList();
        }
        

        /*49*/
        public IEnumerable<TipoLicenciaOperacionResponse> Lista_tipo_licencia_operacion(int id_tipo_lo)
        {
            var result_list = from zp in _TipoLicenciaOperacionRepositorio.Listar(x => (id_tipo_lo == 0 || (id_tipo_lo != 0 && x.ID_TIPO_LICENCIA_OPERACION == id_tipo_lo)))
                              select new TipoLicenciaOperacionResponse
                              {
                                  id_tipo_licencia_operacion = zp.ID_TIPO_LICENCIA_OPERACION,
                                  ruta_pdf = zp.RUTA_PDF,
                                  nombre = zp.NOMBRE
                              };

            return result_list.ToList();
        }
        public IEnumerable<EspeciesHabilitacionesResponse> lista_especies_habilitaciones(string nombre_comun, string nombre_cientifico)
        {
            return _EspeciesHabilitacionesRepositorio.lista_especies_habilitaciones(nombre_comun, nombre_cientifico);
        }


        public IEnumerable<ConsultarPlantasResponse> genera_protocolo_planta()
        {
            return _ConsultarPlantasRepositorio.genera_protocolo_planta();
        }
        
        public IEnumerable<ProtocoloLicenciaOperacionResponse> genera_protocolo_licencia_operacion()
        {
            return _ProtocoloLicenciaOperacionRepositorio.genera_protocolo_licencia_operacion();
        }

        public IEnumerable<ProtocoloAutorizacionInstalacionResponse> genera_protocolo_autorizacion_instalacion()
        {
            return _ProtocoloAutorizacionInstalacionRepositorio.genera_protocolo_autorizacion_instalacion();
        }
        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> genera_protocolo_desembarcadero()
        {
            return _DbGeneralMaeDesembarcaderoRepositorio.genera_protocolo_desembarcadero();
        }
        public IEnumerable<ConsultaEmbarcacionesResponse> genera_protocolo_embarcacion()
        {
            return _ConsultaEmbarcacionesRepositorio.genera_protocolo_embarcacion();
        }

        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> genera_protocolo_almacen()
        {
            return _ConsultarDbGeneralMaeAlmacenSedeRepositorio.genera_protocolo_almacen();
        }

        public IEnumerable<DbGeneralMaeTransporteResponse> genera_protocolo_transporte()
        {
            return _DbGeneralMaeTransporteRepositorio.genera_protocolo_transporte();
        }
        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> genera_protocolo_concesion()
        {
            return _ConsultarDbGeneralMaeConcesionRepositorio.genera_protocolo_concesion();
        }

        
        /*44*/
        public ProtocoloRequest lista_protocolo_x_id(int id_protocolo)
        {
            var result = (from p in _ProtocoloRepositorio.Listar(x => x.ID_PROTOCOLO==id_protocolo)
                          select new ProtocoloRequest
                          {
                              id_protocolo = p.ID_PROTOCOLO,
                              id_seguimiento = p.ID_SEGUIMIENTO,
                              nombre = p.NOMBRE,
                              fecha_inicio = p.FECHA_INICIO,
                              fecha_fin = p.FECHA_FIN,
                              fecha_registro = p.FECHA_REGISTRO,
                              activo = p.ACTIVO,
                              id_ind_pro_esp = p.ID_IND_PRO_ESP,
                              evaluador = p.EVALUADOR,
                              id_est_pro = p.ID_EST_PRO,
                              id_protocolo_reemplaza = p.ID_PROTOCOLO_REEMPLAZA
                          }).AsEnumerable();
            return result.First();
        }

        public ProtocoloTransporteRequest lista_protocolo_transporte_x_id_protocolo(int id_protocolo)
        {
            var result = (from p in _ProtocoloTransporteRepositorio.Listar(x => x.ID_PROTOCOLO == id_protocolo)
                          select new ProtocoloTransporteRequest
                          {
                              id_dat_pro_transporte = p.ID_DAT_PRO_TRANSPORTE,
                              id_protocolo = p.ID_PROTOCOLO,
                              numero = p.NUMERO,
                              anno = p.ANNO,
                              direccion_legal = p.DIRECCION_LEGAL,
                              representante_legal = p.REPRESENTANTE_LEGAL,
                              id_tipo_camara_trans = p.ID_TIPO_CAMARA_TRANS,
                              id_transporte = p.ID_TRANSPORTE,
                              placa = p.PLACA,
                              cod_habilitacion = p.COD_HABILITACION,
                              id_tipo_carroceria = p.ID_TIPO_CARROCERIA,
                              id_um = p.ID_UM,
                              carga_util = p.CARGA_UTIL,
                              acta_inspeccion = p.ACTA_INSPECCION,
                              informe_auditoria = p.INFORME_AUDITORIA,
                              informe_tecnico_evaluacion = p.INFORME_TECNICO_EVALUACION,
                              persona_2 = p.PERSONA_2,
                              id_tipo_atencion = p.ID_TIPO_ATENCION,
                              id_tipo_carroceria_tarpro = p.ID_TIPO_CARROCERIA_TARPRO,
                              informe_sdhpa = p.INFORME_SDHPA
                          }).AsEnumerable();
            return result.First();
        }
        /*44*/
        public IEnumerable<ConsultarPersonaTelefonoResponse> consulta_persona_natural_telefono(string persona_num_documento)
        {
            
            var result = (from p in _ConsultarPersonaTelefonoRepositorio.Listar(x => x.PERSONA_NUM_DOCUMENTO == persona_num_documento)
                          where p.ACTIVO=="1"
                          select new ConsultarPersonaTelefonoResponse
                          {
                              id_persona_telefono =p.ID_PERSONA_TELEFONO,
                              telefono1 = p.TELEFONO1
                          }).AsEnumerable();
            return result;
        }

        public DbGeneralMaeTransporteResponse consulta_db_general_transporte_x_id(int id_transporte)
        {
            DbGeneralMaeTransporteResponse trans_resp = new DbGeneralMaeTransporteResponse();

            var result = (from p in _DbGeneralMaeTransporteRepositorio.Listar(x => x.ID_TRANSPORTE == id_transporte)
                          select new DbGeneralMaeTransporteResponse
                          {
                              id_transporte = p.ID_TRANSPORTE,
                              placa = p.PLACA,
                              cod_habilitacion = p.COD_HABILITACION,
                              id_tipo_carroceria = p.ID_TIPO_CARROCERIA,
                              id_um = p.ID_UM,
                              nombre_carroceria = p.NOMBRE_CARROCERIA,
                              carga_util = p.CARGA_UTIL,
                              nombre_um = p.NOMBRE_UM,
                              id_tipo_furgon = p.ID_TIPO_FURGON,
                              nombre_furgon = p.NOMBRE_FURGON
                          }).AsEnumerable();
            if (result.Count() > 0)
            {
                trans_resp = result.First();
            }
            return trans_resp;

        }

        public IEnumerable<TipoCamaraTransporteResponse> consulta_todo_activo_tipoCamaraTransporte()
        {
            var result = (from p in _TipoCamaraTransporteRepositorio.Listar()
                          select new TipoCamaraTransporteResponse
                          {
                              id_tipo_camara_trans = p.ID_TIPO_CAMARA_TRANS,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result;

        }
        /*20*/
        public IEnumerable<DbGeneralMaeTipoCarroceriaResponse> consulta_todo_activo_tipocarroceria()
        {
            var result = (from p in _DbGeneralMaeTipoCarroceriaRepositorio.Listar()
                          select new DbGeneralMaeTipoCarroceriaResponse
                          {
                              id_tipo_carroceria = p.ID_TIPO_CARROCERIA,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result;

        }

        public DbGeneralMaeTipoCarroceriaResponse consulta_todo_activo_tipocarroceria_x_id(int id_tc)
        {
            var result = (from p in _DbGeneralMaeTipoCarroceriaRepositorio.Listar(x => x.ID_TIPO_CARROCERIA == id_tc)
                          select new DbGeneralMaeTipoCarroceriaResponse
                          {
                              id_tipo_carroceria = p.ID_TIPO_CARROCERIA,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result.First();
        }

        public TipoAtencionInspeccionResponse consulta_tipo_atencion_x_id(int id_ta)
        {
            var result = (from p in _TipoAtencionInspeccionRepositorio.Listar(x => x.ID_TIPO_ATENCION == id_ta)
                          select new TipoAtencionInspeccionResponse
                          {
                              id_tipo_atencion = p.ID_TIPO_ATENCION,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result.First();
        }

        public IEnumerable<ConsultarTipoFurgonTransporteResponse> consulta_todo_activo_tipofurgon(int id_tipo_carroceria)
        {
            var result = (from p in _ConsultarTipoFurgonTransporteRepositorio.Listar(x => x.ID_TIPO_CARROCERIA == id_tipo_carroceria)
                          where p.ACTIVO=="1"
                          select new ConsultarTipoFurgonTransporteResponse
                          {
                              id_tipo_furgon = p.ID_TIPO_FURGON,
                              nombre = p.NOMBRE
                          }).AsEnumerable();
            return result;

        }
        /*20*/
        public IEnumerable<DbGeneralMaeUnidadMedidaResponse> consulta_todo_activo_unidad_medida()
        {
            var result = (from p in _DbGeneralMaeUnidadMedidaRepositorio.Listar()
                          select new DbGeneralMaeUnidadMedidaResponse
                          {
                              id_um = p.ID_UM,
                              nombre = p.NOMBRE,
                              siglas = p.SIGLAS
                          }).AsEnumerable();
            return result;

        }
        public IEnumerable<TipoAtencionInspeccionResponse> consulta_todo_tipo_atencion()
        {
            var result = (from p in _TipoAtencionInspeccionRepositorio.Listar()
                          where p.ACTIVO == "1"
                          select new TipoAtencionInspeccionResponse
                          {
                              id_tipo_atencion = p.ID_TIPO_ATENCION,
                              nombre = p.NOMBRE
                          });
            return result;
        }
        public DbGeneralMaeTransporteResponse registrar_nuevo_transporte(string nueva_placa, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario)
        {
            return _SeguimientoDhcpaRepositorio.registrar_nuevo_transporte( nueva_placa, nueva_codigo_habilitacion, nueva_carroceria, tipo_furgon, nueva_carga_util, nueva_unidad_medida, usuario);
        }

        public DbGeneralMaeTransporteResponse actualizar_nuevo_transporte(int id_transporte, string nueva_codigo_habilitacion, int nueva_carroceria, int tipo_furgon, decimal nueva_carga_util, int nueva_unidad_medida, string usuario)
        {
            return _SeguimientoDhcpaRepositorio.actualizar_nuevo_transporte(id_transporte, nueva_codigo_habilitacion, nueva_carroceria, tipo_furgon, nueva_carga_util, nueva_unidad_medida, usuario);
        }
        public int Generar_numero_protocolo_transporte(int anno)
        {
            return _ProtocoloRepositorio.Generar_numero_protocolo_transporte(anno);
        }


        public int Create_Protocolo_Transporte(ProtocoloTransporteRequest request)
        {
            DAT_PROTOCOLO_TRANSPORTE entity = RequestToEntidad.ProtocoloTransporte(request);

            actividad_estado_protocolo(1, entity.ID_PROTOCOLO ?? 0);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloTransporteRepositorio.Insertar(entity);
                    _unitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_DAT_PRO_TRANSPORTE;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

        public bool Update_Protocolo_Transporte(ProtocoloTransporteRequest request)
        {
            DAT_PROTOCOLO_TRANSPORTE entity = RequestToEntidad.ProtocoloTransporte(request);

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _ProtocoloTransporteRepositorio.Actualizar(entity);
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
        public IEnumerable<Response.SP_CONSULTAR_ACTA_CHKL_INFO_PRU_SI_Result> Lista_acta_info_pru_por_si(int id_sol_ins)
        {
            return _SeguimientoDhcpaRepositorio.Lista_acta_info_pru_por_si(id_sol_ins);
        }
        public SolicitudInspeccionResponse Consultar_solicitud_inspeccion_sdhpa_x_id(int id_sol_ins)
        {
            SolicitudInspeccionResponse resp_si = new SolicitudInspeccionResponse();

            var result = (from x in _SolicitudInspeccionRepositorio.Listar(x => x.ID_SOL_INS==id_sol_ins)
                         select new SolicitudInspeccionResponse()
                         {
                             numero_documento= x.NUMERO_DOCUMENTO,
                             año_crea = x.AÑO_CREA
                         }).First();

            return result;
        }

        /*07*/
        public IEnumerable<DocumentoSeguimientoResponse> GetAllDocumentos_x_rec(string estado, string indicador, string evaluador, string asunto, string externo, string id_tipo_documento, string num_doc, string nom_doc, int oficina_crea, string expediente)
        {
            return _DocumentoSeguimientoRepositorio.GetAllDocumentos_x_rec(estado, indicador, evaluador, asunto, externo, id_tipo_documento, num_doc, nom_doc, oficina_crea, expediente);
        }
        
    }
}
