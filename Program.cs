using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NativeRemocon {
  public class Program {
    public static readonly byte[] SessionKey = CreateKey();
    public static readonly string SessionAudience = Convert.ToBase64String(CreateKey());
    public static readonly IDictionary<string, string> Config = ReadConfig();

    public static string Port { get => Config.ContainsKey("port") ? Config["port"] : "8319"; }

    public static string Url { get => $"https://localhost:{Port}"; }

    static IDictionary<string, string> ReadConfig() {
      StreamReader reader = new StreamReader("app.conf");
      return new Dictionary<string, string>(reader.ReadToEnd().Split(Environment.NewLine)
        .Where(s => s.Trim() != "" && s.Contains("="))
        .Select(s => {
          string[] data = s.Split("=").Select(x => x.Trim()).ToArray();
          return new KeyValuePair<string, string> (data[0], data[1]);
        }));
    }

    static byte[] CreateKey(int length = 64) {
      byte[] result = new byte[length];
      using (RandomNumberGenerator rng = RandomNumberGenerator.Create()) {
        rng.GetBytes(result);
      }
      return result;
    }

    public static void Main(string[] args) {
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder => {
          webBuilder
            .UseUrls($"https://::{Port};https://0.0.0:{Port}")
            .UseStartup<Startup>();
        });
  }
}
