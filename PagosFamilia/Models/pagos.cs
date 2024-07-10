using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PagosFamilia.Models
{
    public class pagos
    {
        public int id { get; set; }
        public decimal monto { get; set; }
        public DateTime fecha { get; set; }= DateTime.Now;
    }
}