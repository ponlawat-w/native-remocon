using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace NativeRemocon {
  public class Startup {
    public Startup(IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      services.AddAuthentication(opts => {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
      }).AddJwtBearer(opts => {
        opts.TokenValidationParameters = new TokenValidationParameters {
          IssuerSigningKey = new SymmetricSecurityKey(Program.SessionKey),
          ValidateIssuer = true,
          ValidIssuer = "NativeRemocon",
          ValidateAudience = true,
          ValidAudience = Program.SessionAudience,
          ValidateLifetime = false,
          ValidateActor = false,
          ValidateIssuerSigningKey = false,
          RequireExpirationTime = false
        };
      });
      services.AddControllers();
    }

    private string GetToken() {
      return new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
        "NativeRemocon",
        Program.SessionAudience,
        null,
        null,
        null,
        new SigningCredentials(new SymmetricSecurityKey(Program.SessionKey), SecurityAlgorithms.HmacSha512)
      ));
    }

    private void StartUpLaunch() {
      string url = $"{Program.Url}/www/access.html?token={HttpUtility.HtmlEncode(GetToken())}";
      Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
      }

      app.UseHttpsRedirection();

      app.UseStaticFiles(new StaticFileOptions{
        FileProvider = new PhysicalFileProvider(Path.Combine(env.ContentRootPath, "FrontEnd")),
        RequestPath = "/www"
      });

      app.UseAuthentication();

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints => {
        endpoints.MapControllers();
      });

      StartUpLaunch();
    }
  }
}
