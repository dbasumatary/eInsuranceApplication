﻿using E_Insurance_App.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_Insurance_App.Data
{
    public class InsuranceDbContext : DbContext
    {
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<InsurancePlan> InsurancePlans { get; set; }
        public DbSet<Scheme> Schemes { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Premium> Premiums { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<PolicyCancellation> PolicyCancellations { get; set; }


        public InsuranceDbContext(DbContextOptions<InsuranceDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Admin entity configuration
            modelBuilder.Entity<Admin>()
                .ToTable("Admins")
                .HasKey(a => a.AdminID);
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Username)
                .IsUnique();
            modelBuilder.Entity<Admin>()
                .HasIndex(a => a.Email)
                .IsUnique();

            // Employee entity configuration
            modelBuilder.Entity<Employee>()
                .ToTable("Employees")
                .HasKey(a => a.EmployeeID);
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Username)
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            // Customer entity configuration
            //modelBuilder.Entity<Customer>()
            //    .HasIndex(c => c.Email)
            //    .IsUnique();
            //modelBuilder.Entity<Customer>()
            //    .HasOne(c => c.InsuranceAgent)
            //    .WithMany(a => a.Customers)
            //    .HasForeignKey(c => c.AgentID)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(c => c.CustomerID);
                entity.Property(a => a.CustomerID)
                      .ValueGeneratedOnAdd();
                entity.Property(a => a.Username)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(a => a.FullName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.Email)
                      .IsRequired()
                      .HasMaxLength(50);
                entity.Property(a => a.Phone)
                      .IsRequired()
                      .HasMaxLength(15);
                entity.Property(a => a.Password)
                      .IsRequired()
                      .HasMaxLength(255);
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Customer>()
                        .HasOne(c => c.Agent) 
                        .WithMany(a => a.Customers) 
                        .HasForeignKey(c => c.AgentID)
                        .OnDelete(DeleteBehavior.Restrict);

            // InsuranceAgent entity configuration
            //modelBuilder.Entity<InsuranceAgent>()
            //    .HasIndex(a => a.Username)
            //    .IsUnique();
            //modelBuilder.Entity<InsuranceAgent>()
            //    .HasIndex(a => a.Email)
            //    .IsUnique();
            //modelBuilder.Entity<InsuranceAgent>()
            //    .HasMany(a => a.Customers)
            //    .WithOne(c => c.InsuranceAgent)
            //    .HasForeignKey(c => c.AgentID)
            //    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agent>(entity =>
            {
                entity.HasKey(a => a.AgentID);

                entity.Property(a => a.AgentID)
                      .ValueGeneratedOnAdd();

                entity.Property(a => a.Username)
                      .IsRequired()
                      .HasMaxLength(50);

                entity.Property(a => a.Password)
                      .IsRequired()
                      .HasMaxLength(255);

                entity.Property(a => a.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.FullName)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(a => a.CommissionRate)
                      .IsRequired()
                      .HasColumnType("decimal(18,2)");

                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });


            modelBuilder.Entity<Agent>()
                .HasMany(a => a.Customers)
                .WithOne(c => c.Agent)
                .HasForeignKey(c => c.AgentID)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<InsurancePlan>(entity =>
            {
                entity.HasKey(a => a.PlanID);
                entity.Property(a => a.PlanName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.PlanDetails)
                      .IsRequired();
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Scheme>(entity =>
            {
                entity.HasKey(entity => entity.SchemeID);
                entity.Property(a => a.SchemeName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.SchemeDetails)
                      .IsRequired();
                entity.Property(a => a.PlanID)
                      .IsRequired();
                entity.Property(e => e.SchemeFactor)
                    .HasColumnType("DECIMAL(10, 2)");
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Scheme>()
                        .HasOne(s => s.Plan)
                        .WithMany(p => p.Schemes)
                        .HasForeignKey(s => s.PlanID)
                        .OnDelete(DeleteBehavior.Restrict);



            // InsurancePlan entity configuration
            //modelBuilder.Entity<InsurancePlan>()
            //    .HasMany(p => p.Schemes)
            //    .WithOne(s => s.InsurancePlan)
            //    .HasForeignKey(s => s.PlanID)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Scheme>()
                        .HasOne(s => s.Plan)
                        .WithMany(p => p.Schemes)
                        .HasForeignKey(s => s.PlanID)
                        .OnDelete(DeleteBehavior.Restrict);

            //// Scheme entity configuration
            //modelBuilder.Entity<Scheme>()
            //    .HasMany(s => s.Policies)
            //    .WithOne(p => p.Scheme)
            //    .HasForeignKey(p => p.SchemeID)
            //    .OnDelete(DeleteBehavior.Cascade);

            // Policy entity configuration
            //modelBuilder.Entity<Policy>()
            //    .HasOne(p => p.Customer)
            //    .WithMany(c => c.Policies)
            //    .HasForeignKey(p => p.CustomerID)
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Policy>()
            //    .HasOne(p => p.Scheme)
            //    .WithMany(s => s.Policies)
            //    .HasForeignKey(p => p.SchemeID)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Policy>(entity =>
            {
                entity.HasKey(entity => entity.PolicyID);
                entity.Property(a => a.CustomerID)
                      .IsRequired();
                entity.Property(a => a.SchemeID)
                      .IsRequired();
                entity.Property(a => a.PolicyDetails)
                      .IsRequired();
                entity.Property(a => a.DateIssued)
                      .IsRequired();
                entity.Property(a => a.MaturityPeriod)
                      .IsRequired();
                entity.Property(a => a.PolicyLapseDate)
                      .IsRequired();
                entity.Property(a => a.Status)
                      .IsRequired()
                      .HasDefaultValue("Pending");
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Policy>()
                        .HasOne(p => p.Customer)
                        .WithMany(c => c.Policies)
                        .HasForeignKey(p => p.CustomerID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Policy>()
                        .HasOne(p => p.Scheme)
                        .WithMany(s => s.Policies)
                        .HasForeignKey(p => p.SchemeID)
                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Premium>(entity =>
            {
                entity.HasKey(entity => entity.PremiumID);
                entity.Property(a => a.CustomerID)
                      .IsRequired();
                entity.Property(a => a.CustomerID)
                      .IsRequired();
                entity.Property(a => a.PolicyID)
                      .IsRequired();
                entity.Property(a => a.SchemeID)
                      .IsRequired();
                entity.Property(a => a.BaseRate)
                      .IsRequired()
                      .HasColumnType("DECIMAL(10, 2)");
                entity.Property(a => a.Age)
                      .IsRequired();
                entity.Property(a => a.CalculatedPremium)
                      .IsRequired()
                      .HasColumnType("DECIMAL(10, 2)");
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Premium>()
                        .HasOne(p => p.Customer)
                        .WithMany(c => c.Premiums)
                        .HasForeignKey(p => p.CustomerID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Premium>()
                        .HasOne(p => p.Policy)
                        .WithMany(pol => pol.Premiums)
                        .HasForeignKey(p => p.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Premium>()
                        .HasOne(p => p.Scheme)
                        .WithMany(s => s.Premiums)
                        .HasForeignKey(p => p.SchemeID)
                        .OnDelete(DeleteBehavior.Restrict);



            // Payment entity configuration
            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Customer)
            //    .WithMany(c => c.Payments)
            //    .HasForeignKey(p => p.CustomerID)
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Payment>()
            //    .HasOne(p => p.Policy)
            //    .WithMany(p => p.Payments)
            //    .HasForeignKey(p => p.PolicyID)
            //    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(entity => entity.PaymentID);
                entity.Property(a => a.CustomerID)
                      .IsRequired();
                entity.Property(a => a.PolicyID)
                      .IsRequired();
                entity.Property(a => a.PremiumID)
                      .IsRequired();
                entity.Property(a => a.Amount)
                      .IsRequired()
                      .HasColumnType("DECIMAL(10, 2)");
                entity.Property(a => a.PaymentDate)
                      .IsRequired();
                entity.Property(a => a.PaymentType)
                      .IsRequired();
                entity.Property(a => a.Status)
                      .IsRequired()
                      .HasDefaultValue("Pending");
            });

            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Customer)
                        .WithMany()
                        .HasForeignKey(p => p.CustomerID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Policy)
                        .WithMany()
                        .HasForeignKey(p => p.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                        .HasOne(p => p.Premium)
                        .WithMany()
                        .HasForeignKey(p => p.PremiumID)
                        .OnDelete(DeleteBehavior.Restrict);

            // Commission entity configuration
            //modelBuilder.Entity<Commission>()
            //    .HasOne(c => c.InsuranceAgent)
            //    .WithMany(a => a.Commissions)
            //    .HasForeignKey(c => c.AgentID)
            //    .OnDelete(DeleteBehavior.Cascade);
            //modelBuilder.Entity<Commission>()
            //    .HasOne(c => c.Policy)
            //    .WithMany(p => p.Commissions)
            //    .HasForeignKey(c => c.PolicyID)
            //    .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Commission>().HasKey(c => c.CommissionID);
            modelBuilder.Entity<Commission>(entity =>
            {
                entity.HasKey(entity => entity.CommissionID);
                entity.Property(a => a.AgentID)
                      .IsRequired();
                entity.Property(a => a.AgentName)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(a => a.PolicyID)
                      .IsRequired();
                entity.Property(a => a.PremiumID)
                      .IsRequired();
                entity.Property(a => a.CommissionAmount)
                      .IsRequired()
                      .HasColumnType("DECIMAL(10, 2)");
                entity.Property(a => a.CreatedAt)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Commission>()
                        .HasOne(c => c.Agent)
                        .WithMany()
                        .HasForeignKey(c => c.AgentID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commission>()
                        .HasOne(c => c.Policy)
                        .WithMany()
                        .HasForeignKey(c => c.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Commission>()
                        .HasOne(c => c.Premium)
                        .WithMany()
                        .HasForeignKey(c => c.PremiumID)
                        .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<PolicyCancellation>(entity =>
            {
                entity.HasKey(entity => entity.CancellationID);
                entity.Property(a => a.PolicyID)
                      .IsRequired();
                entity.Property(a => a.Reason)
                      .IsRequired();
                entity.Property(a => a.CancellationDate)
                      .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(a => a.CancelledBy)
                      .IsRequired();
            });
                
            modelBuilder.Entity<PolicyCancellation>()
                        .HasOne(pc => pc.Policy)
                        .WithMany(c => c.Cancellations)
                        .HasForeignKey(pc => pc.PolicyID)
                        .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
