﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using System.Security.Claims;
using TienDaoAPI.DTOs;

namespace TienDaoAPI.Helpers
{
    public class CustomPolicyEvaluator : IPolicyEvaluator
    {
        private readonly SessionProvider _sessionProvider;

        public CustomPolicyEvaluator(SessionProvider sessionProvider)
        {
            _sessionProvider = sessionProvider;
        }

        public async Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
        {
            var bearerToken = context.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(bearerToken))
            {
                return AuthenticateResult.Fail("Missing token!");
            }

            var token = bearerToken.Replace("Bearer ", "");
            try
            {
                if (!_sessionProvider.VerifyToken(token))
                {
                    return AuthenticateResult.Fail("Verify token failed!");
                }

                var userDTO = _sessionProvider.GetContext(token);
                context.Items["UserDTO"] = userDTO;
                var claims = new[] { new Claim(ClaimTypes.Name, userDTO.Email) };
                var identity = new ClaimsIdentity(claims, "Bearer");
                var principal = new ClaimsPrincipal(identity);

                return AuthenticateResult.Success(new AuthenticationTicket(principal, "Bearer"));
            }
            catch
            {
                return AuthenticateResult.Fail("Invalid token!");
            }
        }

        public async Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy policy, AuthenticateResult authenticateResult, HttpContext context, object resource)
        {
            if (authenticateResult == null || !authenticateResult.Succeeded)
            {
                return PolicyAuthorizationResult.Challenge();
            }
            List<string> requiredRoles = new List<string>();
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                var authorizeAttributes = endpoint.Metadata.OfType<AuthorizeAttribute>().ToList();
                foreach (var authorizeAttribute in authorizeAttributes)
                {
                    if (authorizeAttribute.Roles != null)
                    {
                        requiredRoles.AddRange(authorizeAttribute.Roles.Split(',').Select(role => role.Trim()));
                    }
                }
            }
            if (requiredRoles.Count == 0)
            {
                return PolicyAuthorizationResult.Success();
            }
            if (context.Items.TryGetValue("UserDTO", out var userObj) && userObj is UserDTO user)
            {
                var userRoles = user.Role;

                if (!requiredRoles.Any(role => userRoles.Contains(role)))
                {
                    return PolicyAuthorizationResult.Forbid();
                }
            }
            return PolicyAuthorizationResult.Success();
        }
    }
}
