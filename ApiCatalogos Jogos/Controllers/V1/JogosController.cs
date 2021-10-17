using ApiCatalogosJogos.InputModel;
using ApiCatalogosJogos.Exceptions;
using ApiCatalogosJogos.Services;
using ApiCatalogosJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ApiCatalogosJogos.Controllers.V1
{
    [Route("api/V1[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possivel retornar os jogos se paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo cosultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. mínimo 1 e máximo 50</param>
        ///<response code="200">Retorna a lista de jogos </response>
        ///<reponse code="204">Caso não haja jogos</reponse>
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>>  Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = _jogoService.Obter(pagina, quantidade);

            if (jogos.Result.Count() == 0)
            return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possivel retornar os jogos se paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo cosultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. mínimo 1 e máximo 50</param>
        ///<response code="200">Retorna a lista de jogos </response>
        ///<reponse code="204">Caso não haja jogos</reponse>
        
        [HttpGet ("{idJogo:guid}")]
        
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);

                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta podutora");
            }
        }

        [HttpPut ("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
           try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);

                return Ok();
            }
         catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }
        }

        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try 
            {
                await _jogoService.Remover(idJogo);

                return Ok();

            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo");
            }

        }
    }
}
