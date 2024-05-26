 
 /* using BusinessLogic;
 using BusinessLogic.Exceptions.ControllerExceptions;
 using DepoQuick.Domain;
 using Client = DepoQuick.Domain.Client;
 using DateTime = System.DateTime;
 
 namespace DepoQuickTests;
 
 [TestClass]
 public class ControllerTest
 {
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
     private DateRange _expiredDateRange;
     private DateRange _currentDateRange;
     private DateRange _validDateRange;
 
     [TestInitialize]
     public void TestInitialize()
     {
         _client = new Client(ClientName, ClientEmail, ClientPassword);
         
         _deposit0 = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         
         _expiredDateRange = new DateRange(DateTime.Now.AddDays(-10), DateTime.Now.AddDays(-5));
         _currentDateRange = new DateRange(DateTime.Now, DateTime.Now.AddDays(10));
         _validDateRange = new DateRange(DateTime.Now.AddDays(5), DateTime.Now.AddDays(10));
     }
     
     
     
     
     
     
     
     [TestMethod]
     public void TestSearchForAPromotionById()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
         
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         Promotion promotion2 = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         depositsToAddToPromotion.Add(deposit);
 
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         controller.AddPromotion(promotion2, depositsToAddToPromotion);
         
         int promotion1Id = promotion1.Id;
 
         Assert.AreEqual(promotion1, controller.GetPromotion(promotion1Id));
     }
 
     [TestMethod]
     [ExpectedException(typeof(PromotionNotFoundException))]
     public void TestSearchForAPromotionUsingAnInvalidId()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         Promotion promotion2 = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         Deposit deposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         depositsToAddToPromotion.Add(deposit);
 
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         controller.AddPromotion(promotion2, depositsToAddToPromotion);
 
         controller.GetPromotion(-4);
     }
     
     [TestMethod]
     public void TestUpdatePromotion()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         
         Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
         newDepositsToAddPromotion.Add(newDeposit);
         String newLabel = PromotionLabel0;
         double newDiscountRate = PromotionDiscountRate0;
         DateRange newDateRange = _validDateRange;
 
         controller.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
         controller.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
         
         CollectionAssert.Contains(controller.GetPromotions(), promotion1);
         CollectionAssert.DoesNotContain(promotion1.Deposits, _deposit0);
         CollectionAssert.Contains(promotion1.Deposits, newDeposit);
         CollectionAssert.Contains(newDeposit.Promotions, promotion1);
         CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion1);
         Assert.AreEqual(newLabel, promotion1.Label);
         Assert.AreEqual(newDiscountRate, promotion1.DiscountRate);
         Assert.AreEqual(newDateRange, promotion1.ValidityDate);
     }
     
     [TestMethod]
     [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
     public void TestClientCannotUpdatePromotionData()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         
         Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
         newDepositsToAddPromotion.Add(newDeposit);
         String newLabel = PromotionLabel0;
         double newDiscountRate = PromotionDiscountRate0;
         DateRange newDateRange = _validDateRange;
 
         controller.LogoutUser();
         controller.LoginUser(ClientEmail,ClientPassword);
         controller.UpdatePromotionData(promotion1, newLabel, newDiscountRate, newDateRange);
     }
     
     [TestMethod]
     [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
     public void TestClientCannotUpdatePromotion()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         
         Deposit newDeposit = new Deposit(DepositArea0, DepositSize0, DepositAirConditioning0);
         List<Deposit> newDepositsToAddPromotion = new List<Deposit>();
         newDepositsToAddPromotion.Add(newDeposit);
 
         controller.LogoutUser();
         controller.LoginUser(ClientEmail,ClientPassword);
         controller.UpdatePromotionDeposits(promotion1, newDepositsToAddPromotion);
     }
 
     [TestMethod]
     [ExpectedException(typeof(PromotionNotFoundException))]
     public void TestDeletePromotion()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion = new Promotion();
         List<Deposit> depositsToAddPromotion = new List<Deposit>();
         depositsToAddPromotion.Add(_deposit0);
         controller.AddPromotion(promotion, depositsToAddPromotion);
         int id = promotion.Id;
 
         controller.DeletePromotion(id);
 
         controller.GetPromotion(id);
     }
 
     [TestMethod]
     public void TestDeletePromotionRemovesPromotionFromRelatedDeposits()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion, depositsToAddToPromotion);
         int id = promotion.Id;
 
         controller.DeletePromotion(id);
 
         CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
     }
     
     [TestMethod]
     [ExpectedException(typeof(ActionRestrictedToAdministratorException))]
     public void TestClientCannotDeletePromotion()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
         
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.RegisterClient(ClientName,ClientEmail,ClientPassword,ClientPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion = new Promotion();
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion, depositsToAddToPromotion);
         int id = promotion.Id;
     
         controller.LogoutUser();
         controller.LoginUser(ClientEmail,ClientPassword);
         controller.DeletePromotion(id);
 
         CollectionAssert.DoesNotContain(_deposit0.Promotions, promotion);
     }
     
     [TestMethod]
     public void TestDeleteAllExpiredPromotions()
     {
         Session session = new Session();
         Controller controller = new Controller(session);
 
         controller.RegisterAdministrator(AdminName,AdminEmail,AdminPassword,AdminPassword);
         controller.LoginUser(AdminEmail,AdminPassword);
         Promotion promotion1 = new Promotion();
         Promotion promotion2 = new Promotion();
         promotion1.ValidityDate = _validDateRange;
         promotion2.ValidityDate = _expiredDateRange;
         List<Deposit> depositsToAddToPromotion = new List<Deposit>();
         depositsToAddToPromotion.Add(_deposit0);
         controller.AddPromotion(promotion1, depositsToAddToPromotion);
         controller.AddPromotion(promotion2, depositsToAddToPromotion);
 
         controller.DeleteAllExpiredPromotions();
 
         CollectionAssert.Contains(controller.GetPromotions(), promotion1);
         CollectionAssert.DoesNotContain(controller.GetPromotions(), promotion2);
     }
 }
 */