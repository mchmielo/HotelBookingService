using HotelBookingService.BLL.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace HotelBookingService.BLL
{
    public class MailSender : IMailSender
    {
        private const string _mailSender = "hotelbookingapi.com";

        public void SendMail(UserMailDto mailDto)
        {
            SmtpClient client = new SmtpClient("SmtpServer");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailDto.Username, mailDto.Password);

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("hotelbookingapi@hotelbookingapi.com");
            mailMessage.To.Add(mailDto.MailAddress);
            mailMessage.Body = $"Dear {mailDto.Username},{Environment.NewLine}Your reservation is confirmed."+
                $"{Environment.NewLine}We wish You a great stay in a hotel." +
                $"{Environment.NewLine}hotelbookingapi.com Team";
            mailMessage.Subject = "Reservation confirmation";
            client.Send(mailMessage);
        }
    }
}
