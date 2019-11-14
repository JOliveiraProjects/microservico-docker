using MicroServicos.Autentica.API.Data;
using MicroServicos.Autentica.API.Interfaces;
using MicroServicos.Autentica.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MicroServicos.Autentica.API.Repository
{
    public class AutenticaRepository : IAutenticaRepository, IDisposable
    {
        private readonly BancoContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AutenticaRepository(IPasswordHasher passwordHasher, BancoContext context)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task AddUsuario(AutenticaModel model)
        {
            try
            {
                model.Senha = _passwordHasher.GenerateIdentityV3Hash(model.Senha);
                await _context.Autentica.AddAsync(model);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro para salvar o cadastro! {ex.Message}");
            }
        }

        public async Task UpdateUsuario(AutenticaModel usuario)
        {
            try
            {
                AutenticaModel model = await GetUsuarioById(usuario.UsuarioId);
                model.Ativo = usuario.Ativo;
                model.Senha = usuario.Senha;
                model.RefreshToken = usuario.RefreshToken;

                _context.Attach(model);
                _context.Entry(model).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro para salvar o cadastro! {ex.Message}");
            }
        }

        private Task<IList<AutenticaModel>> GetUsuariosAsync()
        {
            TaskCompletionSource<IList<AutenticaModel>> tcs = new TaskCompletionSource<IList<AutenticaModel>>();
            Task.Run(() =>
            {
                tcs.SetResult((from a in _context.Autentica
                               where a.Ativo
                               select a).ToList());
            });
            return tcs.Task;
        }

        public async Task<IList<AutenticaModel>> GetUsuarios()
        {
            try
            {
                return await GetUsuariosAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao listar dos logs! {ex.Message}");
            }
        }

        public async Task<AutenticaModel> GetUsuario(Expression<Func<AutenticaModel, bool>> predicate)
        {
            try
            {
                return await _context.Autentica.FirstOrDefaultAsync(predicate);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao retornar o usuário! {ex.Message}");
            }
        }

        public async Task<AutenticaModel> GetUsuario(string username, string password)
        {
            try
            {
                return await (from a in _context.Autentica
                              where a.Ativo &&
                              a.Nome.Equals(username) &&
                              a.Senha.Equals(password)
                              select new AutenticaModel
                              {
                                  UsuarioId = a.UsuarioId,
                                  Nome = a.Nome,
                                  Ativo = a.Ativo,
                                  DataCadastro = a.DataCadastro
                              }).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao retornar o usuário! {ex.Message}");
            }
        }

        public async Task<AutenticaModel> GetUsuarioById(int usuarioId)
        {
            try
            {
                return await _context.Autentica.FirstOrDefaultAsync(w => w.UsuarioId == usuarioId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao retornar os logs! {ex.Message}");
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
