using CRUD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRUD.Data
{
    public class ViewModels
    {
        public IEnumerable<Usuario> listaUsuarios { get; set; }
        public string Sesion { get; set; }
        public string rol { get; set; }
        public Usuario usuario { get; set; }

    }
}
