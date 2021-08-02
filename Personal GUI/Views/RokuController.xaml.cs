using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using RokuDeviceLib;

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for RokuController.xaml
    /// </summary>
    /// 
    public class Roku_info
    {
        string stored_ip { get; set; }
    }

    public partial class RokuController : UserControl
    {
        public RokuController()
        {
            InitializeComponent();
            discover_rokuAsync();
        }

        async Task discover_rokuAsync()
        {
            var discoveredDevices = await Discover();
            Console.WriteLine("");
        }


        public const string DISCOVER_MESSAGGE = "M-SEARCH * HTTP/1.1\r\n" +
                            "HOST: {0}:{1}\r\n" +
                            "ST:roku:ecp\r\n" +
                            "MAN:\"ssdp:discover\"\r\n";

        public const string LOCATION_PATTERN = @"location:\s?(?<address>http://[^/]+/)";
        public static async Task<RokuDevice[]> Discover(string ip = "239.255.255.250", int port = 1900, int waitSeconds = 30)
        {

            var token = new CancellationTokenSource();

            var ucs = new UdpClient();

            var mcEndpoint = new IPEndPoint(IPAddress.Parse(ip), port);

            var data = string.Format(DISCOVER_MESSAGGE, ip, port);

            var discoverBytes = Encoding.UTF8.GetBytes(data);

            var startTime = DateTime.Now;
            UdpReceiveResult result;
            string DeviceResponse;
            token.CancelAfter(TimeSpan.FromSeconds(waitSeconds));
            var deviceQueries = new List<Task<RokuDevice>>();

            using (var udp = new UdpClient())
            {
                await ucs.SendAsync(discoverBytes, discoverBytes.Length, mcEndpoint);

                while (!token.IsCancellationRequested)
                {
                    var operation = ucs.ReceiveAsync();

                    try
                    {
                        operation.Wait(token.Token); //Wait for result until cancelled
                    }
                    catch (Exception ex)
                    {
                        //We either have no devices or we didn't get any additional devices
                        //Not sure where to log this but can't display to user
                    }

                    if (operation.IsCompleted && !operation.IsFaulted)
                    {
                        result = operation.Result;
                        DeviceResponse = Encoding.ASCII.GetString(result.Buffer);

                        var match = Regex.Match(DeviceResponse, LOCATION_PATTERN, RegexOptions.IgnoreCase);
                        if (match.Success && DeviceResponse.ToLower().Contains("roku"))
                        {
                            var address = match.Groups["address"].Value;
                            //Don't allow device info request to hold up discovering additional devices
                            var deviceQuery = ReadDevice(address);

                            deviceQueries.Add(deviceQuery);


                        }
                    }
                }

            }

            //Wait seconds does not apply to this
            var devices = await Task.WhenAll(deviceQueries);

            return devices;
        }

        public static async Task<RokuDevice> ReadDevice(string endpoint)
        {
            var client = new HttpClient();
            var requestUrl = endpoint + "query/device-info";
            var result = await client.GetStreamAsync(requestUrl);

            var serializer = new XmlSerializer(typeof(RokuDevice));
            var device = (RokuDevice)serializer.Deserialize(result);
            device.Endpoint = endpoint;

            return device;
        }

    }
}
