using DeliveryManager.Models;
using DeliveryManager.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryManager.Repositories
{
    public class DeliveryWindowRepo : DbContext, IDeliveryWindowRepo
    {
        private readonly IConfiguration _configuration;

        public DbSet<DeliveryWindow> Windows { get; set; }

        public DeliveryWindowRepo(IConfiguration configuration)
        {
            _configuration = configuration;
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetSection("ConnectionStrings:LocalDb").Value);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<DeliveryWindow>()
                .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (DeliveryWindowType)Enum.Parse(typeof(DeliveryWindowType), v));
        }

        public Guid Add(DeliveryWindow window)
        {
            var result = Windows.Add(window).Entity.Id;
            SaveChanges();

            return result;
        }

        public DeliveryWindow Update(Guid id, DeliveryWindow window)
        {
            var entity = Windows.FirstOrDefault(x => x.Id == id);
            if (entity != null)
            {
                entity.Name = window.Name;
                entity.Description = window.Description;
                entity.AvailableFrom = window.AvailableFrom;
                entity.AvailableTo = window.AvailableTo;
                entity.Price = window.Price;
                entity.Type = window.Type;
                entity.ExpectedDeliveryTimeStart = window.ExpectedDeliveryTimeStart;
                entity.ExpectedDeliveryTimeFinish = window.ExpectedDeliveryTimeFinish;
                entity.AvailabilityTimeLimit = window.AvailabilityTimeLimit;
                entity.AvailableDays = window.AvailableDays;
            }
            SaveChanges();

            return entity;
        }

        public IEnumerable<DeliveryWindow> Get(DateTime currentDate)
        {
            return Windows.Where(x =>
                x.AvailableFrom <= currentDate &&
                x.AvailableTo >= currentDate &&
                x.AvailableDays.Contains(currentDate.DayOfWeek.ToString()));
        }
    }
}
