using System;
using System.Threading.Tasks;
using DataAccess.Crud;
using DTO;

namespace AppLogic
{
    public interface IInversionesActivasManager
    {
        Task<string> CreateInversion(InversionesActivas dto);
        List<InformacionPortafolio> GetInversionesByUserId(int userId);
        string SellInversion(VenderAccion dto);
    }

    public class InversionesActivasManager : IInversionesActivasManager
    {
        public async Task<string> CreateInversion(InversionesActivas dto)
        {
            try
            {
                var inversionesCrud = new InversionesActivasCrud();
                inversionesCrud.Create(dto);

                return "Inversión activa registrada exitosamente.";
            }
            catch (Exception ex)
            {
                return "Ocurrió un error inesperado. Intente nuevamente. Detalle: " + ex.Message;
            }
        }
        public List<InformacionPortafolio> GetInversionesByUserId(int userId)
        {
            var crud = new InversionesActivasCrud();
            var inversiones = crud.RetrieveByUserId(userId);

            var resumenList = inversiones.Select(inv => new InformacionPortafolio
            {
                idActivo = inv.Id,
                Nombre = inv.Nombre,
                Ticker = inv.Ticker,
                Cantidad = inv.Cantidad,
                PrecioTotal = inv.PrecioTotal,


            }).ToList();

            return resumenList;
        }

        //venta Accion
        public string SellInversion(VenderAccion dto)
        {
            try
            {
                var inversionesCrud = new InversionesActivasCrud();
                inversionesCrud.SellInversion(dto);
                return "Inversión activa registrada exitosamente.";
            }
            catch (Exception ex)
            {
                return "Ocurrió un error inesperado. Intente nuevamente. Detalle: " + ex.Message;
            }
        }
    }
 }

