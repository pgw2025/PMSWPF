using CommunityToolkit.Mvvm.Messaging.Messages;
using PMSWPF.Enums;

namespace PMSWPF.Message;

public class NotificationMessage : ValueChangedMessage<string>
{
    public NotificationMessage(string msg, NotificationType type = NotificationType.Info, bool isGlobal = false) :
        base(msg)
    {
        Type = type;
        IsGlobal = isGlobal;
    }

    public NotificationType Type { get; set; }
    public bool IsGlobal { get; set; }
}