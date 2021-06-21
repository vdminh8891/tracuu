using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TraCuuBMT.Models
{
    public class SubBieuThue
    {
        public string ID { get; set; }
        public int status { get; set; }
        public string note { get; set; }
        public string description { get; set; }
        public string HS_CODE { get; set; }
        public string DVT_SL2 { get; set; }
        public string link_file_vn { get; set; }
        public string link_file_en { get; set; }
        public string tenBieuThue { get; set; }
        public string Ten_Hanghoa_VN { get; set; }
        public string Ten_Hanghoa_EN { get; set; }
        public string B_Title { get; set; }
        public double? B_Value { get; set; }
       
    }
}