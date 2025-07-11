﻿using CloudinaryDotNet;
using CompanyTraining.Background;
using CompanyTraining.Middleware;
using CompanyTraining.Services;
using CompanyTraining.Utility;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Stripe;
using System.Reflection;
using System.Text;

namespace CompanyTraining
{
   public class Program
    {
        public static async Task Main(string[] args)
        {
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                                  });
            });

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging(); // ✨ أضف السطر ده

            });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

            builder.Services.Configure<CloudianrySettings>(
               builder.Configuration.GetSection("CloudianrySettings")
               );
            builder.Services.AddSingleton<Cloudinary>(
                sp =>
                {
                    var config = sp.GetRequiredService<IOptions<CloudianrySettings>>().Value;
                    var account = new CloudinaryDotNet.Account(config.CloudName, config.ApiKey, config.ApiSecret);
                    return new Cloudinary(account);
                }
              );
            // ✅ Prevent redirects to login page for API requests
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            // Configure JWT Authentication 
            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            builder.Services.AddSingleton<JwtOptions>(jwtOptions);
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPackageRepository, PackageRepository>();
            builder.Services.AddScoped<ISubscribeRepository, SubscribeRepository>();
			builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IModuleRepository,ModuleRepository>();
            builder.Services.AddScoped<ILessonRepository, LessonRepository>();
            builder.Services.AddScoped<IQuizRepository, QuizRepository>();
            builder.Services.AddScoped<IQuestionRepository,QuestionRepository>();
            builder.Services.AddScoped<IEmplyeeRepository, EmplyeeRepository>();
            builder.Services.AddScoped<IUserCourseRepository, UserCourseRepository>();
            builder.Services.AddScoped<IChoiceRepository, ChoiceRepository>();
            builder.Services.AddScoped<IUserAnswerRepository,UserAnswerRepository>();
            builder.Services.AddScoped<IUserQuizAttemptRepository,UserQuizAttemptRepository>();
            builder.Services.AddScoped<IUserLessonRepository,UserLessonRepository>();
            builder.Services.AddScoped<ICertificateRepository,CertificateRepository>();
            builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();


            //Background job
            builder.Services.AddHostedService<AutoSubmitJob>();
            builder.Services.AddScoped<AutoSubmitExpiredAttemptsService>();


            builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailConfiguration"));
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            builder.Services.AddSingleton<IMapper>(new Mapper(config));
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {

                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI();
                //app.MapScalarApiReference();

            }

            // Initialize DB
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    await DbInitializer.InitializeAsync(services);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error seeding the database: {ex.Message}");
                }
            }
            app.UseHttpsRedirection();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseRouting();
            app.UseAuthentication();
            app.UseMiddleware<CompanySubscriptionMiddleware>();

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
