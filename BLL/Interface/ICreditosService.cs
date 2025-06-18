using BLL.Common;
using BLL.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interface
{
    public interface ICreditosService
    {
        Task<RespuestaApi<IEnumerable<ClienteDto>>> ConsultarClienteAsync(string nroDocumento);
        Task<RespuestaApi<bool>> RegistrarNoCreadoAsync(string nroDocumento);
        Task<RespuestaApi<IEnumerable<ModalidadDto>>> ObtenerModalidadesAsync();
        Task<RespuestaApi<IEnumerable<TiendaDto>>> ObtenerTiendasAsync(string marca);
        Task<RespuestaApi<IEnumerable<MarcaDto>>> ObtenerMarcasAsync();
        Task<RespuestaApi<IEnumerable<PlazoDto>>> ObtenerCuotasAsync(string modalidad);
        Task<RespuestaApi<bool>> GuardarCreditoAsync(CreditoDto credito);
    }
}
