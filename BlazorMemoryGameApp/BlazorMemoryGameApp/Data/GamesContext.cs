using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BlazorMemoryGameApp.Shared.Models;

namespace BlazorMemoryGameApp.Data
{
    public class GamesContext : DbContext
    {
        public GamesContext (DbContextOptions<GamesContext> options)
            : base(options)
        {
        }

        public DbSet<BlazorMemoryGameApp.Shared.Models.Games> Games { get; set; } = default!;
    }
}
