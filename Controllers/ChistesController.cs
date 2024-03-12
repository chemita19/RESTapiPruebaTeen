using Newtonsoft.Json.Linq;
using RESTapiPruebaTeen.Models;
using RESTapiPruebaTeen.Models.Clases;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace RESTapiPruebaTeen.Controllers
{
    public class ChistesController : ApiController
    {
        private static readonly HttpClient httpClient = new HttpClient();

        private readonly ApplicationDbContext _context;

        public class ChisteViewModel
        {
            public string chisteTexto { get; set; }
        }


        public ChistesController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpGet]
        [Route("api/chistes/obtener")]
        public async Task<IHttpActionResult> Get(string tipo = null)
        {
            if (string.IsNullOrEmpty(tipo))
            {
                // Obtener chiste aleatorio
                string chisteAleatorio = ObtenerChisteAleatorio();
                return Ok(chisteAleatorio);
            }
            else if (tipo.Equals("Chuck", StringComparison.OrdinalIgnoreCase))
            {
                // Obtener chiste de Chuck Norris API
                string chisteChuck = await ObtenerChisteChuck();
                return Ok(chisteChuck);
            }
            else if (tipo.Equals("Dad", StringComparison.OrdinalIgnoreCase))
            {
                // Obtener chiste de Dad API
                string chisteDad = await ObtenerChisteDad();
                return Ok(chisteDad);
            }
            else
            {
                return BadRequest("Tipo de chiste no válido");
            }
        }

        [HttpPost]
        [Route("api/chistes/guardar")]
        public IHttpActionResult Post([FromBody] ChisteViewModel chisteViewModel)
        {
            if (string.IsNullOrEmpty(chisteViewModel?.chisteTexto))
            {
                return BadRequest("El chiste no puede estar vacío.");
            }

            var chiste = new Chiste { Texto = chisteViewModel.chisteTexto };
            _context.Chistes.Add(chiste);
            _context.SaveChanges();

            return Ok("Chiste guardado");
        }


        [HttpPut]
        [Route("api/chistes/actualizar")]
        public IHttpActionResult Update(int id, [FromBody] string nuevoChisteTexto)
        {
            var chiste = _context.Chistes.Find(id);
            if (chiste == null)
            {
                return NotFound();
            }

            chiste.Texto = nuevoChisteTexto;
            _context.SaveChanges();

            return Ok("Chiste actualizado");
        }

        [HttpDelete]
        [Route("api/chistes/borrar")]
        public IHttpActionResult Delete(int id)
        {
            var chiste = _context.Chistes.Find(id);
            if (chiste == null)
            {
                return NotFound();
            }

            _context.Chistes.Remove(chiste);
            _context.SaveChanges();

            return Ok("Chiste eliminado");
        }

        [HttpGet]
        [Route("api/chistes/todos")]
        public IHttpActionResult GetAllChistes()
        {
            var chistes = _context.Chistes.Select(c => new { Id = c.ID, Texto = c.Texto }).ToList();
            return Ok(chistes);
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }

        private string ObtenerChisteAleatorio()
        {
            using (var dbContext = new ApplicationDbContext())
            {
                // Obtener todos los chistes de la base de datos
                var chistes = dbContext.Chistes.ToList();

                // Verificar si hay chistes en la base de datos
                if (chistes.Count == 0)
                {
                    return "No hay chistes disponibles en la base de datos.";
                }

                // Generar un índice aleatorio
                Random random = new Random();
                int index = random.Next(chistes.Count);

                // Devolver un chiste aleatorio
                return chistes[index].Texto;
            }
        }



        private async Task<string> ObtenerChisteChuck()
    {
        HttpResponseMessage response = await httpClient.GetAsync("https://api.chucknorris.io/jokes/random");
        if (response.IsSuccessStatusCode)
        {
            string json = await response.Content.ReadAsStringAsync();
            JObject jObject = JObject.Parse(json);
            string chiste = (string)jObject["value"];
            return chiste;
        }
        else
        {
            return "Error al obtener el chiste de Chuck Norris";
        }
    }


    private async Task<string> ObtenerChisteDad()
        {
            // Lógica para obtener un chiste de Dad API
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpResponseMessage response = await httpClient.GetAsync("https://icanhazdadjoke.com/");
            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                JObject jObject = JObject.Parse(json);
                string chiste = (string)jObject["joke"];
                return chiste;
            }
            else
            {
                return "Error al obtener el chiste de papá";
            }
        }
    }
}
