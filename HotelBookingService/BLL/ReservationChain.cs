using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HotelBookingService.BLL
{
    public class ReservationChain : IReservationStep
    {
        private readonly List<IReservationStep> _steps = new List<IReservationStep>();
        private string _failedStepName = "";

        public int Count { get { return _steps.Count; } }
        public string FailedStepName { get { return _failedStepName; } }

        public IReservationStep this[int i]
        {
            get { return _steps.ToArray()[i]; }
        }

        public async Task<bool> SendDataAsync()
        {
            foreach (var step in _steps)
            {
                if(! await step.SendDataAsync())
                {
                    _failedStepName = ((ReservationStepBase)step).GetStepName();
                    if(FailedStepName == "SendConfirmationMail")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            _failedStepName = "";
            return true;
        }

        public void AddStep(IReservationStep step)
        {
            _steps.Add(step);
        }

        public void RemoveStep(IReservationStep step)
        {
            _steps.Remove(step);
        }
    }
}
