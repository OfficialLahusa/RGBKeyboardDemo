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
                Console.WriteLine(device.DeviceInfo.DeviceName + " || " + device.DeviceInfo.DeviceType + " || " + device.DeviceInfo.Manufacturer + " || " + device.DeviceInfo.Model);
                groupNum1 = new ListLedGroup(device[LedId.Keyboard_Num1]);
                groupNum2 = new ListLedGroup(device[LedId.Keyboard_Num2]);
                groupNum3 = new ListLedGroup(device[LedId.Keyboard_Num3]);
                groupNum4 = new ListLedGroup(device[LedId.Keyboard_Num4]);
                groupNum5 = new ListLedGroup(device[LedId.Keyboard_Num5]);
                groupNum6 = new ListLedGroup(device[LedId.Keyboard_Num6]);
                groupNum7 = new ListLedGroup(device[LedId.Keyboard_Num7]);
                groupNum8 = new ListLedGroup(device[LedId.Keyboard_Num8]);
                groupNum9 = new ListLedGroup(device[LedId.Keyboard_Num9]);
            }

            if(groupNum1 == null || groupNum2 == null || groupNum3 == null || groupNum4 == null || groupNum5 == null || groupNum6 == null || groupNum7 == null || groupNum8 == null || groupNum9 == null)
            {
                Console.WriteLine("[Error] No numpad LEDs identifiable.");
                return;
            }

            groupNum1.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum2.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum3.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum4.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum5.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum6.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum7.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum8.Brush = new SolidColorBrush(new Color(100, 100, 100));
            groupNum9.Brush = new SolidColorBrush(new Color(100, 100, 100));

            Console.WriteLine("Press [NumPad1-9] to play, [ESC] to quit.\n");

            ConsoleKeyInfo key = Console.ReadKey();
            Console.Write("\r");
            while (key.Key != ConsoleKey.Escape)
            {
                if(key.Key == ConsoleKey.NumPad1 || key.Key == ConsoleKey.NumPad2 || key.Key == ConsoleKey.NumPad3 ||
                    key.Key == ConsoleKey.NumPad4 || key.Key == ConsoleKey.NumPad5 || key.Key == ConsoleKey.NumPad6 ||
                    key.Key == ConsoleKey.NumPad7 || key.Key == ConsoleKey.NumPad8 || key.Key == ConsoleKey.NumPad9)
                {
                    groupNum1.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum2.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum3.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum4.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum5.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum6.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum7.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum8.Brush = new SolidColorBrush(new Color(100, 100, 100));
                    groupNum9.Brush = new SolidColorBrush(new Color(100, 100, 100));
                }

                switch(key.Key)
                {
                    default:
                        break;
                    case ConsoleKey.NumPad1:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum1.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad2:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum2.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad3:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum3.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad4:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum4.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad5:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum5.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad6:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum6.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad7:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum7.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad8:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum8.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                    case ConsoleKey.NumPad9:
                        //groupNumPad.Brush = new SolidColorBrush(new Color(100, 100, 100));
                        groupNum9.Brush = new SolidColorBrush(new Color(255, 0, 0));
                        break;
                }

                key = Console.ReadKey();
                Console.Write("\r");
            }

        }
    }
}
