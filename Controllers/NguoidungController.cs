using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doancoso.Models;

namespace Doancoso.Content
{
    public class NguoidungController : Controller
    {
        dbQLThoitrangDataContext db = new dbQLThoitrangDataContext();
        //
        // GET: /Nguoidung/
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        // POST: Hàm Dangky(post) Nhập liệu từ trang Dangky và thực hiện tạo dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KHACHHANG kh)
        {
            //Gán giá trị người dùng nhập liệu cho các biến
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["MatKhauNhapLai"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải điền tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Phải điền mật khẩu";
            }
            else if (String.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi5"] = " Phải nhập email";
            }
            else if (String.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Nhập địa chỉ";
            }
            else if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Nhập số điện thoại";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới
                kh.HoTen = hoten;
                kh.Taikhoan = tendn;
                kh.Matkhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienthoaiKH = dienthoai;
                kh.Ngaysinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phai nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.Taikhoan == tendn && n.Matkhau == matkhau);
                if (kh != null)
                {
                    Session["Taikhoan"] = kh;
                    return RedirectToAction("Index", "TTrangStore");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }

            return View();
        }
	}
}