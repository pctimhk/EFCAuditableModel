using System;
using System.Collections.Generic;
using System.Text;

namespace AuditableEFModel
{
    /// <summary>
    /// Reference: https://nwb.one/blog/auditing-dotnet-entity-framework-core
    /// </summary>
    public interface IAuditableModel
    {

        DateTime CreatedAt { get; set; }
        String CreatedBy { get; set; }

        DateTime ModifiedAt { get; set; }
        String ModifiedBy { get; set; }

        String ModifiedChannel { get; set; }
    }
}
