using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingService.BLL.Model
{
    public class UserMailDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string MailAddress { get; set; }
    }
}
