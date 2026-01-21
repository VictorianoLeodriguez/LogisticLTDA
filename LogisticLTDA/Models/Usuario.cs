using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LogisticLTDA.Models
{
    public class Usuario
    {
        public int Codigo { get; set; }

        public string Nome { get; set; }

        public string Senha { get; set; }

        public string Email { get; set; }

        public string cpf { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public DateTime? UltimoLogin { get; set; }

    }
}