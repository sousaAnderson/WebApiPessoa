using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiPessoas.Business;
using WebApiPessoas.Models;

namespace WebApiPessoas.Controllers
{
    [Authorize("Bearer")]
    [Route("api/[controller]")]
    public class PessoaController : Controller
    {
        private PessoaService _service;

        public PessoaController(PessoaService service)
        {
            _service = service;
        }

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<Pessoa> Get()
        {
            return _service.ListarTodos();
        }

        // GET api/<controller>/5
        [HttpGet("{codigo}")]
        public IActionResult Get(int codigo)
        {
            var pessoa = _service.ListarPorCodigo(codigo);
            if (pessoa != null)
                return new ObjectResult(pessoa);
            else
                return NotFound();
        }

        [HttpGet("{uf}")]
        public IActionResult GetUf(string uf)
        {
            var pessoa = _service.ListarPorUf(uf);
            if (pessoa != null)
                return new ObjectResult(pessoa);
            else
                return NotFound();
        }

        // POST api/<controller>
        [HttpPost]
        public Resultado Post([FromBody]Pessoa pessoa)
        {
            return _service.Incluir(pessoa);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public Resultado Put([FromBody]Pessoa pessoa)
        {
            return _service.Atualizar(pessoa);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{codigo}")]
        public Resultado Delete(int codigo)
        {
            return _service.Excluir(codigo);
        }
    }
}
