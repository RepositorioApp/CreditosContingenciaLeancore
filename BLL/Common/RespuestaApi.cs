using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Common
{
    public class RespuestaApi<T>
    {
        public bool Exito { get; set; }
        public string? Mensaje { get; set; }
        public T? Resultado { get; set; }

        public static RespuestaApi<T> Ok(T resultado, string mensaje = "")
            => new RespuestaApi<T> { Exito = true, Mensaje = mensaje, Resultado = resultado };

        public static RespuestaApi<T> Error(string mensaje)
            => new RespuestaApi<T> { Exito = false, Mensaje = mensaje };
    }
}
