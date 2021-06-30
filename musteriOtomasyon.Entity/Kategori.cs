using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Kategori", PrimaryColumn = "KategoriID")]
    public class Kategori
    {
        [Atla]
        public int KategoriID { get; set; }
        public int FirmaID { get; set; }
        public string KategoriAdi { get; set; }

        public string Durum { get; set; }



    }
}
