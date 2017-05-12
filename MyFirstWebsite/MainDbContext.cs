using MyFirstWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MyFirstWebsite
{
    public class MainDbContext : DbContext
    {
        public MainDbContext()
            : base("name=DefaultConnection")
        {

        }
        public DbSet<Users> Users{get; set; }
        public DbSet<Lists> Lists { get; set; }
    }
}