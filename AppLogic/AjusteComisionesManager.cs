using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppLogic;
using DataAccess.Mappers;
using DTO;
using InfinityGrowth.DataAccess.Crud;

namespace AppLogic
{
    public interface IAjusteComisionesManager
    {
        List<AjusteComisiones> GetAllComisiones();
        void UpdateComisiones(List<AjusteComisiones> comisionesParaActualizar);
    }

    public class AjusteComisionesManager : IAjusteComisionesManager
    {
        // No se inyecta el CRUD en el constructor según el ejemplo de LoginManager

        public AjusteComisionesManager()
        {
            // El constructor puede estar vacío o recibir otras dependencias vía DI si fueran necesarias
            // (similar a como LoginManager recibe IEmailService)
        }

        /// <summary>
        /// Obtiene todas las configuraciones de comisiones.
        /// </summary>
        public List<AjusteComisiones> GetAllComisiones()
        {
            // Instancia el Mapper y el Crud dentro del método (siguiendo el patrón de LoginManager)
            var mapper = new AjusteComisionesMapper();
            var crud = new CatalogoComisionesCrud(mapper); // Pasamos el mapper al constructor del Crud

            return crud.RetrieveAll();
        }

        /// <summary>
        /// Actualiza los porcentajes de una lista de comisiones.
        /// </summary>
        public void UpdateComisiones(List<AjusteComisiones> comisionesParaActualizar)
        {
            if (comisionesParaActualizar == null || !comisionesParaActualizar.Any())
            {
                return; // O lanzar excepción
            }

            // Instancia el Mapper y el Crud dentro del método
            var mapper = new AjusteComisionesMapper();
            var crud = new CatalogoComisionesCrud(mapper);

            foreach (var comision in comisionesParaActualizar)
            {
                // Validaciones opcionales aquí...
                crud.UpdatePorcentaje(comision.IdTipoComision, comision.Porcentaje);
            }
        }
    }
}



    
     