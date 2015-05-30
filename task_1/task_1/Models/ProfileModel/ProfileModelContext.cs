using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace task_1.Models.ProfileModel
{
    public class ProfileModelContext : DbContext
    {
        public ProfileModelContext()
            : base("UsersProfiles")
        {
        }

        public DbSet<Profile> Profile { get; set; }
        public DbSet<BusinessCard> BusinessCards { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<BusinessCardToField> BusinessCardsToFields { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            OneToManyLinkProfileToUserData(modelBuilder);
        }

        private static void OneToManyLinkProfileToUserData(DbModelBuilder modelBuilder)
        {
            SetLinkToBusinessCards(modelBuilder);
            SetLinkToFields(modelBuilder);
        }

        private static void SetLinkToBusinessCards(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasMany(x => x.BusinessCards)
                .WithOptional()
                .WillCascadeOnDelete(true);
        }

        private static void SetLinkToFields(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Profile>()
                .HasMany(x => x.Fields)
                .WithOptional()
                .WillCascadeOnDelete(true);
        }

    }
}