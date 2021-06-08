using System.Collections.Generic;

namespace BaseProject.Models
{
    public class Category
    {
        // Id   Name        UserId  ReferenceId
        // 1    Kitaplar    1       0 ana kategori
        // 2    Kitaplar2   1       1
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public ICollection<Book> Books { get; set; }
        public int ReferenceId { get; set; }
    }
}