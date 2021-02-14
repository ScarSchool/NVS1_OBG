using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
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
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;

namespace SinusDisplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string MQTT_URL = "broker.hivemq.com:8000/mqtt";
        private const string MQTT_TOPIC = "htlvillach/5BHIF/obg/sinus";
        private IMqttClient client;
        public ValueAddedNotifier SinusAddedNotifier { set; get; }
        public MainWindow()
        {
            InitializeComponent();
            SinusAddedNotifier = new ValueAddedNotifier("Sinus");
            sinusChart.DataContext = SinusAddedNotifier;
            MqttFactory factory = new();
            client = factory.CreateMqttClient();
            client.UseConnectedHandler(ConnectionHandler);
            client.UseApplicationMessageReceivedHandler(MessageReceivedHandler);
            ExecuteThings();
        }

        private async void ExecuteThings()
        {
            var options = new MqttClientOptionsBuilder().WithWebSocketServer(MQTT_URL).Build();
            await client.ConnectAsync(options, CancellationToken.None);
        }

        private void MessageReceivedHandler(MqttApplicationMessageReceivedEventArgs args)
        {
            string message = Encoding.ASCII.GetString(args.ApplicationMessage.Payload).Replace(',', '.');
            if (double.TryParse(message, out double result))
            {
                string date = DateTime.Now.ToLongTimeString();
                SinusAddedNotifier.Labels.Add(date);
                SinusAddedNotifier.DataCollection[0].Values.Add(result);
                MessageBox.Show(message);
            }
        }

        private async void ConnectionHandler(MqttClientConnectedEventArgs args)
        {
            await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(MQTT_TOPIC).Build());
            MessageBox.Show($"Connected to {MQTT_URL} and subscribed to '{MQTT_TOPIC}'");
        }
    }
}
