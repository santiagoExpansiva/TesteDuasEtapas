using System;
using System.Threading.Tasks;
using Google.Authenticator;
using Microsoft.AspNetCore.Mvc;

namespace Autenticacao
{

    public class RetEnable
    {
        public String key { get; set; }

        public String img { get; set; }     

    }

    public class BodyParams
    {
        public String key { get; set; }

        public int id { get; set; }

        public String func { get; set; }

    }

   

    [ApiController]
    [Route("[controller]")]
    public class DuasEtapas :  ControllerBase
    {

        // Cria o qrCode com a key unica do usuario.
        [HttpGet]
        public async Task<IActionResult> Enable(int id)
        {

            Login user = null;
            GlobalClass.User.ForEach((i) =>
            {
                if (i.id == id) user = i;
            });
            if (user == null) return Ok("Erro, user invalido");

            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            // nome aplicativo, email do usuario, key unica do usuario
            var setupInfo = twoFactor.GenerateSetupCode("myapp", user.user, TwoFactorKey(user), false, 3);

            RetEnable ret = new RetEnable();
            ret.key = setupInfo.ManualEntryKey;
            ret.img = setupInfo.QrCodeSetupImageUrl;

            return Ok(ret);
            
        }

        [HttpPost]
        // criei um endpoint apenas para validar cadastrar e remover um parametro no BodyParams define o q deve ser feito, mas pode ser separados
        public async Task<IActionResult> Enable( [FromBody] BodyParams info)
        {
            Login user = null;
            GlobalClass.User.ForEach((i) =>
            {
                if (i.id == info.id) user = i;
            });
            if (user == null) return Ok("Erro, user invalido");

            TwoFactorAuthenticator twoFactor = new TwoFactorAuthenticator();
            // passa a key unica do usuario, o codigo informado, e o tempo que a chave é valida em minutos
            bool isValid = twoFactor.ValidateTwoFactorPIN(TwoFactorKey(user), info.key, TimeSpan.FromMinutes(0.30));
            if (!isValid)
            {
                return Ok("Erro, ao validar codigo");
            }

            if(info.func == "add")
            {

                GlobalClass.User.ForEach((i) =>
                {
                    if (i.id == info.id) i.useAutenticacao = "S";
                });
                return Ok("OK");

            }else if (info.func == "del")
            {

                GlobalClass.User.ForEach((i) =>
                {
                    if (i.id == info.id) i.useAutenticacao = "N";
                });
                return Ok("OK");

            }else
            {
                return Ok("OK");

            }


        }

        //Cria a Key usando o email do usuario a key tem q ter mais de 12 caracteres para ser valida.
        private static string TwoFactorKey(Login user)
        {
            return $"myverysecretkey+{user.user}";
        }

    }
}
