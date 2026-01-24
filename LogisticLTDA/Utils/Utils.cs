using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace LogisticLTDA.Utils
{
    public class Utils
    {
        public static DateTime GetCurrentDate()
        {
            return DateTime.Now.Date;
        }

        public static TimeSpan GetCurrentTime()
        {
            return DateTime.Now.TimeOfDay;
        }

        public static DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }

        public static bool ValidarCpf(string cpf)
        {
            if (cpf.Length > 11 || cpf.Length < 11 || string.IsNullOrEmpty(cpf))
                return false;

            cpf = Regex.Replace(cpf, @"[^\d]", "");

            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;

            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}