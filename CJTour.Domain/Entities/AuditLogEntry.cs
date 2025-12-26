using System;
using System.Collections.Generic;

namespace CJTour.Domain.Entities;

/// <summary>
/// Auth: Audit trail for user actions.
/// </summary>
public partial class AuditLogEntry
{
    public int AuditLogEntryId { get; set; }

    public string Action { get; set; } = null!;

    public string EntityName { get; set; } = null!;

    public string? EntityId { get; set; }

    public string? UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public string? Changes { get; set; }
}
