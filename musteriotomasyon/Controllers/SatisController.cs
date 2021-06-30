using musteriotomasyon.ORM;
using musteriOtomasyon.Entity;
using musteriOtomasyon.ORM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text.pdf.draw;

namespace musteriotomasyon.Controllers
{
    public class SatisController : Controller
    {
        // GET: Satis
        public ActionResult Index()
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();

            parameters.Add("@p1", frmList.FirmaID);
            List<Satislar> satislar = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID= Musteriler.MusteriID INNER JOIN Kullanici ON Satislar.KullaniciID=Kullanici.KullaniciID where Satislar.FirmaID=?",parameters);
            ViewBag.satislar = satislar;


            return View();
        }
        
        public ActionResult SatisYap()

        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("@p1", frmList.FirmaID);

            ViewBag.Musteri = MusterilerORM.Current.Select(" where FirmaID=?", parameters);


            return View();
        }

        public JsonResult UrunleriGetir()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            parameters.Add("@p1", frmList.FirmaID);

            return this.Json(
            new
            {
                Result = (from obj in UrunlerORM.Current.Select(" where FirmaID=?", parameters)
                  select new
                          {
                              ID = obj.UrunID,
                              Marka = obj.Marka,
                              Ad = obj.UrunAdi,
                              Adet = obj.Stok,
                              AlisFiyat = obj.Fiyat,
                              Kdv = "12",
                              SatisFiyat = obj.Fiyat,
                              EklenmeTarih = obj.UrunKodu
                          })
            }, JsonRequestBehavior.AllowGet
            );
        }
        public ActionResult SatisIstatistikleri()

        {
            return View();
        }

        public ActionResult SatisDetay(int id)
        {
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, object> firma = new Dictionary<string, object>();

            
            parameters.Add("@p1", id);

            firma.Add("@p3", frmList.FirmaID);

            ViewBag.firma = FirmalarORM.Current.Select(" where FirmaID =?", firma);
            ViewBag.Satis = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID=Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID  where FaturaKodu=?", parameters);
            ViewBag.SatisDetay = SatisDetayORM.Current.Select(" INNER JOIN Urunler on SatisDetay.UrunID = Urunler.UrunID  where SatisDetay.FaturaKodu=?", parameters);
        

            return View();
        }


        PdfTemplate footerTemplate;
        PdfContentByte cb;
        public void satisfatura(int id)
        {

            //PDF dosyamızı temsil edecek nesnemizi oluşturuyoruz
            Document doc = new Document();

            // Türkçe Karakterlerini tanımlıyoruz
            BaseFont STF_Helvetica_Turkish = BaseFont.CreateFont("Helvetica", "CP1254", BaseFont.NOT_EMBEDDED);

            // Fontumuzu ayarlayıp , Türkçe karakter nesnesini ekliyoruz
            Font fontNormal = new Font(STF_Helvetica_Turkish, 12, Font.NORMAL);
            Font fontNormal1 = new Font(STF_Helvetica_Turkish, 12, Font.BOLD);
            Font fontNormal2 = new Font(STF_Helvetica_Turkish, 10, Font.BOLD);


            //PDF Dosyamıza ekliyeceğimiz metnimizi oluşturuyoruz
            Paragraph head = new Paragraph("BAŞLIK", fontNormal);

            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            Dictionary<string, object> firma = new Dictionary<string, object>();
            parameters.Add("@p1", id);
            firma.Add("@p3", frmList.FirmaID);

            List<Firmalar> firmabilgisi = FirmalarORM.Current.Select(" where FirmaID =?", firma);
            List<Satislar> musteribilgisi = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID= Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID = Kullanici.KullaniciID where FaturaKodu=?", parameters);
            List<SatisDetay> satisbilgisi = SatisDetayORM.Current.Select(" INNER JOIN Urunler on SatisDetay.UrunID = Urunler.UrunID where SatisDetay.FaturaKodu=?", parameters);
            Chunk glue = new Chunk(new VerticalPositionMark());
            LineSeparator line = new LineSeparator();

            Paragraph icerik = new Paragraph(firmabilgisi[0].FirmaAdi, fontNormal);
            icerik.Add(new Chunk(glue));
            icerik.Add("Fatura Kodu : #" + musteribilgisi[0].FaturaKodu);
            Paragraph firmaAdres = new Paragraph(firmabilgisi[0].FirmaAdres, fontNormal);
            firmaAdres.Add(new Chunk(glue));
            firmaAdres.Add("Tarih :" + musteribilgisi[0].Tarih);
            Paragraph firmaTel = new Paragraph(firmabilgisi[0].Tel + "\n \n", fontNormal);
            Paragraph musteriAdi = new Paragraph(musteribilgisi[0].MusteriAdi + " " + musteribilgisi[0].MusteriSoyadi, fontNormal);
            Paragraph musteriAdres = new Paragraph(musteribilgisi[0].Adres, fontNormal);
            Paragraph musteriEmail = new Paragraph(musteribilgisi[0].MusteriEmail, fontNormal);
            Paragraph baslik1 = new Paragraph("Müşteri :", fontNormal1);

            Paragraph bottom = new Paragraph(" ", fontNormal);
            bottom.Add(new Chunk(glue));
            bottom.Add("Ödemenmesi Gereken Toplam Tutar :");

            Paragraph bottom1 = new Paragraph(" ", fontNormal);
            bottom1.Add(new Chunk(glue));
            bottom1.Add(musteribilgisi[0].GenelToplam + " TL    ");
            int n = doc.PageNumber;
            for (int i = 1; i <= n; i++)
            {
                bottom1.Add(n.ToString());
            }
            // Tablo ekleyeceğimiz nesnemizi oluşturuyoruz
            PdfPTable table = new PdfPTable(3);// 2 sütundan oluşan tablo
            float[] width = { 100f };
            table.WidthPercentage = 100f;
            // Tablomıza içerik eklemek için nesnemizi oluşturuyoruz.
            PdfPCell cell = new PdfPCell(new Phrase("Ürün Adı ", fontNormal2));  // Tabloya içeriği yazıyoruz.
            table.AddCell(cell);  // Yazmış olduğumuz içeriği tablomuza ekliyoruz.

            cell = new PdfPCell(new Phrase("Adet", fontNormal2));
            table.AddCell(cell);

            cell = new PdfPCell(new Phrase("Fiyat", fontNormal2));
            table.AddCell(cell);




            foreach (SatisDetay st in satisbilgisi)
            {
                cell = new PdfPCell(new Phrase(st.UrunAdi, fontNormal));
                table.AddCell(cell);

                cell = new PdfPCell(new Phrase(st.Adet.ToString(), fontNormal));
                table.AddCell(cell);
                cell = new PdfPCell(new Phrase(st.Fiyat.ToString(), fontNormal));
                table.AddCell(cell);
            }

            //Dosya tipini PDF olarak belirtiyoruz.
            Response.ContentType = "application/pdf";

            // PDF Dosya ismini belirtiyoruz.
            //Response.AddHeader("content-disposition", "attachment;filename=Örnek_PDF.pdf");

            ////Sayfamızın cache'lenmesini kapatıyoruz
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);

            //PdfWriter PDF dosyamız ile stream'i eşitleyen class'ımız.
            PdfWriter.GetInstance(doc, Response.OutputStream);
            PdfWriter writer = PdfWriter.GetInstance(doc, Response.OutputStream);
            //Dosya işlemleri öncesinde değişiklikler için Open() methodunu çağırıyoruz.
            doc.Open();
            //Add() methodu ile en basit anlamda bir metni PDF dosyamızın içerisine ekliyoruz.
            doc.Add(head);
            doc.Add(new Paragraph("\n")); // Alt satır ekliyoruz
            doc.Add(icerik);
            doc.Add(firmaAdres);
            doc.Add(firmaTel);
            doc.Add(baslik1);
            doc.Add(musteriAdi);
            doc.Add(musteriEmail);
            doc.Add(new Paragraph("\n"));
            doc.Add(table);
            doc.Add(new Paragraph("\n"));
            doc.Add(bottom);
            doc.Add(bottom1);
            writer.PageEvent = new PageEventHelper();


            //Dosya işlemlerinin bittiğini belirtmek için Close() methodunu çağırıyoruz.
            doc.Close();

            //Dosyanın içeriğini Response içerisine aktarıyoruz.
            Response.Write(doc);

            //Son aşama da işlemleri bitirip, ekran çıktısına ulaşıyoruz.
            Response.End();

        }

        //public void SatisFatura(int id)
        //{
        //    //PDF dosyamızı temsil edecek nesnemizi oluşturuyoruz
        //    Document doc = new Document();

        //    // Türkçe Karakterlerini tanımlıyoruz 
        //    BaseFont STF_Helvetica_Turkish = BaseFont.CreateFont("Helvetica", "CP1254", BaseFont.NOT_EMBEDDED);

        //    // Fontumuzu ayarlayıp , Türkçe karakter nesnesini ekliyoruz 
        //    Font fontNormal = new Font(STF_Helvetica_Turkish, 13, Font.NORMAL);
        //    Font fontNormal1 = new Font(STF_Helvetica_Turkish, 13, Font.BOLD);
        //    Font fontNormal2 = new Font(STF_Helvetica_Turkish, 10, Font.BOLD);

        //    //PDF Dosyamıza ekliyeceğimiz metnimizi oluşturuyoruz
        //    Paragraph head = new Paragraph("FATURA DETAYLARI", fontNormal);

        //    Kullanici frmList = (Kullanici)Session["AktifPersonel"];
        //    Dictionary<string, object> parameters = new Dictionary<string, object>();
        //    Dictionary<string, object> firma = new Dictionary<string, object>();
        //    parameters.Add("@p1", id);
        //    firma.Add("@p3", frmList.FirmaID);

        //    List<Firmalar> firmabilgisi = FirmalarORM.Current.Select(" where FirmaID =?", firma);
        //    List<Satislar> musteribilgisi = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID=Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID where FaturaKodu=?", parameters);
        //    List<SatisDetay> satisbilgisi = SatisDetayORM.Current.Select(" INNER JOIN Urunler on SatisDetay.UrunID = Urunler.UrunID where SatisDetay.FaturaKodu=?", parameters);
        //    Chunk glue = new Chunk(new VerticalPositionMark());


        //    Paragraph icerik = new Paragraph(firmabilgisi[0].FirmaAdi, fontNormal);
        //    icerik.Add(new Chunk(glue));
        //    icerik.Add("Fatura Kodu : #" + musteribilgisi[0].FaturaKodu);
        //    Paragraph firmaAdres = new Paragraph(firmabilgisi[0].FirmaAdres, fontNormal);
        //    firmaAdres.Add(new Chunk(glue));
        //    firmaAdres.Add("Tarih: " + musteribilgisi[0].Tarih);
        //    Paragraph firmaTel = new Paragraph(firmabilgisi[0].Tel + "\n \n", fontNormal);
        //    Paragraph musteriAdi = new Paragraph(musteribilgisi[0].MusteriAdi + " " + musteribilgisi[0].MusteriSoyadi, fontNormal);
        //    musteriAdi.Add(new Chunk(glue));

        //    Paragraph musteriAdres = new Paragraph(musteribilgisi[0].Adres, fontNormal);
        //    Paragraph musteriEmail = new Paragraph(musteribilgisi[0].MusteriEmail, fontNormal);
        //    Paragraph baslik1 = new Paragraph("Müşteri :", fontNormal1);
        //    baslik1.Add(new Chunk(glue));

        //    Paragraph bottom = new Paragraph(" ", fontNormal);
        //    bottom.Add(new Chunk(glue));
        //    bottom.Add("Ödenmesi Gereken Toplam Tutar");

        //    Paragraph bottom1 = new Paragraph(" ", fontNormal);
        //    bottom1.Add(new Chunk(glue));

        //    bottom1.Add(musteribilgisi[0].GenelToplam + "   TL");


        //    // Tablo ekleyeceğimiz nesnemizi oluşturuyoruz 
        //    PdfPTable table = new PdfPTable(3);
        //    table.WidthPercentage=100f;

        //    // Tablomıza içerik eklemek için nesnemizi oluşturuyoruz. 
        //    PdfPCell cell = new PdfPCell(new Phrase("ÜRÜN ADI ", fontNormal));  // Tabloya içeriği yazıyoruz. 
        //    table.AddCell(cell);  // Yazmış olduğumuz içeriği tablomuza ekliyoruz.

        //    cell = new PdfPCell(new Phrase("ADET", fontNormal));
        //    table.AddCell(cell);

        //    cell = new PdfPCell(new Phrase("FİYAT", fontNormal));
        //    table.AddCell(cell);


        //    foreach(SatisDetay st in satisbilgisi)
        //    {
        //        cell = new PdfPCell(new Phrase(st.UrunAdi, fontNormal));
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase(st.Adet.ToString(), fontNormal));
        //        table.AddCell(cell);
        //        cell = new PdfPCell(new Phrase(st.Fiyat.ToString(), fontNormal));
        //        table.AddCell(cell);

        //    }


        //    //Dosya tipini PDF olarak belirtiyoruz.
        //    Response.ContentType = "application/pdf";

        //    //PdfWriter PDF dosyamız ile stream'i eşitleyen class'ımız.
        //    PdfWriter.GetInstance(doc, Response.OutputStream);

        //    //Dosya işlemleri öncesinde değişiklikler için Open() methodunu çağırıyoruz.
        //    doc.Open();

        //    //Add() methodu ile en basit anlamda bir metni PDF dosyamızın içerisine ekliyoruz.
        //    doc.Add(head);
        //    doc.Add(new Paragraph("\n")); // Alt satır ekliyoruz 
        //    doc.Add(icerik);
        //    doc.Add(firmaAdres);
        //    doc.Add(firmaTel);
        //    doc.Add(baslik1);
        //    doc.Add(musteriAdi);
        //    doc.Add(musteriEmail);


        //    doc.Add(new Paragraph("\n"));
        //    doc.Add(table);
        //    doc.Add(new Paragraph("\n"));
        //    doc.Add(bottom);
        //    doc.Add(bottom1);
        //    //Dosya işlemlerinin bittiğini belirtmek için Close() methodunu çağırıyoruz.
        //    doc.Close();

        //    //Dosyanın içeriğini Response içerisine aktarıyoruz.
        //    Response.Write(doc);

        //    //Son aşama da işlemleri bitirip, ekran çıktısına ulaşıyoruz.
        //    Response.End();

        //}

        [HttpPost]
        public JsonResult SatisEkle(Array[] data, string indirim, string musID, string tutar)
        {
            SatisDetay sts = new musteriOtomasyon.Entity.SatisDetay();
            Kullanici frmList = (Kullanici)Session["AktifPersonel"];
         
            Satislar satis = new musteriOtomasyon.Entity.Satislar();
            satis.FirmaID = frmList.FirmaID;
         
            try
            {   if (Convert.ToDecimal(tutar) <= 0)
                { return Json("0"); }
                satis.MusteriID = Convert.ToInt32(musID);
                satis.GarantiBitis = Convert.ToString(DateTime.Now);
                satis.GenelToplam = 0;
                satis.TaksitSayisi = 1;
                satis.KullaniciID = frmList.KullaniciID;
                List<Satislar> st = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID= Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID ORDER BY SatisID DESC", null);
                if (st.Any())
                {
                    satis.FaturaKodu = st[0].FaturaKodu + 1;
                }
                else
                {
                    satis.FaturaKodu = 1;
                }
                SatislarORM.Current.Insert(satis);
                int sayac = 0;
                sts.Toplam = Convert.ToDecimal(tutar);
                sts.FaturaKodu= satis.FaturaKodu ;
                
                for (int i = 0; i < data.Length; i++)
                {
                    foreach (var urun in data[i])
                    {
                        if (sayac == 0)
                            sts.UrunID = Convert.ToInt32(urun);
                        else if (sayac == 1)
                            sts.Adet = Convert.ToInt32(urun);
                        else if (sayac == 2)
                            sts.Fiyat = Convert.ToDecimal(urun);
                        sayac++;
                    }
                    sayac = 0;
                    Dictionary<string, object> parameters = new Dictionary<string, object>();
                    parameters.Add("@p1", sts.UrunID);
                    List<Urunler> urunler = UrunlerORM.Current.Select(" Where UrunID=?", parameters);
                    if(urunler.Any())
                    {
                        urunler[0].Stok = urunler[0].Stok - sts.Adet;
                        if(urunler[0].Stok < 0)
                        {
                            return Json("2");
                        }else
                        {
                            UrunlerORM.Current.Update(urunler[0]);
                        }
                    }
                     SatisDetayORM.Current.Insert(sts);
                }
                Dictionary<string, object> toplamguncelle = new Dictionary<string, object>();
                toplamguncelle.Add("@p1", sts.FaturaKodu);
                List<Satislar> ToplamG = SatislarORM.Current.Select(" INNER JOIN Musteriler on Satislar.MusteriID= Musteriler.MusteriID INNER JOIN Kullanici on Satislar.KullaniciID=Kullanici.KullaniciID Where FaturaKodu=?", toplamguncelle);
                ToplamG[0].GenelToplam = Convert.ToDecimal(tutar);
                SatislarORM.Current.Update(ToplamG[0]);
                return Json("1");
            }
            catch { return Json("0"); }
            
        }
    }
}