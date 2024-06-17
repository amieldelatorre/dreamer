using Dreamer.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dreamer.DataAccess.Repository;

public interface IJwtRepository
{
    Task Create(Jwt jwt);
}
