using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Satislar", PrimaryColumn = "SatisID")]
    public class Satislar
    {
        [Atla]
        public int SatisID { get; set; }

        public int FirmaID { get; set; }
        public int FaturaKodu { get; set; }

        public int MusteriID { get; set; }

        public decimal GenelToplam { get; set; }

        public int TaksitSayisi { get; set; }
        public string GarantiBitis { get; set; }
        [Atla]
        public DateTime Tarih { get; set; }

        [Atla]
        public string MusteriAdi { get; set; }
        [Atla]
        public string MusteriSoyadi { get; set; }
        [Atla]
        public string Ceptel { get; set; }
        [Atla]
        public string Adres { get; set; }
        [Atla]
        public string MusteriEmail { get; set; }

        
        public int KullaniciID { get; set; }
        [Atla]
       
        public string Adi { get; set; }
        [Atla]
        public string Soyadi { get; set; }


    }
}
