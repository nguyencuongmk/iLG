using iLG.API.Extensions;
using iLG.API.Models.Responses;
using iLG.Infrastructure.Repositories.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;

namespace iLG.API.Handlers
{
    public class PermissionRequirement(string permission) : IAuthorizationRequirement
    {
        public string Permission { get; } = permission;
    }

    public class PermissionHandler(IUserRepository userRepository, IPermissionRepository permissionRepository) : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPermissionRepository _permissionRepository = permissionRepository;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            _ = int.TryParse(context.User.FindFirst("userId")?.Value, out int userId);
            var user = await _userRepository.GetAsync(expression: u => u.Id == userId && !u.IsLocked && !u.IsDeleted);
            var userRoles = user?.Roles;
            var permissions = await _permissionRepository.GetPermissionsByRoles(userRoles);

            if (permissions.Any(p => p.Name == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else
            {
                //var response = new ApiResponse();

                //response.Errors.Add(new Error
                //{
                //    ErrorMessage = "Forbiden"
                //});

                //var result = response.GetResult(context.HttpContext, "An Exception occurred");

                //await context.Response.WriteAsJsonAsync(result, cancellationToken);
            }
        }
    }
}

