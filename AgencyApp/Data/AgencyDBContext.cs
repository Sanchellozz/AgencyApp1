using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AgencyApp.Models;

namespace AgencyApp.Data
{
    public class AgencyDBContext : DbContext
    {
        public AgencyDBContext (DbContextOptions<AgencyDBContext> options)
            : base(options)
        {
        }

        public DbSet<AgencyApp.Models.Degree> Degrees { get; set; }

        public DbSet<AgencyApp.Models.License> Licenses { get; set; }

        public DbSet<AgencyApp.Models.Client> Clients { get; set; }

        public DbSet<AgencyApp.Models.Agent> Agents { get; set; }

        public DbSet<AgencyApp.Models.Dictionary> Dictionary { get; set; }

        public DbSet<AgencyApp.Models.Contract> Contract { get; set; }

        public DbSet<AgencyApp.Models.Application> Application { get; set; }
    }
}
