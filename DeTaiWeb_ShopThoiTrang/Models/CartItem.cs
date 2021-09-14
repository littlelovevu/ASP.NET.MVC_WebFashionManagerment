using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeTaiWeb_ShopThoiTrang.Models;

namespace DeTaiWeb_ShopThoiTrang.Models
{
    public class CartItem
    {
        DataQLShopThoiTrangDataContext data = new DataQLShopThoiTrangDataContext();
        public int iMaSanPham { get; set; }
        public string sTenSanPham { get; set; }
        public string sAnhBia { get; set; }
        public double dDonGia { get; set; }
        public int iSoLuong { get; set; }

        public double ThanhTien
        {
            get { return iSoLuong * dDonGia; }
        }
        
        // Khởi tạo giỏ hàng
        public CartItem(int msp)
        {
            iMaSanPham = msp;
            SanPham sp = data.SanPhams.Single(n => n.MaSanPham == msp);
            sTenSanPham = sp.TenSanPham;
            sAnhBia = sp.HinhMinhHoa;
            dDonGia = double.Parse(sp.Gia.ToString());
            iSoLuong = 1;
        }
    }
}