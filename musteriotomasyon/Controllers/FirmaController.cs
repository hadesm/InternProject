using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musteriotomasyon.Controllers
{
    public class FirmaController : Controller
    {
        // GET: Firma
        public ActionResult Index()
        {   
            return View();
        }

        public ActionResult FirmaKayit(Firmalar fr)
        {
            if (HttpContext.Request.HttpMethod == "POST")
            {
                Random rnd = new Random();
                int firmakod = rnd.Next(0, 999);
                fr.FirmaKod = firmakod.ToString();
                fr.Durum = "0";
                DateTime now = DateTime.Now;
                fr.FirmaExpDate = now.ToString();
                
                Dictionary<string, object> firmalar = new Dictionary<string, object>();
                firmalar.Add("@p1", fr.FirmaAdi);
                List<Firmalar> varmi = FirmalarORM.Current.Select(" where FirmaAdi=?", firmalar);
                if (varmi.Any())
                {
                    return RedirectToAction("Index", "Kullanici", new { id = "3" });
                }
                bool a = FirmalarORM.Current.Insert(fr);
                
                return RedirectToAction("Index","Kullanici", new { id = (a == true ? "2" : "1") });
            }


            return View();


        }
    }
}