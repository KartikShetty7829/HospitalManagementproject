using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;

namespace Auth_Service.Entity
{
    [Table("Users")]
    public class Users
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [Required]
        [Column("Username")]
        [MaxLength(50)]
        public required string Username { get; set; }

        [Required]
        [Column("PasswordHash")]
        [MaxLength(255)]
        public required string PasswordHash { get; set; }

        [Column("Email")]
        [MaxLength(100)]
        public required string Email { get; set; }

        [ForeignKey("RoleId")]
        public required int RoleId { get; set; }

        [Column("CreatedAt")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation property
        public required Roles Role { get; set; }
    }

}
