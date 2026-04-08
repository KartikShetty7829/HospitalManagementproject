using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Auth_Service.Entity
{
    [Table("Roles")]
    public class Roles
    {
        [Key]
        [Column("RoleId")]
        public int RoleId { get; set; }

        [Required]
        [Column("RoleName")]
        [MaxLength(50)]
        public required string RoleName { get; set; }

        public ICollection<Users> Users { get; set; }
    }

}
