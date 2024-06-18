namespace FotoKlubasSvetaine.Server.Models
{
    public class Narys
    {
        public int NarysID { get; set; }
        public string Vardas { get; set; }
        public string Pavarde { get; set; }
        public string Elpas { get; set; }
        public string TelNR { get; set; }
        public int Naryste { get; set; }
        public DateTime PrisijungimoDAT { get; set; }
        public int KlubasID { get; set; }
        public Klubas Klubas { get; set; }
        public string Slap { get; set; }
        public string Username { get; set; }
        public int Administratorius_AdministratoriusID { get; set; }
        public Administratorius Administratorius { get; set; }
    }
}
