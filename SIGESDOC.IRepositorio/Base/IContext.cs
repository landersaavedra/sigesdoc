using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIGESDOC.IRepositorio
{
    public interface IContext
    {

        /// <summary>
        /// Retorna el contexto actual.
        /// </summary>
        /// <returns></returns>
        DbContext GetContext();

        /// <summary>
        /// Lista los elementos de una entidad de forma paginada.
        /// </summary>
        /// <param name="filter">Expresión linq con el filtro a aplicar.</param>
        /// <param name="pageIndex">Página a mostrar. Por defecto cero.</param>
        /// <param name="pageSize">Tamaño de la página. Por defecto int.MaxValue.</param>
        /// <returns></returns>
        IQueryable<T> Listar<T>(Expression<Func<T, bool>> filter = null, int pageIndex = 1, int pageSize = int.MaxValue) where T : class;

        /// <summary>
        /// Retorna la cantidad de elementos de una entidad.
        /// </summary>
        /// <param name="filter">Expresión linq con el filtro a aplicar.</param>
        /// <returns>Cantidad de elementos encontrados.</returns>
        int Contar<T>(Expression<Func<T, bool>> filter = null) where T : class;

        /// <summary>
        /// Retorna un elemento de una entidad.
        /// </summary>
        /// <param name="predicate">Expresión linq con el filtro a aplicar.</param>
        /// <returns>Elemento encontrado.</returns>
        T ListarUno<T>(Expression<Func<T, bool>> predicate) where T : class;

        /// <summary>
        /// Inserta el elemento en el contexto de datos.
        /// </summary>
        /// <param name="entity">Elemento a insertar.</param>
        void Insertar<T>(T entity) where T : class;

        /// <summary>
        /// Actualiza el elemento en el contexto de datos.
        /// </summary>
        /// <param name="entity">Elemento a actualizar.</param>
        void Actualizar<T>(T entity) where T : class;

        /// <summary>
        /// Actualiza el elemento en el contexto de datos excluyendo ciertas propiedades.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">Elemento a actualizar.</param>
        /// <param name="noChangedPropertyNames">Propiedades a excluir</param>
        void ActualizarParcial<T>(T entity, params string[] noChangedPropertyNames) where T : class;

        /// <summary>
        /// Elimina el elemento en el contexto de datos.
        /// </summary>
        /// <param name="entity">Elemento a eliminiar..</param>
        void Eliminar<T>(T entity) where T : class;

        /// <summary>
        /// Guarda los cambios del contexto subyacente.
        /// </summary>
        int Guardar(bool validate = true);
    }
}
