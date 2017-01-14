using System;

using M2Mqtt;
using M2Mqtt.Messages;

using Newtonsoft.Json.Linq;

class Program
{
    public static MqttClient client = new MqttClient("localhost");

    public static Decimal lastHumi = 60;
    public static Decimal lastTemp = 20;
    public static Decimal actHumi = 60;
    public static Decimal actTemp = 20;

    static void Main(string[] args)
    {
        if (args.Length == 1)
            client = new MqttClient(args[0]);

        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { "garden/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Console.ReadLine();

        client.Disconnect();
    }

    private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string msg = System.Text.Encoding.UTF8.GetString(e.Message);
        Console.WriteLine(DateTime.Now + " " + e.Topic + " " + msg);

        JObject o = JObject.Parse(msg);

        switch (e.Topic)
        {
            case "garden/environment/greenhouse-big/humidity":
                lastHumi = actHumi;
                actHumi = (decimal)o["relative-humidity"][0];
                break;
            case "garden/environment/greenhouse-big/temperature":
                lastTemp = actTemp;
                actTemp = (decimal)o["temperature"][0];
                break;
            default:
                return;
        }

        bool alert = (actHumi < lastHumi * 0.95m || lastHumi * 1.05m < actHumi) || (actTemp < lastTemp * 0.95m || lastTemp * 1.05m < actTemp);

        client.Publish("garden/monitor/greenhouse-big/alert", System.Text.Encoding.UTF8.GetBytes(alert ? "{\"state\": true}" : "{\"state\": false}"));
    }

}