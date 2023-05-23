using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dansnom.Auth;
using Dansnom.Context;
using Dansnom.EmailServices;
using Dansnom.Implementations.Repositories;
using Dansnom.Implementations.Services;
using Dansnom.Interface.Repositories;
using Dansnom.Interface.Services;
using DansnomEmailServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Project
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
             services.AddCors(c => c
                .AddPolicy("Dansnom", builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Project", Version = "v1" });
            });
             services.AddDbContext<DansnomApplicationContext>(options => options
            .UseMySQL(Configuration.GetConnectionString("DansnomConnection")));
            
            services.AddScoped<IAdminServices, AdminServices>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            services.AddScoped<IRoleServices, RoleServices>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            
            services.AddScoped<ICustomerServices, CustomerServices>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            services.AddScoped<IRwavMaterialServuce, RawMaterialServices>();
            services.AddScoped<IRawMaterialRepository, RawMaterialRepository>();

            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddScoped<IProductionServices, ProductionServices>();
            services.AddScoped<IProductionRepository, ProductionRepository>();

            services.AddScoped<IProductServices, ProductServices>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IProductOrdersServices, ProductOrdersServices>();
            services.AddScoped<IProductOrdersRepository, ProductOrdersRepository>();

            services.AddScoped<IReviewServices, ReviewServices>();  
            services.AddScoped<IReviewRepository, ReviewRepository>();

            services.AddScoped<ISalesServices, SalesServices>();
            services.AddScoped<ISalesRepository, SalesRepository>();

            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IWalletServices, WalletServices>();
            services.AddScoped<IWalletRepository, WalletRepository>();

            services.AddScoped<IChatRepository, ChatRepository>();
            services.AddScoped<IChatService, ChatService>();

            services.AddScoped<ILikeRepository,LikeRepository>();
            services.AddScoped<ILikeService,LikeService>();

            services.AddScoped<IAddressRepository,AddressRepository>();
            services.AddScoped<IAddressService,AddressService>();

            services.AddScoped<IVerificationCodeRepository,VarificationCodeRepository>();
            services.AddScoped<IVerificationCodeService, VeryficationCodeService>();

            services.AddScoped<IProductionRawMaterialRepository,ProductionRawMaterialRepository>();
            services.AddScoped<IProductionRawMaterialService,ProductionRawMaterialService>();

            services.AddScoped<ICategoryService,CategoryService>();
            services.AddScoped<ICategoryRepository,CategoryRepository>();

            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartRepository, CartRepository>();

            services.AddScoped<IMailServices,MailService>();

            services.AddScoped<IJWTAuthenticationManager,JWTAuthenticationManager>();
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                //auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Project v1"));
            }

            

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("Dansnom");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
