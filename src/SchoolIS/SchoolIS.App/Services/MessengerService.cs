using CommunityToolkit.Mvvm.Messaging;
using SchoolIS.App.Services.Interfaces;

namespace SchoolIS.App.Services;

public class MessengerService(IMessenger messenger) : IMessengerService {
  public IMessenger Messenger { get; } = messenger;

  public void Send<TMessage>(TMessage message)
    where TMessage : class {
    Messenger.Send(message);
  }
}