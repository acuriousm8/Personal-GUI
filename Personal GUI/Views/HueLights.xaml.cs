using Newtonsoft.Json.Linq;
using Q42.HueApi;
using Q42.HueApi.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Personal_GUI.Views
{
    /// <summary>
    /// Interaction logic for HueLights.xaml
    /// </summary>
    /// 

    public class Hue_lights_info
    {
        public bool auto_innit { get; set; }
        public string bridge_ip_address { get; set; }
        public string bridge_appkey { get; set; }
    }

    public partial class HueLights : UserControl
    {
        string local_path = $@"C:\Users\{Environment.UserName}\AppData\Roaming\Personal_GUI\Hue";
        string bridge_ip;
        string appkey = "CYZc4C8ydeqWl6-bly6uRggfl3fLzk6XAbR17ORd"; //he1Go67nGOudF6OD5NacTkuaGhdDayBlfsYf393I
        ILocalHueClient client;
        Dictionary<string, string> light_dictionary = new Dictionary<string, string>();


        public HueLights()
        {
            InitializeComponent();
            startupAsync();
        }

        bool Check_ip(string ip)
        {
            try
            {
                string html = new WebClient().DownloadString($@"http://{ip}/description.xml");
                if (html.ToLower().Contains("philips hue bridge"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        async Task startupAsync()
        {
            if(System.IO.File.Exists($@"{local_path}\logged_info.json"))
            {
                string json_file_to_string = File.ReadAllText($@"{local_path}\logged_info.json");
                var logged_info = JsonSerializer.Deserialize<Hue_lights_info>(json_file_to_string);

                if(logged_info.auto_innit)
                {
                    if (Check_ip(logged_info.bridge_ip_address))
                    {
                        bridge_ip = logged_info.bridge_ip_address;
                    }    
                    else
                    {
                        await Find_bridgeAsync();
                    }
                    if (logged_info.bridge_appkey != null)
                    {
                        appkey = logged_info.bridge_appkey;
                        client = new LocalHueClient(bridge_ip);
                        client.Initialize(appkey);
                        get_lights();
                    }
                }
            }
        }

        async Task log_to_fileAsync()
        {
            try
            {
                System.IO.File.Delete($@"{local_path}\logged_info.json");
            }
            catch
            { }
            var lights_Info = new Hue_lights_info
            {
                auto_innit = true,
                bridge_ip_address = bridge_ip,
                bridge_appkey = appkey
            };
            using FileStream createStream = File.Create($@"{local_path}\logged_info.json");
            await JsonSerializer.SerializeAsync(createStream, lights_Info);
            await createStream.DisposeAsync();
        }

        async Task Find_bridgeAsync()
        {
            IBridgeLocator locator = new HttpBridgeLocator(); 
            var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(10));
            bridges = await HueBridgeDiscovery.CompleteDiscoveryAsync(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(30));


            foreach (var bridge in bridges)
            {
                ListViewItem item_to_add = new ListViewItem();
                item_to_add.Content = $@"{bridge.IpAddress} : {bridge.BridgeId}";
                bridge_ip = bridge.IpAddress;
                listviewbridges.Items.Add(item_to_add);
            }
        }

        async Task Register_application_hue_bridge()
        {
            //registeration code - HuaAPI register wasn't working 
            var url = $@"http://{bridge_ip}/api";

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";
            httpRequest.ContentType = "application/json";

            var data = $@"{{""devicetype"":""Personal_GUI#{Environment.MachineName}""}}";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var hue_lights_registrer_response = streamReader.ReadToEnd(); //should reply with string
                appkeylabel.Content = hue_lights_registrer_response;

                //skidded code from the api... literal fucking copy paste (and modify)... I still don't know how to properly parse jsons
                JObject? result;
                try
                {
                    JArray jresponse = JArray.Parse(hue_lights_registrer_response);
                    result = (JObject?)jresponse.First;
                    JToken? error;
                    if (result.TryGetValue("error", out error))
                    {
                        if (error["type"]?.Value<int>() == 101) // link button not pressed
                            MessageBox.Show("Link button not pressed");
                        else
                            throw new HueException(error["description"]?.Value<string>());
                    }

                    var username = result["success"]?["username"]?.Value<string>();
                    appkeylabel.Content = username.ToString();
                    appkey = username.ToString();
                }
                catch
                {
                    //Not an expected response. Return response as exception
                    MessageBox.Show($@"Unknown error occured. Please send error to our developer:{System.Environment.NewLine}{hue_lights_registrer_response}");
                }
            }
            //MessageBox.Show(httpResponse.StatusCode.ToString

            
            
        }

        async Task get_lights()
        {
            var result = await client.GetLightsAsync();
            var test = await client.GetGroupsAsync();

            string GetLights = string.Format("Found {0} lights", result.Count());

            listviewlights.Items.Clear();
            light_dictionary.Clear();
            int i = 1;
            
            foreach (Light light in result)
            {
                ListViewItem item_to_add = new ListViewItem();
                item_to_add.Content = $@"{light.Name} : {light.State.On} : {light.ModelId}";
                if (light.State.On == true)
                {
                    item_to_add.Background = Brushes.MediumSpringGreen;
                }
                else
                {
                    item_to_add.Background = Brushes.Tomato;
                }
                listviewlights.Items.Add(item_to_add);
                light_dictionary.Add(light.Name, i.ToString());
                i++;
            }

            foreach(var group in test)
            {
                ListViewItem item_to_add = new ListViewItem();
                item_to_add.Content = $@"{group.Name} : {group.State} : {group.ModelId} : {group.Lights} : GROUP";
                if (group.State.AllOn == true)
                {
                    item_to_add.Background = Brushes.MediumSpringGreen;
                }
                else
                {
                    item_to_add.Background = Brushes.Tomato;
                }
                listviewlights.Items.Add(item_to_add);
                light_dictionary.Add(group.Name, i.ToString());
                i++;
            }
        }

        private void discover_bridges(object sender, RoutedEventArgs e)
        {
            Find_bridgeAsync();
        }

        private void register_button(object sender, RoutedEventArgs e)
        {
            Register_application_hue_bridge();
        }

        private void init_hue_button(object sender, RoutedEventArgs e)
        {
            client = new LocalHueClient(bridge_ip);
            client.Initialize(appkey);
            log_to_fileAsync();
        }

        private void get_light_count_button(object sender, RoutedEventArgs e)
        {
            get_lights();
        }

        private void Turn_on_button(object sender, RoutedEventArgs e)
        {
            var command = new LightCommand();
            command.On = true; //makes on command

            List<string> selected_lights = new List<string>(); // gets selected lights and puts to list
            foreach (ListViewItem item in listviewlights.SelectedItems)
            {
                selected_lights.Add(light_dictionary[item.Content.ToString().Split(" : ")[0]]);
            }

            client.SendCommandAsync(command, selected_lights); //sends on command to selected lights list
            get_lights(); // refreshes list
        }
        private void Turn_off_button(object sender, RoutedEventArgs e)
        {
            var command = new LightCommand();
            command.On = false;

            List<string> selected_lights = new List<string>();
            foreach (ListViewItem item in listviewlights.SelectedItems)
            {
                selected_lights.Add(light_dictionary[item.Content.ToString().Split(" : ")[0]]);
            }

            client.SendCommandAsync(command, selected_lights);
            get_lights();
        }
    }
}
