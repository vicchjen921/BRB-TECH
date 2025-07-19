using Application.Repositories;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Repositories
{
    public class AuditLogRepository : Repository<AuditLog>, IAuditLogRepository
    {
        public AuditLogRepository(BrbTechDbContext context) : base(context)
        {}
    }
}
