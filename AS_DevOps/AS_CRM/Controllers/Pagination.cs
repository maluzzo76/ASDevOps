using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AS_CRM.Models;
using System.Configuration;

namespace AS_CRM.Controllers
{
    public class Pagination<T> where T : class
    {
        private readonly int _RegistrosPorPagina = 7;

        public object paginado(IEnumerable<T> oList, int pagina)
        {
            int _TotalRegistros = 0;
            _TotalRegistros = oList.Count();

            var _TotalPaginas = (int)Math.Ceiling((double)_TotalRegistros / _RegistrosPorPagina);

            PaginadorGenerico<T> _Paginador = new PaginadorGenerico<T>()
            {
                RegistrosPorPagina = _RegistrosPorPagina,
                TotalRegistros = _TotalRegistros,
                TotalPaginas = _TotalPaginas,
                PaginaActual = pagina,
                Resultado = oList.Skip((pagina - 1) * _RegistrosPorPagina).Take(_RegistrosPorPagina).ToList()
            };

            return _Paginador;
        }
    }

    public class PaginadorGenerico<T> where T : class
    {
        public int PaginaActual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public IEnumerable<T> Resultado { get; set; }
    }
}