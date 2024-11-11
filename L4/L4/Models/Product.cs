using SQLite;
using System.ComponentModel.DataAnnotations;

namespace L4.Models
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
      
        public int Code { get; set;}

        [System.ComponentModel.DataAnnotations.MaxLength(50)] // zmapuje na nvarchar(50)
        public string Title { get; set; }
        public string Description { get; set; }

        public string Barcode { get; set; }

        public double Price { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}
