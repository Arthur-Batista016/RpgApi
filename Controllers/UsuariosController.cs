using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;
using RpgApi.Utils;


namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly DataContext _context;
        public UsuariosController(DataContext context)
        {
            _context = context;
        }
        private async Task<bool> UsuarioExistente(string username)
        {
            if (await _context.TB_USUARIOS.AnyAsync(x => x.Username.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        [HttpPost("Registrar")]
        public async Task<IActionResult> RegistrarUsuario(Usuario user)
        {
            try
            {
                if (await UsuarioExistente(user.Username)) throw new System.Exception("Nome de usuário já existe");

                Criptografia.CriaPasswordHash(user.PasswordString, out byte[] hash, out byte[] salt);
                user.PasswordString = string.Empty;
                user.PasswordHash = hash;
                user.PasswordSalt = salt;
                await _context.TB_USUARIOS.AddAsync(user);
                await _context.SaveChangesAsync();

                return Ok(user.Id);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        [HttpPost("Autenticar")]
        public async Task<IActionResult> AutenticarUsuario(Usuario credenciais)
        {
            try
            {
                Usuario? usuario = await _context.TB_USUARIOS.FirstOrDefaultAsync(x => x.Username.ToLower().Equals(credenciais.Username.ToLower()));
                if (usuario == null)
                {
                    throw new System.Exception("Usuário não encontrado.");
                }
                else if (!Criptografia.VerificarPasswordHash(credenciais.PasswordString, usuario.PasswordHash, usuario.PasswordSalt))
                {
                    throw new System.Exception("Senha incorreta.");
                }
                else
                {
                    //EXERCICIO 3 - Finalizado
                    usuario.DataAcesso = DateTime.Now;
                    await _context.SaveChangesAsync();
                    return Ok(usuario);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        
        
        
        
        
         ////EXERCICIO 1 - finalizado
        [HttpPut("AlterarSenha")]
        public async Task<IActionResult> Alterarsenha(Usuario credenciais){
            try{
                Usuario? usuario = await _context.TB_USUARIOS.FirstOrDefaultAsync(x=>x.Username.ToLower() == credenciais.Username.ToLower());            
            
            if (usuario == null)
            throw new System.Exception("Usuário não encontrado.");

       
        Criptografia.CriaPasswordHash(credenciais.PasswordString, out byte[] hash, out byte[] salt);

      
        usuario.PasswordHash = hash;
        usuario.PasswordSalt = salt;
        usuario.PasswordString = string.Empty;

        _context.TB_USUARIOS.Update(usuario);
        await _context.SaveChangesAsync();
        return Ok("Senha alterada com sucesso.");
            
            
            
            
            } catch (System.Exception ex)
    {
        return BadRequest(ex.Message);
    }


        }

         ////EXERCICIO 2 - CONCLUIDO
         [HttpGet("GetAll")]
          public async Task<IActionResult> GetAllUsers(){
            List<Usuario> usuarios = await _context.TB_USUARIOS.ToListAsync();
            if(usuarios == null)
                return BadRequest("registros nao encontrados");
            return Ok(usuarios);
          }
      

    }
}