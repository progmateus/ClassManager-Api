
using MassTransit;
namespace ClassManager.Domain.Libs.MassTransit.Publish;
public class PublishBus : IPublishBus
{
  private readonly IPublishEndpoint _busEndpoint;

  public PublishBus(IPublishEndpoint busEndpoint)
  {
    _busEndpoint = busEndpoint;
  }
  public Task PublicAsync<T>(T message, CancellationToken cancelationTokent = default) where T : class
  {
    return _busEndpoint.Publish(message, cancelationTokent);
  }
}