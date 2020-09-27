using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NativeRemocon.Controllers {
  [ApiController][Route("api/keyboard")][Authorize]
  public class KeyboardController {
    DeviceKeyboardController keyboard;

    static IDictionary<string, byte> keyMapping = new Dictionary<string, byte> {
      {"shift", 0xa0},
      {"ctrl", 0xa2},
      {"left", 37},
      {"right", 39},
      {"up", 38},
      {"down", 40},
      {"0", 48},
      {"1", 49},
      {"2", 50},
      {"3", 51},
      {"4", 52},
      {"5", 53},
      {"6", 54},
      {"7", 55},
      {"8", 56},
      {"9", 57},
      {"c", 67},
      {"f", 70},
      {"t", 84},
      {"space", 32}
    };

    public KeyboardController() {
      keyboard = new DeviceKeyboardController();
    }

    [HttpGet("{key}")]
    public IActionResult Key(string key) {
      if (keyMapping.ContainsKey(key)) {
        keyboard.Send(keyMapping[key]);
      }
      return new OkResult();
    }

    [HttpGet("{combineKey}/{mainKey}")]
    public IActionResult CombineKey(string combineKey, string mainKey) {
      if (keyMapping.ContainsKey(combineKey) && keyMapping.ContainsKey(mainKey)) {
        keyboard.CombinedSend(keyMapping[combineKey], keyMapping[mainKey]);
      }
      return new OkResult();
    }
  }
}