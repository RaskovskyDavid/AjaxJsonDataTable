using AjaxJson.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AjaxJson.Models.ViewModels
{
    public class TablaPersonaViewModel
    {
        public int id { get; set; }
        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Nombre { get; set; }

        [Range(0,150)]
        public int Edad { get; set; }
        [CustomDateRangeAttribute]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime FechaDeNacimiento { get; set; }
    }
    
}