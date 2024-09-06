namespace IProj.DataAccess.Interfaces.MessageBroker;

public interface IRabbitMqProducer
{
    public void SendMessage(string message);
}
