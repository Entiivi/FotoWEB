using System.ComponentModel.DataAnnotations.Schema;

namespace FotoKlubasSvetaine.Server.Models
{
    [Table("klubas")]
    public class Klubas
    {
        public int KlubasID { get; set; }
        public string Salis { get; set; }
        public string Miestas { get; set; }
        public string Adresas { get; set; }
        public int? NariuSK { get; set; }
        public int? DarbuotojuSK { get; set; }
        public string DarboD { get; set; }
        public string Pavadinimas { get; set; }
    }
}
