﻿using Microsoft.EntityFrameworkCore;
using PjoterParker.Api.Database.Configurations;
using PjoterParker.Api.Database.Entities;

namespace PjoterParker.Api.Database
{
    public class ApiDatabaseContext : DbContext, IApiDatabaseContext
    {
        private readonly ApiDatabaseCredentials _credentials;

        public ApiDatabaseContext(ApiDatabaseCredentials credentials)
        {
            _credentials = credentials;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public DbSet<Location> Location { get; set; }

        public DbSet<Spot> Spot { get; set; }

        //public DbSet<Reservation> Reservation { get; set; }

        public void BeginTransaction()
        {
            Database.BeginTransaction();
        }

        public void RollbackTransaction()
        {
            Database.RollbackTransaction();
        }

        public void SaveChanges()
        {
            base.SaveChanges();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_credentials.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new SpotConfiguration());
        }
    }
}