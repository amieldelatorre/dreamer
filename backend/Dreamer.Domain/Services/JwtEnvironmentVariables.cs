using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.Domain.Services;

public class JwtEnvironmentVariables
{
    public required string ValidIssuer { get; set; }
    public required string ValidAudience { get; set; }
    public required string SigningKey { get; set; }
}
