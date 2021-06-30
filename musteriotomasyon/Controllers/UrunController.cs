using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musteriotomasyon.Controllers
{
    public class UrunController : Controller
    {
        // GET: Urun
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
            else if (id == "4")
            {
                ViewBag.Hata = "GHataYok";
            }
            else if (id == "5")
            {
                ViewBag.Hata = "GHataVar";
            }

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            
            List<Urunler> urunler = UrunlerORM.Current.Select(" where FirmaID=?", parameters);
            return View(urunler);
        }
        public ActionResult DurumDegistir(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);

            //parameters.Add("@p2", fid);
            List<Urunler> urunler = UrunlerORM.Current.Select(" where UrunID=?", parameters);
            if (urunler.Any())
            {
                urunler[0].Durum = urunler[0].Durum == "1" ? "0" : "1";
                UrunlerORM.Current.Update(urunler[0]);
            }
            return RedirectToAction("Index");
        }
        public ActionResult UrunEkle(Urunler ur)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            if (HttpContext.Request.HttpMethod == "POST")
            {
                ur.FirmaID = frmList.FirmaID;
                ur.Durum = "1";
                bool a = UrunlerORM.Current.Insert(ur);
                if (a)
                {
                    return RedirectToAction("Index", new { id = "2" });
                }
                else
                {
                    return RedirectToAction("Index", new { id = "1" });
                }
            }
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            List<Kategori> kategori = KategoriORM.Current.Select(" where FirmaID=?", parameters);
            return View(kategori);
        }
        public ActionResult Kategori(string id)
        {
            if (id == "1")
            {
                ViewBag.Hata = "HataVar";
            }
            else if (id == "2")
            {
                ViewBag.Hata = "HataYok";
            }
            else if (id == "3")
            {
                ViewBag.Hata = "AyniVeri";
            }


            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            List<Kategori> kategori = KategoriORM.Current.Select(" where FirmaID=?", parameters);

            return View(kategori);
        }
        public ActionResult KategoriSil(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", id);
            List<Kategori> kategori = KategoriORM.Current.Select(" where  FirmaID=? and KategoriID=?", parameters);
            if (kategori.Any())
            {
                KategoriORM.Current.Delete(kategori[0].KategoriID);
            }
            return RedirectToAction("Kategori", new { id = 111 });
        }
        public ActionResult KategoriEkle(Kategori kt)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            if (HttpContext.Request.HttpMethod == "POST")
            {
                kt.Durum = "1";
                kt.FirmaID = frmList.FirmaID;

                Dictionary<string, object> kategoriadi = new Dictionary<string, object>();
                kategoriadi.Add("@p1", frmList.FirmaID);
                kategoriadi.Add("@p2", kt.KategoriAdi);

                List<Kategori> varmi = KategoriORM.Current.Select(" where FirmaID=? and KategoriAdi=?", kategoriadi);
                if (varmi.Any())
                {
                    return RedirectToAction("Kategori", new { id = "3" });
                }
                else
                {
                    bool a= KategoriORM.Current.Insert(kt);
                    if(!a)
                        {
                        return RedirectToAction("Kategori", new { id = "1" });
                    }
                    return RedirectToAction("Kategori", new { id = "2" });
                }
            }
            return View();
        }
        public ActionResult KDurumDegistir(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);
            List<Kategori> kategori = KategoriORM.Current.Select(" where KategoriID=?", parameters);
            if (kategori.Any())
            {
                kategori[0].Durum = kategori[0].Durum == "1" ? "0" : "1";
                KategoriORM.Current.Update(kategori[0]);
            }
            return RedirectToAction("Kategori", "Urun");
        }
        public ActionResult ZamUygula()
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            List<Kategori> kategori = KategoriORM.Current.Select(" where FirmaID=? AND Durum=1", parameters);
            List<Urunler> urunler = UrunlerORM.Current.Select(" where FirmaID=?", parameters);
            ViewBag.kategori = kategori;
            ViewBag.urunler = urunler;
            return View();
        }
        [HttpPost]
        public ActionResult KategoriZam(KategoriZam kt)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", kt.KategoriID);
            List<Urunler> urunler = UrunlerORM.Current.Select(" where FirmaID=? AND KategoriID=?", parameters);
            decimal zam = kt.ZamMiktari / 100;
            bool a;
            foreach (musteriOtomasyon.Entity.Urunler ur in urunler)
            {
                ur.Fiyat = ur.Fiyat + (ur.Fiyat * zam);
                a = UrunlerORM.Current.Update(ur);
                if (!a)
                {
                    return RedirectToAction("Index", new { id = "5" });
                }
                
            }
            return RedirectToAction("Index", new { id = "4" });
            
        }
        [HttpPost]
        public ActionResult UrunZam(UrunZam uz)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);
            parameters.Add("@p2", uz.UrunID);
            List<Urunler> urunler = UrunlerORM.Current.Select(" where FirmaID=? AND UrunId=?", parameters);
            decimal zam = uz.ZamMiktari / 100;
            foreach (musteriOtomasyon.Entity.Urunler ur in urunler)
            {    
                ur.Fiyat = ur.Fiyat + (ur.Fiyat * zam);
                bool a = UrunlerORM.Current.Update(ur);
                if (!a)
                {
                    return RedirectToAction("Index", new { id = "5" });
                }
            }
            return RedirectToAction("Index", new { id = "4" });
        }
        
    }
    
}