using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.dtos.Comment
{
    public class UpdateCommentRequestDto
    {
        [Required]
        [MinLength(5, ErrorMessage="Title must be at least 5 characters long.")]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(10, ErrorMessage="Content must be at least 10 characters long.")]
        public string Content { get; set; } = string.Empty;
    }
}