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
    public async Task<ActionResult<decimal>> Up() {
      Console.WriteLine("CONTROL: VOLUME UP");
      await audio.VolumeUp();
      double volume = await audio.GetVolume();
      return (decimal)Math.Round(volume);
    }

    [HttpGet("down")]
    public async Task<ActionResult<decimal>> Down() {
      Console.WriteLine("CONTROL: VOLUME DOWN");
      await audio.VolumeDown();
      double volume = await audio.GetVolume();
      return (decimal)Math.Round(volume);
    }

    [HttpGet("mute")]
    public async Task<ActionResult<decimal>> Mute() {
      Console.WriteLine("CONTROL: TOGGLE MUTE");
      await audio.ToggleMute();
      double volume = await audio.GetVolume();
      return (decimal)Math.Round(volume);
    }

    [HttpGet("get")]
    public async Task<ActionResult<decimal>> Get() {
      Console.WriteLine("CONTROL: GET VOLUME");
      double volume = await audio.GetVolume();
      return (decimal)Math.Round(volume);
    }

    [HttpGet("set/{volume}")]
    public async Task<ActionResult<decimal>> Set(double volume) {
      Console.WriteLine($"CONTROL: SET VOLUME = {volume}");
      await audio.SetVolume(volume);
      double newVolume = await audio.GetVolume();
      return (decimal)Math.Round(newVolume);
    }

    [HttpGet("play")]
    public IActionResult Play() {
      Console.WriteLine("CONTROL: TOGGLE PLAY");
      keyboard.Send(0xB3);
      return new OkResult();
    }

    [HttpGet("next")]
    public IActionResult Next() {
      Console.WriteLine("CONTROL: NEXT MEDIA");
      keyboard.Send(0xB0);
      return new OkResult();
    }

    [HttpGet("previous")]
    public IActionResult Previous() {
      Console.WriteLine("CONTROL: PREVIOUS MEDIA");
      keyboard.Send(0xB1);
      return new OkResult();
    }

    [HttpGet("reconnect")]
    public async Task<ActionResult<decimal>> Reconnect() {
      Console.WriteLine("CONTROL: RECONNECT AUDIO DEVICE");
      audio.UpdateDevice();
      double newVolume = await audio.GetVolume();
      return (decimal)Math.Round(newVolume);
    }
  }
}
