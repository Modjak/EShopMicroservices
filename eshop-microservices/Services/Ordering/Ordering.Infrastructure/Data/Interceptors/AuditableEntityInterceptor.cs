using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

//sealed class ~ not able to inherit from 
public sealed class AuditableEntityInterceptor : SaveChangesInterceptor
{
    //SavingChangesAsync runs before the changes are saved in the database 
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
         
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        var entries = context.ChangeTracker.Entries<IEntity>();
        foreach (var entry in entries) // loop through all the IEntity typed Entities that has been changed
        {
            if(entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = "mehmet";
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.HasChangedOwnedEntities())
            {
                entry.Entity.LastModifiedBy = "mehmet";
                entry.Entity.LastModified = DateTime.UtcNow;
            }
        }
    }
}


public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
        r.TargetEntry != null &&
        r.TargetEntry.Metadata.IsOwned() &&
        (r.TargetEntry.State == EntityState.Added || r.TargetEntry.State == EntityState.Modified));
}
