using DataAccess.Crud;

namespace AppLogic
{
    public class RelacionAsesorClienteManager
    {
        private readonly RelacionAsesorClienteCrud _crud = new RelacionAsesorClienteCrud();

        public string AsignarAsesorACliente(int idAsesor, int idCliente)
        {
            return _crud.AsignarAsesorACliente(idAsesor, idCliente);
        }
    }
}

