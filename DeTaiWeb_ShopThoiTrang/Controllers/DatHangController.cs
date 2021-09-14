using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiWeb_ShopThoiTrang.Models;

namespace DeTaiWeb_ShopThoiTrang.Controllers
{
    public class DatHangController : Controller
    {
        //
        // GET: /DatHang/

        DataQLShopThoiTrangDataContext data = new DataQLShopThoiTrangDataContext();

        //Lấy giỏ hàng
        public List<CartItem> LayGioHang()
        {
            List<CartItem> lstGioHang = Session["GioHang"] as List<CartItem>;
            if (lstGioHang == null)
            {
                //Nếu list chưa tồn tại thì khởi tạo
                lstGioHang = new List<CartItem>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        //Thêm giỏ hàng
        public ActionResult ThemGioHang(int msp, string strURL)
        {
            //Lấy giỏ hàng
            List<CartItem> lstGioHang = LayGioHang();
            //Kiếm tra rằng sản phẩm này có tồn tại
            CartItem sanpham = lstGioHang.Find(sp => sp.iMaSanPham == msp);
            if (sanpham == null) //Chưa có hàng trong giỏ
            {
                sanpham = new CartItem(msp);
                lstGioHang.Add(sanpham);
                //return Redirect(strURL);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                sanpham.iSoLuong++;
                return RedirectToAction("Index", "Home");
            }
        }

        //Tổng số lượng hàng trong giỏ
        private int TongSoLuong()
        {
            int tsl = 0;
            List<CartItem> lstGioHang = Session["GioHang"] as List<CartItem>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(sp => sp.iSoLuong);
            }
            return tsl;
        }

        //Tổng thành tiền
        private double TongThanhTien()
        {
            double tongTT = 0;
            List<CartItem> lstGioHang = Session["GioHang"] as List<CartItem>;
            if (lstGioHang != null)
            {
                tongTT += lstGioHang.Sum(sp => sp.ThanhTien);
            }
            return tongTT;
        }

        //Trang giỏ hàng
        public ActionResult GioHang(FormCollection c)
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CartItem> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.ToSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return PartialView();
        }

        //Xóa 1 sản phẩm trong giỏ hàng
        public ActionResult XoaGioHang(int MaSP)
        {
            //Lấy giỏ hàng
            List<CartItem> lstGioHang = LayGioHang();
            //Kiểm tra giỏ hàng rỗng??
            CartItem sp = lstGioHang.Single(s => s.iMaSanPham == MaSP);
            //Kiểm tra tồn tại thì sẽ xóa
            if (sp != null)
            {
                lstGioHang.RemoveAll(s => s.iMaSanPham == MaSP);
                return RedirectToAction("GioHang", "DatHang");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang", "DatHang");
        }

        //Xóa tất cả sản phẩm trong giỏ
        public ActionResult XoaGioHang_All()
        {
            List<CartItem> lstGioHang = LayGioHang();
            //Kiểm tra giỏ hàng rỗng
            lstGioHang.Clear();
            return RedirectToAction("GioHang", "DatHang");
        }

        //Update giỏ hàng
        public ActionResult CapNhatGioHang(int msp, FormCollection col)
        {
            //Lấy giỏ hàng
            List<CartItem> lstGioHang = LayGioHang();
            CartItem sp = lstGioHang.Single(s => s.iMaSanPham == msp);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(col["txtSL"].ToString());
            }
            return RedirectToAction("GioHang", "DatHang");
        }

        //Xác nhận hóa đơn
        [HttpGet]
        public ActionResult XacNhanDonHang()
        {
            //Kiểm tra khách hàng đã đăng nhập chưa
            KhachHang khach = Session["kh"] as KhachHang;
            if (khach == null)
                return RedirectToAction("DangNhap", "KhachHang");
            List<CartItem> lstGioHang = LayGioHang();
            ViewBag.k = khach;
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongThanhTien = TongThanhTien();
            return View(lstGioHang);
        }

        //Add data vào hoadon và chitiethoadon
        [HttpPost]
        public ActionResult TaoDonDatHang(FormCollection x)
        {
            KhachHang khach = Session["kh"] as KhachHang;
            //Lưu một dòng vào bảng hóa đơn
            HoaDon hd = new HoaDon();
            hd.MaKH = khach.MaKhachHang;
            hd.NgayTao = DateTime.Now;
            hd.TinhTrang = "Đã đặt hàng";
            hd.GhiChu = x["txtGhiChu"];
            data.HoaDons.InsertOnSubmit(hd);
            data.SubmitChanges();
            //Lưu nhiều dòng vào bảng chi tiết hóa đơn của dòng hóa đơn đó
            List<CartItem> lstGioHang = LayGioHang();
            foreach (CartItem item in lstGioHang)
            {
                ChiTietHoaDon ct = new ChiTietHoaDon();
                ct.MaHD = hd.MaHoaDon;
                ct.MaSP = item.iMaSanPham;
                ct.SoLuong = item.iSoLuong;
                data.ChiTietHoaDons.InsertOnSubmit(ct);
                data.SubmitChanges();
            }
            ViewBag.name = khach.TenKhachHang;
            return View(lstGioHang);
        }
    }
}