using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TalentManager.Models
{
    [DataContract]
    public class Employee
    {
        public Employee()
        {
        }
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Department { get; set; }
    }
}