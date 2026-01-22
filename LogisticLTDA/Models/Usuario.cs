using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LogisticLTDA.Models
{
    public class Usuario
    {
        public int Codigo { get; set; }

        [Required(ErrorMessage ="O Campo do Nome é Obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage ="O campo de Senha é Obrigatório")]
        public string Senha { get; set; }

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="O Campo de CPF é Obrigatório")]
        [StringLength(11, ErrorMessage ="O CPF Deve ter 11 Caracteres")]
        public string CPF { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime? DataAtualizacao { get; set; }

        public DateTime? UltimoLogin { get; set; }

    }
}