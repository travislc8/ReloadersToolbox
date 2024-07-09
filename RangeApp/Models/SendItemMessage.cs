using CommunityToolkit.Mvvm.Messaging.Messages;
namespace RangeApp.Models;
public class SendItemMessage : ValueChangedMessage<string> 
{
    public SendItemMessage(string value) : base(value)
    {
    }
}
