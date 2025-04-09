using System;
using System.Collections.Generic;
using Int.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Int.Infrastructure.Entities;

public class FiElSekkaContext : IdentityDbContext<User>
{

    public FiElSekkaContext(DbContextOptions<FiElSekkaContext> options)
        : base(options)
    {
    }

    //public virtual DbSet<Admin> Admins { get; set; }

    //public virtual DbSet<AdminMnageCar> AdminMnageCars { get; set; }

    //public virtual DbSet<AdminPhone> AdminPhones { get; set; }

    //public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarPhoto> CarPhotos { get; set; }

    //public virtual DbSet<Color> Colors { get; set; }

    //public virtual DbSet<Guidance> Guidances { get; set; }

    //public virtual DbSet<Message> Messages { get; set; }

    //public virtual DbSet<Model> Models { get; set; }

    //public virtual DbSet<SearchCar> SearchCars { get; set; }

    //public virtual DbSet<SearchHistory> SearchHistories { get; set; }

    //public virtual DbSet<UserMessageSendReceive> UserMessageSendReceives { get; set; }

    //public virtual DbSet<UserSearch> UserSearches { get; set; }



    //    protected override void OnModelCreating(ModelBuilder modelBuilder)
    //    {

    //        OnModelCreatingPartial(modelBuilder);
    //    }
    //    modelBuilder.Entity<Admin>(entity =>
    //        {
    //            entity.HasKey(e => e.ASsn).HasName("PK__Admin__C37734FE4EEB0C72");

    //    entity.ToTable("Admin");

    //            entity.Property(e => e.ASsn)
    //                .ValueGeneratedNever()
    //                .HasColumnName("A_SSN");
    //    entity.Property(e => e.AName)
    //                .HasMaxLength(100)
    //                .HasColumnName("A_Name");
    //    entity.Property(e => e.Email)
    //                .HasMaxLength(100)
    //                .HasColumnName("email");
    //    entity.Property(e => e.Password).HasColumnName("password");
    //});

    //        modelBuilder.Entity<AdminMnageCar>(entity =>
    //        {
    //            entity.HasKey(e => e.CId).HasName("PK__Admin_Mn__A9FDEC12D5B1E801");

    //entity.ToTable("Admin_Mnage_car");

    //            entity.Property(e => e.CId)
    //                .ValueGeneratedNever()
    //                .HasColumnName("C_ID");
    //entity.Property(e => e.ASsn).HasColumnName("A_SSN");

    //entity.HasOne(d => d.ASsnNavigation).WithMany(p => p.AdminMnageCars)
    //                .HasForeignKey(d => d.ASsn)
    //                .HasConstraintName("FK__Admin_Mna__A_SSN__693CA210");

    //entity.HasOne(d => d.CIdNavigation).WithOne(p => p.AdminMnageCar)
    //                .HasForeignKey<AdminMnageCar>(d => d.CId)
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__Admin_Mnag__C_ID__6A30C649");
    //        });

    //modelBuilder.Entity<AdminPhone>(entity =>
    //{
    //    entity.HasKey(e => new { e.ASsn, e.PhoneNumber }).HasName("PK__Admin_Ph__820D013416215AC3");

    //    entity.ToTable("Admin_Phone");

    //    entity.Property(e => e.ASsn).HasColumnName("A_SSN");
    //    entity.Property(e => e.PhoneNumber)
    //        .HasMaxLength(15)
    //        .HasColumnName("Phone_Number");

    //    entity.HasOne(d => d.ASsnNavigation).WithMany(p => p.AdminPhones)
    //        .HasForeignKey(d => d.ASsn)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__Admin_Pho__A_SSN__3E52440B");
    //});

    //modelBuilder.Entity<Brand>(entity =>
    //{
    //    entity.HasKey(e => e.BCode).HasName("PK__brand__56B8275311181CD8");

    //    entity.ToTable("brand");

    //    entity.Property(e => e.BCode).HasColumnName("b_Code");
    //    entity.Property(e => e.BName)
    //        .HasMaxLength(100)
    //        .HasColumnName("b_Name");
    //});

    //modelBuilder.Entity<Car>(entity =>
    //{
    //    entity.HasKey(e => e.CId).HasName("PK__Car__A9FDEC12A64A3DDA");

    //    entity.ToTable("Car");

    //    entity.Property(e => e.CId).HasColumnName("C_ID");
    //    entity.Property(e => e.BCode).HasColumnName("b_Code");
    //    entity.Property(e => e.CCode).HasColumnName("c_Code");
    //    entity.Property(e => e.CPlateNumber).HasColumnName("c_plate_number");
    //    entity.Property(e => e.Description)
    //        .HasMaxLength(255)
    //        .HasColumnName("description");
    //    entity.Property(e => e.Location)
    //        .HasMaxLength(255)
    //        .HasColumnName("location");

    //    entity.HasOne(d => d.BCodeNavigation).WithMany(p => p.Cars)
    //        .HasForeignKey(d => d.BCode)
    //        .HasConstraintName("FK__Car__b_Code__45F365D3");

    //    entity.HasOne(d => d.CCodeNavigation).WithMany(p => p.Cars)
    //        .HasForeignKey(d => d.CCode)
    //        .HasConstraintName("FK__Car__c_Code__44FF419A");
    //});

    //modelBuilder.Entity<CarPhoto>(entity =>
    //{
    //    entity.HasKey(e => e.PhotoId).HasName("PK__Car_Phot__7A07F6C29F750EED");

    //    entity.ToTable("Car_Photo");

    //    entity.Property(e => e.PhotoId).HasColumnName("Photo_ID");
    //    entity.Property(e => e.CId).HasColumnName("C_ID");
    //    entity.Property(e => e.Photo).HasColumnName("photo");

    //    entity.HasOne(d => d.CIdNavigation).WithMany(p => p.CarPhotos)
    //        .HasForeignKey(d => d.CId)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__Car_Photo__C_ID__48CFD27E");
    //});

    //modelBuilder.Entity<Color>(entity =>
    //{
    //    entity.HasKey(e => e.CCode).HasName("PK__color__0B18F0EEC14DE22B");

    //    entity.ToTable("color");

    //    entity.Property(e => e.CCode).HasColumnName("c_Code");
    //    entity.Property(e => e.CName)
    //        .HasMaxLength(50)
    //        .HasColumnName("c_Name");
    //});

    //modelBuilder.Entity<Guidance>(entity =>
    //{
    //    entity.HasKey(e => e.GId).HasName("PK__Guidance__153A8ED2B6A0AA66");

    //    entity.ToTable("Guidance");

    //    entity.Property(e => e.GId).HasColumnName("G_ID");
    //    entity.Property(e => e.ASsn).HasColumnName("A_SSN");
    //    entity.Property(e => e.GContent).HasColumnName("G_content");

    //    entity.HasOne(d => d.ASsnNavigation).WithMany(p => p.Guidances)
    //        .HasForeignKey(d => d.ASsn)
    //        .HasConstraintName("FK__Guidance__A_SSN__5535A963");
    //});

    //modelBuilder.Entity<Message>(entity =>
    //{
    //    entity.HasKey(e => e.MId).HasName("PK__Message__18B1A317860AF466");

    //    entity.ToTable("Message");

    //    entity.Property(e => e.MId).HasColumnName("M_ID");
    //    entity.Property(e => e.Content).HasColumnName("content");
    //    entity.Property(e => e.DateTime).HasColumnType("datetime");
    //});

    //modelBuilder.Entity<Model>(entity =>
    //{
    //    entity.HasKey(e => e.MCode).HasName("PK__model__2B3C4A37A2C28094");

    //    entity.ToTable("model");

    //    entity.Property(e => e.MCode).HasColumnName("m_Code");
    //    entity.Property(e => e.BCode).HasColumnName("b_Code");
    //    entity.Property(e => e.MName)
    //        .HasMaxLength(100)
    //        .HasColumnName("m_Name");

    //    entity.HasOne(d => d.BCodeNavigation).WithMany(p => p.Models)
    //        .HasForeignKey(d => d.BCode)
    //        .HasConstraintName("FK__model__b_Code__4BAC3F29");
    //});

    //modelBuilder.Entity<SearchCar>(entity =>
    //{
    //    entity.HasKey(e => e.CId).HasName("PK__search_c__A9FDEC12D7691AB4");

    //    entity.ToTable("search_car");

    //    entity.Property(e => e.CId)
    //        .ValueGeneratedNever()
    //        .HasColumnName("C_ID");
    //    entity.Property(e => e.SId).HasColumnName("S_ID");

    //    entity.HasOne(d => d.CIdNavigation).WithOne(p => p.SearchCar)
    //        .HasForeignKey<SearchCar>(d => d.CId)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__search_car__C_ID__628FA481");

    //    entity.HasOne(d => d.SIdNavigation).WithMany(p => p.SearchCars)
    //        .HasForeignKey(d => d.SId)
    //        .HasConstraintName("FK__search_car__S_ID__619B8048");
    //});

    //modelBuilder.Entity<SearchHistory>(entity =>
    //{
    //    entity.HasKey(e => e.SId).HasName("PK__search_h__A3DFF16D86D404A9");

    //    entity.ToTable("search_history");

    //    entity.Property(e => e.SId).HasColumnName("S_ID");
    //    entity.Property(e => e.DateTime).HasColumnType("datetime");
    //    entity.Property(e => e.Notification)
    //        .HasMaxLength(255)
    //        .HasColumnName("notification");
    //});

    //modelBuilder.Entity<UserMessageSendReceive>(entity =>
    //{
    //    entity.HasKey(e => new { e.USsn, e.MId, e.ASsn }).HasName("PK__User_Mes__A44E908AC72371DA");

    //    entity.ToTable("User_Message_send_receive");

    //    entity.Property(e => e.USsn).HasColumnName("U_SSN");
    //    entity.Property(e => e.MId).HasColumnName("M_ID");
    //    entity.Property(e => e.ASsn).HasColumnName("A_SSN");

    //    entity.HasOne(d => d.ASsnNavigation).WithMany(p => p.UserMessageSendReceives)
    //        .HasForeignKey(d => d.ASsn)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__User_Mess__A_SSN__52593CB8");

    //    entity.HasOne(d => d.MIdNavigation).WithMany(p => p.UserMessageSendReceives)
    //        .HasForeignKey(d => d.MId)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__User_Messa__M_ID__5165187F");

    //    entity.HasOne(d => d.USsnNavigation).WithMany(p => p.UserMessageSendReceives)
    //        .HasForeignKey(d => d.USsn)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__User_Mess__U_SSN__5070F446");
    //});

    //modelBuilder.Entity<UserPhone>(entity =>
    //{
    //    entity.HasKey(e => new { e.USsn, e.PhoneNumber }).HasName("PK__User_Pho__7B7CC84537928C8E");

    //    entity.ToTable("User_Phone");

    //    entity.Property(e => e.USsn).HasColumnName("U_SSN");
    //    entity.Property(e => e.PhoneNumber)
    //        .HasMaxLength(15)
    //        .HasColumnName("Phone_Number");

    //    entity.HasOne(d => d.USsnNavigation).WithMany(p => p.UserPhones)
    //        .HasForeignKey(d => d.USsn)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__User_Phon__U_SSN__3B75D760");
    //});

    //modelBuilder.Entity<UserSearch>(entity =>
    //{
    //    entity.HasKey(e => e.USsn).HasName("PK__User_sea__3A06FD8FEAED6457");

    //    entity.ToTable("User_search");

    //    entity.Property(e => e.USsn)
    //        .ValueGeneratedNever()
    //        .HasColumnName("U_SSN");
    //    entity.Property(e => e.SId).HasColumnName("S_ID");

    //    entity.HasOne(d => d.SIdNavigation).WithMany(p => p.UserSearches)
    //        .HasForeignKey(d => d.SId)
    //        .HasConstraintName("FK__User_searc__S_ID__66603565");

    //    entity.HasOne(d => d.USsnNavigation).WithOne(p => p.UserSearch)
    //        .HasForeignKey<UserSearch>(d => d.USsn)
    //        .OnDelete(DeleteBehavior.ClientSetNull)
    //        .HasConstraintName("FK__User_sear__U_SSN__656C112C");
    //});

    //modelBuilder.Entity<User>(entity =>
    //{
    //    entity.HasKey(e => e.USsn).HasName("PK__USERR__3A06FD8FA209E569");

    //    entity.ToTable("USERR");

    //    entity.Property(e => e.USsn)
    //        .ValueGeneratedNever()
    //        .HasColumnName("U_SSN");
    //    entity.Property(e => e.Email)
    //        .HasMaxLength(100)
    //        .HasColumnName("email");
    //    entity.Property(e => e.Password).HasColumnName("password");
    //    entity.Property(e => e.UName)
    //        .HasMaxLength(100)
    //        .HasColumnName("U_Name");

    //    entity.HasMany(d => d.ASsns).WithMany(p => p.USsns)
    //        .UsingEntity<Dictionary<string, object>>(
    //            "AdminManageUser",
    //            r => r.HasOne<Admin>().WithMany()
    //                .HasForeignKey("ASsn")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__Admin_Man__A_SSN__6D0D32F4"),
    //            l => l.HasOne<User>().WithMany()
    //                .HasForeignKey("USsn")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__Admin_Man__U_SSN__6E01572D"),
    //            j =>
    //            {
    //                j.HasKey("USsn", "ASsn").HasName("PK__Admin_Ma__C6318EC0C08A3EAE");
    //                j.ToTable("Admin_Manage_USER");
    //                j.IndexerProperty<int>("USsn").HasColumnName("U_SSN");
    //                j.IndexerProperty<int>("ASsn").HasColumnName("A_SSN");
    //            });

    //    entity.HasMany(d => d.CIds).WithMany(p => p.USsns)
    //        .UsingEntity<Dictionary<string, object>>(
    //            "UserCarView",
    //            r => r.HasOne<Car>().WithMany()
    //                .HasForeignKey("CId")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__User_Car_v__C_ID__5CD6CB2B"),
    //            l => l.HasOne<User>().WithMany()
    //                .HasForeignKey("USsn")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__User_Car___U_SSN__5BE2A6F2"),
    //            j =>
    //            {
    //                j.HasKey("USsn", "CId").HasName("PK__User_Car__0099234E2C0F9B90");
    //                j.ToTable("User_Car_view");
    //                j.IndexerProperty<int>("USsn").HasColumnName("U_SSN");
    //                j.IndexerProperty<int>("CId").HasColumnName("C_ID");
    //            });

    //    entity.HasMany(d => d.GIds).WithMany(p => p.USsns)
    //        .UsingEntity<Dictionary<string, object>>(
    //            "UserGuidanceView",
    //            r => r.HasOne<Guidance>().WithMany()
    //                .HasForeignKey("GId")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__User_Guida__G_ID__59063A47"),
    //            l => l.HasOne<User>().WithMany()
    //                .HasForeignKey("USsn")
    //                .OnDelete(DeleteBehavior.ClientSetNull)
    //                .HasConstraintName("FK__User_Guid__U_SSN__5812160E"),
    //            j =>
    //            {
    //                j.HasKey("USsn", "GId").HasName("PK__User_Gui__0B5555625D04C383");
    //                j.ToTable("User_Guidance_view");
    //                j.IndexerProperty<int>("USsn").HasColumnName("U_SSN");
    //                j.IndexerProperty<int>("GId").HasColumnName("G_ID");
    //            });
    //});


    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    //}
}