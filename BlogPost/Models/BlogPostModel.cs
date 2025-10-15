using System;
using System.Collections.Generic;

namespace BlogPost.Models
{
    public class BlogPostModel
    {
        public int Id { get; set; } //Primary key
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Author { get; set; }
        public string? Tags { get; set; }
        public bool IsPublished { get; set; } = false;
        public int Likes { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public string? Category { get; set; }
        public string? FeaturedImageUrl { get; set; }


    }
}