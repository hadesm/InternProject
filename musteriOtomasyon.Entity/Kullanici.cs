using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Kullanici", PrimaryColumn = "KullaniciID")]
    public class Kullanici
    {
        [Atla]
        public int KullaniciID { get; set; }
        public int FirmaID { get; set; }
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Durum { get; set; }
        public string Username { get; set; }
        public string Sifre { get; set; }



     }
}
