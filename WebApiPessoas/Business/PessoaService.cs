using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiPessoas.Data;
using WebApiPessoas.Models;

namespace WebApiPessoas.Business
{
    public class PessoaService
    {
        private ApplicationDbContext _context;

        public PessoaService(ApplicationDbContext context)
        {
            _context = context;
        }

        public Pessoa ListarPorCodigo(int codigo)
        {
            if (codigo > 0)
            {
                return _context.Pessoas.Where(
                    p => p.Codigo == codigo).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public Pessoa ListarPorUf(string uf)
        {
            uf = uf?.Trim().ToUpper();
            if (!String.IsNullOrWhiteSpace(uf))
            {
                return _context.Pessoas.Where(
                    p => p.Uf == uf).FirstOrDefault();
            }

            return null;
        }

        public IEnumerable<Pessoa> ListarTodos()
        {
            return _context.Pessoas
                .OrderBy(p => p.Nome).ToList();
        }

        public Resultado Incluir(Pessoa pessoa)
        {
            Resultado resultado = DadosValidos(pessoa);
            resultado.Acao = "Inclusão de Pessoa";

            if (resultado.Inconsistencias.Count == 0 &&
                _context.Pessoas.Where(
                p => p.Codigo == pessoa.Codigo).Count() > 0)
            {
                resultado.Inconsistencias.Add("Código já cadastrado");
            }

            if (resultado.Inconsistencias.Count == 0)
            {
                _context.Pessoas.Add(pessoa);
                _context.SaveChanges();
            }

            return resultado;
        }

        public Resultado Atualizar(Pessoa dadosPessoa)
        {
            Resultado resultado = DadosValidos(dadosPessoa);
            resultado.Acao = "Atualização de Pessoa";

            if (resultado.Inconsistencias.Count == 0)
            {
                Pessoa pessoa = _context.Pessoas.Where(
                    p => p.Codigo == dadosPessoa.Codigo).FirstOrDefault();
                if (resultado == null)
                {
                    resultado.Inconsistencias.Add(
                        "Pessoa não encontrada");
                }
                else
                {
                    pessoa.Nome = dadosPessoa.Nome;
                    pessoa.Cpf = dadosPessoa.Cpf;
                    pessoa.Uf = dadosPessoa.Uf;
                    pessoa.DataNascimento = dadosPessoa.DataNascimento;
                    _context.SaveChanges();
                }
            }

            return resultado;
        }

        public Resultado Excluir(int codigo)
        {
            Resultado resultado = new Resultado();
            resultado.Acao = "Exclusão de Pessoa";

            Pessoa pessoa = ListarPorCodigo(codigo);
            if (pessoa == null)
            {
                resultado.Inconsistencias.Add("Pessoa não encontrada");
            }
            else
            {
                _context.Pessoas.Remove(pessoa);
                _context.SaveChanges();
            }
            return resultado;
        }

        private Resultado DadosValidos(Pessoa pessoa)
        {
            var resultado = new Resultado();

            if (pessoa == null)
            {
                resultado.Inconsistencias.Add(
                    "Preencha os Dados da Pessoa");
            }
            else
            {
                if (pessoa.Codigo <= 0)
                {
                    resultado.Inconsistencias.Add("Preencha o Código Maior que Zero");
                }
                else
                {
                    if (!ehNumero(pessoa.Codigo.ToString()))
                    {
                        resultado.Inconsistencias.Add("Preencha o Código Numérico");
                    }
                }
                
                if (String.IsNullOrWhiteSpace(pessoa.Nome))
                {
                    resultado.Inconsistencias.Add("Preencha o Nome da Pessoa");
                }
                if (!String.IsNullOrWhiteSpace(pessoa.Cpf))
                {
                    if (!ehCpf(pessoa.Cpf))
                    {
                        resultado.Inconsistencias.Add("Preencha um CPF Válido");
                    }
                }                
            }
            return resultado;
        }

        private static bool ehNumero(string valor)
        {
            int numero = 0;
            if(int.TryParse(valor, out numero))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ehCpf(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
    }
}
