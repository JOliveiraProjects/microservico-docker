using MicroServicos.Autentica.API.Enums;
using MicroServicos.Autentica.API.Interfaces;
using MicroServicos.Autentica.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MicroServicos.Autentica.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticaController : ControllerBase
    {
        private readonly IAutenticaService _autenticaService;

        public AutenticaController(IAutenticaService autenticaService)
        {
            _autenticaService = autenticaService;
        }

        /// <summary>
        /// Autentica para ter acesso as APIs.
        /// <remarks>
        /// Exemplo:
        ///
        ///
        /// </remarks>
        /// </summary>
        /// <param name="loginParam"></param>
        /// <response code="200">Retorna as informações do token privado</response>
        /// <response code="400">Se o token privado não for criado</response>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Post([FromBody] AutenticaViewModel model,
        [FromHeader(Name = "x-grant-type")] GrantType grantType = GrantType.Password,
        [FromHeader(Name = "x-token")] string token = null,
        [FromHeader(Name = "x-refresh-token")] string refreshToken = null)
        {
            ResultModel<TokenViewModel> result = new ResultModel<TokenViewModel>();

            if (grantType == GrantType.RefreshToken)
            {
                if(string.IsNullOrEmpty(token) && string.IsNullOrEmpty(refreshToken))
                {
                    result.Inconsistencias.Add("Precisa passar o token e o refresh token.");
                }
                else
                {
                    result = await _autenticaService.AtualizaToken(token, refreshToken);
                }
            }
            else if (grantType == GrantType.Password)
            {
                if (model == null || (string.IsNullOrEmpty(model.Usuario) && string.IsNullOrEmpty(model.Senha)))
                {
                    result.Inconsistencias.Add("Não preencheu os dados de autenticação.");
                }
                else
                {
                    result = await _autenticaService.Autenticar(model.Usuario, model.Senha);
                }
            }

            if (!result.Sucesso)
                return BadRequest(result);

            return Ok(result.Resultado);
        }
    }
}
