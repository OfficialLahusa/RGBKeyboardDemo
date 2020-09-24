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
using System.Threading;
using RBGKeyboardDemo;

namespace RBGKeyboardDemo
{
    class Program
    {
        private const bool anim_autoplay = true;

        static void Main(string[] args)
        {
            RGBSurface surface = RGBSurface.Instance;
            Random random = new Random();
            surface.Exception += eventArgs => Console.WriteLine(eventArgs.Exception.Message);

            TimerUpdateTrigger updateTrigger = new TimerUpdateTrigger();
            updateTrigger.UpdateFrequency = 1.0 / 200.0;
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
            Console.WriteLine("Device Count: " + surface.Devices.Count());

            int index = 0;
            foreach (IRGBDevice device in surface.Devices)
            {
                Console.WriteLine("[" + index + "] " + device.DeviceInfo.DeviceName + " || " + device.DeviceInfo.DeviceType + " || " + device.DeviceInfo.Manufacturer + " || " + device.DeviceInfo.Model);
                index++;
            }

            //Index of the chosen device
            int choice;

            if (surface.Devices.Count() < 1)
            {
                Console.WriteLine("No devices detected :(");
                Console.ReadKey();

                return;
            } else if(surface.Devices.Count() == 1)
            {
                choice = 0;
            } else
            {
                //Let the user choose which device the effect should be applied to by it's index in the enumeration
                Console.Write("Pick a device [Enter Number]:\n>");


                bool isOutOfRange = false;
                do
                {
                    //Is the entered string an int?
                    while (!int.TryParse(Console.ReadLine(), out choice))
                    {
                        Console.Write("Invalid Number.\n>");
                    }

                    //If it is an int, is it out of range?
                    isOutOfRange = choice < 0 || choice > surface.Devices.Count() - 1;
                    if (isOutOfRange)
                    {
                        Console.Write("Invalid Number.\n>");
                    }
                } while (isOutOfRange);
            }
            

            //Determine device-specific led position minima and maxima (Rectangle Bounds)
            double minx = double.PositiveInfinity, miny = double.PositiveInfinity, maxx = double.NegativeInfinity, maxy = double.NegativeInfinity;
            int count = 0;
            foreach (Led led in surface.Devices.ElementAt(choice))
            {
                if (led.ActualLocation.X < minx) minx = led.ActualLocation.X;
                if (led.ActualLocation.Y < miny) miny = led.ActualLocation.Y;
                if (led.ActualLocation.X > maxx) maxx = led.ActualLocation.X;
                if (led.ActualLocation.Y > maxy) maxy = led.ActualLocation.Y;
                count++;
            }

            Console.WriteLine($"Total LED count: {count}, X: [{minx}, {maxx}], Y: [{miny}, {maxy}]\nPress [ESC] to Close, any other key to continue the animation.");

            //Set background
            ListLedGroup groupBackground = new ListLedGroup(surface.Devices.ElementAt(choice));
            groupBackground.ZIndex = 0;
            groupBackground.Brush = new SolidColorBrush(new Color(0, 0, 0));

            Dictionary<LedId, ListLedGroup> ledGroups = new Dictionary<LedId, ListLedGroup>();

            foreach (Led led in surface.Devices.ElementAt(choice))
            {
                if(!ledGroups.ContainsKey(led.Id))
                {
                    ListLedGroup group = new ListLedGroup(led);
                    group.ZIndex = 1;
                    group.Brush = new SolidColorBrush(new Color(0.0, 0.0, 0.0));
                    group.Brush.IsEnabled = false;

                    ledGroups.Add(led.Id, group);
                }
            }

            ConsoleKeyInfo key = Console.ReadKey();
            Console.Write("\r");
            while (key.Key != ConsoleKey.Escape)
            {
                LedId pressedKeyLedId = KeyConverter.GetLedByConsoleKey(key.Key);

                foreach (Led led in surface.Devices.ElementAt(choice))
                {
                    ledGroups[led.Id].Brush.IsEnabled = false;
                }

                if (pressedKeyLedId != LedId.Invalid)
                {
                    double hue = random.NextDouble() * 360;
                    foreach (Led led in surface.Devices.ElementAt(choice))
                    {
                        const double distScaleFac = 2.0;
                        double deltaX = led.ActualLocation.X - led.ActualSize.Width / 2 - surface.Devices.ElementAt(choice)[pressedKeyLedId].ActualLocation.X - surface.Devices.ElementAt(choice)[pressedKeyLedId].ActualSize.Width / 2;
                        double deltaY = led.ActualLocation.Y - led.ActualSize.Height / 2 - surface.Devices.ElementAt(choice)[pressedKeyLedId].ActualLocation.Y - surface.Devices.ElementAt(choice)[pressedKeyLedId].ActualSize.Height / 2;
                        double dist = distScaleFac * Math.Sqrt(deltaX*deltaX+deltaY*deltaY);

                        Color color = new Color();
                        color = color.SetHSV(hue, 1.0, 1.0);
                        color = color.SetA(Math.Max(255.0 - dist, 0)/255.0);

                        ledGroups[led.Id].Brush = new SolidColorBrush(color);
                        ledGroups[led.Id].Brush.IsEnabled = true;
                    }
                }

                key = Console.ReadKey();
                Console.Write("\r");
            }


            Console.ReadKey();
            return;
        }
    }
}
