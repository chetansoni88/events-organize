using System;

namespace Core.Models
{
    public class Token : IToken
    {
        public Guid UserId { get; set; }
        public Guid Id { get; set; }
    }
}
