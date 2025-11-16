# Real-Life .NET Examples - Supplemental Guide

This document contains additional real-world examples to complement the Advanced .NET Interview Preparation Guide.

## Real-Life Example: Complete Blog Platform with EF Core

Demonstration of advanced Entity Framework Core patterns in a real blogging platform.

```csharp
// ==================== DOMAIN ENTITIES ====================

// Base entity for audit trail
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }  // Soft delete
}

// Blog entity with navigation properties
public class Blog : BaseEntity
{
    public string Title { get; set; }
    public string Description { get; set; }
    public string OwnerId { get; set; }

    // Navigation properties
    public virtual User Owner { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<BlogFollower> Followers { get; set; }

    // Computed property
    public int FollowerCount => Followers?.Count ?? 0;
}

// Post entity with self-referencing relationship
public class Post : BaseEntity
{
    public string Title { get; set; }
    public string Content { get; set; }
    public string Slug { get; set; }
    public DateTime PublishedAt { get; set; }
    public PostStatus Status { get; set; }
    public int BlogId { get; set; }
    public string AuthorId { get; set; }
    public int? ParentPostId { get; set; }  // For threaded posts

    // Concurrency token
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Navigation properties
    public virtual Blog Blog { get; set; }
    public virtual User Author { get; set; }
    public virtual Post ParentPost { get; set; }
    public virtual ICollection<Post> ChildPosts { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<PostTag> PostTags { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
}

// Tag entity (many-to-many with Post)
public class Tag : BaseEntity
{
    public string Name { get; set; }
    public string Slug { get; set; }

    public virtual ICollection<PostTag> PostTags { get; set; }
}

// Junction table for many-to-many relationship
public class PostTag
{
    public int PostId { get; set; }
    public int TagId { get; set; }

    public virtual Post Post { get; set; }
    public virtual Tag Tag { get; set; }
}

// Comment entity
public class Comment : BaseEntity
{
    public string Content { get; set; }
    public int PostId { get; set; }
    public string UserId { get; set; }
    public int? ParentCommentId { get; set; }

    public virtual Post Post { get; set; }
    public virtual User User { get; set; }
    public virtual Comment ParentComment { get; set; }
    public virtual ICollection<Comment> Replies { get; set; }
}

// Like entity
public class Like
{
    public int PostId { get; set; }
    public string UserId { get; set; }
    public DateTime LikedAt { get; set; }

    public virtual Post Post { get; set; }
    public virtual User User { get; set; }
}

public class BlogFollower
{
    public int BlogId { get; set; }
    public string UserId { get; set; }
    public DateTime FollowedAt { get; set; }

    public virtual Blog Blog { get; set; }
    public virtual User User { get; set; }
}

public class User
{
    public string Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Bio { get; set; }

    public virtual ICollection<Blog> Blogs { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }
    public virtual ICollection<Like> Likes { get; set; }
    public virtual ICollection<BlogFollower> Following { get; set; }
}

public enum PostStatus
{
    Draft,
    Published,
    Archived
}

// ==================== DB CONTEXT WITH ADVANCED CONFIGURATION ====================

public class BlogDbContext : DbContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly string _tenantId;

    public BlogDbContext(
        DbContextOptions<BlogDbContext> options,
        IHttpContextAccessor httpContextAccessor) : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
        _tenantId = httpContextAccessor.HttpContext?.Items["TenantId"]?.ToString();
    }

    public DbSet<Blog> Blogs { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<PostTag> PostTags { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<BlogFollower> BlogFollowers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ===== GLOBAL QUERY FILTERS (Soft Delete + Multi-tenancy) =====

        modelBuilder.Entity<Blog>().HasQueryFilter(b => !b.IsDeleted);
        modelBuilder.Entity<Post>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Comment>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Tag>().HasQueryFilter(t => !t.IsDeleted);

        // ===== BLOG CONFIGURATION =====

        modelBuilder.Entity<Blog>(entity =>
        {
            entity.ToTable("Blogs");
            entity.HasKey(b => b.Id);

            entity.Property(b => b.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(b => b.Description)
                .HasMaxLength(500);

            // Index for faster queries
            entity.HasIndex(b => b.OwnerId);
            entity.HasIndex(b => b.CreatedAt);

            // Relationships
            entity.HasOne(b => b.Owner)
                .WithMany(u => u.Blogs)
                .HasForeignKey(b => b.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== POST CONFIGURATION =====

        modelBuilder.Entity<Post>(entity =>
        {
            entity.ToTable("Posts");
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(300);

            entity.Property(p => p.Content)
                .IsRequired();

            entity.Property(p => p.Slug)
                .IsRequired()
                .HasMaxLength(300);

            // Unique constraint on Slug per Blog
            entity.HasIndex(p => new { p.BlogId, p.Slug })
                .IsUnique();

            // Index for queries
            entity.HasIndex(p => p.PublishedAt);
            entity.HasIndex(p => p.Status);
            entity.HasIndex(p => new { p.BlogId, p.Status });

            // Self-referencing relationship
            entity.HasOne(p => p.ParentPost)
                .WithMany(p => p.ChildPosts)
                .HasForeignKey(p => p.ParentPostId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationships
            entity.HasOne(p => p.Blog)
                .WithMany(b => b.Posts)
                .HasForeignKey(p => p.BlogId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== TAG CONFIGURATION =====

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("Tags");
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(t => t.Slug)
                .IsRequired()
                .HasMaxLength(50);

            entity.HasIndex(t => t.Slug)
                .IsUnique();
        });

        // ===== MANY-TO-MANY CONFIGURATION =====

        modelBuilder.Entity<PostTag>(entity =>
        {
            entity.ToTable("PostTags");
            entity.HasKey(pt => new { pt.PostId, pt.TagId });

            entity.HasOne(pt => pt.Post)
                .WithMany(p => p.PostTags)
                .HasForeignKey(pt => pt.PostId);

            entity.HasOne(pt => pt.Tag)
                .WithMany(t => t.PostTags)
                .HasForeignKey(pt => pt.TagId);
        });

        // ===== COMMENT CONFIGURATION =====

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.ToTable("Comments");
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Content)
                .IsRequired()
                .HasMaxLength(1000);

            entity.HasIndex(c => c.PostId);
            entity.HasIndex(c => c.UserId);

            // Self-referencing for nested comments
            entity.HasOne(c => c.ParentComment)
                .WithMany(c => c.Replies)
                .HasForeignKey(c => c.ParentCommentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== LIKE CONFIGURATION =====

        modelBuilder.Entity<Like>(entity =>
        {
            entity.ToTable("Likes");
            entity.HasKey(l => new { l.PostId, l.UserId });

            entity.HasIndex(l => l.PostId);
            entity.HasIndex(l => l.UserId);
        });

        // ===== BLOG FOLLOWER CONFIGURATION =====

        modelBuilder.Entity<BlogFollower>(entity =>
        {
            entity.ToTable("BlogFollowers");
            entity.HasKey(bf => new { bf.BlogId, bf.UserId });

            entity.HasIndex(bf => bf.UserId);
        });

        // ===== SEED DATA =====

        modelBuilder.Entity<Tag>().HasData(
            new Tag { Id = 1, Name = "Technology", Slug = "technology", CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new Tag { Id = 2, Name = "Programming", Slug = "programming", CreatedAt = DateTime.UtcNow, CreatedBy = "system" },
            new Tag { Id = 3, Name = "Design", Slug = "design", CreatedAt = DateTime.UtcNow, CreatedBy = "system" }
        );
    }

    // ===== AUTOMATIC AUDIT TRAIL =====

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker.Entries<BaseEntity>();
        var currentUser = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "system";
        var now = DateTime.UtcNow;

        foreach (var entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = now;
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.IsDeleted = false;
                    break;

                case EntityState.Modified:
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUser;
                    break;

                case EntityState.Deleted:
                    // Soft delete
                    entry.State = EntityState.Modified;
                    entry.Entity.IsDeleted = true;
                    entry.Entity.UpdatedAt = now;
                    entry.Entity.UpdatedBy = currentUser;
                    break;
            }
        }
    }
}

// ==================== REPOSITORY IMPLEMENTATION ====================

public class BlogRepository : IBlogRepository
{
    private readonly BlogDbContext _context;
    private readonly ILogger<BlogRepository> _logger;

    public BlogRepository(BlogDbContext context, ILogger<BlogRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    // Example: Eager Loading with includes
    public async Task<Blog> GetBlogWithPostsAsync(int blogId)
    {
        return await _context.Blogs
            .Include(b => b.Owner)
            .Include(b => b.Posts.Where(p => p.Status == PostStatus.Published))
                .ThenInclude(p => p.Author)
            .Include(b => b.Posts)
                .ThenInclude(p => p.Comments.Take(5))
            .Include(b => b.Followers)
            .FirstOrDefaultAsync(b => b.Id == blogId);
    }

    // Example: No-tracking for read-only queries
    public async Task<List<Blog>> GetAllBlogsAsync()
    {
        return await _context.Blogs
            .AsNoTracking()
            .Include(b => b.Owner)
            .OrderByDescending(b => b.CreatedAt)
            .ToListAsync();
    }

    // Example: Projection to avoid loading unnecessary data
    public async Task<List<BlogSummaryDto>> GetBlogSummariesAsync()
    {
        return await _context.Blogs
            .Select(b => new BlogSummaryDto
            {
                Id = b.Id,
                Title = b.Title,
                OwnerName = b.Owner.Username,
                PostCount = b.Posts.Count(p => p.Status == PostStatus.Published),
                FollowerCount = b.Followers.Count
            })
            .ToListAsync();
    }

    // Example: Compiled query for frequently used queries
    private static readonly Func<BlogDbContext, int, Task<Blog>> _getBlogByIdCompiled =
        EF.CompileAsyncQuery((BlogDbContext context, int id) =>
            context.Blogs
                .Include(b => b.Owner)
                .FirstOrDefault(b => b.Id == id));

    public async Task<Blog> GetBlogByIdOptimizedAsync(int id)
    {
        return await _getBlogByIdCompiled(_context, id);
    }

    // Example: Explicit loading
    public async Task<Blog> GetBlogWithExplicitLoadingAsync(int blogId)
    {
        var blog = await _context.Blogs.FindAsync(blogId);

        if (blog == null)
            return null;

        // Load related data on demand
        await _context.Entry(blog)
            .Collection(b => b.Posts)
            .Query()
            .Where(p => p.Status == PostStatus.Published)
            .LoadAsync();

        await _context.Entry(blog)
            .Reference(b => b.Owner)
            .LoadAsync();

        return blog;
    }

    // Example: Split query for multiple includes
    public async Task<Post> GetPostWithAllDataAsync(int postId)
    {
        return await _context.Posts
            .Include(p => p.Blog)
            .Include(p => p.Author)
            .Include(p => p.Comments)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.Tag)
            .Include(p => p.Likes)
            .AsSplitQuery()  // Generates separate SQL queries
            .FirstOrDefaultAsync(p => p.Id == postId);
    }

    // Example: Raw SQL query
    public async Task<List<PostStatsDto>> GetPopularPostsAsync(int count)
    {
        return await _context.Database
            .SqlQuery<PostStatsDto>($@"
                SELECT TOP {count}
                    p.Id,
                    p.Title,
                    COUNT(DISTINCT l.UserId) as LikeCount,
                    COUNT(DISTINCT c.Id) as CommentCount
                FROM Posts p
                LEFT JOIN Likes l ON p.Id = l.PostId
                LEFT JOIN Comments c ON p.Id = c.PostId
                WHERE p.Status = {PostStatus.Published}
                GROUP BY p.Id, p.Title
                ORDER BY LikeCount DESC, CommentCount DESC
            ")
            .ToListAsync();
    }

    // Example: Transaction handling
    public async Task<Post> CreatePostWithTagsAsync(Post post, List<int> tagIds)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // Add post
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            // Add post-tag relationships
            var postTags = tagIds.Select(tagId => new PostTag
            {
                PostId = post.Id,
                TagId = tagId
            });

            _context.PostTags.AddRange(postTags);
            await _context.SaveChangesAsync();

            // Update blog post count (cached denormalized data)
            var blog = await _context.Blogs.FindAsync(post.BlogId);
            // Update stats...

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return post;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Error creating post with tags");
            throw;
        }
    }

    // Example: Handling concurrency conflicts
    public async Task<bool> UpdatePostAsync(Post post)
    {
        _context.Posts.Update(post);

        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var databaseValues = await entry.GetDatabaseValuesAsync();

            if (databaseValues == null)
            {
                // Post was deleted
                _logger.LogWarning("Post {PostId} was deleted", post.Id);
                return false;
            }

            // Reload values
            var databaseEntity = (Post)databaseValues.ToObject();

            // Resolve conflict - client wins strategy
            _logger.LogWarning("Concurrency conflict for Post {PostId}. Overwriting changes.", post.Id);

            // Reset original values to database values
            entry.OriginalValues.SetValues(databaseValues);

            // Try save again
            await _context.SaveChangesAsync();
            return true;
        }
    }

    // Example: Bulk operations using EF Core Plus or raw SQL
    public async Task<int> BulkPublishDraftPostsAsync(int blogId)
    {
        return await _context.Posts
            .Where(p => p.BlogId == blogId && p.Status == PostStatus.Draft)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Status, PostStatus.Published)
                .SetProperty(p => p.PublishedAt, DateTime.UtcNow));
    }

    // Example: Streaming large datasets
    public async IAsyncEnumerable<Post> GetAllPostsStreamAsync()
    {
        await foreach (var post in _context.Posts.AsAsyncEnumerable())
        {
            yield return post;
        }
    }

    // Example: Querying with Include and filtering
    public async Task<List<Post>> SearchPostsAsync(PostSearchCriteria criteria)
    {
        IQueryable<Post> query = _context.Posts
            .Include(p => p.Blog)
            .Include(p => p.Author);

        // Apply filters
        if (!string.IsNullOrEmpty(criteria.SearchTerm))
        {
            query = query.Where(p =>
                p.Title.Contains(criteria.SearchTerm) ||
                p.Content.Contains(criteria.SearchTerm));
        }

        if (criteria.BlogId.HasValue)
        {
            query = query.Where(p => p.BlogId == criteria.BlogId);
        }

        if (criteria.Status.HasValue)
        {
            query = query.Where(p => p.Status == criteria.Status);
        }

        if (criteria.TagIds != null && criteria.TagIds.Any())
        {
            query = query.Where(p => p.PostTags.Any(pt => criteria.TagIds.Contains(pt.TagId)));
        }

        // Pagination
        var skip = (criteria.PageNumber - 1) * criteria.PageSize;
        query = query.Skip(skip).Take(criteria.PageSize);

        // Sorting
        query = criteria.SortBy?.ToLower() switch
        {
            "title" => criteria.SortDescending
                ? query.OrderByDescending(p => p.Title)
                : query.OrderBy(p => p.Title),
            "published" => criteria.SortDescending
                ? query.OrderByDescending(p => p.PublishedAt)
                : query.OrderBy(p => p.PublishedAt),
            _ => query.OrderByDescending(p => p.CreatedAt)
        };

        return await query.ToListAsync();
    }
}

// ==================== SERVICE LAYER ====================

public class BlogService : IBlogService
{
    private readonly IBlogRepository _repository;
    private readonly IDistributedCache _cache;
    private readonly ILogger<BlogService> _logger;

    public BlogService(
        IBlogRepository repository,
        IDistributedCache cache,
        ILogger<BlogService> logger)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Blog> GetBlogAsync(int id)
    {
        // Try cache first
        var cacheKey = $"blog:{id}";
        var cachedBlog = await _cache.GetStringAsync(cacheKey);

        if (cachedBlog != null)
        {
            return JsonSerializer.Deserialize<Blog>(cachedBlog);
        }

        // Get from database
        var blog = await _repository.GetBlogByIdOptimizedAsync(id);

        if (blog != null)
        {
            // Cache for 10 minutes
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(blog),
                options);
        }

        return blog;
    }

    public async Task InvalidateBlogCacheAsync(int blogId)
    {
        await _cache.RemoveAsync($"blog:{blogId}");
    }
}

// ==================== DTOs ====================

public class BlogSummaryDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string OwnerName { get; set; }
    public int PostCount { get; set; }
    public int FollowerCount { get; set; }
}

public class PostStatsDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int LikeCount { get; set; }
    public int CommentCount { get; set; }
}

public class PostSearchCriteria
{
    public string SearchTerm { get; set; }
    public int? BlogId { get; set; }
    public PostStatus? Status { get; set; }
    public List<int> TagIds { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string SortBy { get; set; }
    public bool SortDescending { get; set; }
}

// ==================== INTERFACES ====================

public interface IBlogRepository
{
    Task<Blog> GetBlogWithPostsAsync(int blogId);
    Task<List<Blog>> GetAllBlogsAsync();
    Task<List<BlogSummaryDto>> GetBlogSummariesAsync();
    Task<Blog> GetBlogByIdOptimizedAsync(int id);
    Task<Blog> GetBlogWithExplicitLoadingAsync(int blogId);
    Task<Post> GetPostWithAllDataAsync(int postId);
    Task<List<PostStatsDto>> GetPopularPostsAsync(int count);
    Task<Post> CreatePostWithTagsAsync(Post post, List<int> tagIds);
    Task<bool> UpdatePostAsync(Post post);
    Task<int> BulkPublishDraftPostsAsync(int blogId);
    IAsyncEnumerable<Post> GetAllPostsStreamAsync();
    Task<List<Post>> SearchPostsAsync(PostSearchCriteria criteria);
}

public interface IBlogService
{
    Task<Blog> GetBlogAsync(int id);
    Task InvalidateBlogCacheAsync(int blogId);
}
```

**Key Real-Life Takeaways from EF Core Example:**

1. **Soft Delete**: Use global query filters
2. **Audit Trail**: Automatic tracking of created/updated timestamps and users
3. **Eager vs Lazy vs Explicit Loading**: Know when to use each
4. **Projections**: Load only what you need with Select()
5. **Compiled Queries**: Cache frequently-used queries
6. **Split Queries**: Better performance for multiple includes
7. **Raw SQL**: When LINQ isn't enough
8. **Transactions**: Ensure data consistency
9. **Concurrency**: Handle conflicts with RowVersion
10. **Caching**: Cache frequently accessed data
11. **Streaming**: Process large datasets efficiently
12. **Complex Filtering**: Build dynamic queries

---

## Real-Life Example: Microservices E-Commerce System

Complete microservices architecture with service communication, resilience patterns, and distributed transactions.

### Architecture Overview

```
┌─────────────────────────────────────────────────────────────┐
│                      API Gateway (Ocelot)                    │
│          Rate Limiting, Auth, Routing, Load Balancing        │
└─────────────────────────────────────────────────────────────┘
                              │
          ┌───────────────────┼───────────────────┐
          │                   │                   │
┌─────────▼────────┐ ┌────────▼────────┐ ┌───────▼───────┐
│  Product Service  │ │  Order Service   │ │ Payment Service│
│   (Port 5001)    │ │   (Port 5002)    │ │  (Port 5003)  │
└──────────────────┘ └──────────────────┘ └────────────────┘
         │                    │                    │
         └────────────────────┼────────────────────┘
                              │
                    ┌─────────▼──────────┐
                    │   Message Queue    │
                    │    (RabbitMQ)      │
                    └────────────────────┘
```

### Implementation

```csharp
// ==================== PRODUCT SERVICE ====================

// Product Service - Independent Microservice
[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductRepository _repository;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<ProductsController> _logger;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [HttpPut("{id}/stock")]
    public async Task<IActionResult> UpdateStock(int id, [FromBody] UpdateStockRequest request)
    {
        var product = await _repository.GetByIdAsync(id);

        if (product == null)
            return NotFound();

        if (product.Stock < request.Quantity && request.Operation == StockOperation.Reserve)
            return BadRequest("Insufficient stock");

        // Update stock
        product.Stock += request.Operation == StockOperation.Reserve
            ? -request.Quantity
            : request.Quantity;

        await _repository.UpdateAsync(product);

        // Publish event
        await _publisher.PublishAsync(new ProductStockUpdatedEvent
        {
            ProductId = id,
            NewStock = product.Stock,
            Timestamp = DateTime.UtcNow
        });

        return Ok();
    }
}

// ==================== ORDER SERVICE WITH SAGA PATTERN ====================

// Order Service - Orchestrates distributed transaction
public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMessagePublisher _publisher;
    private readonly ILogger<OrderService> _logger;

    public async Task<Result<Order>> CreateOrderAsync(CreateOrderRequest request)
    {
        // Saga orchestration
        var sagaId = Guid.NewGuid();
        var sagaState = new OrderSagaState { SagaId = sagaId };

        try
        {
            _logger.LogInformation("Starting order creation saga {SagaId}", sagaId);

            // Step 1: Create order in pending state
            var order = new Order
            {
                CustomerId = request.CustomerId,
                Items = request.Items,
                Status = OrderStatus.Pending,
                TotalAmount = request.Items.Sum(i => i.Price * i.Quantity)
            };

            await _orderRepository.AddAsync(order);
            sagaState.OrderId = order.Id;

            _logger.LogInformation("Order {OrderId} created in pending state", order.Id);

            // Step 2: Reserve inventory (call Product Service)
            var reservationResult = await ReserveInventoryAsync(order.Items);
            if (!reservationResult.Success)
            {
                _logger.LogWarning("Inventory reservation failed for order {OrderId}", order.Id);
                await CompensateAsync(sagaState);
                return Result<Order>.Failure("Insufficient inventory");
            }

            sagaState.InventoryReserved = true;
            _logger.LogInformation("Inventory reserved for order {OrderId}", order.Id);

            // Step 3: Process payment (call Payment Service)
            var paymentResult = await ProcessPaymentAsync(order.TotalAmount, request.PaymentDetails);
            if (!paymentResult.Success)
            {
                _logger.LogWarning("Payment failed for order {OrderId}", order.Id);
                await CompensateAsync(sagaState);
                return Result<Order>.Failure("Payment processing failed");
            }

            sagaState.PaymentProcessed = true;
            sagaState.PaymentId = paymentResult.Data;
            _logger.LogInformation("Payment processed for order {OrderId}", order.Id);

            // Step 4: Confirm order
            order.Status = OrderStatus.Confirmed;
            await _orderRepository.UpdateAsync(order);

            _logger.LogInformation("Order {OrderId} confirmed successfully", order.Id);

            // Publish order created event
            await _publisher.PublishAsync(new OrderCreatedEvent
            {
                OrderId = order.Id,
                CustomerId = order.CustomerId,
                TotalAmount = order.TotalAmount,
                Timestamp = DateTime.UtcNow
            });

            return Result<Order>.Success(order);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in order creation saga {SagaId}", sagaId);
            await CompensateAsync(sagaState);
            return Result<Order>.Failure("Order creation failed");
        }
    }

    // Compensating transactions (rollback)
    private async Task CompensateAsync(OrderSagaState state)
    {
        _logger.LogWarning("Compensating saga {SagaId}", state.SagaId);

        // Rollback in reverse order
        if (state.PaymentProcessed)
        {
            await RefundPaymentAsync(state.PaymentId);
            _logger.LogInformation("Payment refunded for saga {SagaId}", state.SagaId);
        }

        if (state.InventoryReserved)
        {
            await ReleaseInventoryAsync(state.OrderId);
            _logger.LogInformation("Inventory released for saga {SagaId}", state.SagaId);
        }

        if (state.OrderId.HasValue)
        {
            var order = await _orderRepository.GetByIdAsync(state.OrderId.Value);
            order.Status = OrderStatus.Cancelled;
            await _orderRepository.UpdateAsync(order);
            _logger.LogInformation("Order {OrderId} cancelled", state.OrderId);
        }
    }

    // HTTP client with Polly resilience policies
    private async Task<ServiceResult> ReserveInventoryAsync(List<OrderItem> items)
    {
        var client = _httpClientFactory.CreateClient("ProductService");

        foreach (var item in items)
        {
            var request = new UpdateStockRequest
            {
                Quantity = item.Quantity,
                Operation = StockOperation.Reserve
            };

            var response = await client.PutAsJsonAsync($"api/products/{item.ProductId}/stock", request);

            if (!response.IsSuccessStatusCode)
            {
                return ServiceResult.Failure("Inventory reservation failed");
            }
        }

        return ServiceResult.Success();
    }

    private async Task<ServiceResult<string>> ProcessPaymentAsync(decimal amount, PaymentDetails details)
    {
        var client = _httpClientFactory.CreateClient("PaymentService");

        var request = new ProcessPaymentRequest
        {
            Amount = amount,
            PaymentDetails = details
        };

        var response = await client.PostAsJsonAsync("api/payments", request);

        if (!response.IsSuccessStatusCode)
        {
            return ServiceResult<string>.Failure("Payment processing failed");
        }

        var result = await response.Content.ReadFromJsonAsync<PaymentResponse>();
        return ServiceResult<string>.Success(result.TransactionId);
    }

    private async Task ReleaseInventoryAsync(int? orderId)
    {
        if (!orderId.HasValue) return;

        var order = await _orderRepository.GetByIdAsync(orderId.Value);
        var client = _httpClientFactory.CreateClient("ProductService");

        foreach (var item in order.Items)
        {
            var request = new UpdateStockRequest
            {
                Quantity = item.Quantity,
                Operation = StockOperation.Release
            };

            await client.PutAsJsonAsync($"api/products/{item.ProductId}/stock", request);
        }
    }

    private async Task RefundPaymentAsync(string paymentId)
    {
        var client = _httpClientFactory.CreateClient("PaymentService");
        await client.PostAsync($"api/payments/{paymentId}/refund", null);
    }
}

// ==================== RESILIENCE PATTERNS WITH POLLY ====================

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddResilientHttpClients(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Retry policy: exponential backoff
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (outcome, timespan, retryCount, context) =>
                {
                    var logger = context.GetLogger();
                    logger?.LogWarning(
                        "Retry {RetryCount} after {Delay}s due to {Error}",
                        retryCount,
                        timespan.TotalSeconds,
                        outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString());
                });

        // Circuit breaker policy
        var circuitBreakerPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(30),
                onBreak: (outcome, duration) =>
                {
                    // Log circuit breaker opened
                },
                onReset: () =>
                {
                    // Log circuit breaker reset
                });

        // Timeout policy
        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(10));

        // Combine policies
        var policyWrap = Policy.WrapAsync(retryPolicy, circuitBreakerPolicy, timeoutPolicy);

        // Configure HTTP clients
        services.AddHttpClient("ProductService", client =>
        {
            client.BaseAddress = new Uri(configuration["Services:ProductService:Url"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(policyWrap);

        services.AddHttpClient("PaymentService", client =>
        {
            client.BaseAddress = new Uri(configuration["Services:PaymentService:Url"]);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddPolicyHandler(policyWrap);

        return services;
    }
}

// ==================== MESSAGE QUEUE INTEGRATION ====================

// RabbitMQ Publisher
public class RabbitMqPublisher : IMessagePublisher
{
    private readonly IConnection _connection;
    private readonly ILogger<RabbitMqPublisher> _logger;

    public RabbitMqPublisher(IConnection connection, ILogger<RabbitMqPublisher> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task PublishAsync<T>(T message) where T : class
    {
        using var channel = _connection.CreateModel();

        var queueName = typeof(T).Name;

        channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = channel.CreateBasicProperties();
        properties.Persistent = true;
        properties.ContentType = "application/json";
        properties.MessageId = Guid.NewGuid().ToString();
        properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());

        channel.BasicPublish(
            exchange: "",
            routingKey: queueName,
            basicProperties: properties,
            body: body);

        _logger.LogInformation("Published message {MessageId} to queue {Queue}", properties.MessageId, queueName);
    }
}

// RabbitMQ Consumer (Background Service)
public class OrderCreatedEventConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<OrderCreatedEventConsumer> _logger;
    private IModel _channel;

    public OrderCreatedEventConsumer(
        IConnection connection,
        IServiceProvider serviceProvider,
        ILogger<OrderCreatedEventConsumer> logger)
    {
        _connection = connection;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = _connection.CreateModel();

        var queueName = nameof(OrderCreatedEvent);

        _channel.QueueDeclare(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(_channel);

        consumer.Received += async (model, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<OrderCreatedEvent>(message);

                _logger.LogInformation("Received OrderCreatedEvent for order {OrderId}", @event.OrderId);

                // Process event (send email, update analytics, etc.)
                using var scope = _serviceProvider.CreateScope();
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
                await emailService.SendOrderConfirmationAsync(@event.OrderId);

                // Acknowledge message
                _channel.BasicAck(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing OrderCreatedEvent");
                // Reject and requeue
                _channel.BasicNack(ea.DeliveryTag, false, true);
            }
        };

        _channel.BasicConsume(queue: queueName, autoAck: false, consumer: consumer);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
        }
    }

    public override void Dispose()
    {
        _channel?.Close();
        base.Dispose();
    }
}

// ==================== DISTRIBUTED TRACING ====================

public static class OpenTelemetryExtensions
{
    public static IServiceCollection AddOpenTelemetryTracing(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOpenTelemetry()
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService(configuration["ServiceName"]))
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                    })
                    .AddHttpClientInstrumentation()
                    .AddSqlClientInstrumentation(options =>
                    {
                        options.SetDbStatementForText = true;
                        options.RecordException = true;
                    })
                    .AddJaegerExporter(options =>
                    {
                        options.AgentHost = configuration["Jaeger:Host"];
                        options.AgentPort = int.Parse(configuration["Jaeger:Port"]);
                    });
            });

        return services;
    }
}

// Usage in controller
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ActivitySource _activitySource;

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        using var activity = _activitySource.StartActivity("CreateOrder", ActivityKind.Server);

        activity?.SetTag("customer.id", request.CustomerId);
        activity?.SetTag("items.count", request.Items.Count);
        activity?.SetTag("total.amount", request.Items.Sum(i => i.Price * i.Quantity));

        try
        {
            var result = await _orderService.CreateOrderAsync(request);

            if (result.Success)
            {
                activity?.SetStatus(ActivityStatusCode.Ok);
                return Ok(result.Data);
            }

            activity?.SetStatus(ActivityStatusCode.Error, result.ErrorMessage);
            return BadRequest(result.ErrorMessage);
        }
        catch (Exception ex)
        {
            activity?.SetStatus(ActivityStatusCode.Error, ex.Message);
            activity?.RecordException(ex);
            throw;
        }
    }
}

// ==================== SUPPORTING CLASSES ====================

public class OrderSagaState
{
    public Guid SagaId { get; set; }
    public int? OrderId { get; set; }
    public bool InventoryReserved { get; set; }
    public bool PaymentProcessed { get; set; }
    public string PaymentId { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; }
}

public class OrderItem
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Cancelled,
    Shipped,
    Delivered
}

public class CreateOrderRequest
{
    public string CustomerId { get; set; }
    public List<OrderItem> Items { get; set; }
    public PaymentDetails PaymentDetails { get; set; }
}

public class PaymentDetails
{
    public string CardNumber { get; set; }
    public string CVV { get; set; }
    public DateTime ExpiryDate { get; set; }
}

public class UpdateStockRequest
{
    public int Quantity { get; set; }
    public StockOperation Operation { get; set; }
}

public enum StockOperation
{
    Reserve,
    Release
}

public class ProcessPaymentRequest
{
    public decimal Amount { get; set; }
    public PaymentDetails PaymentDetails { get; set; }
}

public class PaymentResponse
{
    public string TransactionId { get; set; }
    public bool Success { get; set; }
}

// Events
public class OrderCreatedEvent
{
    public int OrderId { get; set; }
    public string CustomerId { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ProductStockUpdatedEvent
{
    public int ProductId { get; set; }
    public int NewStock { get; set; }
    public DateTime Timestamp { get; set; }
}

// Result types
public class Result<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string ErrorMessage { get; set; }

    public static Result<T> Success(T data) => new() { Success = true, Data = data };
    public static Result<T> Failure(string error) => new() { Success = false, ErrorMessage = error };
}

public class ServiceResult
{
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }

    public static ServiceResult Success() => new() { Success = true };
    public static ServiceResult Failure(string error) => new() { Success = false, ErrorMessage = error };
}

public class ServiceResult<T> : ServiceResult
{
    public T Data { get; set; }

    public new static ServiceResult<T> Success(T data) => new() { Success = true, Data = data };
    public new static ServiceResult<T> Failure(string error) => new() { Success = false, ErrorMessage = error };
}
```

**Key Microservices Takeaways:**

1. **Saga Pattern**: Coordinate distributed transactions with compensating actions
2. **Resilience**: Retry, circuit breaker, timeout policies with Polly
3. **Service Communication**: HTTP for sync, message queue for async
4. **Distributed Tracing**: OpenTelemetry for tracking requests across services
5. **Event-Driven**: Publish events for loose coupling
6. **Compensating Transactions**: Rollback distributed operations
7. **Message Queue**: RabbitMQ for reliable async communication
8. **Service Discovery**: Dynamic service location
9. **API Gateway**: Single entry point with Ocelot
10. **Monitoring**: Comprehensive logging and tracing

This covers production-ready patterns you'll encounter in real enterprise systems!
