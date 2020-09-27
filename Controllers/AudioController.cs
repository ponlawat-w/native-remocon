using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NativeRemocon.Controllers {
  [ApiController][Route("api/audio")][Authorize]
  public class AudioController {
    static DeviceAudioController audio = new DeviceAudioController();
    DeviceKeyboardController keyboard;

    public AudioController() {
      keyboard = new DeviceKeyboardController();
    }

    [HttpGet("up")]
    public async Task<IActionResult> Up() {
      Console.WriteLine("VOLUME UP");
      await audio.VolumeUp();
      return new OkResult();
    }

    [HttpGet("down")]
    public async Task<IActionResult> Down() {
      Console.WriteLine("VOLUME DOWN");
      await audio.VolumeDown();
      return new OkResult();
    }

    [HttpGet("mute")]
    public async Task<IActionResult> Mute() {
      Console.WriteLine("TOGGLE MUTE");
      await audio.ToggleMute();
      return new OkResult();
    }

    [HttpGet("get")]
    public async Task<ActionResult<double>> Get() {
      Console.WriteLine("GET VOLUME");
      return await audio.GetVolume();
    }

    [HttpGet("set/{volume}")]
    public async Task<IActionResult> Set(double volume) {
      Console.WriteLine($"SET VOLUME = {volume}");
      await audio.SetVolume(volume);
      return new OkResult();
    }

    [HttpGet("play")]
    public IActionResult Play() {
      Console.WriteLine("TOGGLE PLAY");
      keyboard.Send(0xB3);
      return new OkResult();
    }

    [HttpGet("next")]
    public IActionResult Next() {
      Console.WriteLine("NEXT MEDIA");
      keyboard.Send(0xB0);
      return new OkResult();
    }

    [HttpGet("previous")]
    public IActionResult Previous() {
      Console.WriteLine("PREVIOUS MEDIA");
      keyboard.Send(0xB1);
      return new OkResult();
    }

    [HttpGet("reconnect")]
    public IActionResult Reconnect() {
      audio.UpdateDevice();
      return new OkResult();
    }
  }
}
