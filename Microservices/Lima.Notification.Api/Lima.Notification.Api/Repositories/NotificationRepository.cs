using Dapper;
using Lima.Core.Tenant;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Lima.Notification.Api.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private ITenantContext _tenantContext;

        public NotificationRepository(ITenantContext tenantContext)
        {
            _tenantContext = tenantContext;
        }

        public async Task<int> AddNotification(Domain.Notification notification)
        {
            using (SqlConnection connection = new SqlConnection(_tenantContext.CurrentTenant.ConnectionString))
            {
                await connection.OpenAsync();

                return await connection.QueryFirstOrDefaultAsync<int>(
                    sql: @"INSERT INTO NOTIFICATIONS (UserId, [Message], DateCreate)
		                        VALUES (@userId, @message, GETDATE());
                            SELECT IDENT_CURRENT('NOTIFICATION');",
                    param: new { @userId = notification.UserId, @message = notification.Message });
            }
        }
    }
}
