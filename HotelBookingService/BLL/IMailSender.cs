using HotelBookingService.BLL.Model;

namespace HotelBookingService.BLL
{
    public interface IMailSender
    {
        void SendMail(UserMailDto mailDto);
    }
}
