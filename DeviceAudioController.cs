using System.Threading.Tasks;
using AudioSwitcher.AudioApi;
using AudioSwitcher.AudioApi.CoreAudio;

namespace NativeRemocon {
  public class DeviceAudioController {
    private static readonly DeviceType[] DeviceTypePriorities = new DeviceType[] {
      DeviceType.Playback,
      DeviceType.All
    };

    private static readonly Role[] RolePriorities = new Role[] {
      Role.Multimedia,
      Role.Communications,
      Role.Console
    };

    private static CoreAudioDevice GetDefaultDevice() {
      CoreAudioController controller = new CoreAudioController();
      foreach (DeviceType deviceType in DeviceTypePriorities) {
        foreach (Role role in RolePriorities) {
          CoreAudioDevice device = controller.GetDefaultDevice(deviceType, role);
          if (device != null) {
            return device;
          }
        }
      }

      return null;
    }

    public CoreAudioDevice Device = null;

    public async Task ToggleMute() {
      await Device.ToggleMuteAsync();
    }

    public async Task VolumeUp() {
      double volume = await Device.GetVolumeAsync();
      await Device.SetVolumeAsync(volume + 2);
    }
    public async Task VolumeDown() {
      double volume = await Device.GetVolumeAsync();
      await Device.SetVolumeAsync(volume - 2);
    }

    public async Task<double> GetVolume() {
      return await Device.GetVolumeAsync();
    }

    public async Task SetVolume(double volume) {
      await Device.SetVolumeAsync(volume);
    }

    public void UpdateDevice() {
      Device = GetDefaultDevice();
    }

    public DeviceAudioController() {
      UpdateDevice();
    }
  }
}
