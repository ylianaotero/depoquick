using System.Text;
using BusinessLogic.Controllers;
using DepoQuick.Domain;

namespace BusinessLogic.ReservationReport
{
    public class ReportUsingTXT : ReportBase
    {
        public ReportUsingTXT(ReservationController reservationController, PaymentController paymentController)
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
            return string.Format("{0,-20} \t {1,-30} \t {2,-25} \t {3,-10} \t {4,-18} \t {5,-15}", 
                "Numero del deposito", 
                "Rango de fechas de la reserva", 
                "Cliente", 
                "Costo Final", 
                "Incluye Promocion", 
                "Estado de Pago");
        }

        protected override string FormatReservation(Reservation reserv, string paymentStatus, bool promotionHasBeenApplied)
        {
            return string.Format("{0,-20} \t {1,-30} \t {2,-25} \t {3,-10} \t {4,-18} \t {5,-15}",
                reserv.Deposit.Id, 
                reserv.Date.InitialDate.ToString("dd/MM/yyy") +" - "+ reserv.Date.FinalDate.ToString("dd/MM/yyy"),
                reserv.Client.Email, 
                reserv.Price, 
                promotionHasBeenApplied, 
                paymentStatus);

        }
    }
}