using System.Runtime.Serialization;
using System.ComponentModel.DataAnnotations;
using SocialMedia.Core.Enumerations;

namespace SocialMedia.Core.Dtos
{
    [DataContract(Name = "SecurityDto")]
    public class SecurityDto
    {
        [DataMember(Name = "User")]
        public string User { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }
        
        [DataMember(Name = "Password")]
        public string Password { get; set; }
        
        [DataMember(Name = "Role")]
        public Roletype? Role    { get; set; }
    }
}