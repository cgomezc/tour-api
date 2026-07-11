using System;
using System.Collections.Generic;
using CJTour.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CJTour.Infrastructure.Persistence.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdditionalInfo> AdditionalInfos { get; set; }

    public virtual DbSet<AdditionalInfoTranslation> AdditionalInfoTranslations { get; set; }

    public virtual DbSet<AuditLogEntry> AuditLogEntries { get; set; }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<BookingGroup> BookingGroups { get; set; }

    public virtual DbSet<BookingParticipant> BookingParticipants { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<CartItem> CartItems { get; set; }

    public virtual DbSet<Content> Contents { get; set; }

    public virtual DbSet<ContentTranslation> ContentTranslations { get; set; }

    public virtual DbSet<ContentType> ContentTypes { get; set; }

    public virtual DbSet<Destination> Destinations { get; set; }

    public virtual DbSet<DestinationTranslation> DestinationTranslations { get; set; }

    public virtual DbSet<Inclusion> Inclusions { get; set; }

    public virtual DbSet<InclusionTranslation> InclusionTranslations { get; set; }

    public virtual DbSet<InclusionsCategory> InclusionsCategories { get; set; }

    public virtual DbSet<Instance> Instances { get; set; }

    public virtual DbSet<Language> Languages { get; set; }

    public virtual DbSet<LanguageOffered> LanguageOffereds { get; set; }

    public virtual DbSet<Participant> Participants { get; set; }

    public virtual DbSet<PickupLocation> PickupLocations { get; set; }

    public virtual DbSet<Tour> Tours { get; set; }

    public virtual DbSet<TourAdditionalInfo> TourAdditionalInfos { get; set; }

    public virtual DbSet<TourHighlight> TourHighlights { get; set; }

    public virtual DbSet<TourHighlightTranslation> TourHighlightTranslations { get; set; }

    public virtual DbSet<TourInclusion> TourInclusions { get; set; }

    public virtual DbSet<TourPackage> TourPackages { get; set; }

    public virtual DbSet<TourPackageSchedule> TourPackageSchedules { get; set; }

    public virtual DbSet<TourPackageTranslation> TourPackageTranslations { get; set; }

    public virtual DbSet<TourPricing> TourPricings { get; set; }

    public virtual DbSet<TourTranslation> TourTranslations { get; set; }

    public virtual DbSet<Tourcategory> Tourcategories { get; set; }

    public virtual DbSet<Tourdateblock> Tourdateblocks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {



        modelBuilder.Entity<AdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.AdditionalInfoId).HasName("additionalInfo_pkey");

            entity.ToTable("additionalInfo");

            entity.Property(e => e.AdditionalInfoId).HasColumnName("additionalInfoId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.InfoType).HasColumnName("infoType");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<AdditionalInfoTranslation>(entity =>
        {
            entity.HasKey(e => e.TourAdditionalInfoTranslationId).HasName("additionalInfoTranslations_pkey");

            entity.ToTable("additionalInfoTranslations");

            entity.HasIndex(e => new { e.AdditionalInfoId, e.LanguageCode }, "additionalInfoTranslations_additionalInfoId_languageCode_key").IsUnique();

            entity.Property(e => e.TourAdditionalInfoTranslationId).HasColumnName("tourAdditionalInfoTranslationId");
            entity.Property(e => e.AdditionalInfoId).HasColumnName("additionalInfoId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.AdditionalInfo).WithMany(p => p.AdditionalInfoTranslations)
                .HasForeignKey(d => d.AdditionalInfoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("additionalInfoTranslations_additionalInfoId_fkey");
        });



        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("bookings_pkey");

            entity.ToTable("bookings");

            entity.HasIndex(e => e.BookingReference, "bookings_bookingReference_key").IsUnique();

            entity.HasIndex(e => e.GroupId, "bookings_groupId_idx");

            entity.HasIndex(e => e.PackageId, "bookings_packageId_idx");

            entity.HasIndex(e => e.ParticipantId, "bookings_participantId_idx");

            entity.HasIndex(e => e.PickupLocationId, "bookings_pickupLocationId_idx");

            entity.HasIndex(e => e.PricingId, "bookings_pricingId_idx");

            entity.HasIndex(e => e.TourId, "bookings_tourId_idx");

            entity.HasIndex(e => e.UserId, "bookings_userId_idx");

            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.Adults).HasColumnName("adults");
            entity.Property(e => e.BookingDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("bookingDate");
            entity.Property(e => e.BookingReference).HasColumnName("bookingReference");
            entity.Property(e => e.Children)
                .HasDefaultValue(0)
                .HasColumnName("children");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.Infants)
                .HasDefaultValue(0)
                .HasColumnName("infants");
            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.ParticipantId).HasColumnName("participantId");
            entity.Property(e => e.PickupLocationId).HasColumnName("pickupLocationId");
            entity.Property(e => e.PricingId).HasColumnName("pricingId");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.TotalAmount)
                .HasPrecision(12, 2)
                .HasColumnName("totalAmount");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.Group).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.GroupId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("bookings_groupId_fkey");

            entity.HasOne(d => d.Participant).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookings_participantId_fkey");

            entity.HasOne(d => d.PickupLocation).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PickupLocationId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("bookings_pickupLocationId_fkey");

            entity.HasOne(d => d.Pricing).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.PricingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("bookings_pricingId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookings_tourId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookings_userId_fkey");
        });

        modelBuilder.Entity<BookingGroup>(entity =>
        {
            entity.HasKey(e => e.GroupId).HasName("bookingGroup_pkey");

            entity.ToTable("bookingGroup");

            entity.HasIndex(e => e.UserId, "bookingGroup_userId_idx");

            entity.Property(e => e.GroupId).HasColumnName("groupId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.BookingGroups)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookingGroup_userId_fkey");
        });

        modelBuilder.Entity<BookingParticipant>(entity =>
        {
            entity.HasKey(e => e.BookingParticipantId).HasName("bookingParticipants_pkey");

            entity.ToTable("bookingParticipants");

            entity.Property(e => e.BookingParticipantId).HasColumnName("bookingParticipantId");
            entity.Property(e => e.BookingId).HasColumnName("bookingId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.ParticipantId).HasColumnName("participantId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Booking).WithMany(p => p.BookingParticipants)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookingParticipants_bookingId_fkey");

            entity.HasOne(d => d.Participant).WithMany(p => p.BookingParticipants)
                .HasForeignKey(d => d.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("bookingParticipants_participantId_fkey");
        });


        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.CartId).HasName("carts_pkey");

            entity.ToTable("carts");

            entity.HasIndex(e => e.CartUuid, "carts_cartUuid_key").IsUnique();

            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.CartUuid).HasColumnName("cartUuid");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Currency)
                .HasColumnType("character varying")
                .HasColumnName("currency");
            entity.Property(e => e.ExpiresAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("expiresAt");
            entity.Property(e => e.TotalPrice)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("totalPrice");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UserId).HasColumnName("userId");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("carts_userId_fkey");
        });

        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("cartItems_pkey");

            entity.ToTable("cartItems");

            entity.Property(e => e.CartItemId).HasColumnName("cartItemId");
            entity.Property(e => e.Adults).HasColumnName("adults");
            entity.Property(e => e.BookingDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("bookingDate");
            entity.Property(e => e.CartId).HasColumnName("cartId");
            entity.Property(e => e.Children)
                .HasDefaultValue(0)
                .HasColumnName("children");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Infants)
                .HasDefaultValue(0)
                .HasColumnName("infants");
            entity.Property(e => e.ItemTotal)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("itemTotal");
            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.PickupLocationName).HasColumnName("pickupLocationName");
            entity.Property(e => e.PricePerAdult)
                .HasPrecision(12, 2)
                .HasColumnName("pricePerAdult");
            entity.Property(e => e.PricePerChild)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("pricePerChild");
            entity.Property(e => e.PricePerInfant)
                .HasPrecision(12, 2)
                .HasDefaultValueSql("0.00")
                .HasColumnName("pricePerInfant");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Cart).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.CartId)
                .HasConstraintName("cartItems_cartId_fkey");

            entity.HasOne(d => d.Package).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("cartItems_packageId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.CartItems)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("cartItems_tourId_fkey");
        });

        modelBuilder.Entity<Content>(entity =>
        {
            entity.HasKey(e => e.ContentId).HasName("contents_pkey");

            entity.ToTable("contents");

            entity.HasIndex(e => e.ContentTypeId, "contents_contentTypeId_idx");

            entity.HasIndex(e => new { e.ContentTypeId, e.Slug }, "contents_contentTypeId_slug_key").IsUnique();

            entity.Property(e => e.ContentId).HasColumnName("contentId");
            entity.Property(e => e.ContentTypeId).HasColumnName("contentTypeId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Metadata)
                .HasColumnType("jsonb")
                .HasColumnName("metadata");
            entity.Property(e => e.ParentId).HasColumnName("parentId");
            entity.Property(e => e.Position)
                .HasDefaultValue(0)
                .HasColumnName("position");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

            entity.HasOne(d => d.ContentType).WithMany(p => p.Contents)
                .HasForeignKey(d => d.ContentTypeId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("contents_contentTypeId_fkey");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("contents_parentId_fkey");
        });

        modelBuilder.Entity<ContentTranslation>(entity =>
        {
            entity.HasKey(e => e.ContentTranslationId).HasName("contentTranslations_pkey");

            entity.ToTable("contentTranslations");

            entity.HasIndex(e => new { e.ContentId, e.LanguageCode }, "contentTranslations_contentId_languageCode_key").IsUnique();

            entity.Property(e => e.ContentTranslationId).HasColumnName("contentTranslationId");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.ContentId).HasColumnName("contentId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.SeoDescription).HasColumnName("seoDescription");
            entity.Property(e => e.SeoTitle).HasColumnName("seoTitle");
            entity.Property(e => e.Subtitle).HasColumnName("subtitle");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.ContentNavigation).WithMany(p => p.ContentTranslations)
                .HasForeignKey(d => d.ContentId)
                .HasConstraintName("contentTranslations_contentId_fkey");
        });

        modelBuilder.Entity<ContentType>(entity =>
        {
            entity.HasKey(e => e.ContentTypeId).HasName("contentTypes_pkey");

            entity.ToTable("contentTypes");

            entity.HasIndex(e => e.Name, "contentTypes_name_key").IsUnique();

            entity.Property(e => e.ContentTypeId).HasColumnName("contentTypeId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<Destination>(entity =>
        {
            entity.HasKey(e => e.DestinationId).HasName("destinations_pkey");

            entity.ToTable("destinations");

            entity.Property(e => e.DestinationId).HasColumnName("destinationId");
            entity.Property(e => e.Country).HasColumnName("country");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Featured)
                .HasDefaultValue(false)
                .HasColumnName("featured");
            entity.Property(e => e.FeaturedImageUrl)
                .HasColumnType("character varying")
                .HasColumnName("featuredImageUrl");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.Region).HasColumnName("region");
            entity.Property(e => e.Slug).HasColumnName("slug");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<DestinationTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("destinationTranslations_pkey");

            entity.ToTable("destinationTranslations");

            entity.HasIndex(e => new { e.DestinationId, e.LanguageCode }, "destinationTranslations_destinationId_languageCode_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DestinationId).HasColumnName("destinationId");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Destination).WithMany(p => p.DestinationTranslations)
                .HasForeignKey(d => d.DestinationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("destinationTranslations_destinationId_fkey");
        });


        modelBuilder.Entity<Inclusion>(entity =>
        {
            entity.HasKey(e => e.InclusionId).HasName("inclusions_pkey");

            entity.ToTable("inclusions");

            entity.Property(e => e.InclusionId).HasColumnName("inclusionId");
            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("displayOrder");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

            entity.HasOne(d => d.Category).WithMany(p => p.Inclusions)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("inclusions_categoryId_fkey");
        });

        modelBuilder.Entity<InclusionTranslation>(entity =>
        {
            entity.HasKey(e => e.InclusionTranslationId).HasName("inclusionTranslations_pkey");

            entity.ToTable("inclusionTranslations");

            entity.HasIndex(e => new { e.InclusionId, e.LanguageCode }, "inclusionTranslations_inclusionId_languageCode_key").IsUnique();

            entity.Property(e => e.InclusionTranslationId).HasColumnName("inclusionTranslationId");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.InclusionId).HasColumnName("inclusionId");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasOne(d => d.Inclusion).WithMany(p => p.InclusionTranslations)
                .HasForeignKey(d => d.InclusionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("inclusionTranslations_inclusionId_fkey");
        });

        modelBuilder.Entity<InclusionsCategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("inclusionsCategories_pkey");

            entity.ToTable("inclusionsCategories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("displayOrder");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("languages_pkey");

            entity.ToTable("languages");

            entity.HasIndex(e => e.Code, "languages_code_key").IsUnique();

            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.IsDefault)
                .HasDefaultValue(false)
                .HasColumnName("isDefault");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<LanguageOffered>(entity =>
        {
            entity.HasKey(e => new { e.TourId, e.LanguageId }).HasName("languageOffered_pkey");

            entity.ToTable("languageOffered");

            entity.HasIndex(e => e.TourId, "idx_language_offered_tour_id");

            entity.HasIndex(e => e.LanguageId, "languageOffered_languageId_idx");

            entity.HasIndex(e => e.TourId, "languageOffered_tourId_idx");

            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.LanguageId).HasColumnName("languageId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsPrimary)
                .HasDefaultValue(false)
                .HasColumnName("isPrimary");
            entity.Property(e => e.Notes).HasColumnName("notes");
            entity.Property(e => e.Times).HasColumnName("times");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Language).WithMany(p => p.LanguageOffereds)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("languageOffered_languageId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.LanguageOffereds)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("languageOffered_tourId_fkey");
        });

        modelBuilder.Entity<Participant>(entity =>
        {
            entity.HasKey(e => e.ParticipantId).HasName("participants_pkey");

            entity.ToTable("participants");

            entity.Property(e => e.ParticipantId).HasColumnName("participantId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.DateOfBirth)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("dateOfBirth");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.FirstName).HasColumnName("firstName");
            entity.Property(e => e.LastName).HasColumnName("lastName");
            entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });

        modelBuilder.Entity<PickupLocation>(entity =>
        {
            entity.HasKey(e => e.PickUpLocationId).HasName("pickupLocation_pkey");

            entity.ToTable("pickupLocation");

            entity.Property(e => e.PickUpLocationId).HasColumnName("pickUpLocationId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Instructions).HasColumnName("instructions");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.LocationName).HasColumnName("locationName");
            entity.Property(e => e.PickupTime)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("pickupTime");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
        });


        modelBuilder.Entity<Tour>(entity =>
 {
     entity.HasKey(e => e.TourId).HasName("tours_pkey");

     entity.ToTable("tours");

     entity.HasIndex(e => e.Slug, "tours_slug_key").IsUnique();

     entity.Property(e => e.TourId)
         .HasColumnName("tourId")
         .UseIdentityColumn(); // This maps to SERIAL

     entity.Property(e => e.AvailableDays)
         .HasColumnType("integer[]")
         .HasColumnName("availableDays");

     entity.Property(e => e.AverageRating)
         .HasColumnType("numeric(18,10)")
         .HasColumnName("averageRating");

     entity.Property(e => e.CategoryId)
         .HasColumnName("categoryId");

     entity.Property(e => e.CreatedAt)
         .HasColumnType("timestamp(3)")
         .HasDefaultValueSql("CURRENT_TIMESTAMP")
         .IsRequired()
         .HasColumnName("createdAt");

     entity.Property(e => e.CreatedBy)
         .HasColumnType("text")
         .IsRequired()
         .HasColumnName("createdBy");

     entity.Property(e => e.CurrentAvailability)
         .HasColumnName("currentAvailability");

     entity.Property(e => e.DestinationId)
         .IsRequired()
         .HasColumnName("destinationId");

     entity.Property(e => e.DurationDays)
         .IsRequired()
         .HasColumnName("durationDays");

     entity.Property(e => e.DurationHours)
         .HasColumnType("numeric(18,10)")
         .IsRequired()
         .HasColumnName("durationHours");

     entity.Property(e => e.EndTime)
         .HasColumnType("timestamp(3)")
         .HasDefaultValueSql("CURRENT_TIMESTAMP")
         .HasColumnName("endTime");

     entity.Property(e => e.FeaturedImageUrl)
         .HasColumnType("text")
         .HasColumnName("featuredImageUrl");

     entity.Property(e => e.GalleryImageUrls)
         .HasColumnType("text[]")
         .HasColumnName("galleryImageUrls");

     entity.Property(e => e.IsActive)
         .HasColumnName("isActive")
         .IsRequired(); // This column is NOT NULL in the SQL table

     entity.Property(e => e.IsFeatured)
         .HasColumnName("isFeatured");

     entity.Property(e => e.MaxParticipants)
         .IsRequired()
         .HasColumnName("maxParticipants");

     entity.Property(e => e.MinAge)
         .HasColumnName("minAge");

     entity.Property(e => e.MinParticipants)
         .IsRequired()
         .HasColumnName("minParticipants");

     entity.Property(e => e.ReviewCount)
         .HasColumnName("reviewCount");

     entity.Property(e => e.RouteMapUrl)
         .HasColumnType("text")
         .HasColumnName("routeMapUrl");

     entity.Property(e => e.Slug)
         .HasColumnType("text")
         .IsRequired()
         .HasColumnName("slug");

     entity.Property(e => e.StartTime)
         .HasColumnType("timestamp(3)")
         .HasDefaultValueSql("CURRENT_TIMESTAMP")
         .HasColumnName("startTime");

     entity.Property(e => e.TourCode)
         .HasColumnType("text")
         .HasColumnName("tourCode");

     entity.Property(e => e.UpdatedAt)
         .HasColumnType("timestamp(3)")
         .HasDefaultValueSql("CURRENT_TIMESTAMP")
         .HasColumnName("updatedAt");

     entity.Property(e => e.UpdatedBy)
         .HasColumnType("text")
         .HasColumnName("updatedBy");

     entity.Property(e => e.VideoUrl)
         .HasColumnType("text")
         .HasColumnName("videoUrl");

     entity.Property(e => e.AcceptsMinors)
         .HasDefaultValue(true)
         .HasColumnName("acceptsMinors");

     entity.Property(e => e.Accessible)
         .HasDefaultValue(false)
         .IsRequired()
         .HasColumnName("accessible");

     entity.Property(e => e.LocationId)
         .HasColumnName("locationId");

     entity.Property(e => e.TripAdvisorUlrReviews)
         .HasColumnType("text")
         .HasColumnName("tripAdvisorUlrReviews");

     // Foreign key relationships
     entity.HasOne(d => d.Category)
         .WithMany(p => p.Tours)
         .HasForeignKey(d => d.CategoryId)
         .OnDelete(DeleteBehavior.SetNull)
         .HasConstraintName("tours_categoryId_fkey");

     entity.HasOne(d => d.Destination)
         .WithMany(p => p.Tours)
         .HasForeignKey(d => d.DestinationId)
         .OnDelete(DeleteBehavior.Restrict)
         .HasConstraintName("tours_destinationId_fkey");
 });
        modelBuilder.Entity<TourAdditionalInfo>(entity =>
        {
            entity.HasKey(e => e.TourAdditionalInfoId).HasName("tourAdditionalInfo_pkey");

            entity.ToTable("tourAdditionalInfo");

            entity.Property(e => e.TourAdditionalInfoId).HasColumnName("tourAdditionalInfoId");
            entity.Property(e => e.AdditionalInfoId).HasColumnName("additionalInfoId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.AdditionalInfo).WithMany(p => p.TourAdditionalInfos)
                .HasForeignKey(d => d.AdditionalInfoId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourAdditionalInfo_additionalInfoId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourAdditionalInfos)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourAdditionalInfo_tourId_fkey");
        });

        modelBuilder.Entity<TourHighlight>(entity =>
        {
            entity.HasKey(e => e.HighlightId).HasName("tourHighlights_pkey");

            entity.ToTable("tourHighlights");

            entity.Property(e => e.HighlightId).HasColumnName("highlightId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.IconName).HasColumnName("iconName");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.SortOrder)
                .HasDefaultValue(1)
                .HasColumnName("sortOrder");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourHighlights)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("tourHighlights_tourId_fkey");
        });

        modelBuilder.Entity<TourHighlightTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tourHighlightTranslations_pkey");

            entity.ToTable("tourHighlightTranslations");

            entity.HasIndex(e => new { e.HighlightId, e.LanguageCode }, "tourHighlightTranslations_highlightId_languageCode_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.HighlightId).HasColumnName("highlightId");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Highlight).WithMany(p => p.TourHighlightTranslations)
                .HasForeignKey(d => d.HighlightId)
                .HasConstraintName("tourHighlightTranslations_highlightId_fkey");
        });

        modelBuilder.Entity<TourInclusion>(entity =>
        {
            entity.HasKey(e => e.TourInclusionId).HasName("tourInclusions_pkey");

            entity.ToTable("tourInclusions");

            entity.Property(e => e.TourInclusionId).HasColumnName("tourInclusionId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.InclusionId).HasColumnName("inclusionId");
            entity.Property(e => e.IsIncluded)
                .HasDefaultValue(false)
                .HasColumnName("isIncluded");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Inclusion).WithMany(p => p.TourInclusions)
                .HasForeignKey(d => d.InclusionId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourInclusions_inclusionId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourInclusions)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourInclusions_tourId_fkey");
        });

        modelBuilder.Entity<TourPackage>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("tourPackages_pkey");

            entity.ToTable("tourPackages");

            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.CreatedBy).HasColumnName("createdBy");
            entity.Property(e => e.DisplayOrder)
                .HasDefaultValue(0)
                .HasColumnName("displayOrder");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");
            entity.Property(e => e.UpdatedBy).HasColumnName("updatedBy");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourPackages)
                .HasForeignKey(d => d.TourId)
                .HasConstraintName("tourPackages_tourId_fkey");
        });

        modelBuilder.Entity<TourPackageSchedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("tourPackageSchedules_pkey");

            entity.ToTable("tourPackageSchedules");

            entity.HasIndex(e => e.PackageId, "tourPackageSchedules_packageId_idx");

            entity.Property(e => e.ScheduleId).HasColumnName("scheduleId");
            entity.Property(e => e.AvailableSeats)
                .HasDefaultValue(0)
                .HasColumnName("availableSeats");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnName("createdAt");
            entity.Property(e => e.EndDate).HasColumnName("endDate");
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true)
                .HasColumnName("isActive");
            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.StartDate).HasColumnName("startDate");
            entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");

            entity.HasOne(d => d.Package).WithMany(p => p.TourPackageSchedules)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("tourPackageSchedules_packageId_fkey");
        });

        modelBuilder.Entity<TourPackageTranslation>(entity =>
        {
            entity.HasKey(e => e.TourPackageTranslationId).HasName("tourPackageTranslations_pkey");

            entity.ToTable("tourPackageTranslations");

            entity.HasIndex(e => new { e.PackageId, e.LanguageCode }, "tourPackageTranslations_packageId_languageCode_key").IsUnique();

            entity.Property(e => e.TourPackageTranslationId).HasColumnName("tourPackageTranslationId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Package).WithMany(p => p.TourPackageTranslations)
                .HasForeignKey(d => d.PackageId)
                .HasConstraintName("tourPackageTranslations_packageId_fkey");
        });

        modelBuilder.Entity<TourPricing>(entity =>
        {
            entity.HasKey(e => e.PricingId).HasName("tourpricing_pkey");

            entity.ToTable("tourPricing");

            entity.Property(e => e.PricingId)
                .HasDefaultValueSql("nextval('\"tourpricing_pricingId_seq\"'::regclass)")
                .HasColumnName("pricingId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Currency)
                .HasColumnType("character varying")
                .HasColumnName("currency");
            entity.Property(e => e.EffectiveFrom).HasColumnName("effectiveFrom");
            entity.Property(e => e.EffectiveTo).HasColumnName("effectiveTo");
            entity.Property(e => e.IsCurrent).HasColumnName("isCurrent");
            entity.Property(e => e.MaxParticipants).HasColumnName("maxParticipants");
            entity.Property(e => e.MinParticipants).HasColumnName("minParticipants");
            entity.Property(e => e.PackageId).HasColumnName("packageId");
            entity.Property(e => e.PricePerAdult)
                .HasPrecision(12, 2)
                .HasColumnName("pricePerAdult");
            entity.Property(e => e.PricePerChild)
                .HasPrecision(12, 2)
                .HasColumnName("pricePerChild");
            entity.Property(e => e.PricePerInfant)
                .HasPrecision(12, 2)
                .HasColumnName("pricePerInfant");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Package).WithMany(p => p.TourPricings)
                .HasForeignKey(d => d.PackageId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tourpricing_packageId_fkey");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourPricings)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourpricing_tourId_fkey");
        });

        modelBuilder.Entity<TourTranslation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("tourTranslations_pkey");

            entity.ToTable("tourTranslations");

            entity.HasIndex(e => new { e.TourId, e.LanguageCode }, "tourTranslations_tourId_languageCode_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AccessibilityOptions).HasColumnName("accessibilityOptions");
            entity.Property(e => e.ActivityLevel).HasColumnName("activityLevel");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.DetailedItinerary).HasColumnName("detailedItinerary");
            entity.Property(e => e.LanguageCode).HasColumnName("languageCode");
            entity.Property(e => e.MeetingPoint).HasColumnName("meetingPoint");
            entity.Property(e => e.RequiredDocuments).HasColumnName("requiredDocuments");
            entity.Property(e => e.SeoDescription).HasColumnName("seoDescription");
            entity.Property(e => e.SeoTitle).HasColumnName("seoTitle");
            entity.Property(e => e.Subtitle).HasColumnName("subtitle");
            entity.Property(e => e.SuitableFor).HasColumnName("suitableFor");
            entity.Property(e => e.Tags).HasColumnName("tags");
            entity.Property(e => e.Title).HasColumnName("title");
            entity.Property(e => e.TourId).HasColumnName("tourId");

            entity.HasOne(d => d.Tour).WithMany(p => p.TourTranslations)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourTranslations_tourId_fkey");
        });

        modelBuilder.Entity<Tourcategory>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("tourcategories_pkey");

            entity.ToTable("tourcategories");

            entity.Property(e => e.CategoryId).HasColumnName("categoryId");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parentCategoryId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("tourcategories_parentCategoryId_fkey");
        });

        modelBuilder.Entity<Tourdateblock>(entity =>
        {
            entity.HasKey(e => e.TourDateBlockId).HasName("tourdateblock_pkey");

            entity.ToTable("tourdateblock");

            entity.Property(e => e.TourDateBlockId).HasColumnName("tourDateBlockId");
            entity.Property(e => e.BlockedDate)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("blockedDate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("createdAt");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.TourId).HasColumnName("tourId");
            entity.Property(e => e.UpdatedAt)
                .HasColumnType("timestamp(3) without time zone")
                .HasColumnName("updatedAt");

            entity.HasOne(d => d.Tour).WithMany(p => p.Tourdateblocks)
                .HasForeignKey(d => d.TourId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("tourdateblock_tourId_fkey");
        });

        modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId).HasName("users_pkey");
                entity.ToTable("users");
                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.UserId).HasColumnName("userId");
                entity.Property(e => e.Email).HasColumnName("email");
                entity.Property(e => e.Password).HasColumnName("password");
                entity.Property(e => e.FirstName).HasColumnName("firstName");
                entity.Property(e => e.LastName).HasColumnName("lastName");
                entity.Property(e => e.PhoneNumber).HasColumnName("phoneNumber");
                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP")
                    .HasColumnType("timestamp(3) without time zone")
                    .HasColumnName("createdAt");
            }
        );




        modelBuilder.HasSequence<int>("seq_schema_version", "graphql").IsCyclic();

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
