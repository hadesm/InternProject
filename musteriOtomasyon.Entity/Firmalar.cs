using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Firmalar", PrimaryColumn = "FirmaID")]
    public class Firmalar
    {
        [Atla]
        public int FirmaID { get; set; }
        public string FirmaAdi { get; set; }
        [Atla]
        public string FirmaAdres { get; set; }
        public string Tel { get; set; }
        public string VergiNo { get; set; }
        public string FirmaKod { get; set; }
        public string FirmaExpDate { get; set; }
        public string Durum { get; set; }
        public string FirmaSifre { get; set; }

        public string FirmaEmail { get; set; }


    }
}
