﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Tenant
{
    public interface ITenantContext
    {
        Tenant CurrentTenant { get; set; }
    }
}
