using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

#nullable disable

namespace SuccessRecruitment.Models
{
    public partial class RecruitmentDB : DbContext
    {
        private static readonly IConfiguration _configuration;

        static RecruitmentDB()
        {
            // static constructor is used to initialize any static data, or to perform a particular action that needs to be performed only once. It is called automatically before the first instance is created or any static members are referenced
            _configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
           .Build();
        }

        public RecruitmentDB()
        {
        }

        public RecruitmentDB(DbContextOptions<RecruitmentDB> options)
            : base(options)
        {
        }

        public virtual DbSet<TblJob> TblJobs { get; set; }
        public virtual DbSet<TblLogin> TblLogins { get; set; }
        public virtual DbSet<TblPage> TblPages { get; set; }
        public virtual DbSet<TblRole> TblRoles { get; set; }
        public virtual DbSet<TblRolePage> TblRolePages { get; set; }
        public virtual DbSet<TblUserPage> TblUserPages { get; set; }
        public virtual DbSet<TblUserRole> TblUserRoles { get; set; }
        public virtual DbSet<Tbluser> Tblusers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<TblJob>(entity =>
            {
                entity.HasKey(e => e.JobId)
                    .HasName("PK__tblJobs__164AA1A825E0AA53");

                entity.ToTable("tblJobs");

                entity.Property(e => e.JobId).HasColumnName("jobId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.EmployerId).HasColumnName("employerId");

                entity.Property(e => e.Field)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("field");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.JobDescription)
                    .HasMaxLength(2000)
                    .IsUnicode(false)
                    .HasColumnName("jobDescription");

                entity.Property(e => e.JobLocation)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("jobLocation");

                entity.Property(e => e.JobTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("jobTitle");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.HasOne(d => d.Employer)
                    .WithMany(p => p.TblJobs)
                    .HasForeignKey(d => d.EmployerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblJobs__postedB__5070F446");
            });

            modelBuilder.Entity<TblLogin>(entity =>
            {
                entity.HasKey(e => e.LoginId)
                    .HasName("PK__tblLogin__1F5EF4CFD270EF20");

                entity.ToTable("tblLogin");

                entity.HasIndex(e => e.UserId, "UQ__tblLogin__CB9A1CFEFFD1F4B4")
                    .IsUnique();

                entity.Property(e => e.LoginId).HasColumnName("loginId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash");

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasColumnName("passwordSalt");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.TblLogin)
                    .HasForeignKey<TblLogin>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblLogin__userId__1F98B2C1");
            });

            modelBuilder.Entity<TblPage>(entity =>
            {
                entity.HasKey(e => e.PageId)
                    .HasName("PK__tblPages__54B1FF7466F00F0F");

                entity.ToTable("tblPages");

                entity.Property(e => e.PageId).HasColumnName("pageId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsAddEditPage).HasColumnName("isAddEditPage");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.IsExternal).HasColumnName("isExternal");

                entity.Property(e => e.IsTab).HasColumnName("isTab");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.PageName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("pageName");

                entity.Property(e => e.ParentPageId).HasColumnName("parentPageId");

                entity.HasOne(d => d.ParentPage)
                    .WithMany(p => p.InverseParentPage)
                    .HasForeignKey(d => d.ParentPageId)
                    .HasConstraintName("FK__tblPages__parent__2739D489");
            });

            modelBuilder.Entity<TblRole>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PK__tblRoles__CD98462A446C2EF8");

                entity.ToTable("tblRoles");

                entity.HasIndex(e => e.RoleName, "UQ__tblRoles__B1947861DE31CFE7")
                    .IsUnique();

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("roleName");
            });

            modelBuilder.Entity<TblRolePage>(entity =>
            {
                entity.HasKey(e => e.RolePageId)
                    .HasName("PK__tblRoleP__631D09D7ABD0EE54");

                entity.ToTable("tblRolePages");

                entity.HasIndex(e => new { e.RoleId, e.PageId }, "UC_RolePge")
                    .IsUnique();

                entity.Property(e => e.RolePageId).HasColumnName("rolePageId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.PageId).HasColumnName("pageId");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.HasOne(d => d.TblPage)
                    .WithMany(p => p.TblRolePages)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRolePa__pageI__31B762FC");

                entity.HasOne(d => d.TblRole)
                    .WithMany(p => p.TblRolePages)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblRolePa__roleI__30C33EC3");
            });

            modelBuilder.Entity<TblUserPage>(entity =>
            {
                entity.HasKey(e => e.UserPageId)
                    .HasName("PK__tblUserP__8703E2B9F8D77A0E");

                entity.ToTable("tblUserPages");

                entity.Property(e => e.UserPageId).HasColumnName("userPageId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.PageId).HasColumnName("pageId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.TblPage)
                    .WithMany(p => p.TblUserPages)
                    .HasForeignKey(d => d.PageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblUserPa__pageI__2BFE89A6");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserPages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblUserPa__userI__2B0A656D");
            });

            modelBuilder.Entity<TblUserRole>(entity =>
            {
                entity.HasKey(e => e.UserRoleId)
                    .HasName("PK__tblUserR__CD3149CC7E5073B9");

                entity.ToTable("tblUserRoles");

                entity.HasIndex(e => new { e.UserId, e.RoleId }, "UQ__tblUserR__7743989C0D9CF152")
                    .IsUnique();

                entity.Property(e => e.UserRoleId).HasColumnName("userRoleId");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.RoleId).HasColumnName("roleId");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.TblRole)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblUserRo__roleI__33D4B598");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.TblUserRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__tblUserRo__userI__32E0915F");
            });

            modelBuilder.Entity<Tbluser>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__tblusers__CB9A1CFF144C9839");

                entity.ToTable("tblusers");

                entity.HasIndex(e => e.Email, "UQ__tblusers__AB6E616451456D1B")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CreatedBy).HasColumnName("createdBy");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("createdDate")
                    .HasDefaultValueSql("(getutcdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsArchived).HasColumnName("isArchived");

                entity.Property(e => e.ModifiedBy).HasColumnName("modifiedBy");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.Phone)
                    .HasColumnType("decimal(10, 0)")
                    .HasColumnName("phone");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("userName");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
