using Int.Api.Hubs;
using Int.Application.Services;
using Int.Domain;
using Int.Domain.Entities;
using Int.Domain.Repositories.Contract;
using Int.Domain.Services.Contrct;
using Int.Infrastructure.Entities;
using InT.Application.Services;
using InT.Infrastructure;
using InT.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

public class Program
{
	public static async Task Main(string[] args)  // جعلها async وإرجاع Task
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddDbContext<FiElSekkaContext>(options =>
			options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
		);

		builder.Services.AddIdentity<User, IdentityRole>()
			.AddRoles<IdentityRole>()
			.AddEntityFrameworkStores<FiElSekkaContext>()
			.AddDefaultTokenProviders();

		builder.Services.AddControllers();
		builder.Services.AddOpenApi();;
		builder.Services.AddHttpContextAccessor();

		builder.Services.AddSignalR();

		var jwtSettings = builder.Configuration.GetSection("JwtSettings");
		var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);




		builder.Services.AddAuthentication(options =>
		{
			options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
					})
			.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Cookie scheme
			.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
			{
				options.RequireHttpsMetadata = false;
				options.SaveToken = true;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = new SymmetricSecurityKey(key),
					ValidateIssuer = false,
					ValidateAudience = false,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero
				};
			})
			.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
			{
				options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
				options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
				options.CallbackPath = "/signin-google";
				options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // ⬅️ مهم جدًا
				options.SaveTokens = true;
				options.Events.OnCreatingTicket = async context =>
				{
					var email = context.Identity?.FindFirst(ClaimTypes.Email)?.Value;
					var name = context.Identity?.FindFirst(ClaimTypes.Name)?.Value;

					var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<User>>();
					var signInManager = context.HttpContext.RequestServices.GetRequiredService<SignInManager<User>>();

					var user = await userManager.FindByEmailAsync(email);
					if (user == null)
					{
						user = new User
						{
							Email = email,
							UserName = email,
							firstName = name?.Split(' ').FirstOrDefault() ?? "",
							lastName = name?.Split(' ').LastOrDefault() ?? "",
							SSN = null // أو أي قيمة افتراضية
						};
						await userManager.CreateAsync(user);
						await userManager.AddToRoleAsync(user, "User");
					}

					await signInManager.SignInAsync(user, isPersistent: false);
				};
			});







		builder.Services.AddAuthorization();
		builder.Services.AddScoped<ICarService, CarService>();
		builder.Services.AddScoped<ICarRepository, CarRepository>();
		builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
		builder.Services.AddScoped<IRoleService, RoleService>();
		builder.Services.AddScoped<IAuthService, AuthService>();
		builder.Services.AddScoped<ICloudinaryServices, CloudinaryServices>();
		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

		builder.Services.AddSwaggerGen(options =>
		{
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.Http,
				Scheme = "Bearer",
				BearerFormat = "JWT",
				In = ParameterLocation.Header,
				Description = "Enter {your_token}"
			});

			options.AddSecurityRequirement(new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme,
							Id = "Bearer"
						}
					},
					new string[] {}
				}
			});
		});

		var app = builder.Build();
		app.UseHttpsRedirection();

		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}
		app.UseStaticFiles();

		using (var scope = app.Services.CreateScope())
		{
			var services = scope.ServiceProvider;
			var userManager = services.GetRequiredService<UserManager<User>>();
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

			// تأكد من وجود رول "Admin"
			var roleExists = await roleManager.RoleExistsAsync("Admin");
			if (!roleExists)
			{
				await roleManager.CreateAsync(new IdentityRole("Admin"));
			}

			// إنشاء المستخدم لو مفيش مستخدم "أدمن" موجود
			var user = await userManager.FindByEmailAsync("admin@admin.com");
			if (user == null)
			{
				user = new User
				{
					firstName = "admin",
					lastName = "admin",
					UserName = "admin2@admin.com",
					Email = "admin2@admin.com",
					PhoneNumber = "0000000000"
				};
				var result = await userManager.CreateAsync(user, "Password123!");  // تأكد من وضع باسورد قوي
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(user, "Admin");
				}
			}
		}

		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
		app.Run();
	}
}
