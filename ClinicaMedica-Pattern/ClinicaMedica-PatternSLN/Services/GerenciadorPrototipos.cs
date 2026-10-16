using ClinicaMedica_PatternSLN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica_PatternSLN.Services
{
    public class GerenciadorPrototipos
    {
        private Dictionary<string, PacoteExame> _prototipos = new Dictionary<string, PacoteExame>();

        public void CadastrarPrototipo(string chave, PacoteExame prototipo)
        {
            _prototipos[chave] = prototipo;
        }

        public PacoteExame ObterClone(string chave)
        {
            if (_prototipos.ContainsKey(chave))
            {
                // Busca o modelo original e chama o nosso método Clonar() manual
                PacoteExame modeloOriginal = _prototipos[chave];
                return modeloOriginal.Clonar();
            }

            return null;
        }
    }
}
