using AchillService.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AchillService.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Class> Classes { get; set; }

        public DbSet<ClassCourse> ClassCourses { get; set; }

        public DbSet<ClassSubscriber> ClassSubscribers { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<CourseSubscriber> CourseSubscribers { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<PublicKey> PublicKeys { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base (options)
        {

        }
    }
}
