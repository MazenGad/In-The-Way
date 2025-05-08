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
		builder.Services.AddScoped<IEmailService, EmailService>();
		builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


		builder.Services.AddSwaggerGen(options =>
		{
			options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

			// 🔐 تعريف السيكيوريتي
			options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
			{
				Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer abc123\"",
				Name = "Authorization",
				In = ParameterLocation.Header,
				Type = SecuritySchemeType.ApiKey,
				Scheme = "Bearer"
			});

			// ✋ فرض استخدام التوكن على كل الريكويستات
			options.AddSecurityRequirement(new OpenApiSecurityRequirement()
	{
		{
			new OpenApiSecurityScheme
			{
				Reference = new OpenApiReference
				{
					Type = ReferenceType.SecurityScheme,
					Id = "Bearer"
				},
				Scheme = "oauth2",
				Name = "Bearer",
				In = ParameterLocation.Header,
			},
			new List<string>()
		}
	});
		});


		builder.Services.AddCors(options =>
		{
			options.AddPolicy("AllowAll", builder =>
			{
				builder
					.WithOrigins("https://localhost:7246") // Swagger UI origin
					.AllowAnyMethod()
					.AllowAnyHeader();
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
		app.Use(async (context, next) =>
		{
			try
			{
				await next.Invoke();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception: {ex.Message}");
				throw;
			}
		});

		using (var scope = app.Services.CreateScope())
		{
			var services = scope.ServiceProvider;
			var userManager = services.GetRequiredService<UserManager<User>>();
			var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
			var context = services.GetRequiredService<FiElSekkaContext>();

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
			// Seed Brands and Models (Egypt Market)
			if (!context.Brands.Any())
			{
				var brands = new List<Brand>
		{
			new Brand { Name = "Hyundai" },
			new Brand { Name = "Chevrolet" },
			new Brand { Name = "Nissan" },
			new Brand { Name = "BYD" },
			new Brand { Name = "Geely" },
			new Brand { Name = "Toyota" }
		};

				context.Brands.AddRange(brands);
				await context.SaveChangesAsync();

				var models = new List<Model>
		{
			new Model { Name = "Verna", BrandId = brands.First(b => b.Name == "Hyundai").Id },
			new Model { Name = "Elantra", BrandId = brands.First(b => b.Name == "Hyundai").Id },
			new Model { Name = "Lanos", BrandId = brands.First(b => b.Name == "Chevrolet").Id },
			new Model { Name = "Optra", BrandId = brands.First(b => b.Name == "Chevrolet").Id },
			new Model { Name = "Sunny", BrandId = brands.First(b => b.Name == "Nissan").Id },
			new Model { Name = "Tiida", BrandId = brands.First(b => b.Name == "Nissan").Id },
			new Model { Name = "F3", BrandId = brands.First(b => b.Name == "BYD").Id },
			new Model { Name = "L3", BrandId = brands.First(b => b.Name == "BYD").Id },
			new Model { Name = "Emgrand", BrandId = brands.First(b => b.Name == "Geely").Id },
			new Model { Name = "Corolla", BrandId = brands.First(b => b.Name == "Toyota").Id }
		};

				context.Models.AddRange(models);
				await context.SaveChangesAsync();
			}

			// Seed Colors
			if (!context.Colors.Any())
			{
				var colors = new List<Color>
		{
			new Color { Name = "White" },
			new Color { Name = "Black" },
			new Color { Name = "Silver" },
			new Color { Name = "Gray" },
			new Color { Name = "Red" },
			new Color { Name = "Blue" }
		};

				context.Colors.AddRange(colors);
				await context.SaveChangesAsync();
			}

		}
		app.UseCors("AllowAll");
		app.UseAuthentication();
		app.UseAuthorization();

		app.MapControllers();
		app.Run();
	}
}
