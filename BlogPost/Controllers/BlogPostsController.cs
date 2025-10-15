using BlogPost.Data;
using BlogPost.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;


namespace BlogPost.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BlogPostsController(AppDbContext context)
        {
            _context = context;
        }
        // GET: api/blogposts
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<BlogPostsController>>> GetBlogPosts()
        //{
        //    var posts = await _context.BlogPosts.ToListAsync();
        //    return Ok(posts);
        //}
        [HttpGet]
        public IActionResult GetPosts(
    string? author,
    string? category,
    bool? isPublished,
    string? tag,
    string? search,
    int page = 1,
    int pageSize = 5)
        {
            var query = _context.BlogPosts.AsQueryable();

            // Filtering
            if (!string.IsNullOrEmpty(author))
                query = query.Where(p => p.Author == author);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(p => p.Category == category);

            if (isPublished.HasValue)
                query = query.Where(p => p.IsPublished == isPublished.Value);

            if (!string.IsNullOrEmpty(tag))
                query = query.Where(p => p.Tags != null && p.Tags.Contains(tag));

            if (!string.IsNullOrEmpty(search))
                query = query.Where(p => p.Title.Contains(search) || p.Content.Contains(search));

            // Pagination
            var totalItems = query.Count();
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return paged result
            var result = new
            {
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };

            return Ok(result);
        }

        // GET: api/blogposts/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<BlogPostModel>> GetBlogPost(int id)
        {
            var post = await _context.BlogPosts.FindAsync(id);

            if (post == null)
            {
                return NotFound(); // returns 404 if not found
            }

            return Ok(post); // returns 200 + the post data
        }
        // POST: api/blogposts
        [HttpPost]
        public async Task<ActionResult<BlogPostModel>> CreateBlogPost(BlogPostModel blogPost)
        {
            // Automatically set the created date
            blogPost.CreatedAt = DateTime.Now;

            // Add the post to the context
            _context.BlogPosts.Add(blogPost);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return 201 Created + the new post
            return CreatedAtAction(nameof(GetBlogPost), new { id = blogPost.Id }, blogPost);
        }

        // PUT: api/blogposts/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBlogPost(int id, BlogPostModel updatedPost)
        {
            // Check if the route id matches the body id
            if (id != updatedPost.Id)
            {
                return BadRequest("ID in URL does not match ID in body.");
            }

            // Find existing post
            var existingPost = await _context.BlogPosts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }

            // Update fields
            existingPost.Title = updatedPost.Title;
            existingPost.Content = updatedPost.Content;
            existingPost.Author = updatedPost.Author;
            existingPost.IsPublished = updatedPost.IsPublished;
            existingPost.Tags = updatedPost.Tags;
            existingPost.Category = updatedPost.Category;
            existingPost.FeaturedImageUrl = updatedPost.FeaturedImageUrl;
            existingPost.Likes = updatedPost.Likes;
            existingPost.CommentsCount = updatedPost.CommentsCount;

            // Save to DB
            await _context.SaveChangesAsync();

            // Return success (no content)
            return NoContent();
        }
        // DELETE: api/blogposts/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBlogPost(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);

            if (blogPost == null)
            {
                return NotFound();
            }

            _context.BlogPosts.Remove(blogPost);
            await _context.SaveChangesAsync();

            return NoContent(); // 204 No Content → successful deletion
        }


    }
}





