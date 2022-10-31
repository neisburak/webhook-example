using AirlineWeb.Dtos;

namespace AirlineWeb.MessageBus;

public interface IMessageBusClient
{
    void SendMessage(MessageForNotification messageForNotification);
}