using BLL.Common;
using BLL.Dto;
using BLL.Interface;
using BLL.RN;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ApiCreditosContingencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditosController : ControllerBase
    {
        private readonly ICreditosService _creditoService;
        private readonly ILogger<CreditosController> _logger;

        public CreditosController(ICreditosService creditoService, ILogger<CreditosController> logger)
        {
            _creditoService = creditoService;
            _logger = logger;

        }

        [HttpGet("modalidades")]
        public async Task<IActionResult> ObtenerModalidades()
        {
            try
            {
                var result = await _creditoService.ObtenerModalidadesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener modalidades");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al obtener modalidades"));
            }
        }
       
        [HttpGet("tiendas")]
        public async Task<IActionResult> ObtenerTiendas([FromQuery] string marca)
        {
            try
            {
                var result = await _creditoService.ObtenerTiendasAsync(marca);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tiendas");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al obtener las tiendas"));
            }
        }
        [HttpGet("marcas")]
        public async Task<IActionResult> obtenerMarcas()
        {
            try
            {
                var result = await _creditoService.ObtenerMarcasAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tiendas");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al obtener las tiendas"));
            }
        }
        [HttpGet("cuotas")]
        public async Task<IActionResult> ObtenerCuotas([FromQuery] string modalidad)
        {
            try
            {
                var result = await _creditoService.ObtenerCuotasAsync(modalidad);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener cuotas");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al obtener cuotas"));
            }
        }
        [HttpPost("guardar")]
        public async Task<IActionResult> GuardarCredito([FromBody] CreditoDto dto)
        {
            try
            {
                var result = await _creditoService.GuardarCreditoAsync(dto);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validación fallida al guardar crédito");
                return BadRequest(RespuestaApi<string>.Error(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar crédito");
                return StatusCode(500, RespuestaApi<string>.Error(ex.Message));
            }
        }
        [HttpGet("consultarCliente")]
        public async Task<IActionResult> ConsultarCliente([FromQuery] string nro)
        {
            try
            {
                var result = await _creditoService.ConsultarClienteAsync(nro);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogInformation(ex, "Cliente no encontrado");
                return NotFound(RespuestaApi<string>.Error("Cliente no encontrado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar cliente");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al consultar cliente"));
            }
        }

        [HttpPost("registrarNoCreado")]
        public async Task<IActionResult> RegistrarNoCreado([FromBody] ClienteNoCreadoDto dto)
        {
            try
            {
                await _creditoService.RegistrarNoCreadoAsync(dto.NroDocumento);
                return Ok(RespuestaApi<bool>.Ok(true, "Cliente registrado como no creado"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar cliente no creado");
                return StatusCode(500, RespuestaApi<string>.Error("Error interno al registrar cliente no creado"));
            }
        }

    }
}
