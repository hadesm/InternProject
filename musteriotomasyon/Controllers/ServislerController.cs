using musteriotomasyon.ORM;
using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musteriotomasyon.Controllers
{
    public class ServislerController : Controller
    {
        // GET: Servisler
        public ActionResult Index(string id)
        {
            if (id == "2")
            {
                ViewBag.Hata = "HataYok";
            }
            else if (id == "1")
            {
                ViewBag.Hata = "HataVar";

            }

            return View();
        }

        public JsonResult ServisleriListele()
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p2", frmList.FirmaID);
            return this.Json(
            new
            {
                Result = (from obj in ServislerORM.Current.Select(" INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=?", parameters)
                          select new
                          {
                              title = obj.MusteriAdi + " " + obj.MusteriSoyadi,
                              start = obj.ServisTarih.ToString("yyyy-MM-dd"),
                              durum = obj.Durum,
                              url = "/Servisler/Duzenle/" + obj.ServisID
                          })
            }, JsonRequestBehavior.AllowGet
            );
        }

        public ActionResult ServisEkle(int id, Servisler sr)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);
            parameters.Add("@p2", frmList.FirmaID);
            Dictionary<string, object> Fatura = new Dictionary<string, object>();
            Fatura.Add("@p1", id);
            ViewBag.Musteri = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID=Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID where FaturaKodu=?", Fatura);
            ViewBag.Urunler = SatisDetayORM.Current.Select(" INNER JOIN Urunler on SatisDetay.UrunID = Urunler.UrunID where SatisDetay.FaturaKodu=? AND Urunler.FirmaID=?", parameters);
            return View();
        }

        [HttpPost]
        public ActionResult YeniServis(Servisler sr)
        {

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            Dictionary<string, object> Fatura = new Dictionary<string, object>();
            Fatura.Add("@p1", sr.FaturaKodu);
            List<Satislar> satisbilgileri = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID=Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID where FaturaKodu=?", Fatura);
            sr.Durum = "0";
            sr.FirmaID = frmList.FirmaID;
            for (int i = sr.ServisSayisi; i > 0; i--)
            {

                DateTime trh = Convert.ToDateTime(sr.Tarih);

                sr.ServisTarih = trh.AddMonths(sr.Period);
                ServislerORM.Current.Insert(sr);
                sr.Tarih = Convert.ToString(sr.ServisTarih);

            }
            return RedirectToAction("Index", "Servisler");
        }

        public ActionResult Duzenle(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", id);
            ViewBag.Servis = ServislerORM.Current.Select(" INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=? AND Servisler.ServisID=?",parameters);
            
            List <Servisler> UrunID = ServislerORM.Current.Select(" INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=? AND Servisler.ServisID=?", parameters);
            Dictionary<string, object> SecilenUrun = new Dictionary<string, object>();
            SecilenUrun.Add("@p1", UrunID[0].UrunID);

            ViewBag.Urun = UrunlerORM.Current.Select(" where UrunID=? ", SecilenUrun);
            return View();
        }

        [HttpPost]
        public ActionResult ServisGuncelle(Servisler sr)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", sr.ServisID);
            List<Servisler> servis = ServislerORM.Current.Select(" INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=? AND Servisler.ServisID=?", parameters);
            if (servis.Any())
            {
                if (sr.ServisTarih.Date >= DateTime.Now.Date)
                {
                    servis[0].ServisTarih = sr.ServisTarih;
                    bool a = ServislerORM.Current.Update(servis[0]);
                    if (a)
                    {
                        return RedirectToAction("Index", new { id = "2" });
                    }
                    else
                    {
                        return RedirectToAction("Index", new { id = "1" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", new { id = "1" });
                    }
                
            }

            return RedirectToAction("Index", new { id = "1" });

        }
        public ActionResult DurumDegistir(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", id);
            List <Servisler> servis= ServislerORM.Current.Select(" INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=? AND Servisler.ServisID=?", parameters);
            if(servis.Any())
            {
                servis[0].Durum = servis[0].Durum == "1" ? "0" : "1";
                ServislerORM.Current.Update(servis[0]);

            }

            return RedirectToAction("Index/", "Servisler");
        }
    }

}