using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using TraCuuBMT.Models;

namespace TraCuuBMT.General
{
    public static class Util
    {
        private static readonly Random getrandom = new Random();
        public static List<GeneralStatus> ListGeneralStatus = new List<GeneralStatus>();
        public static List<GeneralStatus> ListTransactionStatus = new List<GeneralStatus>();

        static Util()
        {
            InitListGeneralStatus();
            InitListTransactionStatus();
        }

        public static List<Transaction> GetListTransactionByUserId(string userId)
        {
            List<Transaction> result = new List<Transaction>();
            try
            {
                using (var db = new TraCuuBMTEntities())
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        result = db.Transactions.Where(w => w.userId == userId && w.status >=1 && w.status < 4).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return result;
        }

        public static int CreateTransactionByPackageId(string userId, string packageId)
        {
            int result = 0;
            try
            {
                using (var db = new TraCuuBMTEntities())
                {
                    if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(packageId))
                    {
                        User user = GetUserById(userId);
                        Package package = GetPackageById(packageId);

                        if (user != null && package != null)
                        {
                            Transaction newItem = new Transaction();
                            newItem.ID = Util.GenerateID("transaction");
                            newItem.createDate = DateTime.Now;
                            newItem.creator = user.email;
                            newItem.currentPrice = package.price;
                            newItem.status = 0;//ok, register, chưa thanh toán, not approved
                            newItem.type = 2;//created by user
                            newItem.userId = user.ID;
                            newItem.packageId = packageId;
                            newItem.amount = package.amount;
                            db.Transactions.Add(newItem);
                            result = db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return result; 
        }

        public static int CreateTransactionByAmount(string userId, int amount=1)
        {
            int result = 0;
            try
            {
                using (var db = new TraCuuBMTEntities())
                {
                    if (!string.IsNullOrEmpty(userId))
                    {
                        User user = GetUserById(userId);

                        if (user != null)
                        {
                            Transaction newItem = new Transaction();
                            newItem.ID = Util.GenerateID("transaction");
                            newItem.createDate = DateTime.Now;
                            newItem.creator = user.email;
                            newItem.currentPrice = 0;
                            newItem.status = 1;//ok, not approved
                            newItem.type = 2;//created by user
                            newItem.userId = user.ID;
                            newItem.packageId = "";
                            newItem.amount = amount;
                            db.Transactions.Add(newItem);
                            result = db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //log
            }
            return result;
        }

        public static User GetCurrentUser()
        {
            User user = null;
            try
            {
                user = (User)System.Web.HttpContext.Current.Session["userInfo"];
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        public static User GetUserById(string userId)
        {
            User user = null;
            try
            {
                using (var db = new TraCuuBMTEntities())
                {
                    user = db.Users.Where(w => w.ID == userId).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return user;
        }

        public static Package GetPackageById(string id)
        {
            Package result = null;
            try
            {
                using (var db = new TraCuuBMTEntities())
                {
                    result = db.Packages.Where(w => w.ID == id).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }



        public static bool CheckAuthenAndAuthor(int role)
        {
            bool result = false;
            if (System.Web.HttpContext.Current.Session != null && System.Web.HttpContext.Current.Session["userInfo"] != null)
            {
                try
                {
                    User currentUser = (User)System.Web.HttpContext.Current.Session["userInfo"];
                    using (var db = new TraCuuBMTEntities())
                    {
                        if (currentUser != null)
                        {
                            var temp = db.Users.Where(w => w.status > 0 && w.ID == currentUser.ID && w.role == role).FirstOrDefault();
                            if (temp != null)
                            {
                                result = true;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }

            return result;
        }

        public static KetQuaPhanTichPhanLoai GetKQPTPLById(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                KetQuaPhanTichPhanLoai result = db.KetQuaPhanTichPhanLoais.Where(w => w.ID == id).FirstOrDefault();
                return result;
            }
        }

        public static List<SubBieuThue> GetListSubBieuThue(List<BieuThue> listBieuThue)
        {
            List<SubBieuThue> result = new List<SubBieuThue>();
            if (listBieuThue.Count > 0)
            {
                foreach (BieuThue item in listBieuThue)
                {
                    if (item.B01 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B01";
                        temp.B_Value = item.B01;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B02 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B02";
                        temp.B_Value = item.B02;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B03 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B03";
                        temp.B_Value = item.B03;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B04 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B04";
                        temp.B_Value = item.B04;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B05 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B05";
                        temp.B_Value = item.B05;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B06 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B06";
                        temp.B_Value = item.B06;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B07 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B07";
                        temp.B_Value = item.B07;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B08 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B08";
                        temp.B_Value = item.B08;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B09 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B09";
                        temp.B_Value = item.B09;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B10 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B10";
                        temp.B_Value = item.B10;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B11 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B11";
                        temp.B_Value = item.B11;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B12 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B12";
                        temp.B_Value = item.B12;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B13 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B13";
                        temp.B_Value = item.B13;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B14 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B14";
                        temp.B_Value = item.B14;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B15 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B15";
                        temp.B_Value = item.B15;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B16 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B16";
                        temp.B_Value = item.B16;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B17 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B17";
                        temp.B_Value = item.B17;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B18 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B18";
                        temp.B_Value = item.B18;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B19 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B19";
                        temp.B_Value = item.B19;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B20 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B20";
                        temp.B_Value = item.B20;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B21 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B21";
                        temp.B_Value = item.B21;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B22 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B22";
                        temp.B_Value = item.B22;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B23 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B23";
                        temp.B_Value = item.B23;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B24 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B24";
                        temp.B_Value = item.B24;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B25 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B25";
                        temp.B_Value = item.B25;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }

                    if (item.B30 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B30";
                        temp.B_Value = item.B30;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);

                    }

                    if (item.B61 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B61";
                        temp.B_Value = item.B61;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);

                    }

                    if (item.B99 >= 0)
                    {
                        SubBieuThue temp = new SubBieuThue();
                        InitGeneralValueSubBieuThue(item, ref temp);
                        temp.B_Title = "B99";
                        temp.B_Value = item.B99;
                        temp.tenBieuThue = getBieuThueName(temp.B_Title);
                        result.Add(temp);
                    }
                }
            }
            return result;
        }

        public static string getBieuThueName(string input)
        {
            string result = "";
            switch (input)
            {
                case "B01":
                    result = "Biểu thuế nhập khẩu ưu đãi";
                    break;

                case "B02":
                    result = "Chương 98 (1) - Biểu thuế nhập khẩu ưu đãi ";
                    break;

                case "B03":
                    result = "Biểu thuế nhập khẩu thông thường";
                    break;

                case "B04":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại hàng hóa ASEAN (ATIGA)";
                    break;

                case "B05":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Khu vực Mậu dịch Tự do ASEAN - Trung Quốc";
                    break;

                case "B06":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Khu vực Mậu dịch Tự do ASEAN - Hàn Quốc";
                    break;

                case "B07":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Khu vực Thương mại tự do ASEAN - Úc - Niu Di lân";
                    break;

                case "B08":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại Hàng hoá ASEAN - Ấn Độ";
                    break;

                case "B09":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Đối tác kinh tế toàn diện ASEAN - Nhật Bản";
                    break;

                case "B10":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định đối tác kinh tế Việt Nam - Nhật Bản";
                    break;

                case "B11":
                    result = "Biểu thuế thuế nhập khẩu đối với các mặt hàng được áp dụng ưu đãi thuế suất thuế nhập khẩu Việt - Lào";
                    break;

                case "B12":
                    result = "Biểu thuế thuế nhập khẩu đối với hàng hoá có xuất xứ Campuchia";
                    break;

                case "B13":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại tự do Việt Nam - Chi Lê";
                    break;

                case "B14":
                    result = "Biểu thuế NK ngoài hạn ngạch ";
                    break;

                case "B15":
                    result = "Biểu thuế nhập khẩu tuyệt đối";
                    break;

                case "B16":
                    result = "Biểu thuế nhập khẩu hỗn hợp";
                    break;

                case "B17":
                    result = "Chương 98 (2) - Biểu thuế nhập khẩu ưu đãi";
                    break;

                case "B18":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại tự do Việt Nam - Hàn Quốc";
                    break;

                case "B19":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại tự do Việt Nam - Liên minh kinh tế Á-ÂU và các nước thành viên";
                    break;

                case "B20":
                    result = "Biểu thuế nhập khẩu ưu đại đặc biệt để thực hiện Hiệp định Đối tác toàn diện và Tiến bộ xuyên Thái Bình Dương giai đoạn 2019 - 2022 ( gọi tắt là CPTPP). Hàng hóa nhập khẩu từ Liên bang Mê hi cô";
                    break;

                case "B21":
                    result = "Biểu thuế nhập khẩu ưu đại đặc biệt để thực hiện Hiệp định Đối tác toàn diện và Tiến bộ xuyên Thái Bình Dương giai đoạn 2019 - 2022 ( gọi tắt là CPTPP). Hàng hóa nhập khẩu từ Ố xtơ rây lia, Canada, Nhật Bản, Niu di lân, Cộng hòa Xing ga po.";
                    break;

                case "B22":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt để thực hiện Hiệp định Đối tác toàn diện và Tiến bộ xuyên Thái Bình Dương giai đoạn 2019 - 2022 ( gọi tắt là CPTPP). Mặt hàng ô tô đã qua sử dụng nhập khẩu trong lượng hạn ngạch thuế quan từ Liên bang Me hi cô, Ốt xtơ rây lia, ";
                    break;

                case "B23":
                    result = "Biểu thuế nhập khẩu ưu đãi đặc biệt của Việt Nam để thực hiện Hiệp định Thương mại Tự do ASEAN-Hông Kông, Trung Quốc giai đoạn 2019-2020 (AHKFTA)";
                    break;

                case "B24":
                    result = "Hiệp định thương mại Việt Nam – Cu Ba giai đoạn 2020- 2023";
                    break;

                case "B25":
                    result = "Hiệp định thương mại tự do giữa Cộng hòa xã hội chủ nghĩa Việt Nam và Liên minh Châu Âu giai đoạn 2020- 2022 (EVFTA) ";
                    break;

                case "B30":
                    result = "Mã biểu thuế áp dụng cho đối tượng không chịu thuế nhập khẩu";
                    break;

                case "B61":
                    result = "Mã biểu thuế áp dụng cho loại hình gia công và chế xuất với thuế suất 0%";
                    break;

                case "B99":
                    result = "Biểu thuế khác";
                    break;


            }
            return result;
        }

        public static void InitGeneralValueSubBieuThue(BieuThue item, ref SubBieuThue temp)
        {
            temp.description = item.description;
            temp.DVT_SL2 = item.DVT_SL2;
            temp.HS_CODE = item.HS_CODE;
            temp.ID = item.ID;
            temp.link_file_en = item.link_file_en;
            temp.link_file_vn = item.link_file_vn;
            temp.note = item.note;
            temp.status = item.status;
            temp.Ten_Hanghoa_EN = item.Ten_Hanghoa_EN;
            temp.Ten_Hanghoa_VN = item.Ten_Hanghoa_VN;
        }

        public static BieuThue GetBieuThueById(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                BieuThue result = db.BieuThues.Where(w => w.ID == id).FirstOrDefault();
                return result;
            }
        }

        public static List<BieuThue> GetListBieuThueBy(string hsCode, string mota)
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<BieuThue> result = new List<BieuThue>();
                result = db.BieuThues.Where(w => w.status >= 0).OrderByDescending(w => w.ID).ToList();

                result = result.Where(w => w.HS_CODE.Contains(hsCode) || w.Ten_Hanghoa_EN.Contains(mota) || w.Ten_Hanghoa_VN.Contains(mota)).ToList();

                return result;
            }
        }

        public static List<BieuThue> GetListAllBieuThue()
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<BieuThue> result = new List<BieuThue>();
                result = db.BieuThues.Where(w => w.status >= 0).OrderByDescending(w => w.ID).ToList();
                return result;
            }
        }

        public static List<ThueVAT> GetListThueVAT()
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<ThueVAT> result = new List<ThueVAT>();
                result = db.ThueVATs.Where(w => w.status >= 0).OrderBy(w => w.ID).ToList();
                return result;
            }
        }


        public static List<Package> GetListPackage()
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<Package> result = new List<Package>();
                result = db.Packages.Where(w => w.status >= 0).OrderBy(w => w.amount).ToList();
                return result;
            }
        }

        public static List<KetQuaPhanTichPhanLoai> GetListKTPTPLBy(string hsCode, string mota)
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<KetQuaPhanTichPhanLoai> result = new List<KetQuaPhanTichPhanLoai>();
                result = db.KetQuaPhanTichPhanLoais.Where(w => w.status >= 0 && (w.hsCode.Contains(hsCode) || w.Mota_Dnkhaibao.Contains(mota) || w.Mota_KQPTPL.Contains(mota) || w.description.Contains(mota))).OrderByDescending(w => w.ID).ToList();
                return result;
            }
        }


        public static List<KetQuaPhanTichPhanLoai> GetListKTPTPL()
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<KetQuaPhanTichPhanLoai> result = new List<KetQuaPhanTichPhanLoai>();
                result = db.KetQuaPhanTichPhanLoais.Where(w => w.status >= 0).OrderByDescending(w => w.ID).ToList();
                return result;
            }
        }

        public static bool SendEmail(string toEmail, string fromEmail, string subject, string body, string passwordFromEmail, string attachFileUrl)
        {
            bool result = false;

            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress(fromEmail, "Hệ thống tra cứu", Encoding.UTF8);
            mail.Subject = subject;
            mail.SubjectEncoding = Encoding.UTF8;
            mail.Body = body;
            mail.BodyEncoding = Encoding.UTF8;
            if (!string.IsNullOrEmpty(attachFileUrl))
            {
                if (File.Exists(attachFileUrl))
                {
                    Attachment attachment = new Attachment(attachFileUrl);
                    mail.Attachments.Add(attachment);
                }
            }

            mail.IsBodyHtml = true;
            mail.Priority = MailPriority.Normal;
            SmtpClient client = new SmtpClient();
            client.Credentials = new System.Net.NetworkCredential(fromEmail, passwordFromEmail);
            client.Port = 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            try
            {
                client.Send(mail);
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        public static int ParseStringToInt(string input)
        {
            int result = -99;
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    result = int.Parse(input);
                }
            }
            catch (Exception ex)
            {
                //ghi log
            }

            return result;
        }

        public static double ParseStringToFloat(string input)
        {
            double result = -99;
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    result = float.Parse(input);
                    result = Math.Round(result, 2);
                }
            }
            catch (Exception ex)
            {
                //ghi log
            }

            return result;
        }

        public static void InitListTransactionStatus()
        {
            ListTransactionStatus = new List<GeneralStatus>();
            GeneralStatus item1 = new GeneralStatus
            {
                StatusValue = -1,
                StatusName = "Đã xóa",
            };
            GeneralStatus item2 = new GeneralStatus
            {
                StatusValue = 0,
                StatusName = "Mới yêu cầu",
            };
            GeneralStatus item3 = new GeneralStatus
            {
                StatusValue = 1,
                StatusName = "Đã duyệt - tự động",
            };
            GeneralStatus item4 = new GeneralStatus
            {
                StatusValue = 2,
                StatusName = "Chưa TT - Kích hoạt",
            };
            GeneralStatus item5 = new GeneralStatus
            {
                StatusValue = 3,
                StatusName = "Đã TT - Kích hoạt",
            };
            GeneralStatus item6 = new GeneralStatus
            {
                StatusValue = 4,
                StatusName = "Đang khóa",
            };
            ListTransactionStatus.Add(item1);
            ListTransactionStatus.Add(item2);
            ListTransactionStatus.Add(item3);
            ListTransactionStatus.Add(item4);
            ListTransactionStatus.Add(item5);
            ListTransactionStatus.Add(item6);
        }

        public static void InitListGeneralStatus()
        {
            ListGeneralStatus = new List<GeneralStatus>();
            GeneralStatus item1 = new GeneralStatus
            {
                StatusValue = -1,
                StatusName = "Xóa",
            };
            GeneralStatus item2 = new GeneralStatus
            {
                StatusValue = 1,
                StatusName = "Kích hoạt",
            };
            GeneralStatus item3 = new GeneralStatus
            {
                StatusValue = 2,
                StatusName = "Khóa",
            };
            ListGeneralStatus.Add(item1);
            ListGeneralStatus.Add(item2);
            ListGeneralStatus.Add(item3);
        }


        public static string GenerateID(string prefix)
        {
            return prefix + "_" + DateTimeOffset.Now.ToUnixTimeSeconds() + "_" + getrandom.Next(1, 1000000);
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }

    public class GeneralStatus
    {
        public int StatusValue { get; set; }
        public string StatusName { get; set; }
    }

    public class TransactionStatus
    {
        public int StatusValue { get; set; }
        public string StatusName { get; set; }
    }
}