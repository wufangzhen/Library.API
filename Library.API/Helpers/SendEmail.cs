using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Library.API.Helpers
{
    public class SendEmail
    {
        public static void SendEmailBy(string toaddress, string name, string subject,string body,bool isBodyHtml)
        {
            SmtpClient client = new SmtpClient("smtp.qq.com");
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("1040079213@qq.com", "voisvykvbgembedg");

            MailAddress from = new MailAddress("1040079213@qq.com", "fwq", Encoding.UTF8);//初始化发件人

            MailAddress to = new MailAddress(toaddress, name, Encoding.UTF8);//初始化收件人

            //设置邮件内容
            MailMessage message = new MailMessage(from, to);
            message.Body = body;
            //message.BodyEncoding = mail.BodyEncoding;
            message.Subject = subject;
            //message.SubjectEncoding = mail.SubjectEncoding;
            message.IsBodyHtml = isBodyHtml;

            //发送邮件
            try
            {
                client.Send(message);
            }
            catch (InvalidOperationException iex)
            {
                
            }
            catch (Exception ex)
            {
                var p= ex.Message;
            }
        }
    }
}
