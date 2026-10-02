using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaMedica.Interfaces
{
    public interface INotificavel
    {
        void EnviarNotificacao(string mensagem);
    }
}
