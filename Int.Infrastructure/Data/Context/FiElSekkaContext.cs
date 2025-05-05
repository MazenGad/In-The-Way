using System;
using System.Collections.Generic;
using Int.Domain.Entities;
using Int.Domain.Entities.Chat;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Int.Infrastructure.Entities;

public class FiElSekkaContext : IdentityDbContext<User>
{
	private readonly IServiceProvider _serviceProvider;

	public FiElSekkaContext(DbContextOptions<FiElSekkaContext> options, IServiceProvider serviceProvider)
        : base(options)
    {
		_serviceProvider = serviceProvider;
	}
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		var userManager = _serviceProvider.GetRequiredService<UserManager<User>>();

		// إعداد القيود الأجنبية لتحديد سلوك الحذف
		modelBuilder.Entity<Chat>()
			.HasOne(c => c.Sender)
			.WithMany() // يمكن أن تتغير هذه العلاقة حسب احتياجك
			.HasForeignKey(c => c.SenderId)
			.OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)

		modelBuilder.Entity<Chat>()
			.HasOne(c => c.Receiver)
			.WithMany() // يمكن أن تتغير هذه العلاقة حسب احتياجك
			.HasForeignKey(c => c.ReceiverId)
			.OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)

        modelBuilder.Entity<SearchHistory>()
        .HasOne(c => c.User)
        .WithMany() // يمكن أن تتغير هذه العلاقة حسب احتياجك
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)

        modelBuilder.Entity<Car>()
        .HasOne(c => c.User)
        .WithMany() // يمكن أن تتغير هذه العلاقة حسب احتياجك
        .HasForeignKey(c => c.UserId)
        .OnDelete(DeleteBehavior.Restrict); // أو .OnDelete(DeleteBehavior.NoAction)

       
    }


	public virtual DbSet<Car> Cars { get; set; }

	public virtual DbSet<Chat> Chats { get; set; }

	public virtual DbSet<CarPhoto> CarPhotos { get; set; }
	public virtual DbSet<SearchHistory> SearchHistories { get; set; }

	public virtual DbSet<PasswordResetToken> PasswordResetTokens { get; set; }



}