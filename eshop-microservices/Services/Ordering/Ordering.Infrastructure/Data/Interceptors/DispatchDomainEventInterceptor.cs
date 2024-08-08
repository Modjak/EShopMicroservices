//using Microsoft.EntityFrameworkCore.Diagnostics;

//namespace Ordering.Infrastructure.Data.Interceptors;
//public class DispatchDomainEventInterceptor : SaveChangesInterceptor
//{
//    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
//    {
//        DispatchDomainevent(eventData.Context).GetAwaiter().GetResult()
//        return base.SavingChanges(eventData, result);
//    }

//    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
//    {
//        await DispatchDomainevent(eventData.Context);
//        return await base.SavingChangesAsync(eventData, result, cancellationToken);
//    }
//    private async Task DispatchDomainevent(DbContext? context)
//    {
//        if(context == null)
//        {
//            return;
//        }

//        var aggregates = context.ChangeTracker
//            .Entries<IAggregate>()
//            .Where(a => a.Entity.DomainEvents.Any())
//            .Select(a => a.Entity);
//    }
//}
