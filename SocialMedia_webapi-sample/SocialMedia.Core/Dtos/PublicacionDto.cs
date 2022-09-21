using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Core.Dtos
{
    [DataContract(Name = "PublicacionDto")]
    public class PublicacionDto
    {
        public PublicacionDto()
        {            
        }


        [DataMember(Name = "IdPublicacion")]
        public int IdPublicacion { get; set; }
        
        [DataMember(Name = "IdUsuario")]
        public int IdUsuario { get; set; }

        [DataMember(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [DataMember(Name = "Descripcion")] 
        [Required]
        public string Descripcion { get; set; }

        [DataMember(Name = "Imagen")]
        public string Imagen { get; set; }

    }
}
