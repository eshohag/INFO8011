using System.ComponentModel.DataAnnotations;

namespace ContactListApi.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("ContactListItems")]
    public class ContactListItem
    {
        [Key]
        public long Id { get; set; }
        [System.ComponentModel.DisplayName("First Name")]
        [Required]
        public string FirstName { get; set; }
        [System.ComponentModel.DisplayName("Last Name")]
        public string LastName { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        [System.ComponentModel.DisplayName("Job Title")]
        public string JobTitle { get; set; }
        public string Company { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
    }
}
