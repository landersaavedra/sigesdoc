using SIGESDOC.Contexto;
using SIGESDOC.IRepositorio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.Repositorio
{
    public class BaseRepositorio<T> : IBaseRepositorio<T> where T : class
    {
        private readonly IContext _context;

        protected IContext Context
        {
            get { return _context; }
        }

        public BaseRepositorio(IContext context)
        {
            _context = context;
        }

        public IQueryable<T> Listar(Expression<Func<T, bool>> filter = null, int pageIndex = 1, int pageSize = int.MaxValue)
        {
            return _context.Listar(filter, pageIndex, pageSize);
        }

        public int Contar(Expression<Func<T, bool>> filter = null)
        {
            return _context.Contar(filter);
        }

        public T ListarUno(Expression<Func<T, bool>> predicate)
        {
            return _context.ListarUno(predicate);
        }

        public void Insertar(T entity)
        {
            _context.Insertar(entity);
        }

        public void ActualizarParcial(T entity, params string[] noChangedPropertyNames)
        {
            _context.ActualizarParcial(entity, noChangedPropertyNames);
        }

        public void Actualizar(T entity)
        {
            _context.Actualizar(entity);
        }

        public void Eliminar(T entity)
        {
            _context.Eliminar(entity);
        }
    }
}
