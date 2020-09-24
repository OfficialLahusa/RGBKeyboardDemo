using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using RGB.NET;
using RGB.NET.Brushes;
using RGB.NET.Brushes.Gradients;
using RGB.NET.Core;
using RGB.NET.Groups;


namespace RBGKeyboardDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            RGBSurface surface = RGBSurface.Instance;
            surface.Exception += eventArgs => Console.WriteLine(eventArgs.Exception.Message);

            TimerUpdateTrigger updateTrigger = new TimerUpdateTrigger();
            updateTrigger.UpdateFrequency = 1.0 / 60.0;
            surface.RegisterUpdateTrigger(updateTrigger);
            updateTrigger.Start();

            bool throwExceptions = true;
            RGBDeviceType loadType = (RGBDeviceType)(-1);
            //RGBDeviceType loadType = RGBDeviceType.Keyboard;

            surface.LoadDevices(RGB.NET.Devices.CoolerMaster.CoolerMasterDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.Corsair.CorsairDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.DMX.DMXDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.Logitech.LogitechDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            //MSI DLL Broken -- surface.LoadDevices(RGB.NET.Devices.Msi.MsiDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.Novation.NovationDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.Razer.RazerDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.SteelSeries.SteelSeriesDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);
            surface.LoadDevices(RGB.NET.Devices.WS281X.WS281XDeviceProvider.Instance, loadType, throwExceptions: throwExceptions);

            surface.AlignDevices();

            //List Detected Devices of loadType
            Console.WriteLine("Device Count: " + RGBSurface.Instance.Devices.Count());

            foreach(IRGBDevice device in surface.Devices)
            {
                Console.WriteLine(device.DeviceInfo.DeviceName + " || " + device.DeviceInfo.DeviceType + " || " + device.DeviceInfo.Manufacturer + " || " + device.DeviceInfo.Model);
            }

            ILedGroup ledGroup = new ListLedGroup(surface.Leds);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            while (true)
            {
                double time = stopwatch.Elapsed.TotalMilliseconds / 400.0;
                ledGroup.Brush = new RadialGradientBrush(new Point(0.5 + 0.4*Math.Cos(time), 0.5 + 0.4 * Math.Sin(time)), new RainbowGradient());
            }

            Console.ReadKey();
        }
    }
}
