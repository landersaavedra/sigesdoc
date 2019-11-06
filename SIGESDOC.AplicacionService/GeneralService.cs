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
    public class GeneralService : IGeneralService
    {

        /*01*/  private readonly ITipoTramiteRepositorio _TipoTramiteRepositorio;
        /*02*/  private readonly IConsultarOficinaRepositorio _ConsultarOficinaRepositorio;
        /*03*/  private readonly IConsultarSedeOficinaRepositorio _ConsultarSedeOficinaRepositorio;
        /*04*/  private readonly ITipoDocumentoRepositorio _TipoDocumentoRepositorio;
        /*05*/  private readonly IEstadoTramiteRepositorio _EstadoTramiteRepositorio;
        /*06*/  private readonly IConsultarPersonalRepositorio _ConsultarPersonalRepositorio;
        /*07*/  private readonly IHojaTramiteRepositorio _HojaTramiteRepositorio;
        /*08*/  private readonly IConsultarDepartamentoRepositorio _ConsultarDepartamentoRepositorio;
        /*09*/  private readonly IConsultarProvinciaRepositorio _ConsultarProvinciaRepositorio;
        /*10*/  private readonly IConsultarUbigeoRepositorio _ConsultarUbigeoRepositorio;
        /*11*/  private readonly IConsultarTipoDocumentoIdentidadRepositorio _ConsultarTipoDocumentoIdentidadRepositorio;
        /*12*/  private readonly ITipoExpedienteRepositorio _TipoExpedienteRepositorio;
        /*13*/  private readonly IExpedientesRepositorio _ExpedientesRepositorio;
        /*14*/  private readonly ITipoProcedimientoRepositorio _TipoProcedimientoRepositorio;
        /*15*/  private readonly IConsultaEmbarcacionesRepositorio _ConsultaEmbarcacionesRepositorio;
        /*16*/  private readonly IConsultaFacturasRepositorio _ConsultaFacturasRepositorio;
        /*17*/  private readonly IUnitOfWork _unitOfWork;
        /*18*/  private readonly IConsultarUsuarioRepositorio _ConsultarUsuarioRepositorio;
        /*19*/  private readonly IConsultarDniRepositorio _ConsultarDniRepositorio;
        /*20*/  private readonly IConsultarTipoPlantaRepositorio _ConsultarTipoPlantaRepositorio;
        /*21*/  private readonly IConsultarPlantasRepositorio _ConsultarPlantasRepositorio;
        /*22*/  private readonly IConsultaTipoEmbarcacionesRepositorio _ConsultaTipoEmbarcacionesRepositorio;
        /*23*/  private readonly ITipoConsumoHumanoRepositorio _TipoConsumoHumanoRepositorio;
        /*24*/  private readonly ITupaRepositorio _TupaRepositorio;
        /*25*/  private readonly IFilialDhcpaRepositorio _FilialDhcpaRepositorio;
        /*26*/  private readonly IConsultarTipoActividadPlantaRepositorio _ConsultarTipoActividadPlantaRepositorio;
        /*27*/  private readonly IProtocoloRepositorio _ProtocoloRepositorio;
        /*28*/  private readonly IServicioDhcpaRepositorio _ServicioDhcpaRepositorio;
        /*29*/  private readonly ITipoSeguimientoRepositorio _TipoSeguimientoRepositorio;
        /*30*/  private readonly IConsultarCodHabEmbarcacionRepositorio _ConsultarCodHabEmbarcacionRepositorio;
        /*31*/  private readonly IConsultarActvEmbarcacionRepositorio _ConsultarActvEmbarcacionRepositorio;
        /*32*/  private readonly IConsultarOficinaDireccionLegalRepositorio _ConsultarOficinaDireccionLegalRepositorio;
        /*33*/  private readonly IConsultarEmpresaPersonaLegalRepositorio _ConsultarEmpresaPersonaLegalRepositorio;
        /*34*/  private readonly ITipoProtocoloEmbarcacionRepositorio _TipoProtocoloEmbarcacionRepositorio;
        /*35*/  private readonly IConsultarDbGeneralMaeAlmacenSedeRepositorio _ConsultarDbGeneralMaeAlmacenSedeRepositorio;
        /*36*/  private readonly IConsultarActvAlmacenRepositorio _ConsultarActvAlmacenRepositorio;
        /*37*/  private readonly IConsultarCodHabAlmacenRepositorio _ConsultarCodHabAlmacenRepositorio;
        /*38*/  private readonly IConsultarDbGeneralMaeZonaProduccionRepositorio _ConsultarDbGeneralMaeZonaProduccionRepositorio;
        /*39*/  private readonly IConsultarDbGeneralMaeAreaProduccionRepositorio _ConsultarDbGeneralMaeAreaProduccionRepositorio;
        /*40*/  private readonly IConsultarDbGeneralMaeConcesionRepositorio _ConsultarDbGeneralMaeConcesionRepositorio;
        /*41*/  private readonly IConsultarDbGeneralMaeTipoConcesionRepositorio _ConsultarDbGeneralMaeTipoConcesionRepositorio;
        /*42*/  private readonly ITipoActividadConcesionRepositorio _TipoActividadConcesionRepositorio;
        /*43*/  private readonly IDbGeneralMaeTipoDesembarcaderoRepositorio _DbGeneralMaeTipoDesembarcaderoRepositorio;
        /*44*/  private readonly IDbGeneralMaeCodigoDesembarcaderoRepositorio _DbGeneralMaeCodigoDesembarcaderoRepositorio;
        /*45*/  private readonly IDbGeneralMaeDesembarcaderoRepositorio _DbGeneralMaeDesembarcaderoRepositorio;
        /*46*/  private readonly ITipoPedidoSigaRepositorio _TipoPedidoSigaRepositorio;
        /*46*/  private readonly IConsultarRucRepositorio _ConsultarRucRepositorio;
        /*46*/
        private readonly IDbGeneralMaeTransporteRepositorio _DbGeneralMaeTransporteRepositorio;
        private readonly IProtocoloTransporteRepositorio _ProtocoloTransporteRepositorio;
        private readonly IDestinoSolicitudInspeccionRepositorio _DestinoSolicitudInspeccionRepositorio;
        private readonly IConsultarDniPersonalLegalRepositorio _ConsultarDniPersonalLegalRepositorio;
        private readonly ITipoTupaRepositorio _TipoTupaRepositorio;
        private readonly IConsultaDbGeneralMaeFacturaRepositorio _ConsultaDbGeneralMaeFacturaRepositorio;
        private readonly IConsultaReciboSerie1Repositorio _ConsultaReciboSerie1Repositorio;
        private readonly IConsultaDbGeneralMaeTipoFacturaRepositorio _ConsultaDbGeneralMaeTipoFacturaRepositorio;
        private readonly IConsultaDbGeneralMaeValorFacturadoExpRepositorio _ConsultaDbGeneralMaeValorFacturadoExpRepositorio;
        private readonly IConsultaPersonaReciboSerie1Repositorio _ConsultaPersonaReciboSerie1Repositorio;
        private readonly IConsultaDbGeneralMaeOperacionRepositorio _ConsultaDbGeneralMaeOperacionRepositorio;
        private readonly IReporteComprobanteXMesConsultaRepositorio _ReporteComprobanteXMesConsultaRepositorio;
        private readonly IConsultaReporteDiarioSerie1Repositorio _ConsultaReporteDiarioSerie1Repositorio;
        private readonly IConsultarDireccionRepositorio _ConsultarDireccionRepositorio;
        
        
        
        
        
        
        
        
        

        public GeneralService(
            /*01*/  ITipoTramiteRepositorio TipoTramiteRepositorio,
            /*02*/  IConsultarOficinaRepositorio ConsultarOficinaRepositorio,
            /*03*/  IConsultarSedeOficinaRepositorio ConsultarSedeOficinaRepositorio,
            /*04*/  ITipoDocumentoRepositorio TipoDocumentoRepositorio,
            /*05*/  IEstadoTramiteRepositorio EstadoTramiteRepositorio,
            /*06*/  IConsultarPersonalRepositorio ConsultarPersonalRepositorio,
            /*07*/  IHojaTramiteRepositorio HojaTramiteRepositorio,
            /*08*/  IConsultarDepartamentoRepositorio ConsultarDepartamentoRepositorio,
            /*09*/  IConsultarProvinciaRepositorio ConsultarProvinciaRepositorio,
            /*10*/  IConsultarUbigeoRepositorio ConsultarUbigeoRepositorio,
            /*11*/  IConsultarTipoDocumentoIdentidadRepositorio ConsultarTipoDocumentoIdentidadRepositorio,
            /*12*/  ITipoExpedienteRepositorio TipoExpedienteRepositorio,
            /*13*/  IExpedientesRepositorio ExpedientesRepositorio,
            /*14*/  ITipoProcedimientoRepositorio TipoProcedimientoRepositorio,
            /*15*/  IConsultaEmbarcacionesRepositorio ConsultaEmbarcacionesRepositorio,
            /*16*/  IConsultaFacturasRepositorio ConsultaFacturasRepositorio,
            /*17*/  IUnitOfWork unitOfWork,
            /*18*/  IConsultarUsuarioRepositorio ConsultarUsuarioRepositorio,
            /*19*/  IConsultarDniRepositorio ConsultarDniRepositorio,
            /*20*/  IConsultarTipoPlantaRepositorio ConsultarTipoPlantaRepositorio,
            /*21*/  IConsultarPlantasRepositorio ConsultarPlantasRepositorio,
            /*22*/  IConsultaTipoEmbarcacionesRepositorio ConsultaTipoEmbarcacionesRepositorio,
            /*23*/  ITipoConsumoHumanoRepositorio TipoConsumoHumanoRepositorio,
            /*24*/  ITupaRepositorio TupaRepositorio,
            /*25*/  IFilialDhcpaRepositorio FilialDhcpaRepositorio,
            /*26*/  IConsultarTipoActividadPlantaRepositorio ConsultarTipoActividadPlantaRepositorio,
            /*27*/  IProtocoloRepositorio ProtocoloRepositorio,
            /*28*/  IServicioDhcpaRepositorio ServicioDhcpaRepositorio,
            /*29*/  ITipoSeguimientoRepositorio TipoSeguimientoRepositorio,
            /*30*/  IConsultarCodHabEmbarcacionRepositorio ConsultarCodHabEmbarcacionRepositorio,
            /*31*/  IConsultarActvEmbarcacionRepositorio ConsultarActvEmbarcacionRepositorio,
            /*32*/  IConsultarOficinaDireccionLegalRepositorio ConsultarOficinaDireccionLegalRepositorio,
            /*33*/  IConsultarEmpresaPersonaLegalRepositorio ConsultarEmpresaPersonaLegalRepositorio,
            /*34*/  ITipoProtocoloEmbarcacionRepositorio TipoProtocoloEmbarcacionRepositorio,
            /*35*/  IConsultarDbGeneralMaeAlmacenSedeRepositorio ConsultarDbGeneralMaeAlmacenSedeRepositorio,
            /*36*/  IConsultarActvAlmacenRepositorio ConsultarActvAlmacenRepositorio,
            /*37*/  IConsultarCodHabAlmacenRepositorio ConsultarCodHabAlmacenRepositorio,
            /*38*/  IConsultarDbGeneralMaeZonaProduccionRepositorio ConsultarDbGeneralMaeZonaProduccionRepositorio,
            /*39*/  IConsultarDbGeneralMaeAreaProduccionRepositorio ConsultarDbGeneralMaeAreaProduccionRepositorio,
            /*40*/  IConsultarDbGeneralMaeConcesionRepositorio ConsultarDbGeneralMaeConcesionRepositorio,
            /*41*/  IConsultarDbGeneralMaeTipoConcesionRepositorio ConsultarDbGeneralMaeTipoConcesionRepositorio,
            /*42*/  ITipoActividadConcesionRepositorio TipoActividadConcesionRepositorio,
            /*43*/  IDbGeneralMaeTipoDesembarcaderoRepositorio DbGeneralMaeTipoDesembarcaderoRepositorio,
            /*44*/  IDbGeneralMaeCodigoDesembarcaderoRepositorio DbGeneralMaeCodigoDesembarcaderoRepositorio,
            /*45*/  IDbGeneralMaeDesembarcaderoRepositorio DbGeneralMaeDesembarcaderoRepositorio,
            /*46*/  ITipoPedidoSigaRepositorio TipoPedidoSigaRepositorio,
            IDbGeneralMaeTransporteRepositorio DbGeneralMaeTransporteRepositorio,
            
        IProtocoloTransporteRepositorio ProtocoloTransporteRepositorio,
            IDestinoSolicitudInspeccionRepositorio DestinoSolicitudInspeccionRepositorio,
            IConsultarDniPersonalLegalRepositorio ConsultarDniPersonalLegalRepositorio,
            IConsultarRucRepositorio ConsultarRucRepositorio,
            ITipoTupaRepositorio TipoTupaRepositorio,
            IConsultaDbGeneralMaeFacturaRepositorio ConsultaDbGeneralMaeFacturaRepositorio,
            IConsultaReciboSerie1Repositorio ConsultaReciboSerie1Repositorio,
            IConsultaDbGeneralMaeTipoFacturaRepositorio ConsultaDbGeneralMaeTipoFacturaRepositorio,
            IConsultaDbGeneralMaeValorFacturadoExpRepositorio ConsultaDbGeneralMaeValorFacturadoExpRepositorio,
            IConsultaPersonaReciboSerie1Repositorio ConsultaPersonaReciboSerie1Repositorio,
            IConsultaDbGeneralMaeOperacionRepositorio ConsultaDbGeneralMaeOperacionRepositorio,
            IReporteComprobanteXMesConsultaRepositorio ReporteComprobanteXMesConsultaRepositorio,
            IConsultaReporteDiarioSerie1Repositorio ConsultaReporteDiarioSerie1Repositorio,
            IConsultarDireccionRepositorio ConsultarDireccionRepositorio
            )
        {
            /*01*/  _TipoTramiteRepositorio = TipoTramiteRepositorio;
            /*02*/  _ConsultarOficinaRepositorio = ConsultarOficinaRepositorio;
            /*03*/  _ConsultarSedeOficinaRepositorio = ConsultarSedeOficinaRepositorio;
            /*04*/  _TipoDocumentoRepositorio = TipoDocumentoRepositorio;
            /*05*/  _EstadoTramiteRepositorio = EstadoTramiteRepositorio;
            /*06*/  _ConsultarPersonalRepositorio = ConsultarPersonalRepositorio;
            /*07*/  _HojaTramiteRepositorio = HojaTramiteRepositorio;
            /*08*/  _ConsultarDepartamentoRepositorio = ConsultarDepartamentoRepositorio;
            /*09*/  _ConsultarProvinciaRepositorio = ConsultarProvinciaRepositorio;
            /*10*/  _ConsultarUbigeoRepositorio = ConsultarUbigeoRepositorio;
            /*11*/  _ConsultarTipoDocumentoIdentidadRepositorio = ConsultarTipoDocumentoIdentidadRepositorio;
            /*12*/  _TipoExpedienteRepositorio = TipoExpedienteRepositorio;
            /*13*/  _ExpedientesRepositorio = ExpedientesRepositorio;
            /*14*/  _TipoProcedimientoRepositorio = TipoProcedimientoRepositorio;
            /*15*/  _ConsultaEmbarcacionesRepositorio = ConsultaEmbarcacionesRepositorio;
            /*16*/  _ConsultaFacturasRepositorio = ConsultaFacturasRepositorio;
            /*17*/  _unitOfWork = unitOfWork;
            /*18*/  _ConsultarUsuarioRepositorio = ConsultarUsuarioRepositorio;
            /*19*/  _ConsultarDniRepositorio = ConsultarDniRepositorio;
            /*20*/  _ConsultarTipoPlantaRepositorio = ConsultarTipoPlantaRepositorio;
            /*21*/  _ConsultarPlantasRepositorio = ConsultarPlantasRepositorio;
            /*22*/  _ConsultaTipoEmbarcacionesRepositorio = ConsultaTipoEmbarcacionesRepositorio;
            /*23*/  _TipoConsumoHumanoRepositorio = TipoConsumoHumanoRepositorio;
            /*24*/  _TupaRepositorio = TupaRepositorio;
            /*25*/  _FilialDhcpaRepositorio = FilialDhcpaRepositorio;
            /*26*/  _ConsultarTipoActividadPlantaRepositorio = ConsultarTipoActividadPlantaRepositorio;
            /*27*/  _ProtocoloRepositorio = ProtocoloRepositorio;
            /*28*/  _ServicioDhcpaRepositorio = ServicioDhcpaRepositorio;
            /*29*/  _TipoSeguimientoRepositorio = TipoSeguimientoRepositorio;
            /*30*/  _ConsultarCodHabEmbarcacionRepositorio = ConsultarCodHabEmbarcacionRepositorio;
            /*31*/  _ConsultarActvEmbarcacionRepositorio = ConsultarActvEmbarcacionRepositorio;
            /*32*/  _ConsultarOficinaDireccionLegalRepositorio = ConsultarOficinaDireccionLegalRepositorio;
            /*33*/  _ConsultarEmpresaPersonaLegalRepositorio = ConsultarEmpresaPersonaLegalRepositorio;
            /*34*/  _TipoProtocoloEmbarcacionRepositorio = TipoProtocoloEmbarcacionRepositorio;
            /*35*/  _ConsultarDbGeneralMaeAlmacenSedeRepositorio = ConsultarDbGeneralMaeAlmacenSedeRepositorio;
            /*36*/  _ConsultarActvAlmacenRepositorio = ConsultarActvAlmacenRepositorio;
            /*37*/  _ConsultarCodHabAlmacenRepositorio = ConsultarCodHabAlmacenRepositorio;
            /*38*/  _ConsultarDbGeneralMaeZonaProduccionRepositorio = ConsultarDbGeneralMaeZonaProduccionRepositorio;
            /*39*/  _ConsultarDbGeneralMaeAreaProduccionRepositorio = ConsultarDbGeneralMaeAreaProduccionRepositorio;
            /*40*/  _ConsultarDbGeneralMaeConcesionRepositorio = ConsultarDbGeneralMaeConcesionRepositorio;
            /*41*/  _ConsultarDbGeneralMaeTipoConcesionRepositorio = ConsultarDbGeneralMaeTipoConcesionRepositorio;
            /*42*/  _TipoActividadConcesionRepositorio = TipoActividadConcesionRepositorio;
            /*43*/  _DbGeneralMaeTipoDesembarcaderoRepositorio = DbGeneralMaeTipoDesembarcaderoRepositorio;
            /*44*/  _DbGeneralMaeCodigoDesembarcaderoRepositorio = DbGeneralMaeCodigoDesembarcaderoRepositorio;
            /*45*/  _DbGeneralMaeDesembarcaderoRepositorio = DbGeneralMaeDesembarcaderoRepositorio;
            /*46*/  _TipoPedidoSigaRepositorio = TipoPedidoSigaRepositorio;
            _DbGeneralMaeTransporteRepositorio = DbGeneralMaeTransporteRepositorio;
        _ProtocoloTransporteRepositorio = ProtocoloTransporteRepositorio;
            _DestinoSolicitudInspeccionRepositorio = DestinoSolicitudInspeccionRepositorio;
            _ConsultarDniPersonalLegalRepositorio = ConsultarDniPersonalLegalRepositorio;
            _ConsultarRucRepositorio = ConsultarRucRepositorio;
            _TipoTupaRepositorio = TipoTupaRepositorio;
            _ConsultaDbGeneralMaeFacturaRepositorio = ConsultaDbGeneralMaeFacturaRepositorio;
            _ConsultaReciboSerie1Repositorio = ConsultaReciboSerie1Repositorio;
            _ConsultaDbGeneralMaeTipoFacturaRepositorio = ConsultaDbGeneralMaeTipoFacturaRepositorio;
            _ConsultaDbGeneralMaeValorFacturadoExpRepositorio = ConsultaDbGeneralMaeValorFacturadoExpRepositorio;
            _ConsultaPersonaReciboSerie1Repositorio = ConsultaPersonaReciboSerie1Repositorio;
            _ConsultaDbGeneralMaeOperacionRepositorio = ConsultaDbGeneralMaeOperacionRepositorio;
            _ReporteComprobanteXMesConsultaRepositorio = ReporteComprobanteXMesConsultaRepositorio;
            _ConsultaReporteDiarioSerie1Repositorio = ConsultaReporteDiarioSerie1Repositorio;
            _ConsultarDireccionRepositorio = ConsultarDireccionRepositorio;
        }

        /*01*/
        public IEnumerable<TipoTramiteResponse> Recupera_tipo_tramite_todo()
        {
            var result = from zp in _TipoTramiteRepositorio.Listar()
                         select new TipoTramiteResponse
                         {
                             id_tipo_tramite = zp.ID_TIPO_TRAMITE,
                             nombre = zp.NOMBRE
                         };

            return result.ToList();
        }
        /*02*/
        public IEnumerable<TipoExpedienteResponse> Recupera_tipo_expediente()
        {
            var result = from zp in _TipoExpedienteRepositorio.Listar()
                         select new TipoExpedienteResponse
                         {
                             id_tipo_expediente = zp.ID_TIPO_EXPEDIENTE,
                             nombre = zp.NOMBRE
                         };

            return result.ToList();
        }
        /*03*/
        public IEnumerable<ExpedientesResponse> Recupera_expedientes()
        {
            return _HojaTramiteRepositorio.GetallRecupera_expediente();
        }
        
        /*04*/
        public IEnumerable<ConsultarOficinaResponse> Recupera_oficina_todo()
        {
            var result = (from zp in _ConsultarOficinaRepositorio.Listar()
                         select new ConsultarOficinaResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nombre = zp.NOMBRE,
                             id_ofi_padre = zp.ID_OFI_PADRE,
                             siglas = zp.SIGLAS,
                             ruc = zp.RUC
                         }).OrderBy(r => r.nombre);

            return result.ToList();
        }
        /*05*/
        public IEnumerable<ConsultarOficinaResponse> Recupera_oficina_todo_x_bus(string nombre)
        {
            var result = (from zp in _ConsultarOficinaRepositorio.Listar()
                         where zp.NOMBRE.Contains(nombre) && zp.RUC!="20565429656"
                         select new ConsultarOficinaResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nombre = zp.NOMBRE,
                             id_ofi_padre = zp.ID_OFI_PADRE,
                             siglas = zp.SIGLAS,
                             ruc = zp.RUC
                         }).OrderBy(r => r.nombre);

            return result.ToList();
        }
        /*06*/
        public IEnumerable<ConsultarUsuarioResponse> Consulta_Usuario(string ruc, string persona_num_documento)
        {

            var result = (from zp in _ConsultarUsuarioRepositorio.Listar(x => x.persona_num_documento == persona_num_documento && x.RUC == ruc)
                         select new ConsultarUsuarioResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nom_ofi = zp.NOM_OFI,
                             id_oficina_direccion = zp.ID_OFICINA_DIRECCION,
                             id_sede = zp.ID_SEDE,
                             nom_sede = zp.NOM_SEDE,
                             persona = zp.persona
                         }).AsEnumerable();

            return result;
        }
        /*07*/
        public IEnumerable<ConsultarUsuarioResponse> Recupera_oficina_dni_y_sede(string persona_num_documento, int id_sede)
        {

            var result = from zp in _ConsultarUsuarioRepositorio.Listar(x => x.persona_num_documento == persona_num_documento && x.ID_SEDE == id_sede)
                         select new ConsultarUsuarioResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nom_ofi = zp.NOM_OFI,
                             id_oficina_direccion = zp.ID_OFICINA_DIRECCION,
                             id_sede = zp.ID_SEDE,
                             nom_sede = zp.NOM_SEDE,
                             persona = zp.persona
                         };

            return result.ToList();
        }
        /*08*/
        public IEnumerable<ConsultarOficinaResponse> Recupera_oficina_all_x_sede(int sede)
        {
            return _HojaTramiteRepositorio.GetallOficina_x_sede(sede);
        }
        /*09*/
        public IEnumerable<ConsultarOficinaResponse> Recupera_oficina_all_x_ruc(string ruc)
        {
            var result = (from zp in _ConsultarOficinaRepositorio.Listar(x => x.RUC.Contains(ruc))
                         select new ConsultarOficinaResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nombre = zp.NOMBRE,
                             id_ofi_padre = zp.ID_OFI_PADRE,
                             siglas = zp.SIGLAS,
                             ruc = zp.RUC
                         }).OrderBy(r => r.nombre);
            return result.ToList();
        }


        /*10*/
        public ConsultarSedeOficinaResponse Recupera_sede_x_id_ofi_dir(int id_ofi_dir)
        {

            int var_id_sed = _ConsultarDireccionRepositorio.Listar(x => x.ID_OFICINA_DIRECCION == id_ofi_dir).First().ID_SEDE;

            var result = (from zp in _ConsultarSedeOficinaRepositorio.Listar(x => x.ACTIVO == true && x.ID_SEDE == var_id_sed)
                          select new ConsultarSedeOficinaResponse
                          {
                              id_sede = zp.ID_SEDE,
                              nombre = zp.NOMBRE,
                              direccion = zp.DIRECCION,
                              referencia = zp.REFERENCIA
                          }).First();
            return result;
        }

        /*10*/
        public IEnumerable<ConsultarSedeOficinaResponse> Recupera_sede_all(int id_oficina)
        {
            var result = (from zp in _ConsultarSedeOficinaRepositorio.Listar(x => x.ACTIVO == true && x.ID_OFICINA == id_oficina)
                         select new ConsultarSedeOficinaResponse
                         {
                             id_sede = zp.ID_SEDE,
                             nombre = zp.NOMBRE,
                             direccion = zp.DIRECCION,
                             referencia = zp.REFERENCIA
                         }).OrderBy(r => r.nombre);
            return result.ToList();
        }
        /*11*/
        public IEnumerable<TipoDocumentoResponse> Consulta_Tipo_Documento(int id)
        {
            var result = (from zp in _TipoDocumentoRepositorio.Listar(x => x.ID_TIPO_DOCUMENTO==id)
                         select new TipoDocumentoResponse
                         {
                             id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                             nombre = zp.NOMBRE,
                             tp_e_i = zp.TP_E_I
                         }).OrderBy(r => r.nombre);

            return result.ToList();
        }
        /*12*/
        public IEnumerable<TipoDocumentoResponse> Recupera_tipo_documento_todo(string tipo_e_i = "", string tipo_e_i_2="")
        {
            if (tipo_e_i_2 == "0")
            {
                var result = (from zp in _TipoDocumentoRepositorio.Listar(x => x.TP_E_I.Trim() != tipo_e_i)
                              select new TipoDocumentoResponse
                              {
                                  id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                                  nombre = zp.NOMBRE,
                                  tp_e_i = zp.TP_E_I
                              }).OrderBy(r => r.nombre);

                return result.ToList();
            }
            else
            {
                var result = (from zp in _TipoDocumentoRepositorio.Listar(x => x.TP_E_I.Trim() == tipo_e_i)
                              select new TipoDocumentoResponse
                              {
                                  id_tipo_documento = zp.ID_TIPO_DOCUMENTO,
                                  nombre = zp.NOMBRE,
                                  tp_e_i = zp.TP_E_I
                              }).OrderBy(r => r.nombre);

                return result.ToList();
            }
        }
        /*13*/
        public IEnumerable<EstadoTramiteResponse> Recupera_estado_tramite_todo()
        {
            var result = from zp in _EstadoTramiteRepositorio.Listar()
                         select new EstadoTramiteResponse
                         {
                             id_est_tramite= zp.ID_EST_TRAMITE,
                             nombre = zp.NOMBRE
                         };

            return result.ToList();
        }
        /*14*/
        public IEnumerable<ConsultarPersonalResponse> Recupera_personal_todo()
        {
            var result = from zp in _ConsultarPersonalRepositorio.Listar(x => x.ACTIVO == true)
                         select new ConsultarPersonalResponse
                         {
                             id_per_empresa = zp.ID_PER_EMPRESA,
                             persona_num_documento = zp.persona_num_documento,
                             id_oficina = zp.ID_OFICINA
                         };

            return result.ToList();
        }
        /*15*/
        public IEnumerable<ConsultarPersonalResponse> Recupera_personal_oficina(int id_oficina)
        {
            var result = (from zp in _ConsultarPersonalRepositorio.Listar(x => x.ACTIVO == true && x.ID_OFICINA_DIRECCION==id_oficina)
                         select new ConsultarPersonalResponse
                         {
                             persona_num_documento = zp.persona_num_documento,
                             nom_persona = zp.NOM_PERSONA
                         }).Distinct().OrderBy(r => r.nom_persona);

            return result.ToList();
        }
        /*16*/
        public IEnumerable<ConsultarDepartamentoResponse> llenar_departamento()
        {
            var result = (from zp in _ConsultarDepartamentoRepositorio.Listar()
                          select new ConsultarDepartamentoResponse
                          {
                              codigo_departamento = zp.CODIGO_DEPARTAMENTO,
                              departamento = zp.DEPARTAMENTO
                          }).Distinct().OrderBy(r => r.departamento);

            return result.ToList();
        }
        /*17*/
        public IEnumerable<ConsultarProvinciaResponse> llenar_provincia_x_departamento(string id_departamento)
        {
            var result = (from zp in _ConsultarProvinciaRepositorio.Listar(x => x.CODIGO_DEPARTAMENTO==id_departamento)
                          select new ConsultarProvinciaResponse
                          {
                              codigo_provincia = zp.CODIGO_PROVINCIA,
                              provincia = zp.PROVINCIA
                          }).Distinct().OrderBy(r => r.provincia);

            return result.ToList();
        }
        /*18*/
        public IEnumerable<ConsultarUbigeoResponse> llenar_distrito_x_provincia(string id_provincia)
        {
            var result = (from zp in _ConsultarUbigeoRepositorio.Listar(x => x.UBIGEO.Substring(0, 4) == id_provincia)
                          select new ConsultarUbigeoResponse
                          {
                              ubigeo = zp.UBIGEO,
                              distrito = zp.DISTRITO
                          }).Distinct().OrderBy(r => r.distrito);

            return result.ToList();
        }
        /*19*/
        public IEnumerable<ConsultarTipoDocumentoIdentidadResponse> llenar_tipo_documento_identidad()
        {
            var result = (from zp in _ConsultarTipoDocumentoIdentidadRepositorio.Listar()
                          select new ConsultarTipoDocumentoIdentidadResponse
                          {
                              tipo_doc_iden = zp.TIPO_DOC_IDEN,
                              nombre = zp.NOMBRE,
                              siglas = zp.SIGLAS
                          }).Distinct().OrderBy(r => r.nombre);

            return result.ToList();
        }
        
        /*19*/
        public IEnumerable<TipoPedidoSigaResponse> llenar_tipo_pedido_siga()
        {
            var result = (from zp in _TipoPedidoSigaRepositorio.Listar()
                          where zp.ACTIVO=="1"
                          select new TipoPedidoSigaResponse
                          {
                              id_tipo_pedido_siga = zp.ID_TIPO_PEDIDO_SIGA,
                              nombre = zp.NOMBRE
                          }).Distinct().OrderBy(r => r.nombre);
            return result.ToList();
        }

        /*20*/
        public IEnumerable<ExpedientesResponse> llenar_expediente(string indicador)
        {
            var result = (from zp in _ExpedientesRepositorio.Listar(x => x.INDICADOR_SEGUIMIENTO.Contains(indicador))
                          select new ExpedientesResponse
                          {
                              id_expediente = zp.ID_EXPEDIENTE,
                              numero_expediente = zp.NUMERO_EXPEDIENTE,
                              id_tipo_expediente = zp.ID_TIPO_EXPEDIENTE,
                              nom_expediente = zp.NOM_EXPEDIENTE,
                              año_crea = zp.AÑO_CREA
                          }).Distinct().OrderBy(r => r.numero_expediente);
            return result.ToList();
        
        }
        
        /*20*/
        public IEnumerable<ConsultaPersonaReciboSerie1Response> lista_personareciboserie1_sin_direc(string documento)
        {
            var result = (from zp in _ConsultaPersonaReciboSerie1Repositorio.Listar().Where(x => x.DOCUMENTO.Contains(documento))
                          select new ConsultaPersonaReciboSerie1Response
                          {
                              documento = zp.DOCUMENTO,
                              nombre = zp.NOMBRE
                          });
            return result.ToList().Distinct();

        }

        
        /*20*/
        public IEnumerable<ConsultaPersonaReciboSerie1Response> lista_direc_personareciboserie1(string documento)
        {
            var result = (from zp in _ConsultaPersonaReciboSerie1Repositorio.Listar().Where(x => x.DOCUMENTO == documento)
                          select new ConsultaPersonaReciboSerie1Response
                          {
                              direccion = zp.DIRECCION
                          }).Distinct();
            return result.ToList();

        }

        
        public IEnumerable<SubTupaResponse> recuperatupa(decimal monto)
        {
            return _ExpedientesRepositorio.recuperatupa(monto);
        }

        /*20*/
        public IEnumerable<ConsultaDbGeneralMaeTipoFacturaResponse> lista_tipo_comprobante()
        {
            var result = (from zp in _ConsultaDbGeneralMaeTipoFacturaRepositorio.Listar()
                          select new ConsultaDbGeneralMaeTipoFacturaResponse
                          {
                              id_tipo_factura = zp.ID_TIPO_FACTURA,
                              nombre = zp.NOMBRE
                          });
            return result.ToList();
        }

        public IEnumerable<ConsultaDbGeneralMaeOperacionResponse> lista_operacion(int num_ope)
        {
            var result = (from zp in _ConsultaDbGeneralMaeOperacionRepositorio.Listar().Where(x => x.NUMERO == num_ope)
                          select new ConsultaDbGeneralMaeOperacionResponse
                          {
                              id_operacion = zp.ID_OPERACION,
                              fecha_deposito = zp.FECHA_DEPOSITO,
                              abono = zp.ABONO,
                              cargo =zp.CARGO,
                              oficina = zp.OFICINA,
                              factura = zp.FACTURA,
                              numero = zp.NUMERO
                          });
            return result.ToList();
        }


        public ConsultaDbGeneralMaeOperacionResponse lista_operacion_x_id(int id_operacion)
        {
            VW_CONSULTA_DB_GENERAL_MAE_OPERACION m_operacion = new VW_CONSULTA_DB_GENERAL_MAE_OPERACION();
            m_operacion = _ConsultaDbGeneralMaeOperacionRepositorio.ListarUno(x => x.ID_OPERACION == id_operacion);

            return EntidadToResponse.dbgeneraloperacion(m_operacion);
        }


        public IEnumerable<ConsultaDbGeneralMaeOperacionResponse> busca_operacion_x_num_x_fecha_oficina(int num_ope,DateTime fecha, int oficina)
        {
            var result = (from zp in _ConsultaDbGeneralMaeOperacionRepositorio.Listar().Where(x => x.NUMERO == num_ope && x.FECHA_DEPOSITO == fecha && x.OFICINA == oficina)
                          select new ConsultaDbGeneralMaeOperacionResponse
                          {
                              id_operacion = zp.ID_OPERACION,
                              fecha_deposito = zp.FECHA_DEPOSITO,
                              abono = zp.ABONO,
                              cargo = zp.CARGO,
                              oficina = zp.OFICINA,
                              factura = zp.FACTURA,
                              numero = zp.NUMERO
                          });
            return result.ToList();
        }
        /*20*/
        public IEnumerable<TipoExpedienteResponse> llenar_tipo_expediente(int id_tipo, int id_oficina_dir)
        {
            if (id_tipo == 0)
            {
                var result = (from zp in _TipoExpedienteRepositorio.Listar()
                              where zp.ID_OFICINA_DIR == id_oficina_dir
                              select new TipoExpedienteResponse
                              {
                                  id_tipo_expediente = zp.ID_TIPO_EXPEDIENTE,
                                  nombre = zp.NOMBRE
                              }).Distinct().OrderBy(r => r.nombre);
                return result.ToList();
            }
            else
            {
                var result = (from zp in _TipoExpedienteRepositorio.Listar(x => x.ID_TIPO_EXPEDIENTE==id_tipo)
                              select new TipoExpedienteResponse
                              {
                                  id_tipo_expediente = zp.ID_TIPO_EXPEDIENTE,
                                  nombre = zp.NOMBRE
                              }).Distinct().OrderBy(r => r.nombre);
                return result.ToList();
            }
            
        }

        /*22*/
        public IEnumerable<TipoProcedimientoResponse> llenar_tipo_procedimiento(int id_tipo_procedimiento)
        {
            
            if (id_tipo_procedimiento == 0)
            {
                var result = (from zp in _TipoProcedimientoRepositorio.Listar()
                              select new TipoProcedimientoResponse
                              {
                                  id_tipo_procedimiento = zp.ID_TIPO_PROCEDIMIENTO,
                                  nombre = zp.NOMBRE
                              }).Distinct().OrderBy(r => r.id_tipo_procedimiento);
                return result.ToList();
            }
            else
            {
                var result = (from zp in _TipoProcedimientoRepositorio.Listar(x => x.ID_TIPO_PROCEDIMIENTO==id_tipo_procedimiento)
                              select new TipoProcedimientoResponse
                              {
                                  id_tipo_procedimiento = zp.ID_TIPO_PROCEDIMIENTO,
                                  nombre = zp.NOMBRE
                              }).Distinct().OrderBy(r => r.id_tipo_procedimiento);
                return result.ToList();
            }
        }
        
        public IEnumerable<ConsultarCodHabEmbarcacionResponse> llenar_codigo_embarcacion()
        {

            var result = (from zp in _ConsultarCodHabEmbarcacionRepositorio.Listar()
                          where zp.ACTIVO=="1"
                              select new ConsultarCodHabEmbarcacionResponse
                              {
                                  id_cod_hab_emb = zp.ID_COD_HAB_EMB,
                                  codigo = zp.CODIGO
                              }).Distinct().OrderBy(r => r.codigo);
                return result.ToList();
        }
        
        public IEnumerable<ConsultarActvEmbarcacionResponse> llenar_actividad_embarcacion()
        {
            var result = (from zp in _ConsultarActvEmbarcacionRepositorio.Listar()
                          where zp.ACTIVO == "1"
                          select new ConsultarActvEmbarcacionResponse
                          {
                              id_tipo_act_emb = zp.ID_TIPO_ACT_EMB,
                              nombre = zp.NOMBRE
                          }).Distinct().OrderBy(r => r.nombre);
            return result.ToList();
        }
        /*23*/
        public IEnumerable<ConsultaEmbarcacionesResponse> listar_embarcaciones(string matricula)
        {
            var result = (from zp in _ConsultaEmbarcacionesRepositorio.Listar(x => x.MATRICULA.Contains(matricula))
                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = zp.ID_EMBARCACION,
                              matricula = zp.MATRICULA,
                              nombre = zp.NOMBRE,
                          }).Distinct().OrderBy(r => r.matricula);
            return result.ToList();
        }

        
        /*24*/
        public ConsultaReciboSerie1Response lista_recibo(int id)
        {
            var result = (from zp in _ConsultaReciboSerie1Repositorio.Listar(x => x.ID_FACTURA==id)
                          select new ConsultaReciboSerie1Response
                          {
                              id_factura = zp.ID_FACTURA,
                              nombre = zp.NOMBRE,
                              direccion = zp.DIRECCION,
                              ruc_dni = zp.RUC_DNI,
                              fecha_emision = zp.FECHA_EMISION,
                              dia = zp.DIA,
                              año = zp.AÑO,
                              mes_text = zp.MES_TEXT,
                              letra_importe = zp.LETRA_IMPORTE,
                              importe_total = zp.IMPORTE_TOTAL,
                              decimal_text = zp.DECIMAL_TEXT,
                              nombre_vfe = zp.NOMBRE_VFE,
                              num_fact = zp.NUM_FACT,
                              operacion = zp.OPERACION,
                              fecha_operacion = zp.FECHA_OPERACION,
                              tupa_serv = zp.TUPA_SERV,
                              cantidad = zp.CANTIDAD
                          }).Distinct().First();
            return result;
        }
        /*24*/
        public IEnumerable<ConsultaFacturasResponse> listar_factura()
        {
            var result = (from zp in _ConsultaFacturasRepositorio.Listar()
                          select new ConsultaFacturasResponse
                          {
                              id_factura = zp.ID_FACTURA,
                              num1_fact = zp.NUM1_FACT,
                              num2_fact = zp.NUM2_FACT,
                              importe_total = zp.IMPORTE_TOTAL,
                              fecha_fact = zp.FECHA_FACT
                          }).Distinct().OrderBy(r => r.num1_fact).OrderBy(x => x.num2_fact);
            return result.ToList();
        }
        
        /*24*/
        public int ultimo_numero_comprobante(int id_tipo_comprobante)
        {
            var result = (from zp in _ConsultaFacturasRepositorio.Listar().Where(x => x.ID_TIPO_FACTURA == id_tipo_comprobante)
                          select new ConsultaFacturasResponse
                          {    
                              id_factura = zp.ID_FACTURA,
                              num2_fact = zp.NUM2_FACT
                          }).Distinct().OrderByDescending(r => r.id_factura);
            return result.ToList().First().num2_fact ?? 0;
        }
        /*25*/

        public bool Guardar_Embarcacion(string matricula, string nombre, int id_tipo_embarcacion, string usuario, int codigo_hab, int num_cod_hab, string nom_cod_hab, int id_tipo_act_emb, string fecha_const)
        {

            try
            {
                string id_embarcacion = _ConsultaEmbarcacionesRepositorio.Guarda_Embarcacion(matricula,nombre,id_tipo_embarcacion,usuario,codigo_hab,num_cod_hab,nom_cod_hab,id_tipo_act_emb, fecha_const).First().id_embarcacion.ToString();
            }
            catch
            {
                throw new InvalidOperationException();
            }
            return true;
        }
         
        /*26*/ 
        public Response.ConsultaFacturasResponse Guardar_Factura(string num1, string num2, DateTime fecha, decimal importe_total, string usuario, int id_tipo_factura, string ruc_dni, string nombre, string direccion, int id_sub_tupa, int cantidad, int id_ofi_crea)
        {
            return _ConsultaFacturasRepositorio.Guardar_Factura(num1, num2, fecha, importe_total, usuario, id_tipo_factura, ruc_dni, nombre, direccion, id_sub_tupa,cantidad,id_ofi_crea);
        }
        
        public Response.P_INSERT_UPDATE_DAT_DET_OPERACION_FACTURA_Result Guardar_det_fac_opera(int id_factura, int id_operacion)
        {
            return _ConsultaFacturasRepositorio.Guardar_det_fac_opera(id_factura, id_operacion);
        }

        public Response.P_INSERT_UPDATE_MAE_OPERACION_Result Guardar_Operacion(int numero, DateTime fecha, int oficina, decimal abono,string usuario) 
        {
            return _ConsultaFacturasRepositorio.Guardar_Operacion(numero, fecha, oficina, abono, usuario);
        }


        public void update_db_general_mae_operacion(ConsultaDbGeneralMaeOperacionResponse ope_rq)
        {
            _ConsultaFacturasRepositorio.update_db_general_mae_operacion(ope_rq);
        }

        /*27*/
        public int Guardar_Expediente(ExpedientesRequest request)
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
        
        /*28*/
        public IEnumerable<ConsultaEmbarcacionesResponse> GetAllEmbarcaciones_sin_paginado(string matricula, string nombre, int cmb_actividad)
        {
            return _ConsultaEmbarcacionesRepositorio.GetAllEmbarcaciones_sin_paginado(matricula, nombre, cmb_actividad);
        }
        
        /*30*/
        public int buscar_embarcacion(string matricula)
        {
            var result = (from zp in _ConsultaEmbarcacionesRepositorio.Listar(x => x.MATRICULA == matricula)

                          select new ConsultaEmbarcacionesResponse
                          {
                              id_embarcacion = zp.ID_EMBARCACION,
                              matricula = zp.MATRICULA,
                              nombre = zp.NOMBRE,
                              activo = zp.ACTIVO
                          }).OrderByDescending(r => r.id_embarcacion).AsEnumerable();
            return result.Count();
        }

        public IEnumerable<Response.SP_EDITA_DB_GENERAL_MAE_SEDE_Result> Edita_db_general_mae_sede(int id_sede, string direccion, string ubigeo, string sede, string referencia)
        {
            return _ConsultarOficinaRepositorio.Edita_db_general_mae_sede(id_sede, direccion, ubigeo, sede, referencia);
        }

        /*18*/
        public IEnumerable<ConsultarOficinaDireccionLegalResponse> GetAllDireccionLegal_x_ruc(string RUC)
        {
            var result = from zp in _ConsultarOficinaDireccionLegalRepositorio.Listar(x => x.RUC == RUC)
                         select new ConsultarOficinaDireccionLegalResponse
                         {                            
                             id_oficina_direccion_legal = zp.ID_OFICINA_DIRECCION_LEGAL,
                             id_sede = zp.ID_SEDE,
                             direccion = zp.DIRECCION,
                             ubigeo = zp.UBIGEO,
                             activo = zp.ACTIVO,
                             fecha_desactivado = zp.FECHA_DESACTIVADO,
                             fecha_registro = zp.FECHA_REGISTRO,
                             id_ubigeo = zp.ID_UBIGEO,
                             nom_direccion = zp.NOM_DIRECCION,
                             nom_referencia = zp.NOM_REFERENCIA,
                             nom_sede = zp.NOM_SEDE
                         };
            return result.ToList().OrderByDescending(x => x.activo);
        }
        public IEnumerable<ConsultarEmpresaPersonaLegalResponse> GetAllPersonaLegal_x_ruc(string RUC)
        {
            var result = from zp in _ConsultarEmpresaPersonaLegalRepositorio.Listar(x => x.RUC == RUC)
                         select new ConsultarEmpresaPersonaLegalResponse
                         {
                             id_persona_legal = zp.ID_PERSONA_LEGAL,
                             activo = zp.ACTIVO,
                             documento = zp.DOCUMENTO,
                             nombres_y_apellidos = zp.NOMBRES_Y_APELLIDOS,
                             telefono = zp.TELEFONO,
                             correo = zp.CORREO,
                             fecha_desactivado = zp.FECHA_DESACTIVADO,
                             fecha_registro = zp.FECHA_REGISTRO
                         };
            return result.ToList().OrderByDescending(x => x.activo);
        }
        
        public IEnumerable<ConsultarDniPersonalLegalResponse> GetAllPersonaLegal_x_dni(string DNI)
        {
            var result = from zp in _ConsultarDniPersonalLegalRepositorio.Listar(x => x.DNI == DNI)
                         select new ConsultarDniPersonalLegalResponse
                         {
                             id_dni_persona_legal = zp.ID_DNI_PERSONA_LEGAL,
                             activo = zp.ACTIVO,
                             documento = zp.DOCUMENTO,
                             nombres_y_apellidos = zp.NOMBRES_Y_APELLIDOS,
                             telefono = zp.TELEFONO,
                             correo = zp.CORREO,
                             fecha_desactivado = zp.FECHA_DESACTIVADO,
                             fecha_registro = zp.FECHA_REGISTRO
                         };
            return result.ToList().OrderByDescending(x => x.activo);
        }

        public IEnumerable<ConsultaDbGeneralMaeOperacionResponse> Lista_todo_operacion(string operacion, string factura)
        {
            if(factura.Trim()!="" && factura != null)
            {
                var result = from zp in _ConsultaDbGeneralMaeOperacionRepositorio.Listar(x => x.NUMERO.ToString().Contains(operacion) && x.FACTURA.Contains(factura))
                             select new ConsultaDbGeneralMaeOperacionResponse
                             {
                                 id_operacion = zp.ID_OPERACION,
                                 fecha_deposito = zp.FECHA_DEPOSITO,
                                 abono = zp.ABONO,
                                 cargo = zp.CARGO,
                                 oficina = zp.OFICINA,
                                 factura = zp.FACTURA,
                                 numero = zp.NUMERO
                             };
                return result.ToList().OrderByDescending(x => x.fecha_deposito).AsEnumerable();
            }
            else
            {
                var result = from zp in _ConsultaDbGeneralMaeOperacionRepositorio.Listar(x => x.NUMERO.ToString().Contains(operacion))
                             select new ConsultaDbGeneralMaeOperacionResponse
                             {
                                 id_operacion = zp.ID_OPERACION,
                                 fecha_deposito = zp.FECHA_DEPOSITO,
                                 abono = zp.ABONO,
                                 cargo = zp.CARGO,
                                 oficina = zp.OFICINA,
                                 factura = zp.FACTURA,
                                 numero = zp.NUMERO
                             };
                return result.ToList().OrderByDescending(x => x.fecha_deposito).AsEnumerable();
            }
        }

        /*31*/
        public IEnumerable<ConsultaDbGeneralMaeFacturaResponse> GetAllFacturas(string comprobante, string tipo_comprobante, string documento, string externo,string operac) 
        {
                var result = (from zp in _ConsultaDbGeneralMaeFacturaRepositorio.Listar(x => x.COMPROBANTE.Contains(comprobante) && x.TIPO_FACTURA.Contains(tipo_comprobante) && x.DOCUMENTO.Contains(documento) && x.EXTERNO.Contains(externo) && x.OPERACIONES.Contains(operac))

                              select new ConsultaDbGeneralMaeFacturaResponse
                              {
                                  id_factura = zp.ID_FACTURA,
                                  fecha = zp.FECHA,
                                  fecha_text = zp.FECHA_TEXT,
                                  comprobante = zp.COMPROBANTE,
                                  importe = zp.IMPORTE,
                                  tupa_serv = zp.TUPA_SERV,
                                  valor_fact_exp = zp.VALOR_FACT_EXP,
                                  documento = zp.DOCUMENTO,
                                  externo = zp.EXTERNO,
                                  direccion = zp.DIRECCION,
                                  id_operacion = zp.ID_OPERACION,
                                  operaciones = zp.OPERACIONES,
                                  tipo_factura = zp.TIPO_FACTURA,
                                  usuario_registro = zp.USUARIO_REGISTRO,
                                  ruta_pdf = zp.RUTA_PDF
                                  
                              }).OrderByDescending(r => r.id_factura).Take(500).AsEnumerable();
                return result;
            
        }
        
        /*31*/
        public IEnumerable<ConsultaReporteDiarioSerie1Response> recupera_reporte_diario_serie1(int fecha)
        {
            var result = (from zp in _ConsultaReporteDiarioSerie1Repositorio.Listar(x => x.FECHA_NUM == fecha.ToString())

                          select new ConsultaReporteDiarioSerie1Response
                          {
                              num_fact = zp.NUM_FACT,
                              fecha = zp.FECHA,
                              documento = zp.DOCUMENTO,
                              datos = zp.DATOS,
                              concepto = zp.CONCEPTO,
                              expediente = zp.EXPEDIENTE,
                              cantidad = zp.CANTIDAD,
                              tupa = zp.TUPA,
                              importe_total = zp.IMPORTE_TOTAL,
                              operacion = zp.OPERACION,
                              fecha_operacion = zp.FECHA_OPERACION,
                              id_factura = zp.ID_FACTURA,
                              id_tipo_factura = zp.ID_TIPO_FACTURA,
                              fecha_num = zp.FECHA_NUM,
                              n_oficina_crea = zp.N_OFICINA_CREA
                          }).OrderBy(x => x.id_factura).AsEnumerable();
            return result;
        }
        /*31*/
        public IEnumerable<ReporteComprobanteXMesConsultaResponse> GetAllComprobantes_x_fecha(int fecha)
        {
            _ConsultaFacturasRepositorio.genera_reporte_comprobante_x_mes();

            var result = (from zp in _ReporteComprobanteXMesConsultaRepositorio.Listar(x => x.FECHA == fecha)

                          select new ReporteComprobanteXMesConsultaResponse
                          {
                              mes = zp.MES,
                              dia_semana = zp.DIA_SEMANA,
                              dia_num = zp.DIA_NUM,
                              mes_num = zp.MES_NUM,
                              anio_num = zp.ANIO_NUM,
                              fecha = zp.FECHA,
                              venta_cert = zp.VENTA_CERT,
                              compr_cert = zp.COMPR_CERT,
                              venta_prot = zp.VENTA_PROT,
                              compr_prot =zp.COMPR_PROT,
                              venta_ensayo = zp.VENTA_ENSAYO,
                              compr_ensayo = zp.COMPR_ENSAYO,
                              acceso_info = zp.ACCESO_INFO,
                              compr_ainfo = zp.COMPR_AINFO,
                              total = zp.TOTAL
                          }).OrderBy(x => x.fecha).AsEnumerable();
            return result;

        }
        /*31*/
        public IEnumerable<ReporteComprobanteXMesConsultaResponse> GetAllComprobantes_x_mes(int mes, int anio) 
        {
            _ConsultaFacturasRepositorio.genera_reporte_comprobante_x_mes();

            var result = (from zp in _ReporteComprobanteXMesConsultaRepositorio.Listar(x => x.MES_NUM == mes && x.ANIO_NUM == anio)

                          select new ReporteComprobanteXMesConsultaResponse
                          {
                              mes = zp.MES,
                              dia_semana = zp.DIA_SEMANA,
                              dia_num = zp.DIA_NUM,
                              mes_num = zp.MES_NUM,
                              anio_num = zp.ANIO_NUM,
                              fecha = zp.FECHA,
                              venta_cert = zp.VENTA_CERT,
                              venta_prot = zp.VENTA_PROT,
                              venta_ensayo = zp.VENTA_ENSAYO,
                              acceso_info = zp.ACCESO_INFO,
                              total = zp.TOTAL
                          }).OrderBy(x => x.fecha).AsEnumerable();
            return result;

        }

        /*33*/
        public int buscar_factura(int num1, int num2)
        {
            var result = (from zp in _ConsultaFacturasRepositorio.Listar(x => x.NUM1_FACT == num1 && x.NUM2_FACT==num2)

                          select new ConsultaFacturasResponse
                          {
                              id_factura = zp.ID_FACTURA,
                              num1_fact = zp.NUM1_FACT,
                              num2_fact = zp.NUM2_FACT,
                              importe_total = zp.IMPORTE_TOTAL
                          }).OrderBy(r => r.num2_fact).AsEnumerable();

            return result.Count();
        }
        /*34*/
        public int buscar_persona(string persona_num_documento)
        {
            var result = (from zp in _ConsultarDniRepositorio.Listar(x => x.persona_num_documento == persona_num_documento)

                          select new ConsultarDniResponse
                          {
                              persona_num_documento = zp.persona_num_documento,
                              nombres = zp.nombres,
                              paterno = zp.paterno,
                              materno = zp.materno
                          }).OrderBy(r => r.paterno).AsEnumerable();

            return result.Count();
        }

        /*34*/
        public ConsultarDniRequest buscar_persona_resp(string persona_num_documento)
        {
            ConsultarDniRequest per_res = new ConsultarDniRequest();

            var result = (from zp in _ConsultarDniRepositorio.Listar(x => x.persona_num_documento == persona_num_documento)

                          select new ConsultarDniRequest
                          {
                              persona_num_documento = zp.persona_num_documento,
                              nombres = zp.nombres,
                              paterno = zp.paterno,
                              materno = zp.materno
                          }).OrderBy(r => r.paterno).AsEnumerable();
            if (result.Count() > 0)
            {
                per_res = result.First();
            }
            return per_res;
        }
                
        /*35*/
        public IEnumerable<ExpedientesResponse> GetAllExpediente_sin_paginado(string numero_exp, int id_oficina_dir, string usuario)
        {
            return _ExpedientesRepositorio.GetAllExpediente_sin_paginado(numero_exp, id_oficina_dir, usuario);
        }
        
        /*37*/
        public int buscar_expediente(int numero_exp, int id_tipo_expediente, int año_crea)
        {
            var result = (from zp in _ExpedientesRepositorio.Listar(x => x.NUMERO_EXPEDIENTE == numero_exp && x.ID_TIPO_EXPEDIENTE==id_tipo_expediente && x.AÑO_CREA==año_crea)

                          select new ExpedientesResponse
                          {
                              id_expediente = zp.ID_EXPEDIENTE
                          }).OrderBy(r => r.id_expediente).AsEnumerable();
            return result.Count();
        }
        /*38*/
        public IEnumerable<ConsultarTipoPlantaResponse> recupera_tipo_planta()
        {
            var result = (from zp in _ConsultarTipoPlantaRepositorio.Listar()

                          select new ConsultarTipoPlantaResponse
                          {
                              id_tipo_planta = zp.ID_TIPO_PLANTA,
                              siglas=zp.SIGLAS,
                              nombre = zp.NOMBRE
                          }).OrderBy(r => r.id_tipo_planta).AsEnumerable();
            return result;
        }
        /*39*/
        public bool Guardar_Planta(int id_sede, int id_tipo_planta, int numero, string nombre_planta, int id_tipo_actividad, int filial, string usuario)
        {

            try
            {
                string id_planta = _ConsultarPlantasRepositorio.Guarda_Plantas(id_sede,id_tipo_planta,numero,nombre_planta,id_tipo_actividad,filial, usuario).First().id_planta.ToString();
            }
            catch
            {
                throw new InvalidOperationException();
            }
            return true;
        }
         
        
        /*40*/
        public IEnumerable<ConsultarPlantasResponse> GetAllPlantas_sin_paginado(string id_tipo_planta, string var_numero, string var_nombre, int var_id_filial, int var_id_actividad, string var_entidad)
        {
            return _ConsultarPlantasRepositorio.GetAllPlantas_sin_paginado(id_tipo_planta, var_numero, var_nombre, var_id_filial, var_id_actividad, var_entidad);
        }
        /*42*/
        public IEnumerable<ConsultaTipoEmbarcacionesResponse> recupera_tipo_embarcacion(int id_tipo_embarcacion)
        {
            var result = from zp in _ConsultaTipoEmbarcacionesRepositorio.Listar(x => (id_tipo_embarcacion == 0 || (id_tipo_embarcacion != 0 && x.ID_TIPO_EMBARCACION==id_tipo_embarcacion)))
                         select new ConsultaTipoEmbarcacionesResponse
            {
                id_tipo_embarcacion= zp.ID_TIPO_EMBARCACION,
                ruta_ftp = zp.RUTA_FTP,
                nombre = zp.NOMBRE
            };

            return result;
        }
        /*43*/
        public IEnumerable<TipoConsumoHumanoResponse> recupera_tipo_consumo()
        {
            var result = from zp in _TipoConsumoHumanoRepositorio.Listar()
                         select new TipoConsumoHumanoResponse
                         {
                             id_tipo_ch = zp.ID_TIPO_CH,
                             siglas = zp.SIGLAS,
                             nombre = zp.NOMBRE
                         };
            return result;
        }
        /*44*/
        public IEnumerable<TupaResponse> recupera_tupa()
        {
            var result = (from zp in _TupaRepositorio.Listar(x => x.ACTIVO == "1")
                         select new TupaResponse
                         {
                             id_tupa = zp.ID_TUPA,
                             numero = zp.NUMERO,
                             id_tipo_procedimiento = zp.ID_TIPO_PROCEDIMIENTO,
                             asunto = zp.ASUNTO,
                             dias_tupa = zp.DIAS_TUPA,
                             id_oficina = zp.ID_OFICINA,
                             id_tipo_tupa = zp.ID_TIPO_TUPA
                         }).OrderBy(x => x.id_tipo_tupa).ThenBy(y => y.numero);
            return result;
        }


        public IEnumerable<TipoTupaResponse> recupera_tipo_tupa()
        {
            var result = (from zp in _TipoTupaRepositorio.Listar()
                          select new TipoTupaResponse
                          {
                              id_tipo_tupa = zp.ID_TIPO_TUPA,
                              nombre = zp.NOMBRE
                          });
            return result;
        }
        public IEnumerable<DestinoSolicitudInspeccionResponse> recupera_destino_si()
        {
            var result = (from zp in _DestinoSolicitudInspeccionRepositorio.Listar(x => x.ACTIVO == "1")
                          select new DestinoSolicitudInspeccionResponse
                          {
                              id_dest_sol_ins = zp.ID_DEST_SOL_INS,
                              nombre = zp.NOMBRE
                          });
            return result;
        }
        /*44_A*/
        public IEnumerable<TipoSeguimientoResponse> recupera_tipo_seguimiento()
        {
            var result = from zp in _TipoSeguimientoRepositorio.Listar(x => x.ACTIVO == "1")
                         select new TipoSeguimientoResponse
                         {
                             id_tipo_seguimiento = zp.ID_TIPO_SEGUIMIENTO,
                             nombre = zp.NOMBRE
                         };
            return result;
        }
        /*45*/
        public IEnumerable<ConsultarPlantasResponse> recupera_planta_x_direccion(int id_direccion, string activo)
        {
            return _ConsultarPlantasRepositorio.Consulta_planta(id_direccion, activo);
        }
        /*46*/
        public IEnumerable<FilialDhcpaResponse> recupera_filial(string tipo_e_i)
        {
            var result = from zp in _FilialDhcpaRepositorio.Listar(x => x.TP_E_I.Contains(tipo_e_i))
                         select new FilialDhcpaResponse
                         {
                             id_filial=zp.ID_FILIAL,
                             nombre = zp.NOMBRE,
                             tp_e_i = zp.TP_E_I
                         };
            return result;
        }
        /*47*/
        public IEnumerable<ConsultarTipoActividadPlantaResponse> recupera_tipo_actividad_planta(int id_tipo_planta)
        {
            var result = from zp in _ConsultarTipoActividadPlantaRepositorio.Listar(x => x.ID_TIPO_PLANTA == id_tipo_planta)
                         select new ConsultarTipoActividadPlantaResponse
                         {
                             id_tipo_actividad = zp.ID_TIPO_ACTIVIDAD,
                             id_tipo_planta = zp.ID_TIPO_PLANTA,
                             nombre = zp.NOMBRE,
                             ruta_ftp = zp.RUTA_FTP
                         };
            return result;
        }
        
        /*48*/
        public ConsultarOficinaResponse recupera_oficina(int id_oficina_direccion)
        {
            int var_id_ofi = _ConsultarDireccionRepositorio.Listar(x => x.ID_OFICINA_DIRECCION == id_oficina_direccion).First().ID_OFICINA;

            var result = (from zp in _ConsultarOficinaRepositorio.Listar(x => x.ID_OFICINA == var_id_ofi)
                         select new ConsultarOficinaResponse
                         {
                             id_oficina = zp.ID_OFICINA,
                             nombre = zp.NOMBRE,
                             siglas= zp.SIGLAS
                         }).First();
            return result;
        }
        /*48*/
        public IEnumerable<ConsultarTipoActividadPlantaResponse> recupera_toda_tipo_actividad_planta()
        {
            var result = from zp in _ConsultarTipoActividadPlantaRepositorio.Listar()
                         select new ConsultarTipoActividadPlantaResponse
                         {
                             id_tipo_actividad = zp.ID_TIPO_ACTIVIDAD,
                             id_tipo_planta = zp.ID_TIPO_PLANTA,
                             nombre = zp.NOMBRE,
                             ruta_ftp = zp.RUTA_FTP
                         };
            return result;
        }
        /*48*/
        public ConsultarTipoActividadPlantaResponse recupera_toda_tipo_actividad_planta_x_id(int id_tipo_actividad_planta)
        {
            var result = from zp in _ConsultarTipoActividadPlantaRepositorio.Listar(x => x.ID_TIPO_ACTIVIDAD == id_tipo_actividad_planta)
                         select new ConsultarTipoActividadPlantaResponse
                         {
                             id_tipo_actividad = zp.ID_TIPO_ACTIVIDAD,
                             id_tipo_planta = zp.ID_TIPO_PLANTA,
                             nombre = zp.NOMBRE,
                             ruta_ftp = zp.RUTA_FTP
                         };
            return result.First();
        }
        /*49*/
        public IEnumerable<ProtocoloResponse> GetAllProtocolo_x_planta(int id_planta)
        {
            return _ProtocoloRepositorio.GetAllProtocolo_x_planta(id_planta);
        }
        
        /*51*/
        public ConsultarPlantasResponse recupera_planta_x_id(int id_planta)
        {
            return _ConsultarPlantasRepositorio.Recupera_Planta(0, id_planta);
        }
        
        /*52*/
        public IEnumerable<ServicioDhcpaResponse> llenar_servicio_dhcpa()
        {
            var result = from zp in _ServicioDhcpaRepositorio.Listar()
                         select new ServicioDhcpaResponse
                         {
                             id_servicio_dhcpa= zp.ID_SERVICIO_DHCPA,
                             nombre = zp.NOMBRE
                         };
            return result;
        }
        
        /*52*/
        public IEnumerable<TipoProtocoloEmbarcacionResponse> Lista_tipo_protocolo_emb()
        {
            var result = from zp in _TipoProtocoloEmbarcacionRepositorio.Listar()
                         select new TipoProtocoloEmbarcacionResponse
                         {
                             id_tip_pro_emb = zp.ID_TIP_PRO_EMB,
                             nombre = zp.NOMBRE
                         };
            return result;
        }
        /*28*/
        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> GetAllAlmacenes_sin_paginado(string CODIGO_ALMACEN, int ID_ACTIVIDAD_ALMACEN, int ID_FILIAL, string EXTERNO)
        {
            return _ConsultarDbGeneralMaeAlmacenSedeRepositorio.GetAllAlmacenes_sin_paginado(CODIGO_ALMACEN, ID_ACTIVIDAD_ALMACEN, ID_FILIAL, EXTERNO);

        }
        
        public IEnumerable<ConsultarActvAlmacenResponse> recupera_actividad_almacen()
        {
            var result = from zp in _ConsultarActvAlmacenRepositorio.Listar()
                         where zp.ACTIVO=="1"
                         select new ConsultarActvAlmacenResponse
                         {
                             id_actividad_almacen = zp.ID_ACTIVIDAD_ALMACEN,
                             nombre_actividad = zp.NOMBRE_ACTIVIDAD
                         };
            return result;
        }

        /*28*/
        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> Guarda_Almacen(int ID_ALMACEN, int ID_SEDE, int ID_CODIGO_ALMACEN, int NUM_ALMACEN, string NOM_ALMACEN, int ID_FILIAL, int ID_ACTIVIDAD_ALMACEN, string USUARIO)
        {
            return _ConsultarDbGeneralMaeAlmacenSedeRepositorio.Guarda_Almacen(ID_ALMACEN, ID_SEDE, ID_CODIGO_ALMACEN, NUM_ALMACEN, NOM_ALMACEN, ID_FILIAL, ID_ACTIVIDAD_ALMACEN, USUARIO);
        }
        
        public IEnumerable<ConsultarCodHabAlmacenResponse> recupera_codigo_almacen()
        {
            var result = from zp in _ConsultarCodHabAlmacenRepositorio.Listar()
                         where zp.ACTIVO == "1"
                         select new ConsultarCodHabAlmacenResponse
                         {
                             id_codigo_almacen = zp.ID_CODIGO_ALMACEN,
                             siglas = zp.SIGLAS
                         };
            return result;
        }

        /*23*/
        public IEnumerable<ConsultarDbGeneralMaeAlmacenSedeResponse> lista_almacen(string COD_ALMACEN, int var_id_oficina_dir)
        {
            return _ConsultarDbGeneralMaeAlmacenSedeRepositorio.lista_almacen(COD_ALMACEN, var_id_oficina_dir);
        }
        
        
        /*30*/
        public ConsultaEmbarcacionesResponse buscar_embarcacion_x_seguimiento(int id_seguimiento)
        {
            return _ConsultaEmbarcacionesRepositorio.buscar_embarcacion_x_seguimiento(id_seguimiento);
        }

        /*51*/
        public ConsultarDbGeneralMaeAlmacenSedeResponse recupera_almacen_x_id(int id_almacen)
        {
            return _ConsultarDbGeneralMaeAlmacenSedeRepositorio.recupera_almacen_x_id(id_almacen);
        }

        /*55*/
        public IEnumerable<ConsultarDbGeneralMaeZonaProduccionResponse> recupera_zona_produccion()
        {
            var result = from zp in _ConsultarDbGeneralMaeZonaProduccionRepositorio.Listar()
                         select new ConsultarDbGeneralMaeZonaProduccionResponse
                         {
                             id_zona_produccion = zp.ID_ZONA_PRODUCCION,
                             cod_zona_produccion = zp.COD_ZONA_PRODUCCION
                         };
            return result;
        }
        

        /*55*/
        public IEnumerable<ConsultarDbGeneralMaeZonaProduccionResponse> recupera_zona_produccion_x_ubigeo(string ubigeo)
        {
            var result = from zp in _ConsultarDbGeneralMaeZonaProduccionRepositorio.Listar()
                         where zp.UBIGEO==ubigeo
                         select new ConsultarDbGeneralMaeZonaProduccionResponse
                         {
                             id_zona_produccion = zp.ID_ZONA_PRODUCCION,
                             cod_zona_produccion = zp.COD_ZONA_PRODUCCION,
                             nombre = zp.NOMBRE
                         };
            return result;
        }
        /*55*/
        public IEnumerable<ConsultarDbGeneralMaeAreaProduccionResponse> recupera_area_produccion(int id_zona_produccion)
        {
            var result = from zp in _ConsultarDbGeneralMaeAreaProduccionRepositorio.Listar(x => x.ID_ZONA_PRODUCCION==id_zona_produccion)
                         select new ConsultarDbGeneralMaeAreaProduccionResponse
                         {
                             id_area_produccion = zp.ID_AREA_PRODUCCION,
                             cod_area_produccion = zp.COD_AREA_PRODUCCION,
                             nombre = zp.NOMBRE
                         };
            return result;
        }

        /*28*/
        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> GetAllconsecion_sin_paginado(int id_zona_produccion, int id_area_produccion, int id_tipo_concesion, string externo)
        {
            return _ConsultarDbGeneralMaeConcesionRepositorio.GetAllconsecion_sin_paginado(id_zona_produccion, id_area_produccion, id_tipo_concesion, externo);
        }
        
        /*55*/
        public IEnumerable<ConsultarDbGeneralMaeTipoConcesionResponse> recupera_tipo_concesion()
        {
            var result = from zp in _ConsultarDbGeneralMaeTipoConcesionRepositorio.Listar()
                         select new ConsultarDbGeneralMaeTipoConcesionResponse
                         {
                             id_tipo_concesion = zp.ID_TIPO_CONCESION,
                             nombre = zp.NOMBRE,
                             ruta_pdf = zp.RUTA_PDF
                         };
            return result;
        }


        /*55*/
        public IEnumerable<ConsultarOficinaResponse> recupera_entidad()
        {
            var result = (from zp in _ConsultarOficinaRepositorio.Listar()
                         where zp.ID_OFI_PADRE==null
                         select new ConsultarOficinaResponse
                         {
                             ruc = zp.RUC,
                             nombre = zp.NOMBRE
                         }).OrderBy(x => x.nombre);
            return result;
        }

        /*25*/

        public bool Guardar_Concesion(int ID_CONCESION, string RUC, string CODIGO_HABILITACION, string PARTIDA_REGISTRAL, string UBICACION, string UBIGEO, int ID_AREA_PRODUCCION, int ID_TIPO_CONCESION, int ID_TIPO_ACTIVIDAD_CONCESION, string USUARIO)
        {

            try
            {
                string id_concesion = _ConsultarDbGeneralMaeConcesionRepositorio.Guardar_Concesion(ID_CONCESION, RUC, CODIGO_HABILITACION, PARTIDA_REGISTRAL, UBICACION, UBIGEO, ID_AREA_PRODUCCION, ID_TIPO_CONCESION,ID_TIPO_ACTIVIDAD_CONCESION, USUARIO).First().id_concesion.ToString();
            }
            catch
            {
                throw new InvalidOperationException();
            }
            return true;
        }

        /*23*/
        public IEnumerable<ConsultarDbGeneralMaeConcesionResponse> lista_concesion(string COD_CONCESION, string documento)
        {
                var result = (from zp in _ConsultarDbGeneralMaeConcesionRepositorio.Listar(x => x.CODIGO_HABILITACION.Contains(COD_CONCESION) && x.RUC == documento)
                          select new ConsultarDbGeneralMaeConcesionResponse
                          {
                              id_concesion = zp.ID_CONCESION,
                              codigo_habilitacion = zp.CODIGO_HABILITACION
                          });
                return result;
            
        }


        /*55*/
        public IEnumerable<TipoActividadConcesionResponse> recupera_actividad_concesion()
        {
            var result = from zp in _TipoActividadConcesionRepositorio.Listar()
                         select new TipoActividadConcesionResponse
                         {
                             id_tip_act_conce = zp.ID_TIP_ACT_CONCE,
                             nombre = zp.NOMBRE
                         };
            return result;
        }

        /*54*/

        public ConsultarDbGeneralMaeConcesionResponse recupera_mae_concesion_x_id(int id_concesion)
        {
            return _ConsultarDbGeneralMaeConcesionRepositorio.recupera_mae_concesion_x_id(id_concesion);
        }


        /*55*/
        public IEnumerable<DbGeneralMaeTipoDesembarcaderoResponse> recupera_tipo_desembarcadero()
        {
            var result = from zp in _DbGeneralMaeTipoDesembarcaderoRepositorio.Listar()
                         select new DbGeneralMaeTipoDesembarcaderoResponse
                         {
                             id_tipo_desembarcadero = zp.ID_TIPO_DESEMBARCADERO,
                             nombre = zp.NOMBRE,
                             ruta_pdf = zp.RUTA_PDF
                         };
            return result;
        }

        /*55*/
        public IEnumerable<DbGeneralMaeCodigoDesembarcaderoResponse> recupera_codigo_desembarcadero(int id_tipo_desembarcadero)
        {
            var result = from zp in _DbGeneralMaeCodigoDesembarcaderoRepositorio.Listar(x => x.ID_TIPO_DESEMBARCADERO==id_tipo_desembarcadero)
                         select new DbGeneralMaeCodigoDesembarcaderoResponse
                         {
                             id_cod_desemb = zp.ID_COD_DESEMB,
                             codigo = zp.CODIGO
                         };
            return result;
        }

        /*55*/
        public DbGeneralMaeTipoDesembarcaderoResponse recupera_tipo_desembarcadero_x_id_desembarcadero(int id_desembarcadero)
        {
            int id_tipo_desembarcadero = (from zp in _DbGeneralMaeDesembarcaderoRepositorio.Listar(x => x.ID_DESEMBARCADERO == id_desembarcadero)
                         select new DbGeneralMaeDesembarcaderoResponse
                         {
                             id_tipo_desembarcadero = zp.ID_TIPO_DESEMBARCADERO
                         }).First().id_tipo_desembarcadero ?? 0;

            var result = from zp in _DbGeneralMaeTipoDesembarcaderoRepositorio.Listar(y => y.ID_TIPO_DESEMBARCADERO == id_tipo_desembarcadero)
                         select new DbGeneralMaeTipoDesembarcaderoResponse
                         {
                             id_tipo_desembarcadero = zp.ID_TIPO_DESEMBARCADERO,
                             nombre = zp.NOMBRE,
                             ruta_pdf = zp.RUTA_PDF
                         };
            return result.First();
        }
        /*25*/

        public bool Guardar_Desembarcadero(int ID_DESEMBARCADERO, int ID_SEDE, int ID_TIPO_DESEMBARCADERO, int ID_COD_DESEMB, int NUM_DESEMB, string NOMBRE_DESEMB, string DENOMINACION, string TEMPORAL, double LATITUD, double LONGITUD, string USUARIO)
        {

            try
            {
                string id_desembarcadero = _DbGeneralMaeDesembarcaderoRepositorio.Guardar_Desembarcadero(ID_DESEMBARCADERO, ID_SEDE, ID_TIPO_DESEMBARCADERO, ID_COD_DESEMB, NUM_DESEMB, NOMBRE_DESEMB, DENOMINACION, TEMPORAL, LATITUD, LONGITUD, USUARIO).First().id_desembarcadero.ToString();
            }
            catch
            {
                throw new InvalidOperationException();
            }
            return true;
        }

        /*28*/
        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> GetAlldesembarcadero_sin_paginado(int id_tipo_desembarcadero, string codigo_desembarcadero,string externo)
        {
            return _DbGeneralMaeDesembarcaderoRepositorio.GetAlldesembarcadero_sin_paginado(id_tipo_desembarcadero, codigo_desembarcadero, externo);
        }

        
        /*28*/
        public IEnumerable<DbGeneralMaeDesembarcaderoResponse> lista_desembarcadero_x_sede(int var_id_oficina_dir)
        {
            return _DbGeneralMaeDesembarcaderoRepositorio.lista_desembarcadero_x_sede(var_id_oficina_dir);
        }
        
        /*28*/
        public IEnumerable<DbGeneralMaeTransporteResponse> listar_transporte_x_placa(string placa)
        {

            var result = from zp in _DbGeneralMaeTransporteRepositorio.Listar(x => x.PLACA.Contains(placa))
                         select new DbGeneralMaeTransporteResponse
                         {
                             id_transporte = zp.ID_TRANSPORTE,
                             placa = zp.PLACA,
                             nombre_carroceria = zp.NOMBRE_CARROCERIA
                         };
            return result;
        }

        
        /*28*/
        public DbGeneralMaeTransporteResponse recuperar_transporte_x_id_transporte(int id_transporte)
        {

            var result = (from zp in _DbGeneralMaeTransporteRepositorio.Listar(x => x.ID_TRANSPORTE==id_transporte)
                         select new DbGeneralMaeTransporteResponse
                         {
                             id_transporte = zp.ID_TRANSPORTE,
                             placa = zp.PLACA,
                             cod_habilitacion = zp.COD_HABILITACION,
                             nombre_carroceria = zp.NOMBRE_CARROCERIA,
                             nombre_furgon = zp.NOMBRE_FURGON,
                             carga_util = zp.CARGA_UTIL,
                             nombre_um = zp.NOMBRE_UM,
                             siglas_um = zp.SIGLAS_UM
                         }).AsEnumerable().First();
            return result;
        }
        


        public ConsultarDniResponse actualizar_persona(string persona_num_documento, string direccion, string ubigeo, string usuario)
        {
            return _ConsultarDniRepositorio.actualizar_persona(persona_num_documento, direccion, ubigeo, usuario);
        }

        public IEnumerable<UnionentidadpersonaResponse> buscar_entidad_persona(string nombre)
        {
            var result = (from zp in _ConsultarDniRepositorio.Listar(x => (x.paterno + " "+x.materno+" "+x.nombres).Contains(nombre) || (x.nombres+" "+x.paterno+" "+x.materno).Contains(nombre))
                          select new UnionentidadpersonaResponse
                          {
                              documento = zp.persona_num_documento,
                              nombre = zp.nombres + " " + zp.paterno + " " + zp.materno
                          }).AsEnumerable();

            var result2 = (from zp in _ConsultarRucRepositorio.Listar(x => x.RAZON_SOCIAL.Contains(nombre))
                           select new UnionentidadpersonaResponse
                          {
                              documento = zp.RUC,
                              nombre = zp.RAZON_SOCIAL
                          }).AsEnumerable();

            return result.Union(result2);
        }
        public string Recupera_RUC_x_ID_OFI_DIR(int id_ofi_dir)
        {
            return _ConsultarOficinaRepositorio.Recupera_RUC_x_ID_OFI_DIR(id_ofi_dir);
        }
        public Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_Result Consultar_documentos_pendientes(string documento, int id_ofi_dir)
        {
            return _HojaTramiteRepositorio.Consultar_documentos_pendientes(documento, id_ofi_dir);
        }
        public IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_Result> Consultar_documentos_pendientes_detalle(string documento, int id_ofi_dir)
        {
            return _HojaTramiteRepositorio.Consultar_documentos_pendientes_detalle(documento, id_ofi_dir);
        }

        public IEnumerable<Response.SP_CONSULTAR_DOCUMENTOS_PENDIENTES_DETALLE_DESAGREGADO_Result> Consultar_documentos_pendientes_detalle_desagregado(string documento, int id_ofi_dir, string fecha)
        {
            return _HojaTramiteRepositorio.Consultar_documentos_pendientes_detalle_desagregado(documento, id_ofi_dir, fecha);
        }

        public IEnumerable<Response.SP_ACTUALIZA_NOM_EMPRESA_Result> Edita_db_general_nom_empresa(string nombres, string ruc, string usuario)
        {
            return _ConsultarOficinaRepositorio.Edita_db_general_nom_empresa(nombres, ruc, usuario);
        }
    }
}
