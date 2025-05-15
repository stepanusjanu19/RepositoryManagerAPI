using System.ComponentModel.DataAnnotations;

namespace RepositoryManagerLib.Data
{
    public class RepositoryItem
    {
        [Key]
        public string ItemName { get; set; } = string.Empty;
        public string ItemContent { get; set; } = string.Empty;
        public int ItemType { get; set; }
    }
}