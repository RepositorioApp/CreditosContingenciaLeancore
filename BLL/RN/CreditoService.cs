using BLL.Common;
using BLL.Dto;
using BLL.Interface;
using DataLayer.Interface;
using DataLayer.RN;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.RN
{
    public class CreditoService : ICreditosService
    {
        private readonly IRepository _repo;
        public CreditoService(IRepository repo)
        {
            _repo = repo;
        }

        public async Task<RespuestaApi<IEnumerable<ClienteDto>>> ConsultarClienteAsync(string nroDocumento)
        {
            //var cliente = await _repo.QueryTextAsync<ClienteDto>("sp_ObtenerClientePorDocumento", new { NroDocumento = nroDocumento });
           var cliente = await _repo.QueryTextAsync<ClienteDto>(
          "select documento,NOMBRE1 + ISNULl(' '+nombre2,'') + ' ' +APELLIDO1 + ISNULl(' '+APELLIDO2,'') as nombre from tbl_Clientes_nuevos_creditos where documento = @doc",
          new { doc = nroDocumento });
            if (cliente == null)
                return RespuestaApi<IEnumerable<ClienteDto>>.Error("Cliente no encontrado");
            return RespuestaApi<IEnumerable<ClienteDto>>.Ok(cliente);
        }


        public async Task<RespuestaApi<bool>> RegistrarNoCreadoAsync(string nroDocumento)
        {
            int rows = await _repo.ExecuteAsync("sp_GuardarClienteNoExistenteCreditos", new { documento = nroDocumento });
            return rows > 0 ? RespuestaApi<bool>.Ok(true, "Cliente registrado") : RespuestaApi<bool>.Error("No se pudo registrar");
        }

        public async Task<RespuestaApi<IEnumerable<ModalidadDto>>> ObtenerModalidadesAsync()
        {
            IEnumerable<ModalidadDto> modalidades = await _repo.QueryTextAsync<ModalidadDto>("SELECT IdPago, NombrePago FROM tbl_pagos with(nolock)");
            return RespuestaApi<IEnumerable<ModalidadDto>>.Ok(modalidades);
        }
        public async Task<RespuestaApi<IEnumerable<TiendaDto>>> ObtenerTiendasAsync(string marca)
        {
            IEnumerable<TiendaDto> tiendas = await _repo.QueryTextAsync<TiendaDto>(
            //"select  tienda as ceco, descripcion_tienda as tienda from tbl_tiendas_leancore with(nolock) where tienda != '' group by tienda,descripcion_tienda");
            "select  tienda as ceco, descripcion_tienda as tienda from tbl_tiendas_leancore with(nolock) where marca =  @mar group by tienda,descripcion_tienda",
            new { mar = marca });
            return RespuestaApi<IEnumerable<TiendaDto>>.Ok(tiendas);
        }
        public async Task<RespuestaApi<IEnumerable<MarcaDto>>> ObtenerMarcasAsync()
        {
            IEnumerable<MarcaDto> tiendas = await _repo.QueryTextAsync<MarcaDto>("SELECT nombreMarca, descripcionMarca FROM tbl_Marca WHERE estadoMarca = 1");
            return RespuestaApi<IEnumerable<MarcaDto>>.Ok(tiendas);
        }

        public async Task<RespuestaApi<IEnumerable<PlazoDto>>> ObtenerCuotasAsync(string mod)
        {
            //IEnumerable<int> cuotas = await _repo.QueryAsync<int>("sp_ObtenerCuotasPorModalidad", new { Modalidad = modalidad });
            IEnumerable<PlazoDto> registros = await _repo.QueryTextAsync<PlazoDto>(
            "SELECT IdPlazo, TiempoPlazo FROM tbl_plazos with(nolock) where IdPago = @modalidad",
            new { modalidad = mod });
            return RespuestaApi<IEnumerable<PlazoDto>>.Ok(registros);
        }

        public async Task<RespuestaApi<bool>> GuardarCreditoAsync(CreditoDto credito)
        {
            if (credito != null)
            {
                await _repo.ExecuteAsync("sp_GuardarCreditoManualConti", credito);
                return RespuestaApi<bool>.Ok(true, "Crédito guardado correctamente");
            }
            return RespuestaApi<bool>.Error("Error guardando el credito");

        }


    }
}
