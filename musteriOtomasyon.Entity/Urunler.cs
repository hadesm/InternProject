using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Urunler", PrimaryColumn = "UrunID")]
    public class Urunler
    {
        [Atla]
        public int UrunID { get; set; }
        public int FirmaID { get; set; }
        public string UrunAdi { get; set; }
        public int Stok { get; set; }
        public string Durum { get; set; }
        public string Marka { get; set; }
        public decimal Fiyat { get; set; }
        public string UrunKodu { get; set; }
        public int KategoriID { get; set; }



    }
}
