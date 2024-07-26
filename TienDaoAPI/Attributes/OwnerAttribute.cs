using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TienDaoAPI.DTOs;
using TienDaoAPI.Repositories.IRepositories;
using TienDaoAPI.Utils;

namespace TienDaoAPI.Attributes
{
    public class OwnerAttribute : Attribute, IAsyncActionFilter
    {
        private readonly Type _entityType;
        public OwnerAttribute(Type entityType)
        {
            _entityType = entityType;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var idObj) && idObj is int id)
            {
                var userId = (context.HttpContext.Items["UserDTO"] as UserDTO)!.Id;

                // Resolve the appropriate service based on the entity type
                var repositoryType = typeof(IRepository<>).MakeGenericType(_entityType);
                var service = context.HttpContext.RequestServices.GetService(repositoryType);

                if (service != null)
                {
                    var getByIdMethod = repositoryType.GetMethod("GetByIdAsync");
                    if (getByIdMethod != null)
                    {
                        var task = (Task)getByIdMethod.Invoke(service, new object[] { id })!;
                        await task.ConfigureAwait(false);
                        var entity = task.GetType().GetProperty("Result")!.GetValue(task);

                        if (entity == null)
                        {
                            context.Result = new NotFoundObjectResult(new Response
                            {
                                StatusCode = HttpStatusCode.NotFound,
                                Message = $"{_entityType.Name} does not exist",
                            });
                            return;
                        }

                        // Perform the ownership check
                        var isOwner = await CheckOwnershipAsync(entity, userId);
                        if (!isOwner)
                        {
                            await context.HttpContext.Response.WriteAsJsonAsync(new Response
                            {
                                StatusCode = HttpStatusCode.Forbidden,
                                IsSuccess = false,
                                Message = "Bạn không có đủ quyền để thực hiện hành động này"
                            });
                            return;
                        }
                    }
                }

                await next();
            }
        }

        private Task<bool> CheckOwnershipAsync(object entity, int userId)
        {
            // Use reflection to check ownership
            var ownerProperty = entity.GetType().GetProperty("OwnerId");
            if (ownerProperty != null)
            {
                int ownerId = (int)ownerProperty.GetValue(entity)!;
                return Task.FromResult(ownerId == userId);
            }

            return Task.FromResult(false); // No ownership property found
        }
    }
}
