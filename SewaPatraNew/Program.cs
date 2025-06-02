using SewaPatra.BusinessLayer.Reports;
using SewaPatra.BusinessLayer;
using SewaPatra.DataAccess.Reports;
using SewaPatra.DataAccess;
using SewaPatra.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

// Add distributed memory cache (or another cache provider)
builder.Services.AddDistributedMemoryCache();


// Configure Database Connection String
builder.Services.AddSingleton<AreaRepository>();
builder.Services.AddSingleton<AreaService>();
builder.Services.AddSingleton<CoordinatorRepository>();
builder.Services.AddSingleton<CoordinatorService>();
builder.Services.AddSingleton<DonationBoxRepository>();
builder.Services.AddSingleton<DonationBoxService>();
builder.Services.AddSingleton<DonorRepository>();
builder.Services.AddSingleton<DonorService>();
builder.Services.AddSingleton<SewaPatraIssueRepository>();
builder.Services.AddSingleton<SewaPatraIssueService>();
builder.Services.AddSingleton<SewaPatraReceiptService>();
builder.Services.AddSingleton<SewaPatraReceiptRepository>();
builder.Services.AddSingleton<PaymentVoucherRepository>();
builder.Services.AddSingleton<PaymentVoucherService>();
builder.Services.AddSingleton<ReceiptVoucherRepository>();
builder.Services.AddSingleton<ReceiptVoucherService>();
builder.Services.AddSingleton<DropDownService>();
builder.Services.AddSingleton<ReportRepository>();
builder.Services.AddSingleton<ReportService>();
builder.Services.AddSingleton<SewaPatraIssueRegisterRepos>();
builder.Services.AddSingleton<SewaPatraIssueRegisterService>();
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<UserRegisterService>();

// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Adjust timeout as needed
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make session cookie essential
});
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
