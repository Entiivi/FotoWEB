namespace FotoKlubasSvetaine.DTOs
{
    public class FotografijaDto
    {
        public int FotoID { get; set; }
        public string Pavadinimas { get; set; }
        public string Aprasymas { get; set; }
        public DateTime? Data { get; set; }  // Date as string
        public int NarysID { get; set; }
        public int KlubasID { get; set; }
        public string FotoData { get; set; }  // Base64 string
    }
}
