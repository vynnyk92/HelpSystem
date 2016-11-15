using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using HelpDesk.Domain.Entities;

namespace HelpDesk.Domain.Concrete
{
    public class HelpdeskContext:DbContext
    {
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Proposal> Proposals { get; set; }
        public DbSet<Lifecycle> Lifecycles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments{ get; set; }
        public DbSet<Activ> Activs { get; set; }
    }
}
