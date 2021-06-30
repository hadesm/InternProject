using DAUYoklama.Common;
using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace musteriotomasyon.Controllers
{
    public class KullaniciController : Controller
    {
        // GET: Kullanici
        public ActionResult Index(string id)
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

            return View();
        }
        [HttpPost]
        public ActionResult GirisYap(string Username, string Sifre, string FirmaKod)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", Username);
            parameters.Add("@p2", Sifre);
            parameters.Add("@p3", FirmaKod);
            List<Kullanici> users = KullaniciORM.Current.Select(" INNER JOIN Firmalar ON Kullanici.FirmaID = Firmalar.FirmaID where Username=? AND Sifre=? AND Firmalar.FirmaKod=?", parameters);
            if (users.Any() && users[0].Durum == "1")//kullanıcı var demektir
            {
                FormsAuthentication.RedirectFromLoginPage(string.Format("{0} {1}", users[0].Adi, users[0].Soyadi), false);
                Session["AktifPersonel"] = users[0];
                return RedirectToAction("Index", "Home");
            }
            else
            {
                
                return RedirectToAction("Index", new { id = 1 });
            }
        }
        [Authorize]
        public ActionResult Kullanicilar(string id)
        {
            if (id == "2")
            {
                ViewBag.Hata = "HataYok";
            }
            else if (id == "1")
            {
                ViewBag.Hata = "HataVar";
            }
            else if (id == "3")
            {
                ViewBag.Hata = "AyniVeri";
            }

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
           
            parameters.Add("@p1", frmList.FirmaID);
            List<Kullanici> users = KullaniciORM.Current.Select(" where FirmaID=?", parameters);
            return View(users);
        }
        public ActionResult DurumDegistir(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);
            
            //parameters.Add("@p2", fid);
            List<Kullanici> users = KullaniciORM.Current.Select(" where KullaniciID=?", parameters);
            if (users.Any())
            {
                users[0].Durum = users[0].Durum == "1" ? "0" : "1";
                KullaniciORM.Current.Update(users[0]);
            }
            return RedirectToAction("Kullanicilar");
        }
        public ActionResult KullaniciEkle()
        { 
           
            return View();
        }
        public ActionResult YeniKullanici(Kullanici kl)
        {

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);

            kl.Durum = "1";
            kl.FirmaID = frmList.FirmaID;
            //kl.Username = kl.Adi;
            Dictionary<string, object> kullanici = new Dictionary<string, object>();

            kullanici.Add("@p1", frmList.FirmaID);
            kullanici.Add("@p2", kl.Username);
            List<Kullanici> varmi = KullaniciORM.Current.Select(" where FirmaID=? AND Username=?", kullanici);
            if( varmi.Any())
            {
                return RedirectToAction("Kullanicilar", new { id = "3" });
            }
            bool a = KullaniciORM.Current.Insert(kl);
           
            return RedirectToAction("Kullanicilar" , new { id= (a==true? "2" : "1")});

        }
        public ActionResult Roller(string id)
        {
            if (id == "2")
            {
                ViewBag.Hata = "HataYok";
            }
            else if (id == "1")
            {
                ViewBag.Hata = "HataVar";
            }
            else if (id == "3")
            {
                ViewBag.Hata = "AyniVeri";
            }
            else if (id == "6")
            {
                ViewBag.Hata = "Silindi";
            }
            else if (id == "7")
            {
                ViewBag.Hata = "Silinmedi";
            }
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);

            List<Roller> roller = RollerORM.Current.Select(" where FirmaID=?", parameters);

            return View(roller);
        }
        public ActionResult RolEkle( Roller rl)
        {
            if (HttpContext.Request.HttpMethod == "POST")
            {
                Kullanici frmList = (Kullanici)Session["AktifPersonel"];
                rl.FirmaID = frmList.FirmaID;
                Dictionary<string, object> roller = new Dictionary<string, object>();
                roller.Add("@p1", frmList.FirmaID);
                roller.Add("@p2", rl.RolAdi);
                List<Roller> varmi = RollerORM.Current.Select(" where FirmaID=? AND RolAdi=?", roller);
                if (varmi.Any())
                {
                    return RedirectToAction("Roller", new { id = "3" });
                }
                bool a = RollerORM.Current.Insert(rl);

                return RedirectToAction("Roller", new { id = (a == true ? "2" : "1") });
            }
            return View();

        }
        public ActionResult RolSil(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];

            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", id);
            parameters.Add("@p2", frmList.FirmaID);


            List<Roller> roller = RollerORM.Current.Select(" where RolID =? AND FirmaID=? " , parameters);
            bool a = false;
                if (roller.Any())
            {
                a= RollerORM.Current.Delete(roller[0].RolID);

            }
               return RedirectToAction("Roller", new { id= (a==true? 6:7)});
        }
        public ActionResult KullaniciRolSil(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);
            parameters.Add("@p2", frmList.FirmaID);
            List<KullaniciRol> klroller = KullaniciRolORM.Current.Select(" INNER JOIN Kullanici on KullaniciRol.KullaniciID = Kullanici.KullaniciID INNER JOIN Roller on KullaniciRol.RolID = Roller.RolID where KullaniciRolID =? AND KullaniciRol.FirmaID=? ", parameters);
            bool a = false;
            if (klroller.Any())
            {
                a=KullaniciRolORM.Current.Delete(klroller[0].KullaniciRolID);
            }
            return RedirectToAction("KullaniciRol", new { id = (a == true ? "6" : "7") });
        }
        public ActionResult KullaniciRol(string id)
        {
            if (id == "2")
            {
                ViewBag.Hata = "HataYok";
            }
            else if (id == "1")
            {
                ViewBag.Hata = "HataVar";
            }
            else if (id == "6")
            {
                ViewBag.Hata = "Silindi";
            }
            else if (id == "7")
            {
                ViewBag.Hata = "Silinmedi";
            }
            else if (id == "3")
            {
                ViewBag.Hata = "AyniVeri";
            }

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);
            List<KullaniciRol> kroller = KullaniciRolORM.Current.Select(" INNER JOIN Kullanici on KullaniciRol.KullaniciID = Kullanici.KullaniciID INNER JOIN Roller on KullaniciRol.RolID = Roller.RolID where KullaniciRol.FirmaID=?", parameters);

              return View(kroller);
        }
        public ActionResult RolAta(KullaniciRol krol)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);


            List<Kullanici> kullanicilar = KullaniciORM.Current.Select(" where FirmaID=?",parameters);
            List<Roller> roller = RollerORM.Current.Select(" where FirmaID=?", parameters);
            ViewBag.kullanicilar = kullanicilar;
            ViewBag.roller = roller;

            if (HttpContext.Request.HttpMethod == "POST")
            {
                krol.FirmaID = frmList.FirmaID;
                Dictionary<string, object> rolata = new Dictionary<string, object>();
                rolata.Add("@p1", frmList.FirmaID);
                rolata.Add("@p2", krol.KullaniciID);
                rolata.Add("@p3", krol.RolID);

                List<KullaniciRol> varmi = KullaniciRolORM.Current.Select(" INNER JOIN Kullanici on KullaniciRol.KullaniciID = Kullanici.KullaniciID INNER JOIN Roller on KullaniciRol.RolID = Roller.RolID where KullaniciRol.FirmaID=? AND KullaniciRol.KullaniciID=? AND KullaniciRol.RolID=?", rolata);
                if (varmi.Any())
                {
                    return RedirectToAction("KullaniciRol", new { id = "3" });
                }
                bool a= KullaniciRolORM.Current.Insert(krol);
                return RedirectToAction("KullaniciRol", new { id = (a == true ? "2" : "1") });
            }
            return View();

        }
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Kullanici");


        }
        public ActionResult SifreYenile()
        {
            
            return View();

        }
        public ActionResult FirmaKayit()
        {

            return View();

        }

    }
}