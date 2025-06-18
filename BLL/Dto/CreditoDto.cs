using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dto
{
    public class CreditoDto
    {
        public string NroDocumento { get; set; }
        public string Marca { get; set; }
        public string Tienda { get; set; }
        public string Obligacion { get; set; }
        public string? Caja { get; set; }
        public string? Cajero { get; set; }
        public string ValorCredito { get; set; }
        public string? ValorCuotaInicial { get; set; } = "0";
        public string InteresTeorico { get; set; }
        public string IvaInteres { get; set; }
        public string PjTasaPeriodo { get; set; }
        public string PsTasaEA { get; set; }
        public string? DescModalidad { get; set; }
        
        public int NumCuotas { get; set; }
        public DateTime? FechaVencimiento { get; set; } = new DateTime(1900, 1, 1);
    }
}
