using System.Text;
using BusinessLogic.Controllers;
using BusinessLogic.Exceptions.PaymentControllerExceptions;
using DepoQuick.Domain;

namespace Interface.ReservationReport
{
    public abstract class ReportBase : IReport
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

        private List<Reservation> GetListOfReservations()
        {
            return _reservationController.GetReservations(); 
        }

        private StringBuilder GenerateReportInStringBuilder(StringBuilder stringBuilder, List<Reservation> reservations)
        {
            stringBuilder.AppendLine(GetHeader());
            foreach (var reserv in reservations)
            {
                string paymentStatus = GetPaymentStatus(reserv);
                bool promotionHasBeenApplied = PromotionHasBeenApplied(reserv); 
                
                stringBuilder.AppendLine(FormatReservation(reserv, paymentStatus, promotionHasBeenApplied));
            }

            return stringBuilder; 
        }

        private string GetPaymentStatus(Reservation reserv)
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

        private bool PromotionHasBeenApplied(Reservation reserv)
        {
            return _reservationController.PromotionHasBeenApplied(reserv);
        }

        protected abstract string GetHeader();

        protected abstract string FormatReservation(Reservation reserv, string paymentStatus, bool promotionHasBeenApplied);
    }
}
