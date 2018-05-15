using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Data;
using System.Data.SqlClient;
using Doancoso.Models;
using System.IO;
using System.Data.Entity;

namespace Doancoso.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        dbQLThoitrangDataContext db = new dbQLThoitrangDataContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sanpham(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
    
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MaSanPham).ToPagedList(pageNumber, pageSize));    
        }
        public ActionResult login()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            ViewBag.MaCL = new SelectList(db.CHUNGLOAIs.ToList().OrderBy(n => n.TenCL), "MaCL", "TenCL");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisanpham(SANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            ViewBag.MaCL = new SelectList(db.CHUNGLOAIs.ToList().OrderBy(n => n.TenCL), "MaCL", "TenCL");
            if (fileUpload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }
                    sanpham.Anh = fileName;
                    db.SANPHAMs.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Sanpham");
        }
        //Hiển thị sản phẩm
        public ActionResult Chitietsanpham(int id)
        {
            //Lay ra doi tuong sach theo ma
            SANPHAM SANPHAM = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSANPHAM = SANPHAM.MaSanPham;
            if (SANPHAM == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(SANPHAM);
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập or mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Xoasanpham(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SANPHAM sanpham= db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = sanpham.MaSanPham;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }
        [HttpPost, ActionName("XoaSanPham")]
        public ActionResult Xacnhanxoa(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            ViewBag.MaSanPham = sanpham.MaSanPham;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Sanpham");
        }
        [HttpGet]
        public ActionResult Suasanpham(int id)
        {
            SANPHAM sanpham = db.SANPHAMs.SingleOrDefault(n => n.MaSanPham == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào dropdownlist
            ViewBag.MaCL = new SelectList(db.CHUNGLOAIs.ToList().OrderBy(n => n.TenCL), "MaCL", "TenCL", sanpham.MaCL);
            return View(sanpham);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suasanpham(SANPHAM sanpham, HttpPostedFileBase fileupload)
        {
            //Đưa dữ liệu vào dropdownload
            //ViewBag.MaCL = new SelectList(db.CHUNGLOAIs.ToList().OrderBy(n => n.TenCL), "MaCL", "TenCL");
            //if (ModelState.IsValid)
            //{
            //    UpdateModel(sanpham);
            //    db.SubmitChanges();
            //}
            //return RedirectToAction("Sanpham");

            ViewBag.MaCL = new SelectList(db.CHUNGLOAIs.ToList().OrderBy(n => n.TenCL), "MaCL", "TenCL");
            //Kiểm tra đường dẫn vào file
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            //Thêm CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file, lưu ý bổ sung thêm thư viện using System.IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiểm tra hình ảnh tồn tại chưa?
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hinh ảnh đã tồn tại";
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn
                        fileupload.SaveAs(path);
                    }
                    sanpham.Anh = fileName;
                    //Luu vào CSDL
                     UpdateModel(sanpham);
                    db.SubmitChanges();
                }
                return RedirectToAction("sanpham");
            }
        }
        public ActionResult Donhang(int ?page)
        {
           
             int pageNumber = (page ?? 1);
             int pageSize = 10;
             return View(db.CHITIETDONHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(pageNumber, pageSize));
        }

        //public ActionResult Chitietdonhang(int id)
        //{
        //    //Lay ra doi tuong sach theo ma
        //    CHITIETDONHANG ctdh = db.CHITIETDONHANGs.SingleOrDefault(n => n.MaDonHang == id);
        //    ViewBag.MaDonHang = ctdh.MaDonHang;
        //    if (ctdh == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    return View(ctdh);
        //}
        //[HttpGet]
        //public ActionResult Xoadonhang(int id)
        //{
        //    //Lay ra doi tuong sach can xoa theo ma
        //    CHITIETDONHANG ctdh = db.CHITIETDONHANGs.SingleOrDefault(n => n.MaDonHang == id);
        //    ViewBag.MaDonHang = ctdh.MaDonHang;
        //      if (ctdh == null)
        //        {
        //            Response.StatusCode = 404;
        //            return null;
        //        }
        //    return View(ctdh);
        //}
        //[HttpPost, ActionName("XoaDonhang")]
        //public ActionResult Xacnhanxoadonhang(int id)
        //{
        //    //Lay ra doi tuong sach can xoa theo ma
        //    CHITIETDONHANG ctdh = db.CHITIETDONHANGs.SingleOrDefault(n => n.MaDonHang == id);
        //    ViewBag.MaDonHang = ctdh.MaDonHang;
        //    if (ctdh == null)
        //    {
        //        Response.StatusCode = 404;
        //        return null;
        //    }
        //    db.CHITIETDONHANGs.DeleteOnSubmit(ctdh);
        //    db.SubmitChanges();
        //    return RedirectToAction("Donhang");
        //}
        public ActionResult Nguoidung(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
            //return View(db.KHACHHANGs.ToList());
        }
        [HttpGet]
        public ActionResult Themmoinguoidung()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoinguoidung(KHACHHANG nguoidung, HttpPostedFileBase fileUpload)
        {
            {
                db.KHACHHANGs.InsertOnSubmit(nguoidung);
                db.SubmitChanges();
            }
            
            return RedirectToAction("Nguoidung");
        }
        //Hiển thị nguoidung
        public ActionResult Chitietnguoidung(int id)
        {
            //Lay ra doi tuong sach theo ma
            KHACHHANG nguoidung = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = nguoidung.MaKH;
            if (nguoidung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nguoidung);
        }
        [HttpGet]
        public ActionResult Xoanguoidung(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHHANG nguoidung = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = nguoidung.MaKH;
            if (nguoidung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nguoidung);
        }
        [HttpPost, ActionName("Xoanguoidung")]
        public ActionResult Xacnhanxoanguoidung(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            KHACHHANG nguoidung = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            ViewBag.MaKH = nguoidung.MaKH;
            if (nguoidung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KHACHHANGs.DeleteOnSubmit(nguoidung);
            db.SubmitChanges();
            return RedirectToAction("nguoidung");
        }
        [HttpGet]
        public ActionResult Suanguoidung(int id)
        {
            KHACHHANG nguoidung = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (nguoidung == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Đưa dữ liệu vào dropdownlist
            return View(nguoidung);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Suanguoidung(KHACHHANG nguoidung,FormCollection f)
        {
            if (ModelState.IsValid)
            {
                
                UpdateModel(nguoidung);
                db.SubmitChanges();
            }
            return RedirectToAction("Nguoidung");
            
        }
        public ActionResult Chungloai(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.CHUNGLOAIs.ToList().OrderBy(n => n.MaCL).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult Themmoichungloai()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoichungloai(CHUNGLOAI chungloai)
        {
            {
                db.CHUNGLOAIs.InsertOnSubmit(chungloai);
                db.SubmitChanges();
            }

            return RedirectToAction("Chungloai");
        }
        [HttpGet]
        public ActionResult Xoachungloai(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            CHUNGLOAI chungloai = db.CHUNGLOAIs.SingleOrDefault(n => n.MaCL == id);
            ViewBag.MaKH = chungloai.MaCL;
            if (chungloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chungloai);
        }
        [HttpPost, ActionName("Xoachungloai")]
        public ActionResult Xacnhanxoachungloai(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            CHUNGLOAI chungloai = db.CHUNGLOAIs.SingleOrDefault(n => n.MaCL == id);
            ViewBag.MaCL = chungloai.MaCL;
            if (chungloai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.CHUNGLOAIs.DeleteOnSubmit(chungloai);
            db.SubmitChanges();
            return RedirectToAction("Chungloai");
        }
        public ActionResult Lienhe(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            return View(db.LIENHEs.ToList().OrderBy(n => n.MaLH).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Chitietlienhe(int id)
        {
            //Lay ra doi tuong sach theo ma
            LIENHE lienhe = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
            ViewBag.MaLH = lienhe.MaLH;
            if (lienhe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lienhe);
        }
        [HttpGet]
        public ActionResult Xoalienhe(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LIENHE lienhe = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
            ViewBag.MaLH = lienhe.MaLH;
            if (lienhe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lienhe);
        }
        [HttpPost, ActionName("Xoalienhe")]
        public ActionResult Xacnhanxoalienhe(int id)
        {
            //Lay ra doi tuong sach can xoa theo ma
            LIENHE lienhe = db.LIENHEs.SingleOrDefault(n => n.MaLH == id);
            ViewBag.MaLH = lienhe.MaLH;
            if (lienhe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LIENHEs.DeleteOnSubmit(lienhe);
            db.SubmitChanges();
            return RedirectToAction("Lienhe");
        }
    }
}