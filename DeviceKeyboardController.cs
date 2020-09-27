using System;
using System.Runtime.InteropServices;

namespace NativeRemocon {
  public class DeviceKeyboardController {

    [DllImport("user32.dll", SetLastError = true)]
    private static extern void keybd_event(
      byte virtualKey,
      byte scanCode,
      uint flags,
      IntPtr extraInfo
    );

    public DeviceKeyboardController() {}

    public void Send(byte keyCode) {
      keybd_event(keyCode, 0, 1, IntPtr.Zero);
    }

    public void CombinedSend(byte combiningKeyCode, byte mainKeyCode) {
      keybd_event(combiningKeyCode, 0, 1, IntPtr.Zero);
      keybd_event(mainKeyCode, 0, 1, IntPtr.Zero);
      keybd_event(mainKeyCode, 0, 3, IntPtr.Zero);
      keybd_event(combiningKeyCode, 0, 3, IntPtr.Zero);
    }
  }
}
