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

        public static KetQuaPhanTichPhanLoai GetKQPTPLById(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                KetQuaPhanTichPhanLoai result = db.KetQuaPhanTichPhanLoais.Where(w => w.ID == id).FirstOrDefault();
                return result;
            }
        }

        public static BieuThue GetBieuThueById(string id)
        {
            using (var db = new TraCuuBMTEntities())
            {
                BieuThue result = db.BieuThues.Where(w => w.ID == id).FirstOrDefault();
                return result;
            }
        }
        public static List<BieuThue> GetListBieuThue()
        {
            using (var db = new TraCuuBMTEntities())
            {
                List<BieuThue> result = new List<BieuThue>();
                result = db.BieuThues.Where(w => w.status >= 0).OrderByDescending(w => w.ID).ToList();
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
}