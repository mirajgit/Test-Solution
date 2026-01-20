using System;
using System.Collections.Generic;

namespace WebAPI.Data;

public partial class View_SMS_PendingMessage
{
    public string LoginName { get; set; } = null!;

    public string? UserEid { get; set; }

    public int? CountMessage { get; set; }
}
