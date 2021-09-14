using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeTaiWeb_ShopThoiTrang.Models;
using System.IO;

namespace DeTaiWeb_ShopThoiTrang.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

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
            KhachHang khach = data.KhachHangs.SingleOrDefault(k => k.TenTaiKhoan == taiKhoan && k.MatKhau == matKhau && k.MaLTK == 1);
            if (khach == null) //không thành công
            {
                ViewBag.tb = "Thông tin đăng nhập sai";
                return View();
            }
            Session["kh"] = khach;
            return RedirectToAction("DanhSachSanPham", "Admin");
        }

        public ActionResult DangXuat()
        {
            Session["kh"] = null;
            return RedirectToAction("DangNhap");
        }

        //Danh sách sản phẩm

        public ActionResult DanhSachSanPham()
        {
            if (Session["kh"] == null || Session["kh"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            List<SanPham> dsSP = data.SanPhams.ToList();
            return View(dsSP);
        }

        [HttpGet]
        public ActionResult ThemSanPham()
        {
            ViewBag.MaNhaSanXuat = new SelectList(data.NhaSanXuats.ToList(), "MaNhaSanXuat", "TenNhaSanXuat");
            ViewBag.MaLoaiSanPham = new SelectList(data.LoaiSanPhams.ToList(), "MaLoaiSanPham", "TenLoaiSanPham");
            return View();
        }
        [HttpPost]
        public ActionResult ThemSanPham_KQ(SanPham sp, FormCollection col, HttpPostedFileBase fup)
        {
            string ten = col["txtTen"];
            int gia = Convert.ToInt32(col["txtGia"].ToString().Trim());
            //fup.SaveAs(Server.MapPath("~/Content/Images/" + fup.FileName));
            string hinhMinhHoa = col["fup"];
            string dsHinh = col["txtDSHinh"];
            int loaiSP = int.Parse(col["MaNhaSanXuat"]);
            int loaiNSX = int.Parse(col["MaLoaiSanPham"]);
            //Lưu một dòng vào bảng sản phẩm
            //SanPham sp = new SanPham();
            sp.TenSanPham = ten;
            sp.Gia = gia;
            //sp.HinhMinhHoa = fup.FileName;
            sp.HinhMinhHoa = hinhMinhHoa;
            sp.DanhSachHinh = dsHinh;
            sp.MaLSP = loaiSP;
            sp.MaNSX = loaiNSX;
            data.SanPhams.InsertOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("DanhSachSanPham");
        }

        public ActionResult ChiTietSanPham(int id)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(item => item.MaSanPham == id);
            return View(sp);
        }

        public ActionResult SuaSanPham(int id)
        {
            var Edit_SanPham = data.SanPhams.First(m => m.MaSanPham == id);
            return View(Edit_SanPham);
        }
        [HttpPost]
        public ActionResult SuaSanPham(int id, FormCollection c)
        {
            var lstSanPham = data.SanPhams.First(m => m.MaSanPham == id);
            var CB_1 = c["TenSanPham"];
            var CB_2 = c["Gia"];
            var CB_3 = c["HinhMinhHoa"];
            var CB_4 = c["DanhSachHinh"];
            var CB_5 = c["MaLSP"];
            var CB_6 = c["MaNSX"];
            lstSanPham.MaSanPham = id;
            UpdateModel(lstSanPham);
            data.SubmitChanges();
            return RedirectToAction("DanhSachSanPham");
        }

        public ActionResult XoaSanPham(int id)
        {
            var Delete_SanPham = data.SanPhams.First(m => m.MaSanPham == id);
            return View(Delete_SanPham);
        }
        [HttpPost]
        public ActionResult XoaSanPham(int id, FormCollection collection)
        {
            var Xoa_SanPham = data.SanPhams.Where(m => m.MaSanPham == id).First();
            data.SanPhams.DeleteOnSubmit(Xoa_SanPham);
            data.SubmitChanges();
            return RedirectToAction("DanhSachSanPham");
        }

        //Qua mục khách hàng

        public ActionResult DanhSachKhachHang()
        {
            if (Session["kh"] == null || Session["kh"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            List<KhachHang> dsKH = data.KhachHangs.ToList();
            return View(dsKH);
        }

        [HttpGet]
        public ActionResult ThemNguoiDung()
        {
            ViewBag.MaLoaiTaiKhoan = new SelectList(data.LoaiTaiKhoans.ToList(), "MaLoaiTaiKhoan", "TenLoaiTaiKhoan");
            return View();
        }
        [HttpPost]
        public ActionResult ThemNguoiDung_KQ(KhachHang kh, FormCollection col)
        {
            string ten = col["txtTen"];
            string taikhoan = col["txtTenTK"];
            string matkhau = col["txtPass"];
            string diachi = col["txtDiaChi"];
            string sdt = col["txtSDT"];
            int loaiTK = int.Parse(col["MaLoaiTaiKhoan"]);
            kh.TenKhachHang = ten;
            kh.TenTaiKhoan = taikhoan;
            kh.MatKhau = matkhau;
            kh.DiaChi = diachi;
            kh.SDT = sdt;
            kh.MaLTK = loaiTK;
            data.KhachHangs.InsertOnSubmit(kh);
            data.SubmitChanges();
            return RedirectToAction("DanhSachKhachHang");
        }

        public ActionResult ChiTietNguoiDung(int id)
        {
            KhachHang kh = data.KhachHangs.SingleOrDefault(item => item.MaKhachHang == id);
            return View(kh);
        }

        public ActionResult SuaNguoiDung(int id)
        {
            var Edit_NguoiDung = data.KhachHangs.First(m => m.MaKhachHang == id);
            return View(Edit_NguoiDung);
        }
        [HttpPost]
        public ActionResult SuaNguoiDung(int id, FormCollection c)
        {
            var lstNguoiDung = data.KhachHangs.First(m => m.MaKhachHang == id);
            var CB_1 = c["TenKhachHang"];
            var CB_2 = c["TenTaiKhoan"];
            var CB_3 = c["MatKhau"];
            var CB_4 = c["DiaChi"];
            var CB_5 = c["SDT"];
            var CB_6 = c["MaLTK"];
            lstNguoiDung.MaKhachHang = id;
            UpdateModel(lstNguoiDung);
            data.SubmitChanges();
            return RedirectToAction("DanhSachKhachHang");
        }

        public ActionResult XoaNguoiDung(int id)
        {
            var Delete_NguoiDung = data.KhachHangs.First(m => m.MaKhachHang == id);
            return View(Delete_NguoiDung);
        }
        [HttpPost]
        public ActionResult XoaNguoiDung(int id, FormCollection collection)
        {
            var Xoa_NguoiDung = data.KhachHangs.Where(m => m.MaKhachHang == id).First();
            data.KhachHangs.DeleteOnSubmit(Xoa_NguoiDung);
            data.SubmitChanges();
            return RedirectToAction("DanhSachKhachHang");
        }

        //Qua mục hóa đơn
        QuanLyAPIController api = new QuanLyAPIController();
        public ActionResult DanhSachHoaDon()
        {
            List<HoaDon> dsHoaDon = api.GetListHoaDon();
            return View(dsHoaDon);
        }
        public ActionResult ChiTietHoaDon(int id)
        {
            if (Session["kh"] == null || Session["kh"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            List<ChiTietHoaDon> dsCTHD = data.ChiTietHoaDons.Where(x => x.MaHD == id).ToList();
            return View(dsCTHD);
        }
        


        //Qua mục liên hệ

        public ActionResult DanhSachLienHe()
        {
            return View();
        }

        public JsonResult ListLienHe()
        {
            return Json(data.LienHes.ToList(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult XoaLienHe(int ID)
        {
            var f = data.LienHes.FirstOrDefault(x => x.MaLienHe == ID);
            data.LienHes.DeleteOnSubmit(f);
            data.SubmitChanges();
            return Json(data.LienHes.ToList(), JsonRequestBehavior.AllowGet);
        }
    }
}