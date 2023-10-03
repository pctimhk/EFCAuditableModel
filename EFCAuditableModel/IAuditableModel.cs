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

        DateTime CreatedDateTime { get; set; }
        String CreatedBy { get; set; }

        DateTime ModifiedDateTime { get; set; }
        String ModifiedBy { get; set; }

        String ModifiedChannel { get; set; }
    }
}
