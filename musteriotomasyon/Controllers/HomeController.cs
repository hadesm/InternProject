using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musteriotomasyon.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);
            Dictionary<string, object> servis = new Dictionary<string, object>();

            servis.Add("@p1", frmList.FirmaID);
            servis.Add("@p2", "0");

            ViewBag.musteriler = MusterilerORM.Current.Select(" where FirmaID=?", parameters).Count;
            ViewBag.Servisler = ServislerORM.Current.Select("  INNER JOIN Satislar on Servisler.FaturaKodu = Satislar.FaturaKodu INNER JOIN Musteriler on Servisler.MusteriID= Musteriler.MusteriID where Servisler.FirmaID=? AND Servisler.Durum=?", servis);
            return View();
        }

        public JsonResult SatislariGetir()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            parameters.Add("@p1", frmList.FirmaID);

            return this.Json(
            new
            {
                Result = (from obj in SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID= Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID where Satislar.FirmaID=?", parameters)
                          select new
                          {
                              //ID = obj.UrunID,
                              Musteri = obj.MusteriAdi + " " +obj.MusteriSoyadi,
                              GenelToplam =  Convert.ToString(obj.GenelToplam),
                              Tarih =Convert.ToString(obj.Tarih.ToString("yyyy-MM-dd")),
                              GarantiBitis = obj.GarantiBitis,
                              DetayID = obj.FaturaKodu
                          })
            }, JsonRequestBehavior.AllowGet
            );




        }
    }
}