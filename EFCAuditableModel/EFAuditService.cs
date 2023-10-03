using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuditableEFModel
{
    /// <summary>
    /// To access the HttpContext during a request, add the following code to the ConfigureServices method in the Startup.cs class.
    /// services.AddHttpContextAccessor();
    /// Reference: https://nwb.one/blog/auditing-dotnet-entity-framework-core
    /// </summary>
    public class EFAuditService
    {

        IHttpContextAccessor httpContextAccessor { get; set; }


        public EFAuditService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public List<IAuditableModel> OnBeforeSaveChanges(DbContext dbContext)
        {
            var changedEntries = new List<IAuditableModel>();

            // Get all the entities that inherit from AuditableEntity
            // and have a state of Added or Modified
            var entries = dbContext.ChangeTracker.Entries().Where(e => e.Entity is IAuditableModel 
                        && (e.State == EntityState.Added || e.State == EntityState.Modified));

            // For each entity we will set the Audit properties
            foreach (var entityEntry in entries)
            {
                var myEntity = (IAuditableModel)entityEntry.Entity;

                // If the entity state is Added let's set
                // the CreatedAt and CreatedBy properties
                if (entityEntry.State == EntityState.Added)
                {
                    myEntity.CreatedAt = DateTime.UtcNow;
                    if (this.httpContextAccessor != null)
                        myEntity.CreatedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "?";
                    else
                        myEntity.CreatedBy = Environment.UserName;
                }
                else
                {
                    // If the state is Modified then we don't want
                    // to modify the CreatedAt and CreatedBy properties
                    // so we set their state as IsModified to false
                    dbContext.Entry((IAuditableModel)entityEntry.Entity).Property(p => p.CreatedAt).IsModified = false;
                    dbContext.Entry((IAuditableModel)entityEntry.Entity).Property(p => p.CreatedBy).IsModified = false;
                }

                // In any case we always want to set the properties
                // ModifiedAt and ModifiedBy
                myEntity.ModifiedAt = DateTime.UtcNow;
                if (this.httpContextAccessor != null)
                    myEntity.ModifiedBy = this.httpContextAccessor?.HttpContext?.User?.Identity?.Name ?? "?";
                else
                    myEntity.ModifiedBy = Environment.UserName;

                changedEntries.Add(myEntity);
            }

            // After we set all the needed properties
            // we call the base implementation of SaveChangesAsync
            // to actually save our entities in the database
            return changedEntries;
        }

    }
}
