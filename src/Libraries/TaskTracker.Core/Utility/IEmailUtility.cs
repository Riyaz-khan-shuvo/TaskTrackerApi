namespace TaskTracker.Core.Utility
{
    public interface IEmailUtility
    {
        void SendEmail(string receiverEmail, string receiverName, string subject, string body);
    }
}
