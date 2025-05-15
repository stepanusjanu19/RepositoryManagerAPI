using System.ComponentModel.DataAnnotations;

namespace RepositoryManagerAPI.DTO
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "ItemName is required")]
        public string ItemName { get; set; } = string.Empty;

        [Required(ErrorMessage = "ItemContent is required")]
        public string ItemContent { get; set; } = string.Empty;

        [Range(1, 2, ErrorMessage = "ItemType must be a answer 1 : (JSON) OR 2 : (XML)")]
        public int ItemType { get; set; }
    }
}

