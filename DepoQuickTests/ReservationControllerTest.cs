using BusinessLogic;
using DepoQuick.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BusinessLogic.Exceptions.PaymentControllerExceptions;
using BusinessLogic.Exceptions.ReservationControllerExceptions;
using BusinessLogic.Exceptions.UserControllerExceptions;
using DepoQuick.Exceptions.ReservationExceptions;

namespace DepoQuickTests
{
    [TestClass]
    public class ReservationControllerTest
    {
        private const string ExpectedMessageForAnApprovedReservation = " ha sido aprobada";
        private const string ExpectedMessageForAnRejectedReservation = " ha sido rechazada";
        
        private ReservationController _reservationController;
        private PaymentController _paymentController;
        private UserController _userController;
        private DepoQuickContext _context;
        private Session _session;
        private NotificationController _notificationController;
        
        private const string AdminName = "Administrator";
        private const string AdminEmail = "administrator@domain.com";
        private const string AdminPassword = "Password1#";
        private const string ClientName = "Client";
        private const string ClientEmail = "client@domain.com";
        private const string ClientPassword = "Password2#";
        private const string ClientName2 = "ClientDos";
        private const string ClientEmail2 = "client2@domain.com";
        private const string ClientPassword2 = "Password2#";

        private const string DepositName = "Deposito";
        private const char DepositArea0 = 'A';
        private const string DepositSize0 = "Pequeño";
        private const bool DepositAirConditioning0 = true;
        private const char DepositArea1 = 'B';
        private const string DepositSize1 = "Grande";
        private const bool DepositAirConditioning1 = false;
        
     
        private const int ApprovedReservationState = 1;
        private const int PendingReservationState = 0;
        private const int RejectedReservationState = -1;
     
        private const string UserLogInLogMessage = "Ingresó al sistema";
        private const string UserLogOutLogMessage = "Cerró sesión";
        
        private LogController _logController;
        
        private Client _client;
        private Client _client2;
        private Deposit _deposit0;
        private Deposit _deposit1;
        private Deposit _deposit2;
        private DateRange _validDateRange;
        private DateRange _expiredDateRange;
        private DateRange _currentDateRange;
        
        [TestInitialize]
        public void TestInitialize()
        {
            if (_context != null)
            {
                _context.Database.EnsureDeleted();
                _context.Dispose();
            }
            
            _context = TestContextFactory.CreateContext();
            IRepository<User> _userRepository = new SqlRepository<User>(_context);
            _userController = new UserController(_userRepository);
            _logController = new LogController(new SqlRepository<LogEntry>(_context));
            _session = new Session(_userController, _logController);
            IRepository<Reservation> _reservationRepository = new SqlRepository<Reservation>(_context);
            
            
            IRepository<Payment> _paymentRepository = new SqlRepository<Payment>(_context);
            _paymentController = new PaymentController(_paymentRepository);
            
            _notificationController = new NotificationController(new SqlRepository<Notification>(_context));


           _reservationController = new ReservationController(_reservationRepository,_session,_paymentController,_notificationController  );
            
            _userController.RegisterAdministrator(AdminName, AdminEmail, AdminPassword, AdminPassword);

            _deposit0 = new Deposit (DepositName,DepositArea0,DepositSize0,DepositAirConditioning0);
            _deposit1 = new Deposit(DepositName,DepositArea1,DepositSize1,DepositAirConditioning1);
            _deposit2 = new Deposit(DepositName,DepositArea0,DepositSize1,DepositAirConditioning1);
            
            _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
            _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
            _expiredDateRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));

        }

        [TestMethod]
        public void TestGetAReservationByDateRange()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
    
            _reservationController.Add(reservation1);

            Reservation result = _reservationController.Get(_validDateRange);
            Assert.AreEqual(result.Id,reservation1.Id);
        }
        
        [TestMethod]
        public void TestAddAReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
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
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
            
            int reservation1Id = reservation1.Id;
            
            CollectionAssert.Contains(_reservationController.GetReservations(), reservation1);
            Assert.AreEqual(_deposit0, _reservationController.Get(reservation1Id).Deposit);
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
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
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
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
 
            _reservationController.Get(_currentDateRange);
        }

        [TestMethod]
        public void TestGetAllReservations()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit0, _client, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
            
            Assert.AreEqual(2, _reservationController.GetReservations().Count);
        }
        
        [TestMethod]
        public void TestGetAllReservationsById()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            _userController.RegisterClient(ClientName2, ClientEmail2, ClientPassword2, ClientPassword2);
            _client2 = (Client)_userController.GetUserByEmail(ClientEmail2);
            
            Reservation reservation1 = new Reservation(_deposit0, _client, _validDateRange);
            Reservation reservation2 = new Reservation(_deposit1, _client, _currentDateRange);
            Reservation reservation3 = new Reservation(_deposit2, _client2, _expiredDateRange);
    
            _reservationController.Add(reservation1);
            _reservationController.Add(reservation2);
            _reservationController.Add(reservation3);
            
            Assert.AreEqual(2, _reservationController.GetReservationsByUserId(_client.Id).Count);
        }
        
        [TestMethod]
        public void TestApproveReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            _session.LoginUser(AdminEmail, AdminPassword);
            var reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);

            _reservationController.PayReservation(reservation);

            _reservationController.ApproveReservation(reservation);
            
            Assert.AreEqual(true,reservation.Deposit.IsReserved(_validDateRange));
            Assert.AreEqual(ApprovedReservationState, reservation.Status);
        }
        
        [TestMethod]
        public void TestRejectReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);

            Reservation reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);
            _reservationController.PayReservation(reservation);
            
            _session.LoginUser(AdminEmail, AdminPassword);
            
            string rejectionReason = "Precio demasiado elevado";
            _reservationController.RejectReservation(reservation, rejectionReason);
            
            Reservation updatedReservation = _context.Reservations.Find(reservation.Id);
            Assert.IsNotNull(updatedReservation);
            Assert.AreEqual(RejectedReservationState, updatedReservation.Status); // Verificar que el estado es rechazado
            Assert.AreEqual(rejectionReason, updatedReservation.Message);
            Assert.AreEqual(false, _deposit0.IsReserved(_validDateRange));
        }
        
        [TestMethod]
        [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
        public void TestApproveReservationWithoutAdmin()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            _reservationController.Add(reservation);
            
            _session.LoginUser(ClientEmail, ClientPassword);

           _reservationController.ApproveReservation(reservation);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
        public void TestRejectReservationBecauseThereIsNoAdministrator()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            _session.LoginUser(ClientEmail, ClientPassword);
            
            Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            _reservationController.Add(reservation);
            
            
            _reservationController.RejectReservation(reservation, "Reason");
        }
        
        [TestMethod]
        [ExpectedException(typeof(ReservationNotFoundException))]
        public void TestRejectReservationBeauseOfNullReservation()
        {
            String reason = "Esta reason es valida";
            
            _session.LoginUser(AdminEmail,AdminPassword);
            
            _reservationController.RejectReservation(null, reason);
            
            
        }
        
        [TestMethod]
        [ExpectedException(typeof(ReservationWithEmptyMessageException))]
        public void TestRejectReservationBecauseOfEmptyReason()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            
            _reservationController.Add(reservation);
            
            _session.LoginUser(AdminEmail,AdminPassword);
            
            _reservationController.RejectReservation(reservation, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ReservationWithEmptyMessageException))]
        public void TestRejectReservationBecauseOfNullReason()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            Deposit deposit = new Deposit(DepositName,DepositArea0, DepositSize0, DepositAirConditioning0);
            Reservation reservation = new Reservation(deposit, _client, _validDateRange);
            _reservationController.Add(reservation);
            
            _session.LoginUser(AdminEmail,AdminPassword);
            
            _reservationController.RejectReservation(reservation, null);
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
       /* 
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
         
            Assert.IsTrue(logs.Any(log => log.Message == "Agregó valoración de la reserva " + reservation.Id));
            Assert.IsTrue(logs.Any(log => now.Date == log.Timestamp.Date 
                                          && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
        }
        
        
        [TestMethod]
        [ExpectedException(typeof(ActionRestrictedToClientException))]
        public void TestAdministratorCannotRateReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.Get(ClientEmail);
            
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);
            Rating rating = new Rating(5, "Excelente");
 
            _session.LoginUser(AdminEmail,AdminPassword);
            _reservationController.RateReservation(reservation, rating);
        }*/
        
        [TestMethod]
        public void TestPayReservation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);
            _reservationController.PayReservation(reservation);
            
            Assert.AreEqual("reservado", _paymentController.Get(reservation).Status);
        }
        
        [TestMethod]
        [ExpectedException(typeof(PaymentNotFoundException))]
        public void TestPaymentNotFoundException()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);

            _paymentController.Get(reservation); 

        }
        
        [TestMethod]
        public void TestPaymentCaptured()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            _session.LoginUser(AdminEmail, AdminPassword);
            
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);
            
            _reservationController.PayReservation(reservation);

            _reservationController.ApproveReservation(reservation); 
            
            Assert.AreEqual("capturado", _paymentController.Get(reservation).Status);
            
        }
        
        
        [TestMethod]
        [ExpectedException(typeof(PaymentNotFoundException))]
        public void TestPaymentNotFoundExceptionToCapture()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            
            _session.LoginUser(AdminEmail, AdminPassword);
            
            Reservation reservation = new Reservation(_deposit0, _client, _currentDateRange);
            _reservationController.Add(reservation);

            _reservationController.ApproveReservation(reservation); 
            
        }
        
        [TestMethod]
        public void TestApproveReservationNotifcation()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);
            _session.LoginUser(AdminEmail, AdminPassword);
            var reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);

            _reservationController.PayReservation(reservation);

            _reservationController.ApproveReservation(reservation);
            
            List<Notification> listOfNotifications = _notificationController.GetNotifications(_client); 
            
            DateTime now = DateTime.Now.AddSeconds(-DateTime.Now.Second);
            
            Assert.AreEqual(1,listOfNotifications.Count);
            Assert.AreEqual(listOfNotifications[0].Message , "Su reserva del deposito "+reservation.Deposit.Id+" en las fechas "+reservation.Date.InitialDate.ToString("dd/MM/yyyy")+" a "+reservation.Date.FinalDate.ToString("dd/MM/yyyy") + ExpectedMessageForAnApprovedReservation);
            Assert.IsTrue(listOfNotifications.Any(log => now.Date == log.Timestamp.Date
                                                         && now.Hour == log.Timestamp.Hour && now.Minute == log.Timestamp.Minute));
            Assert.AreEqual(listOfNotifications[0].Client , _client);
            CollectionAssert.Contains(_client.Notifications,listOfNotifications[0]);
        }
        
        [TestMethod]
        public void TestRejectReservationNotification()
        {
            _userController.RegisterClient(ClientName, ClientEmail, ClientPassword, ClientPassword);
            _client = (Client)_userController.GetUserByEmail(ClientEmail);

            Reservation reservation = new Reservation(_deposit0, _client, _validDateRange);
            _reservationController.Add(reservation);
            _reservationController.PayReservation(reservation);
            
            _session.LoginUser(AdminEmail, AdminPassword);
            
            string rejectionReason = "Precio demasiado elevado";
            _reservationController.RejectReservation(reservation, rejectionReason);
            
            List<Notification> listOfNotifications = _notificationController.GetNotifications(_client);

            Notification notification = listOfNotifications[0];

            _notificationController.Delete(notification); 
            
            CollectionAssert.DoesNotContain(_client.Notifications,notification);
        }
        

    }
}