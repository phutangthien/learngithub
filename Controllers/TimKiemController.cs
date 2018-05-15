using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Doancoso.Models;

using PagedList;
using PagedList.Mvc;

namespace Doancoso.Controllers
{
    public class TimKiemController : Controller
    {
        //
        // GET: /TimKiem/
        dbQLThoitrangDataContext db = new dbQLThoitrangDataContext();
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f,int? page)
        {
            String sTuKhoa = f["txtTimKiem"].ToString();
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> lstKQTK = db.SANPHAMs.Where(n => n.TenSanPham.Contains(sTuKhoa)).ToList();
            int pageSize = 8;
            int pageNumber = (page ?? 1); ;
            if (lstKQTK.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào";
                return View(db.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber,pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " Kết quả!";
            return View(lstKQTK.OrderBy(n=>n.TenSanPham).ToPagedList(pageNumber,pageSize));
        }
        [HttpGet]
        public ActionResult KetQuaTimKiem(String sTuKhoa, int? page)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<SANPHAM> lstKQTK = db.SANPHAMs.Where(n => n.TenSanPham.Contains(sTuKhoa)).ToList();
            int pageSize = 8;
            int pageNumber = (page ?? 1); ;
            if (lstKQTK.Count == 0)
            {
                ViewBag.Thongbao = "Không tìm thấy sản phẩm nào";
                return View(db.SANPHAMs.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
            }
            ViewBag.ThongBao = "Đã tìm thấy " + lstKQTK.Count + " Kết quả!";
            return View(lstKQTK.OrderBy(n => n.TenSanPham).ToPagedList(pageNumber, pageSize));
        }
	}
}