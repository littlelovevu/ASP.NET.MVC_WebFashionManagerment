using DeTaiWeb_ShopThoiTrang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DeTaiWeb_ShopThoiTrang.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        DataQLShopThoiTrangDataContext data = new DataQLShopThoiTrangDataContext();

        public ActionResult Index()
        {
            List<SanPham> dsSP = data.SanPhams.Take(20).ToList();
            ViewBag.td = "Sản phẩm mới";
            return View(dsSP);
        }

        public ActionResult DSMenu_LoaiSanPham()
        {
            List<LoaiSanPham> dsLSP = data.LoaiSanPhams.Take(10).ToList();
            return PartialView(dsLSP);
        }

        public ActionResult DSMenu_NhaSanXuat()
        {
            List<NhaSanXuat> dsNSX = data.NhaSanXuats.Take(10).ToList();
            return PartialView(dsNSX);
        }

        public ActionResult HTSanPhamLoaiSP(int mlsp, string tlsp)
        {
            List<SanPham> dsSP = data.SanPhams.Where(sp => sp.MaLSP == mlsp).ToList();
            ViewBag.tb = "Đây là tất cả sản phẩm thuộc loại: " + tlsp;
            return View("Index", dsSP);
        }

        public ActionResult HTSanPhamNSX(int mnsx, string tnsx)
        {
            List<SanPham> dsSP = data.SanPhams.Where(sp => sp.MaNSX == mnsx).ToList();
            ViewBag.tb = "Đây là tất cả sản phẩm thuộc nhà sản xuất: " + tnsx;
            return View("Index", dsSP);
        }

        public ActionResult DSMenu_LoaiSanPham2()
        {
            List<LoaiSanPham> dsLSP = data.LoaiSanPhams.Take(10).ToList();
            return PartialView(dsLSP);
        }

        public ActionResult HTSanPhamLoaiSP2(int mlsp, string tlsp)
        {
            List<SanPham> dsSP = data.SanPhams.Where(sp => sp.MaLSP == mlsp).ToList();
            ViewBag.tb = "Đây là tất cả sản phẩm thuộc loại: " + tlsp;
            return View("Index", dsSP);
        }

        public ActionResult Search(FormCollection col)
        {
            string keyword = col["txtKeyword"];
            List<SanPham> dsSearch = data.SanPhams.Where(s => s.TenSanPham.Contains(keyword)).ToList();
            ViewBag.tb = "Tìm kiếm: " + keyword.ToString();
            return View("Index", dsSearch);
        }

        public ActionResult ChiTiet(int maSP)
        {
            SanPham sp = data.SanPhams.SingleOrDefault(item => item.MaSanPham == maSP);
            ////tạo biến kiểu dữ liệu sách cùng chủ đề
            //List<sanpham> dsSach_DM = data.sanphams.Where(s => s.loaisp == sp.loaisp).Take(5).ToList();
            //ViewBag.dm = dsSach_DM;
            return View(sp);
        }

        public ActionResult SearchPro()
        {
            ViewBag.MaNhaSanXuat = new SelectList(data.NhaSanXuats.ToList(), "MaNhaSanXuat", "TenNhaSanXuat");
            return View();
        }
        [HttpPost]
        public ActionResult SearchPro(FormCollection c)
        {
            string ten = c["txtTen"];
            int maNSX = int.Parse(c["MaNhaSanXuat"]);

            List<SanPham> dstk = data.SanPhams.Where(t => t.TenSanPham.Contains(ten)).ToList();
            List<SanPham> ds2 = dstk.Where(t => t.MaNSX == maNSX).ToList();
            List<SanPham> dsSP = new List<SanPham>();

            if (c["g1"] == "1")
            {
                List<SanPham> d1 = ds2.Where(s => s.Gia > 0 && s.Gia <= 100000).ToList();
                dsSP.AddRange(d1);
            }
            if (c["g2"] == "2")
            {
                List<SanPham> d2 = ds2.Where(s => s.Gia > 100000 && s.Gia <= 200000).ToList();
                dsSP.AddRange(d2);
            }
            if (c["g3"] == "3")
            {
                List<SanPham> d3 = ds2.Where(s => s.Gia > 200000 && s.Gia <= 400000).ToList();
                dsSP.AddRange(d3);
            }
            if (c["g4"] == "4")
            {
                List<SanPham> d4 = ds2.Where(s => s.Gia > 400000).ToList();
                dsSP.AddRange(d4);
            }
            return View("Index", dsSP);
        }

        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LienHe_KQ(FormCollection col)
        {
            string ten = col["txtTen"];
            string sdt = col["txtSDT"];
            string email = col["txtEmail"];
            string tieuDe = col["txtTieuDe"];
            string noiDung = col["txtNoiDung"];
            //Lưu một dòng vào bảng khách hàng
            LienHe lh = new LienHe();
            lh.Ten = ten;
            lh.SDT = sdt;
            lh.Email = email;
            lh.TieuDe = tieuDe;
            lh.NoiDung = noiDung;
            data.LienHes.InsertOnSubmit(lh);
            data.SubmitChanges();
            return View(lh);
        }
    }
}