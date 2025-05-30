using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Meloman.Models;

namespace Meloman.Data
{
    public class MvcTrackContext : DbContext
    {
        public MvcTrackContext (DbContextOptions<MvcTrackContext> options)
            : base(options)
        {
        }

        public DbSet<Meloman.Models.Track> Track { get; set; } = default!;
    }
}
