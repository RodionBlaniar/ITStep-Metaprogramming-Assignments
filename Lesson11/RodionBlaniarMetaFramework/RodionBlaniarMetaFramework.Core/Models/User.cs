using RodionBlaniarMetaFramework.Core.Mapping;
using RodionBlaniarMetaFramework.Core.Validation;

namespace RodionBlaniarMetaFramework.Core.Models
{
    public class User
    {
        [Column("username")]
        [Required]
        [StringLength(3, 20)]
        public string Username { get; set; }

        [Column("email")]
        [Required]
        public string Email { get; set; }

        [Column("age")]
        [Range(1, 120)]
        public int Age { get; set; }

        [Ignore]
        public bool IsAdult => Age >= 18;
    }
}