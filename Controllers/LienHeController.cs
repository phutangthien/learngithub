using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doancoso.Models;
namespace Doancoso.Controllers
{
    public class LienHeController : Controller
    {
        dbQLThoitrangDataContext db = new dbQLThoitrangDataContext();
        //
        // GET: /LienHe/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Lienhe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Lienhe(FormCollection collection, LIENHE lh)
        {
            //Gán giá trị liên hệ nhập liệu cho các biến
            var hoten = collection["text"];
            var email = collection["email"];
            var noidung = collection["texta"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Email không được để trống";
            }
            else if (String.IsNullOrEmpty(noidung))
            {
                ViewData["Loi3"] = "Nội dung không được để trống";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới
                lh.TenKH = hoten;
                lh.Email = email;
                lh.Mota = noidung;
                db.LIENHEs.InsertOnSubmit(lh);
                db.SubmitChanges();
                return RedirectToAction("Lienhe");
            }
            return this.Lienhe();
        }
	}
}