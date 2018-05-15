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
    public class TTrangStoreController : Controller
    {
        //Tao 1 doi tuong chua toan bo CSDL tu dbQLThoiTrang
        dbQLThoitrangDataContext data = new dbQLThoitrangDataContext();

        private List<SANPHAM> LayTTrangmoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 8;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            //Lấy top 5 ablum bán chạy nhất
            var ttrangmoi = LayTTrangmoi(15);
            return View(ttrangmoi.ToPagedList(pageNum,pageSize));
        }

        public ActionResult Chungloai()
        {
          
            var chungloai = from cl in data.CHUNGLOAIs select cl;
            return PartialView(chungloai);
        }
        public ActionResult SPTheochungloai(int id,int ?page)
        {
            //Tạo biến quy định số sản phẩm trên mỗi trang
            int pageSize = 16;
            //Tạo biến số trang
            int pageNum = (page ?? 1);
            var ttrang = from s in data.SANPHAMs where s.MaCL == id select s;
            return View(ttrang.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Details(int id)
        {
            var ttrang = from s in data.SANPHAMs
                         where s.MaSanPham == id
                         select s;
            return View(ttrang.Single());
        }
	}
}