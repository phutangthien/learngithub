using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Doancoso.Models;

namespace Doancoso.Models
{
    public class Giohang
    {
        dbQLThoitrangDataContext data = new dbQLThoitrangDataContext();
        public int iMaSanPham { set; get; }
        public string sTenSanPham { set; get; }
        public string sAnh { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhTien
        {
            get { return iSoluong * dDongia; }
        }
        public Giohang(int Masanpham)
        {
            iMaSanPham = Masanpham;
            SANPHAM sanpham = data.SANPHAMs.Single(n => n.MaSanPham == iMaSanPham);
            sTenSanPham = sanpham.TenSanPham;
            sAnh = sanpham.Anh;
            dDongia = double.Parse(sanpham.Giaban.ToString());
            iSoluong = 1;
        }
    }
}