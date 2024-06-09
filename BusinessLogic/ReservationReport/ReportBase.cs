using System.Text;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.PaymentControllerExceptions;
using BusinessLogic.Exceptions.ReservationControllerExceptions;
using DepoQuick.Domain;

namespace BusinessLogic.ReservationReport
{
    public abstract class ReportBase
    {
        private const string BasePaymentStatus = "No pagado";
        
        protected ReservationController _reservationController;
        protected PaymentController _paymentController;

        protected ReportBase(ReservationController reservationController, PaymentController paymentController)
        {
            _paymentController = paymentController;
            _reservationController = reservationController;
        }

        public string GenerateReport()
        {
            List<Reservation> reservations = GetListOfReservations();
            
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder = GenerateReportInStringBuilder(stringBuilder, reservations); 

            return stringBuilder.ToString();
        }
        
        protected List<Reservation> GetListOfReservations()
        {
            return _reservationController.GetReservations(); 
        }
        

        protected string GetPaymentStatus(Reservation reserv)
        {
            try
            {
                return _paymentController.Get(reserv).Status;
            }
            catch (PaymentNotFoundException)
            {
                return BasePaymentStatus; 
            }
        }

        protected bool PromotionHasBeenApplied(Reservation reserv)
        {
            return _reservationController.PromotionHasBeenApplied(reserv);
        }
        
        protected abstract StringBuilder GenerateReportInStringBuilder(StringBuilder stringBuilder,
            List<Reservation> reservations); 

        protected abstract string GetHeader();

        protected abstract string FormatReservation(Reservation reserv, string paymentStatus, bool promotionHasBeenApplied);
    }
}
