using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TienDaoAPI.Models;

namespace TienDaoAPI.Helpers
{
    public static class ExpressionProvider<T>
    {
        public static Func<IQueryable<T>, IOrderedQueryable<T>> GetSortExpression(string? sortBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query => query.OrderBy(o => EF.Property<object>(o!, "Id"));
            }

            var sortParts = sortBy!.Trim().Split(' ');

            var sortField = sortParts[0];
            var sortDirection = "asc";

            if (sortField.StartsWith("-"))
            {
                sortField = sortField.Substring(1);
                sortDirection = "desc";
            }
            var parameter = Expression.Parameter(typeof(T), "b");
            var property = Expression.Property(parameter, sortField);
            var sortExpression = Expression.Lambda(property, parameter);

            var methodName = sortDirection == "desc" ? "OrderByDescending" : "OrderBy";
            var method = typeof(Queryable).GetMethods()
                .FirstOrDefault(m => m.Name == methodName && m.GetParameters().Length == 2)?
                .MakeGenericMethod(typeof(T), property.Type);

            if (method == null)
            {
                throw new InvalidOperationException($"Method '{methodName}' not found.");
            }
            var queryParam = Expression.Parameter(typeof(IQueryable<T>), "query");
            var resultExpression = Expression.Call(method, queryParam, sortExpression);

            var sortFunction = Expression.Lambda<Func<IQueryable<T>, IOrderedQueryable<T>>>(
                resultExpression,
                queryParam
                ).Compile();
            return sortFunction;
        }

        public static Expression<Func<Book, bool>> BuildBookFilter(BookFilter filter)
        {
            var b = Expression.Parameter(typeof(Book), "b");
            var filterParts = new List<Expression>();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var keyword = Expression.Constant(filter.Keyword);
                var title = Expression.Property(b, nameof(Book.Title));
                var containsKeyword = Expression.Call(title, "Contains", null, keyword);
                filterParts.Add(containsKeyword);
            }

            if (filter.Status.HasValue)
            {
                var statusProperty = Expression.Property(b, nameof(Book.Status));
                var status = Expression.Constant(filter.Status.Value);
                filterParts.Add(Expression.Equal(statusProperty, status));
            }

            if (!string.IsNullOrEmpty(filter.Genres))
            {
                var genreIds = filter.Genres.Split(',').Select(int.Parse).ToList();
                if (genreIds.Any())
                {
                    var genreExpressions = genreIds.Select(genreId =>
                        Expression.Equal(
                            Expression.Property(b, nameof(Book.GenreId)),
                            Expression.Constant(genreId)
                        )
                    );

                    filterParts.Add(genreExpressions.Aggregate(Expression.OrElse));
                }
            }

            //if (!string.IsNullOrEmpty(filter.Tags))
            //{
            //    var tagsProperty = Expression.Property(b, nameof(Book.Tags));
            //    var tags = Expression.Constant(filter.Tags, typeof(string));
            //    filterParts.Add(Expression.Equal(tagsProperty, tags));
            //}

            //if (!string.IsNullOrEmpty(filter.Chapter))
            //{
            //    var chapterProperty = Expression.Property(b, nameof(Book.Chapter));
            //    var chapter = Expression.Constant(filter.Chapter, typeof(string));
            //    filterParts.Add(Expression.Equal(chapterProperty, chapter));
            //}

            var deletedAtProperty = Expression.Property(b, nameof(Book.DeletedAt));
            var nullDeletedAt = Expression.Equal(deletedAtProperty, Expression.Constant(null, typeof(DateTime?)));
            filterParts.Add(nullDeletedAt);

            if (filterParts.Count == 0) return b => b.DeletedAt == null;

            var body = filterParts.Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<Book, bool>>(body, b);
        }

        public static Expression<Func<User, bool>> BuildUserFilter(UserFilter filter)
        {
            var u = Expression.Parameter(typeof(User), "u");
            var filterParts = new List<Expression>();

            if (!string.IsNullOrEmpty(filter.Keyword))
            {
                var keyword = Expression.Constant(filter.Keyword);
                var email = Expression.Property(u, nameof(User.Email));
                var fullName = Expression.Property(u, nameof(User.FullName));
                var containsEmail = Expression.Call(email, "Contains", null, keyword);
                var containsFullName = Expression.Call(fullName, "Contains", null, keyword);
                var containsKeyword = Expression.OrElse(containsEmail, containsFullName);
                filterParts.Add(containsKeyword);
            }

            if (!string.IsNullOrEmpty(filter.Status))
            {
                var statusProperty = Expression.Property(u, nameof(User.Status));
                var status = Expression.Constant(filter.Status);

                filterParts.Add(Expression.Equal(statusProperty, status));
            }

            if (filterParts.Count == 0)
            {
                return Expression.Lambda<Func<User, bool>>(Expression.Constant(true), u);
            }

            var body = filterParts.Aggregate(Expression.AndAlso);
            return Expression.Lambda<Func<User, bool>>(body, u);
        }
    }
}
