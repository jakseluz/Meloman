using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Meloman.Models;

namespace Meloman.Data
{
    public class MelomanContext : DbContext
    {
        public MelomanContext (DbContextOptions<MelomanContext> options)
            : base(options)
        {
        }

        public DbSet<Meloman.Models.Artist> Artist { get; set; } = default!;
    }
}
