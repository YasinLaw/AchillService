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

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Issue> Issues { get; set; }

        public DbSet<PublicKey> PublicKeys { get; set; }
        
        // Navigation tables

        public DbSet<ApplicationUserClass> ApplicationUsersClasses { get; set; }

        public DbSet<ApplicationUserCourse> ApplicationUserCourses { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ApplicationUserClass>().HasKey(ac => new { ac.ApplicationUserId, ac.ClassId});
            builder.Entity<ApplicationUserCourse>().HasKey(ac => new { ac.ApplicationUserId, ac.CourseId });

            builder.Entity<ApplicationUserClass>()
                .HasOne(ac => ac.ApplicationUser)
                .WithMany(a => a.ApplicationUserClasses)
                .HasForeignKey(ac => ac.ApplicationUserId);

            builder.Entity<ApplicationUserClass>()
                .HasOne(ac => ac.Class)
                .WithMany(c => c.ApplicationUserClasses)
                .HasForeignKey(ac => ac.ClassId);

            builder.Entity<ApplicationUserCourse>()
                .HasOne(ac => ac.ApplicationUser)
                .WithMany(a => a.ApplicationUserCourses)
                .HasForeignKey(ac => ac.ApplicationUserId);

            builder.Entity<ApplicationUserCourse>()
                .HasOne(ac => ac.Course)
                .WithMany(c => c.ApplicationUserCourses)
                .HasForeignKey(ac => ac.CourseId);

            builder.Entity<Comment>()
                .HasOne(c => c.Issue)
                .WithMany(i => i.Comments);

            builder.Entity<Issue>()
                .HasMany(c => c.Comments)
                .WithOne(i => i.Issue);

            base.OnModelCreating(builder);
        }
    }
}
