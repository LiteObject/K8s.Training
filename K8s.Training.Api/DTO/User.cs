using System.ComponentModel.DataAnnotations;

namespace K8s.Training.Api.DTO
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }

        [Phone]
        [StringLength(50)]
        public string PhoneNumber { get; set; }
    }
}
