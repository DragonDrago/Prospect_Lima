using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lima.Core.Policy
{
    public class GrantAuthorizationHandler : AuthorizationHandler<GrantRequirement>
    {
        private Permissions _permissions;
        private ILogger _logger;

        public GrantAuthorizationHandler(Permissions permissions, ILogger logger)
        {
            _permissions = permissions;
            _logger = logger;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, GrantRequirement requirement)
        {
            if (_permissions.Grants.Contains(requirement.GrantName))
                context.Succeed(requirement);

            return Task.FromResult(0);
        }
    }
}
