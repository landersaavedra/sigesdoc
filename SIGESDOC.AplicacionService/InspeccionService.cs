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
    public class InspeccionService : IInspeccionService
    {
        /*01*/
        private readonly IConsultaSolicitudInspeccionOdRepositorio _ConsultaSolicitudInspeccionOdRepositorio;
        private readonly ISolicitudInspeccionRepositorio _SolicitudInspeccionRepositorio;
        private readonly IUnitOfWork _UnitOfWork;

        public InspeccionService(
            /*01*/  IConsultaSolicitudInspeccionOdRepositorio ConsultaSolicitudInspeccionOdRepositorio,
            ISolicitudInspeccionRepositorio SolicitudInspeccionRepositorio,
            IUnitOfWork UnitOfWork
            )
        {
            _ConsultaSolicitudInspeccionOdRepositorio = ConsultaSolicitudInspeccionOdRepositorio;
            _SolicitudInspeccionRepositorio = SolicitudInspeccionRepositorio;
            _UnitOfWork = UnitOfWork;
        }


        public IEnumerable<ConsultaSolicitudInspeccionOdResponse> Recupera_lista_si_od(int id_ofi_dir)
        {
            var result = (from zp in _ConsultaSolicitudInspeccionOdRepositorio.Listar()
                          where zp.ID_OD_INSP == id_ofi_dir
                          select new ConsultaSolicitudInspeccionOdResponse
                          {
                              id_sol_ins = zp.ID_SOL_INS,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              expediente = zp.EXPEDIENTE,
                              id_estado = zp.ID_ESTADO,
                              fecha_crea = zp.FECHA_CREA,
                              fecha_crea_text = zp.FECHA_CREA_TEXT,
                              externo = zp.EXTERNO,
                              nombre_estado = zp.NOMBRE_ESTADO,
                              nombre_tipo_solicitud = zp.NOMBRE_TIPO_SOLICITUD,
                              solicitud_inspeccion = zp.SOLICITUD_INSPECCION,
                              id_od_insp = zp.ID_OD_INSP,
                              persona_contacto = zp.PERSONA_CONTACTO,
                              telefono_oficina = zp.TELEFONO_OFICINA,
                              telefono_planta = zp.TELEFONO_PLANTA,
                              correo = zp.CORREO,
                              resolucion = zp.CORREO,
                              observaciones = zp.OBSERVACIONES,
                              fecha_inspeccion = zp.FECHA_INSPECCION,
                              fecha_inspeccion_text = zp.FECHA_INSPECCION_TEXT,
                              inspector = zp.INSPECTOR,
                              nom_inspector =zp.NOM_INSPECTOR,
                              fecha_recepcion_inspector = zp.FECHA_RECEPCION_INSPECTOR,
                              fecha_recepcion_inspector_text = zp.FECHA_RECEPCION_INSPECTOR_TEXT
                          }).OrderByDescending(x => x.fecha_crea);

            return result;
        }

        public IEnumerable<ConsultaSolicitudInspeccionOdResponse> Recupera_lista_si_od_x_inspector(int id_ofi_dir,string inspector)
        {
            var result = (from zp in _ConsultaSolicitudInspeccionOdRepositorio.Listar()
                          where zp.ID_OD_INSP == id_ofi_dir && zp.INSPECTOR == inspector
                          select new ConsultaSolicitudInspeccionOdResponse
                          {
                              id_sol_ins = zp.ID_SOL_INS,
                              id_seguimiento = zp.ID_SEGUIMIENTO,
                              expediente = zp.EXPEDIENTE,
                              id_estado = zp.ID_ESTADO,
                              fecha_crea = zp.FECHA_CREA,
                              fecha_crea_text = zp.FECHA_CREA_TEXT,
                              externo = zp.EXTERNO,
                              nombre_estado = zp.NOMBRE_ESTADO,
                              nombre_tipo_solicitud = zp.NOMBRE_TIPO_SOLICITUD,
                              solicitud_inspeccion = zp.SOLICITUD_INSPECCION,
                              id_od_insp = zp.ID_OD_INSP,
                              persona_contacto = zp.PERSONA_CONTACTO,
                              telefono_oficina = zp.TELEFONO_OFICINA,
                              telefono_planta = zp.TELEFONO_PLANTA,
                              correo = zp.CORREO,
                              resolucion = zp.CORREO,
                              observaciones = zp.OBSERVACIONES,
                              fecha_inspeccion = zp.FECHA_INSPECCION,
                              fecha_inspeccion_text = zp.FECHA_INSPECCION_TEXT,
                              inspector = zp.INSPECTOR,
                              fecha_recepcion_inspector = zp.FECHA_RECEPCION_INSPECTOR,
                              fecha_recepcion_inspector_text = zp.FECHA_RECEPCION_INSPECTOR_TEXT
                          }).OrderByDescending(x => x.fecha_recepcion_inspector);

            return result;
        }

        public IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp(string usuario, int id_ofi_dir)
        {

            return _ConsultaSolicitudInspeccionOdRepositorio.recibe_soli_insp(usuario, id_ofi_dir);
        }

        public IEnumerable<SolicitudInspeccionResponse> recibe_soli_insp_x_inspector(string usuario, int id_ofi_dir, string inspector)
        {

            return _ConsultaSolicitudInspeccionOdRepositorio.recibe_soli_insp_x_inspector(usuario, id_ofi_dir, inspector);
        }

        public int asigna_inspector(int id_sol_insp, string inspector, string fec_inspeccion)
        {
            MAE_SOLICITUD_INSPECCION entity;

            entity = _SolicitudInspeccionRepositorio.ListarUno(x => x.ID_SOL_INS == id_sol_insp);
            //entity.FECHA_INSPECCION = DateTime.Now;
            entity.INSPECTOR = inspector;
            entity.FECHA_INSPECCION = Convert.ToDateTime(fec_inspeccion);
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _SolicitudInspeccionRepositorio.Actualizar(entity);
                    _UnitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_SOL_INS;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }


        public int atender_inspector(int id_sol_insp_od)
        {
            MAE_SOLICITUD_INSPECCION entity;

            entity = _SolicitudInspeccionRepositorio.ListarUno(x => x.ID_SOL_INS == id_sol_insp_od);
            //entity.FECHA_INSPECCION = DateTime.Now;
            entity.ID_ESTADO = 3;

            try
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    _SolicitudInspeccionRepositorio.Actualizar(entity);
                    _UnitOfWork.Guardar();
                    scope.Complete();
                }

                return entity.ID_SOL_INS;
            }
            catch
            {
                throw new InvalidOperationException();
            }
        }

    }
}
