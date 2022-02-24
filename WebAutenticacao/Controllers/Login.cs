using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace Autenticacao
{

    public class Login
    {

        public int id { get; set; }
        public String user { get; set; }

        public String senha { get; set; }

        public String useAutenticacao { get; set; }

    }


    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
            
        }

        [HttpPost]
        public async Task<IActionResult> OnPostLogin([FromBody] Login person)
        {

            bool isValid = false;
            string msg = "Erro, Usuario invalido!";
            Login ret = null;
            GlobalClass.User.ForEach((us) =>
            {

                if(us.user == person.user && us.senha == person.senha)
                {
                    ret = us;
                    isValid = true;
                }else if (us.user == person.user && us.senha != person.senha)
                {
                    msg = "Erro, Senha invalida!";
                }

            });

            if (isValid && ret != null)
            {
                return Ok(ret);
            }
            else
            {
                return Ok(new Exception(msg).Message);
            }      

        }


    }

}

