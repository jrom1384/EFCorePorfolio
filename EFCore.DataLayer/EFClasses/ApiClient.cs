using EFCore.Utilities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCore.DataLayer.EFClasses
{
    [Table("ApiClient")]
    public class ApiClient
    {
        public ApiClient()
        {

        }

        public ApiClient(long apiClientId, string lastName, string firstName, string username, string password)
        {
            this.ApiClientID = apiClientId;
            this.LastName = lastName;
            this.FirstName = firstName;
            this.Username = username;

            byte[] passwordHash;
            byte[] passwordSalt;
            HashHelper.Create(password, out passwordHash, out passwordSalt);
            this.PasswordHash = passwordHash;
            this.PasswordSalt = passwordSalt;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [Column("ApiClientID", Order = 1)]
        public long ApiClientID { get; set; }

        [Column("LastName", Order = 2)]
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Column("FirstName", Order = 3)]
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Column("Username", Order = 4)]
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Column("PasswordHash", Order = 5)]
        [Required]
        public byte[] PasswordHash { get; set; }

        [Column("PasswordSalt", Order = 6)]
        [Required]
        public byte[] PasswordSalt { get; set; }
    }
}
