using BusinessLogic;
using BusinessLogic.Controllers;
using DepoQuick.Domain;
using Interface.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

//DbContextOptionsBuilder<DepoQuickContext> optionsBuilder = new DbContextOptionsBuilder<DepoQuickContext>();
//optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//DepoQuickContext context = new DepoQuickContext(optionsBuilder.Options, false);

builder.Services.AddDbContext<DepoQuickContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
DepoQuickContext context = builder.Configuration.Get<DepoQuickContext>();

SqlRepository<User> userRepository = new SqlRepository<User>(context);
UserController userController = new UserController(userRepository);

SqlRepository<LogEntry> logRepository = new SqlRepository<LogEntry>(context);
LogController logController = new LogController(logRepository);

SessionController sessionController = new SessionController(userController, logController);

SqlRepository<Payment> paymentRepository = new SqlRepository<Payment>(context);
PaymentController paymentController = new PaymentController(paymentRepository);

SqlRepository<Deposit> depositRepository = new SqlRepository<Deposit>(context);
SqlRepository<Promotion> promotionRepository = new SqlRepository<Promotion>(context);
DepositController depositController = new DepositController(depositRepository, sessionController);
PromotionController promotionController = new PromotionController(promotionRepository, sessionController, depositController);


SqlRepository<Notification> notificationRepository = new SqlRepository<Notification>(context);
NotificationController notificationController = new NotificationController(notificationRepository );

SqlRepository<Reservation> reservationRepository = new SqlRepository<Reservation>(context);
ReservationController reservationController = new ReservationController(reservationRepository, sessionController, paymentController,notificationController);



SqlRepository<Rating> ratingRepository = new SqlRepository<Rating>(context);
RatingController ratingController = new RatingController(ratingRepository, sessionController, logController);


builder.Services.AddSingleton<SessionController>(sessionController);
builder.Services.AddSingleton<UserController>(userController);
builder.Services.AddSingleton<LogController>(logController);
builder.Services.AddSingleton<ReservationController>(reservationController);
builder.Services.AddSingleton<PaymentController>(paymentController);
builder.Services.AddSingleton<DepositController>(depositController);
builder.Services.AddSingleton<PromotionController>(promotionController);
builder.Services.AddSingleton<RatingController>(ratingController);
builder.Services.AddSingleton<NotificationController>(notificationController);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();