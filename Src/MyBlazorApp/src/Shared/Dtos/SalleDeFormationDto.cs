namespace Shared.Dtos
{
    public class SalleDeFormationDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Formateur { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime DateFin { get; set; }
    }
}
