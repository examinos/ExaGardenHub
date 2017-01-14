using System;

using M2Mqtt;
using M2Mqtt.Messages;

class Program
{
    public static MqttClient client = new MqttClient("localhost");

    static void Main(string[] args)
    {
        if (args.Length == 1)
            client = new MqttClient(args[0]);

        client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;

        string clientId = Guid.NewGuid().ToString();
        client.Connect(clientId);

        client.Subscribe(new string[] { "nodes/#", "garden/#" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });

        Console.ReadLine();

        client.Disconnect();
    }

    private static void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
        string msg = System.Text.Encoding.UTF8.GetString(e.Message);
        Console.WriteLine(DateTime.Now + " " + e.Topic + " " + msg);

        string trans = "";
        switch (e.Topic)
        {
            case "nodes/remote/humidity-sensor/i2c0-40":
                trans = "garden/environment/greenhouse-big/humidity";
                break;
            case "nodes/remote/thermometer/i2c0-49":
                trans = "garden/environment/greenhouse-big/temperature";
                break;

            case "garden/monitor/greenhouse-big/alert":
                trans = "nodes/base/light/-/set";
                break;
            default:
                break;
        }
        if (trans != "")
            client.Publish(trans, System.Text.Encoding.UTF8.GetBytes(msg));
    }

}