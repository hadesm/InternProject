using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace musteriotomasyon.Controllers
{
    public class MusterilerController : Controller
    {
        // GET: Musteriler
        public ActionResult Index(string id)
        {
            if (id == "1")
            { ViewBag.Hata = "HataVar"; }
            else if (id == "2")
            { ViewBag.Hata = "HataYok"; }


            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);



            List<Musteriler> musteriler = MusterilerORM.Current.Select(" where FirmaID=?", parameters);
            return View(musteriler);

        }

        public ActionResult DurumDegistir(string id)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", id);


            List<Musteriler> musteriler = MusterilerORM.Current.Select(" where MusteriID=?", parameters);
            if (musteriler.Any())
            {
                musteriler[0].Durum = musteriler[0].Durum == "1" ? "0" : "1";
                MusterilerORM.Current.Update(musteriler[0]);
            }
            return RedirectToAction("Index");
        }
        public ActionResult MusteriEkle(Musteriler ms)
        {

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];

            if (HttpContext.Request.HttpMethod == "POST")
            {
                ms.FirmaID = frmList.FirmaID;
                ms.Durum = "1";
                bool a= MusterilerORM.Current.Insert(ms);
                return RedirectToAction("Index", new { id=(a==true ? "2": "1")});
            }
            return View();
        }
    }

}