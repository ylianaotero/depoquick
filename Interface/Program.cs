using BusinessLogic;
using DepoQuick.Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

Session session = new Session(userController, logController);

SqlRepository<Payment> paymentRepository = new SqlRepository<Payment>(context);
PaymentController paymentController = new PaymentController(paymentRepository);

SqlRepository<Deposit> depositRepository = new SqlRepository<Deposit>(context);
SqlRepository<Promotion> promotionRepository = new SqlRepository<Promotion>(context);
DepositController depositController = new DepositController(depositRepository, promotionRepository, session);
PromotionController promotionController = new PromotionController(depositRepository, promotionRepository, session);


SqlRepository<Reservation> reservationRepository = new SqlRepository<Reservation>(context);
ReservationController reservationController = new ReservationController(reservationRepository, session, paymentController);


SqlRepository<Rating> ratingRepository = new SqlRepository<Rating>(context);
RatingController ratingController = new RatingController(ratingRepository, session, logController);


builder.Services.AddSingleton<Session>(session);
builder.Services.AddSingleton<UserController>(userController);
builder.Services.AddSingleton<LogController>(logController);
builder.Services.AddSingleton<ReservationController>(reservationController);
builder.Services.AddSingleton<PaymentController>(paymentController);
builder.Services.AddSingleton<DepositController>(depositController);
builder.Services.AddSingleton<PromotionController>(promotionController);
builder.Services.AddSingleton<RatingController>(ratingController);

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