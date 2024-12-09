/*

using Microsoft.AspNetCore.Mvc;
using SapMvcApp.Models;

namespace SapMvcApp.Controllers
{
    [Route("sap")]
    public class SapController : Controller
    {
        [HttpPost("printnome")]
        public IActionResult PrintNome([FromBody] NomeRequest nomeRequest, [FromHeader] string gxtoken)
        {
            try
            {
                // Imprime os valores recebidos
                Console.WriteLine($"Nome recebido: {nomeRequest.Nome}");
                Console.WriteLine($"Token recebido: {gxtoken}");

                // Retorna o nome recebido
                return Content($"Nome recebido: {nomeRequest.Nome}", "text/plain");
            }
            catch (Exception ex)
            {
                // Retorna erro no caso de exceção
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
*/