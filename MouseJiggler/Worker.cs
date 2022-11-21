using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PInvoke;

namespace MouseJiggler;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var inp = new User32.INPUT
            {
                type = User32.InputType.INPUT_MOUSE,
                Inputs = new User32.INPUT.InputUnion
                {
                    mi = new User32.MOUSEINPUT
                    {
                        dx = 0,
                        dy = 0,
                        mouseData = 0,
                        dwFlags = User32.MOUSEEVENTF.MOUSEEVENTF_MOVE,
                        time = 0,
                        dwExtraInfo_IntPtr = IntPtr.Zero,
                    },
                },
            };

            uint returnValue = User32.SendInput(nInputs: 1, pInputs: new[] { inp, }, cbSize: Marshal.SizeOf<User32.INPUT>());

            if (returnValue != 1)
            {
                int errorCode = Marshal.GetLastWin32Error();

                Debugger.Log(level: 1,
                              category: "Jiggle",
                              message:
                              $"failed to insert event to input stream; retval={returnValue}, errcode=0x{errorCode:x8}\n");

            }

            await Task.Delay(60000, stoppingToken);
        }
    }
}
