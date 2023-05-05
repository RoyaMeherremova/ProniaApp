namespace Pronia.Models
{
    public class Bannner:BaseEntity
    {
        public string Image { get; set; }   

        public string Title { get; set; }

        public string Name { get; set; }

        public bool IsLarge { get; set; }=false;
    }
}
