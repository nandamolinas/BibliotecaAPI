using Microsoft.AspNetCore.Mvc;

namespace SeuProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController] // Certifique-se de adicionar a anotação ApiController
    public class EmprestimosController : ControllerBase
    {
        // Ação para cadastrar um empréstimo
        [HttpPost("cadastrar")]
        public IActionResult CadastrarEmprestimo([FromBody] Emprestimo emprestimo)
        {
            // Lógica para salvar o empréstimo no banco de dados ou na memória
            // Exemplo: _context.Emprestimos.Add(emprestimo); 
            // _context.SaveChanges();
            
            return Ok(emprestimo); // Retorna o objeto que foi cadastrado
        }
    }
}
