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
            updateTrigger.UpdateFrequency = 1.0 / 144.0;
            surface.RegisterUpdateTrigger(updateTrigger);
            updateTrigger.Start();

            bool throwExceptions = true;
            //RGBDeviceType loadType = (RGBDeviceType)(-1);
            RGBDeviceType loadType = RGBDeviceType.Keyboard;

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

            ILedGroup   groupNum1 = null, groupNum2 = null, groupNum3 = null,
                        groupNum4 = null, groupNum5 = null, groupNum6 = null,
                        groupNum7 = null, groupNum8 = null, groupNum9 = null,
                        groupNumPad = null;

            foreach (IRGBDevice device in surface.Devices)
            {
                //Determine device-specific led position minima and maxima (Rectangle Bounds)
                double minx = double.PositiveInfinity, miny = double.PositiveInfinity, maxx = double.NegativeInfinity, maxy = double.NegativeInfinity;
                int count = 0;
                foreach(Led led in device)
                {
                    if (led.ActualLocation.X < minx) minx = led.ActualLocation.X;
                    if (led.ActualLocation.Y < miny) miny = led.ActualLocation.Y;
                    if (led.ActualLocation.X > maxx) maxx = led.ActualLocation.X;
                    if (led.ActualLocation.Y > maxy) maxy = led.ActualLocation.Y;
                    count++;
                }

                foreach (Led led in device)
                {
                    ListLedGroup group = new ListLedGroup(led);
                    group.Brush = new SolidColorBrush(new Color(led.ActualLocation.X.Map(minx, maxx, 0.0, 1.0), led.ActualLocation.Y.Map(miny, maxy, 0.0, 1.0), 0.0));
                }

                Console.WriteLine($"Device {device.DeviceInfo.DeviceName} - Total LED count: {count}, X: [{minx}, {maxx}], Y: [{miny}, {maxy}]");
            }

            Console.ReadKey();
        }
    }
}
