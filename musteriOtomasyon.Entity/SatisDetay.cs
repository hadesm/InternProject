using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "SatisDetay", PrimaryColumn = "SatisKodu")]
    public class SatisDetay
    {
        [Atla]
        public int SatisKodu { get; set; }
        public int SatisID { get; set; }
        public int FaturaKodu { get; set; }
        [Atla]
        public int FirmaID { get; set; }
        public int UrunID { get; set; }
        public int Adet { get; set; }
        public decimal Fiyat { get; set; }
        public decimal Toplam { get; set; }
        [Atla]
        public string UrunAdi { get; set; }
    }
}
