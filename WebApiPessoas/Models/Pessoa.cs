using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiPessoas.Models
{
    public class Pessoa
    {
        private int _codigo;
        public int Codigo
        {
            get => _codigo;
            set => _codigo = value;
        }

        private string _nome;
        public string Nome
        {
            get => _nome;
            set => _nome = value?.Trim().ToUpper();
        }

        private string _uf;
        public string Uf
        {
            get => _uf;
            set => _uf = value?.Trim().ToUpper();
        }

        private string _cpf;
        public string Cpf
        {
            get => _cpf;
            set => _cpf = value?.Trim().ToUpper();
        }

        private string _dataNascimento;
        public string DataNascimento
        {
            get => _dataNascimento;
            set => _dataNascimento = value?.Trim().ToUpper();
        }
    }
}
