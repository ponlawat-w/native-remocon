using System.Collections.Generic;
using System.Net.NetworkInformation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NativeRemocon.Controllers {
  [ApiController][Route("api/network")][Authorize]
  public class NetworkController {
    [HttpGet("list")]
    public ActionResult<IEnumerable<object>> ListAll() {
      List<object> results = new List<object>();
      foreach (NetworkInterface ni in NetworkInterface.GetAllNetworkInterfaces()) {
        if (ni.GetIPProperties().UnicastAddresses.Count < 2) {
          continue;
        }
        results.Add(new {
          Name = ni.Name,
          Address = ni.GetIPProperties().UnicastAddresses[1].Address.ToString()
        });
      }

      return results;
    }
  }
}
