using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisputasController : Controller
    {
       private readonly DataContext _context;
       public DisputasController(DataContext context){
        _context = context;
       }

       [HttpPost("Arma")]
       public async Task<IActionResult> AtaqueComArmaAsync(Disputa d){
        try
        {

        }
        catch (System.Exception ex)
        {
            return BadRequest(ex.Message);
        }
       }
    }
}