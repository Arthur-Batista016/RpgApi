using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonagemHabilidadesController : ControllerBase
    {
        private readonly DataContext _context;

        public PersonagemHabilidadesController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagemHabilidadeAsync(PersonagemHabilidade novoPersonagemHabilidade)
        {
            try
            {
                Personagem personagem = await _context.TB_PERSONAGENS
                    .Include(p => p.Arma)
                    .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                    .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);

                if (personagem == null)
                    throw new System.Exception("Personagem não encontrado para o Id informado.");

                Habilidade habilidade = await _context.TB_HABILIDADES
                    .FirstOrDefaultAsync(h => h.Id == novoPersonagemHabilidade.HabilidadeId);

                if (habilidade == null)
                    throw new System.Exception("Habilidade não encontrada.");

                PersonagemHabilidade ph = new PersonagemHabilidade();
                ph.Personagem = personagem;
                ph.Habilidade = habilidade;
                await _context.TB_PERSONAGENS_HABILIDADES.AddAsync(ph);
                int linhasAfetadas = await _context.SaveChangesAsync();

                return Ok(linhasAfetadas);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Get(){
            List<PersonagemHabilidade> itens = await _context.TB_PERSONAGENS_HABILIDADES.ToListAsync();
            return Ok(itens);
        }

        //Exercicio 6 - Finalizado
         [HttpGet("GetHabilidades")]
        public async Task<IActionResult> GetHabilidades(){
            List<Habilidade> habilidades = await _context.TB_PERSONAGENS_HABILIDADES.Include(ph => ph.Habilidade)
        .Select(ph => ph.Habilidade)
        .Distinct()
        .ToListAsync();
            if (habilidades == null){
                return BadRequest("Habilidades nao encontradas");
            }else{
            return Ok(habilidades);
            }
        }

        //Exercicio 5 - Finalizado
        [HttpGet("PersonagemHabilidadeId/{Id_personagem}")]
        public async Task<IActionResult> PersonagemHabilidadeId(int Id_personagem){
            List<PersonagemHabilidade> habilidades_personagens = await _context.TB_PERSONAGENS_HABILIDADES.Where(ph=>ph.PersonagemId==Id_personagem).ToListAsync();
            if(habilidades_personagens == null)
                return BadRequest("Registros nao encontrados");
            return Ok(habilidades_personagens);
        }


        //Exercicio 7 - Tive dificuldades para executar o exercício e não consegui finalizá-lo
        [HttpPost("DeletePersonagemHabilidade")]
        public async Task<IActionResult> DeletePersonagemHabilidade(int habilidadeId,int personagemId) {
            PersonagemHabilidade personagemHabilidade = await _context.TB_PERSONAGENS_HABILIDADES.Include(ph=>ph.PersonagemId)
            .Include(ph=>ph.HabilidadeId).FirstOrDefaultAsync(ph => ph.PersonagemId == personagemId
            && ph.HabilidadeId == habilidadeId);
            _context.TB_PERSONAGENS_HABILIDADES.Remove(personagemHabilidade);
            await _context.SaveChangesAsync();
            return Ok("PersonagemHabilidade Removido com sucesso do BD");
        }
    }
}