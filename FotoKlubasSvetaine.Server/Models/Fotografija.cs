namespace FotoKlubasSvetaine.Server.Models
{
    public class Fotografija
    {
        public int FotoID { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime? Data { get; set; }
        public int NarysID { get; set; }
        public Narys Narys { get; set; }
        public int KlubasID { get; set; }
        public Klubas Klubas { get; set; }
        public byte[] FotoData { get; set; }
    }
}
