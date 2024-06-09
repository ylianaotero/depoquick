using System.Text;
using BusinessLogic.Controllers;
using DepoQuick.Domain;

namespace BusinessLogic.ReservationReport
{
    public class ReportUsingCSV : ReportBase
    {
        public ReportUsingCSV(ReservationController reservationController, PaymentController paymentController)
            : base(reservationController, paymentController)
        {
        }
        
        protected override StringBuilder GenerateReportInStringBuilder(StringBuilder stringBuilder, List<Reservation> reservations)
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

        protected override string GetHeader()
        {
            return "Numero del deposito,Rango de fechas de la reserva, Cliente, Costo Final, Incluye Promocion,Estado de Pago";
        }

        protected override string FormatReservation(Reservation reserv, string paymentStatus, bool promotionHasBeenApplied)
        {
            return $"{reserv.Deposit.Id},{reserv.Date.InitialDate:dd/MM/yyyy} - {reserv.Date.FinalDate:dd/MM/yyyy},{reserv.Client.Email},{reserv.Price},{promotionHasBeenApplied},{paymentStatus}";
        }
    }
}