using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiWeb_ShopThoiTrang.Models;

namespace DeTaiWeb_ShopThoiTrang.Controllers
{
    public class KhachHangController : Controller
    {
        //
        // GET: /KhachHang/

        DataQLShopThoiTrangDataContext data = new DataQLShopThoiTrangDataContext();

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection col)
        {
            string taiKhoan = col["txtTaiKhoan"];
            string matKhau = col["txtMatKhau"];
            KhachHang khach1 = data.KhachHangs.SingleOrDefault(k => k.TenTaiKhoan == taiKhoan && k.MatKhau == matKhau && k.MaLTK == 1);
            KhachHang khach2 = data.KhachHangs.SingleOrDefault(k => k.TenTaiKhoan == taiKhoan && k.MatKhau == matKhau && k.MaLTK == 2);
            if (khach1 == null && khach2 == null) //không thành công
            {
                ViewBag.tb = "Thông tin đăng nhập sai";
                return View();
            }
            if (khach1 != null)
            {
                Session["kh"] = khach1;
                return RedirectToAction("DanhSachSanPham", "Admin");
            }
            Session["kh"] = khach2;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangXuat()
        {
            Session["kh"] = null;
            return RedirectToAction("DangNhap");
        }

        [HttpGet]
        public ActionResult DangKi()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKi_Them(FormCollection col)
        {
            string ten = col["txtTen"];
            string taikhoan = col["txtTaiKhoan"];
            string matkhau = col["txtMatKhau"];
            string diachi = col["txtDiaChi"];
            string sodienthoai = col["txtSoDienThoai"];
            int loaiTaiKhoan = 2;
            //Lưu một dòng vào bảng khách hàng
            KhachHang kh = new KhachHang();
            kh.TenKhachHang = ten;
            kh.TenTaiKhoan = taikhoan;
            kh.MatKhau = matkhau;
            kh.DiaChi = diachi;
            kh.SDT = sodienthoai;
            kh.MaLTK = loaiTaiKhoan;
            data.KhachHangs.InsertOnSubmit(kh);
            data.SubmitChanges();
            ViewBag.tk = taikhoan;
            return View(kh);
        }
    }
}
