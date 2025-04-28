using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RpgApi.Models;
using RpgApi.Models.Enuns;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[Controller]")]
    public class PersonagensExercicioController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>()
        {
            new Personagem() { Id = 1, Nome = "Frodo", PontosVida=95, Forca=17, Defesa=23, Inteligencia=33, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=90, Forca=15, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=110, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=120, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago },
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=94, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro },
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=117, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo },
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=115, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago }
        };

        //Método que seleciona o personagem de acordo com o nome digitado.
        [HttpGet("GetByNome/{nome}")]
        public IActionResult GetByNome(string nome)
        {
            if (!personagens.Any(personagens => personagens.Nome == nome))
            {
                nome = "NotFound";
                return Ok(nome);
            }else{
                List<Personagem> nomePersonagem = personagens.FindAll(p => p.Nome == nome);
                return Ok(nomePersonagem);
            }                        
        }

        //Método que remova os cavaleiros, e exiba a lista em ordem decrescente por PontosVida.
        [HttpGet("GetClericoMago")]
        public IActionResult GetClericoMago()
        {
            personagens.RemoveAll(p => p.Classe == ClasseEnum.Cavaleiro);
            List<Personagem> ordemPontosVida = personagens.OrderByDescending(p => p.PontosVida).ToList();
            return Ok(ordemPontosVida);
        }

        //Método que exiba a quantidade de personagens na lista, e a soma de suas inteligencias.
        [HttpGet("GetEstatisticas")]
        public IActionResult GetEstatisticas()
        {
            int registrados = personagens.Count();
            int somaInteligencias = 0;
            for(int i=0; i < personagens.Count(); i++)
            {
                somaInteligencias = somaInteligencias + personagens[i].Inteligencia;
            }
            return Ok($"Personagens registrados: {registrados}\nSoma Inteligencias: {somaInteligencias}");
        }

        //Método que não permita que um personagem seja adicionado com defesa menor que 10 ou inteligencia maior que 30.
        [HttpPost("PostValidacao/{novoPersonagem}")]
        public IActionResult PostValidacao(Personagem novoPersonagem)
        {
            if(novoPersonagem.Defesa < 10 || novoPersonagem.Inteligencia > 30)
            {
                return BadRequest("Criação interrompida, valores adicionados invalidos. (Defesa/Inteligencia)");
            }else{
                personagens.Add(novoPersonagem);
                return Ok(personagens);
            }
        }

        //Método que não permita que um Mago seja inserido com inteligencia menor que 35.
        [HttpPost("PostValidacaoMago/{novoPersonagem}")]
        public IActionResult PostValidacaoMago(Personagem novoPersonagem)
        {
            if(novoPersonagem.Classe == (ClasseEnum)2)
            {
                if(novoPersonagem.Inteligencia < 35)
                {
                    return BadRequest("Criação interrompida, inteligencia menor que 35.");
                }                               
            }
            personagens.Add(novoPersonagem);
            return Ok(personagens);
        }

        //Método que exibe a lista de personagens de acordo com a classe digitada.
        [HttpGet("GetByClasse/{id}")]
        public IActionResult GetByClasse(int id)
        {            
            ClasseEnum idClasse = (ClasseEnum)id;
            int quantidadeClasses = Enum.GetValues(typeof(ClasseEnum)).Length;
            if (id > quantidadeClasses || id <= 0)
            {
                return BadRequest("Valor inserido inexistente.");
            }else{
                var personagensFiltrados = personagens.Where(personagens => personagens.Classe == idClasse).ToList();
                if (personagensFiltrados.Count() == 0)
                {
                    return NotFound("Nenhum personagem registrado nessa classe.");
                }
                return Ok(personagensFiltrados);
            }            
        }
    }
}