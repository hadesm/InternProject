using DAUYoklama.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace musteriOtomasyon.Entity
{
    [Table(TableName = "Roller", PrimaryColumn = "RolID")]
    public class Roller
    {       [Atla]
            public int RolID { get; set; }
            public string RolAdi { get; set; }
            public int FirmaID { get; set; }



    }
    }

