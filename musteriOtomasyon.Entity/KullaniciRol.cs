using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "KullaniciRol", PrimaryColumn = "KullaniciRolID")]
    public class KullaniciRol
    {
        public int FirmaID { get; set; }
        
        public int KullaniciID { get; set; }
        public int RolID { get; set; }
        [Atla]
        public int KullaniciRolID { get; set; }
        [Atla]
        public string Adi { get; set; }
        [Atla]
        public string Soyadi { get; set; }
        [Atla]
        public string Username { get; set; }
        [Atla]
        public string RolAdi { get; set; }

    }
}
