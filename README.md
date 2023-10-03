# EFCAuditableModel
Auditable Model for Entity Framework Core to keep update the following properties for every saving
- CreatedAt
- CreatedBy
- ModifiedAt
- ModifiedBy

## To access the HttpContext during a request, add the following code to the ConfigureServices method in the Startup.cs class.
    /// services.AddHttpContextAccessor();
    /// Reference: https://nwb.one/blog/auditing-dotnet-entity-framework-core

### If not httpContextAccessor service found, the current user name will be used.
