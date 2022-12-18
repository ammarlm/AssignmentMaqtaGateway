using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AssignmentMaqtaGateway3.Model
{
    public enum MaritalStatusEnum { Single, Married, Divorced }
    public enum GenderEnum { Male, Female }
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string FullName { get; set; }
        [Required]
        [MaxLength(500)]
        public string Address { get; set; }
        [Required]
        [MaxLength(14)]
        public string PhoneNumber { get; set; }
        [Required]
        [MaxLength(255)]
        public string Email { get; set; }
        //[Required]
        //[MaxLength(36)]
        //public string IdentificationNumber { get; set; }
        //public float Salary { get; set; }
        //public MaritalStatusEnum MaritalStatus { get; set; }
        //public GenderEnum Gender { get; set; }
    }
}
