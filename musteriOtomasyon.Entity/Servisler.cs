using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Servisler", PrimaryColumn = "ServisID")]
    public class Servisler
    {
        [Atla]
        public int ServisID { get; set; }

        public int FaturaKodu { get; set; }
        [Atla]
        public string MusteriAdi { get; set; }
        [Atla]
        public string MusteriSoyadi { get; set; }
        [Atla]
        public string Tarih { get; set; }
        public int UrunID { get; set; }
        public int FirmaID { get; set; }
        public int MusteriID { get; set; }
        public string Durum { get; set; }
        public DateTime ServisTarih { get; set; }
        public int SatisID { get; set; }
        public int Period { get; set; }
        public int ServisSayisi { get; set; }
        

    }
}
