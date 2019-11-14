using MicroServicos.Autentica.API.Models;
using System.Threading.Tasks;

namespace MicroServicos.Autentica.API.Interfaces
{
    public interface IAutenticaService
    {
        Task<ResultModel<TokenViewModel>> Autenticar(string username, string password);
        Task<ResultModel<TokenViewModel>> AtualizaToken(string token, string refreshToken);
    }
}
