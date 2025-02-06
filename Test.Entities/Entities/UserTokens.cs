using System;
using System.Collections.Generic;

namespace Test.Entities;

public partial class UserTokens
{
    public int TokenId { get; set; }

    public string LoginName { get; set; } = null!;

    public string RefreshToken { get; set; } = null!;

    public DateTime Expiration { get; set; }
}
