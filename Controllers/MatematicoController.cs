using System;
using System.Web.Http;

namespace RESTapiPruebaTeen.Controllers
{
    public class MatematicoController : ApiController
    {
        [HttpGet]
        [Route("api/matematico/MCM")]
        public IHttpActionResult MCM(string numbers)
        {
            if (string.IsNullOrEmpty(numbers))
            {
                return BadRequest("No se han proporcionado números.");
            }

            string[] numerosString = numbers.Split(',');
            int[] numeros = Array.ConvertAll(numerosString, int.Parse);

            int mcm = CalcularMCM(numeros);

            return Ok($"El mínimo común múltiplo de los números {numbers} es: {mcm}");
        }

        [HttpGet]
        [Route("api/matematico/Incrementar")]
        public IHttpActionResult Incrementar(int number)
        {
            int resultado = number + 1;
            return Ok($"El número incrementado en 1 es: {resultado}");
        }

        private int CalcularMCM(int[] numeros)
        {
            // Calcular el mínimo común múltiplo (MCM) de los números
            int mcm = numeros[0];
            for (int i = 1; i < numeros.Length; i++)
            {
                mcm = MCM(mcm, numeros[i]);
            }
            return mcm;
        }

        private int MCM(int a, int b)
        {
            // Calcular el MCM utilizando el MCD (Máximo Común Divisor)
            return a * b / MCD(a, b);
        }

        private int MCD(int a, int b)
        {
            // Calcular el MCD (Máximo Común Divisor) utilizando el algoritmo de Euclides
            while (b != 0)
            {
                int temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
    }
}
