namespace Pronia.Models
{
    public abstract class BaseEntity
    {
        public int Id { get; set; } 

        public bool SofDelete { get; set; } = false;

        public DateTime CreadtedDate { get; set;} = DateTime.Now;

        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}



