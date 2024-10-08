namespace ClassManager.Domain.Libs.MassTransit.Publish;

public interface IPublishBus
{
  Task PublicAsync<T>(T message, CancellationToken cancelationTokent = default) where T : class;
}