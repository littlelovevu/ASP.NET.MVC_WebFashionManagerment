using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DeTaiWeb_ShopThoiTrang.Models;

namespace DeTaiWeb_ShopThoiTrang.Controllers
{
    public class QuanLyAPIController : ApiController
    {
        [HttpGet]
        public List<HoaDon> GetListHoaDon()
        {
            DataQLShopThoiTrangDataContext data = new DataQLShopThoiTrangDataContext();
            return data.HoaDons.ToList();
        }
    }
}
