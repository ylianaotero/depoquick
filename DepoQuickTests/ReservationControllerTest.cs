using BusinessLogic;
using BusinessLogic.Exceptions.ControllerExceptions;
using DepoQuick.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DepoQuickTests
{
    [TestClass]
    public class ReservationControllerTest
    {
        private ReservationController _reservationController;
        private UserController _userController;
        private DepoQuickContext _context;
        private Session _session;
        
        private const string AdminName = "Administrator";
        private const string AdminEmail = "administrator@domain.com";
        private const string AdminPassword = "Password1#";
        private const string ClientName = "Client";
        private const string ClientEmail = "client@domain.com";
        private const string ClientPassword = "Password2#";
     
        private const char DepositArea0 = 'A';
        private const string DepositSize0 = "Pequeño";
        private const bool DepositAirConditioning0 = true;
        private const char DepositArea1 = 'B';
        private const string DepositSize1 = "Grande";
        private const bool DepositAirConditioning1 = false;
     
        private const string PromotionLabel0 = "Promotion 0";
        private const double PromotionDiscountRate0 = 0.5;
     
        private const int ApprovedReservationState = 1;
        private const int PendingReservationState = 0;
        private const int RejectedReservationState = -1;
     
        private const string UserLogInLogMessage = "Ingresó al sistema";
        private const string UserLogOutLogMessage = "Cerró sesión";
        
        private Client _client;
        private Deposit _deposit0;
        private DateRange _validDateRange;
        private DateRange _expiredDateRange;
        private DateRange _currentDateRange;
        
        [TestInitialize]
        public void Initialize()
        {
            _context = TestContextFactory.CreateContext();
            _userController = new UserController(_context);
            _session = new Session(_userController);
           _reservationController = new ReservationController(_context,_session);
            
            _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

            _deposit0 = new Deposit (DepositArea0,DepositSize0,DepositAirConditioning0);
            _context.Deposits.Add(_deposit0);
            _context.SaveChanges();
            
            _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
            _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
            _expiredDateRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));

        }

        [TestMethod]
        public void TestGetAReservationByDateRange()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            int reservation1Id = reservation1.Id;
    
            _reservationController.Add(reservation1);

            Reservation result = _reservationController.Get(_validDateRange);
            Assert.AreEqual(result.Id,reservation1.Id);
        }
        
        [TestMethod]
        public void TestAddAReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);

            Reservation result = _reservationController.Get(reservation.Id);
            Assert.IsNotNull(result);
            Assert.AreEqual(reservation.Id, result.Id);
        }
        
        [TestMethod]
        public void TestSearchForAReservationById()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
            int reservation1Id = reservation1.Id;
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
            
            Assert.AreEqual(_deposit0, _reservationController.Get(reservation1Id).Deposit);
            Assert.AreEqual(_client, _reservationController.Get(reservation1Id).Client);
            Assert.AreEqual(_validDateRange, _reservationController.Get(reservation1Id).Date);
            Assert.AreEqual(reservation1Id, _reservationController.Get(reservation1Id).Id);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ReservationNotFoundException))]
        public void TestSearchForAReservationUsingAnInvalidId()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
 
            _reservationController.Get(-34);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ReservationNotFoundException))]
        public void TestReservationUsingInvalidDate()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
 
            _reservationController.Get(_currentDateRange);
        }
        
        [TestMethod]
        public void TestApproveReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            var reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);

            _reservationController.ApproveReservation(reservation);
            
            Assert.AreEqual(true,reservation.Deposit.IsReserved(_validDateRange));
            Assert.AreEqual(ApprovedReservationState, reservation.Status);
        }
        
        [TestMethod]
        public void TestRejectReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);
            
            string rejectionReason = "Precio demasiado elevado";
            _reservationController.RejectReservation(reservation, rejectionReason);
            
            Reservation updatedReservation = _context.Reservations.Find(reservation.Id);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual(RejectedReservationState, updatedReservation.Status); // Verificar que el estado es rechazado
            Assert.AreEqual(rejectionReason, updatedReservation.Message);
            Assert.AreEqual(false, _deposit0.IsReserved(_validDateRange));
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void TestApproveReservationWithoutAdmin()
        {
            Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            
            List<Administrator> admins = _context.Users.OfType<Administrator>().ToList();
            _context.Users.RemoveRange(admins);
            _context.SaveChanges();

           _reservationController.ApproveReservation(reservation);
        }
        
        [TestMethod]
        [ExpectedException(typeof(UserDoesNotExistException))]
        public void TestRejectReservationBecauseThereIsNoAdministrator()
        {
            Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            
            var admins = _context.Users.OfType<Administrator>().ToList();
            _context.Users.RemoveRange(admins);
            _context.SaveChanges();
            
            _reservationController.RejectReservation(reservation, "Reason");
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRejectReservationBeauseOfNullReservation()
        {
            String reason = "Esta reason es valida";
            
            Administrator admin = new Administrator("Emiliano", "emiliano@gmail.com", "Pass12Emi#");
            _context.Users.Add(admin);
            _context.SaveChanges();
            
            _reservationController.RejectReservation(null, reason);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRejectReservationBecauseOfEmptyReason()
        {
            Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            
            Administrator admin = new Administrator("Emiliano", "emiliano@gmail.com", "Pass12Emi#");
            _context.Users.Add(admin);
            _context.SaveChanges();
            
            _reservationController.RejectReservation(reservation, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestRejectReservationBecauseOfNullReason()
        {
            Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            
            Administrator admin = new Administrator("Emiliano", "emiliano@gmail.com", "Pass12Emi#");
            _context.Users.Add(admin);
            
            _reservationController.RejectReservation(reservation, null);
        }
        
        [TestMethod]
        public void TestCancelRejectionOfReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            _session.LoginUser(ClientEmail, ClientPassword);
            
            List<Promotion> promotionsToAddToDeposit = new List<Promotion>();
            foreach (Promotion promotion in promotionsToAddToDeposit)
            {
                _deposit0.AddPromotion(promotion);
                promotion.AddDeposit(_deposit0);
            }
            
            //TODO: Cambiar el agregar al controller
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            //_reservationController.Add(reservation);
 
            /*
             _session.LoginUser(AdminEmail, AdminPassword);
            _reservationController.CancelRejectionOfReservation(reservation);
 
            Assert.AreEqual(PendingReservationState, reservation.Status);
            Assert.AreEqual("-", reservation.Message);
             */
        }
    
        [TestMethod]
        [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
        public void TestClientCannotCancelRejectionOfReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _session.LoginUser(ClientEmail, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);
            _reservationController.CancelRejectionOfReservation(reservation);
        }
        
        /*
        [TestMethod]
        public void TestRateReservation()
        {
            _userController.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
            _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.AddReservation(reservation);
            Rating rating = new Rating(5, "Excelente");
 
            _session.LoginUser(ClientEmail,ClientPassword);
            _reservationController.RateReservation(reservation, rating);
 
            CollectionAssert.Contains(_deposit0.Ratings, rating);
            CollectionAssert.Contains(controller.GetRatings(), rating);
            Assert.AreEqual(reservation.Rating, rating);
        }
        */
        
        [TestMethod]
        public void TestRateReservationLog()
        {
            _userController.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);
            Rating rating = new Rating(5, "Excelente");
 
            _session.LoginUser(ClientEmail,ClientPassword);
            _reservationController.RateReservation(reservation, rating);
         
            DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            List<LogEntry> logs = _session.ActiveUser.Logs;
         
            Assert.IsTrue(logs.Any(log => log.Message == UserLogInLogMessage));
            Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date 
                                          && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        }
        
        /*
        [TestMethod]
        [ExpectedException(typeof(ActionRestrictedToClientException))]
        public void TestAdministratorCannotRateReservation()
        {
            _userController.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.AddReservation(reservation);
            Rating rating = new Rating(5, "Excelente");
 
            _session.LoginUser(AdminEmail,AdminPassword);
            _reservationController.RateReservation(reservation, rating);
 
            CollectionAssert.Contains(_deposit0.Ratings, rating);
            CollectionAssert.Contains(.GetRatings(), rating);
            Assert.AreEqual(reservation.Rating, rating);
        }
        */

    }
}
