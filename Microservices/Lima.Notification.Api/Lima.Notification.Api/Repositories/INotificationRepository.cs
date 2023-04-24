using System.Threading.Tasks;


namespace Lima.Notification.Api.Repositories
{
    public interface INotificationRepository
    {
        Task<int> AddNotification(Domain.Notification notification);
    }
}
