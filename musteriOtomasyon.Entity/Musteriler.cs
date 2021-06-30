using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{

    [Table(TableName = "Musteriler", PrimaryColumn = "MusteriID")]
    public class Musteriler
    {
        [Atla]
        public int MusteriID { get; set; }

        public int FirmaID { get; set; }
        public string MusteriAdi { get; set; }

        public string MusteriSoyadi { get; set; }

        public string MusteriEmail { get; set; }

        public string Adres { get; set; }

        public string Ceptel { get; set; }

        public string Evtel { get; set; }
        public string Durum { get; set; }

    }
}

