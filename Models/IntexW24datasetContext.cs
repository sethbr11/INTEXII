using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace INTEXII.Models;

public partial class IntexW24datasetContext : DbContext
{
    public IntexW24datasetContext()
    {
    }

    public IntexW24datasetContext(DbContextOptions<IntexW24datasetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Recommendation> Recommendations { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:myfreesqldbserverbrock.database.windows.net,1433;Initial Catalog=SethTestDb;Persist Security Info=False;User ID=sethab;Password=REMOVED_SECRET;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasIndex(e => e.CustomerId, "IX_Customers_customer_ID").IsUnique();

            entity.Property(e => e.CustomerId)
                .HasColumnType("SMALLINT")
                .HasColumnName("customer_ID");
            entity.Property(e => e.Age)
                .HasColumnType("FLOAT")
                .HasColumnName("age");
            entity.Property(e => e.BirthDate)
                .HasColumnName("birth_date");
            entity.Property(e => e.CountryOfResidence)
                .HasColumnName("country_of_residence");
            entity.Property(e => e.FirstName)
                .HasColumnName("first_name");
            entity.Property(e => e.Gender)
                .HasColumnName("gender");
            entity.Property(e => e.LastName)
                .HasColumnName("last_name");
            entity.Property(e => e.Username)
                .HasColumnName("username");
        });

        modelBuilder.Entity<LineItem>(entity =>
        {
            entity.HasKey(e => new { e.TransactionId, e.ProductId });

            entity.Property(e => e.TransactionId)
                .HasColumnType("INTEGER")
                .HasColumnName("transaction_ID");
            entity.Property(e => e.ProductId)
                .HasColumnType("TINYINT")
                .HasColumnName("product_ID");
            entity.Property(e => e.Qty)
                .HasColumnType("TINYINT")
                .HasColumnName("qty");
            entity.Property(e => e.Rating)
                .HasColumnType("TINYINT")
                .HasColumnName("rating");
        });

        modelBuilder.Entity<Recommendation>(entity => {
            entity.HasKey(e => e.CustomerId);

            entity.Property(e => e.CustomerId)
                .HasColumnType("SMALLINT")
                .HasColumnName("customer_id");
            entity.Property(e => e.RecProdId1)
                .HasColumnType("TINYINT")
                .HasColumnName("rec_product_id_1");
            entity.Property(e => e.RecProdId2)
                .HasColumnType("TINYINT")
                .HasColumnName("rec_product_id_2");
            entity.Property(e => e.RecProdId3)
                .HasColumnType("TINYINT")
                .HasColumnName("rec_product_id_3");
            entity.Property(e => e.RecProdId4)
                .HasColumnType("TINYINT")
                .HasColumnName("rec_product_id_4");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.TransactionId);

            entity.HasIndex(e => e.TransactionId, "IX_Orders_transaction_ID").IsUnique();

            entity.Property(e => e.TransactionId)
                .HasColumnType("INTEGER")
                .ValueGeneratedNever()
                .HasColumnName("transaction_ID");
			entity.Property(e => e.CustomerId)
                .HasColumnType("SMALLINT")
	            .HasColumnName("customer_ID");
			entity.Property(e => e.Date)
				.HasColumnName("date");
			entity.Property(e => e.DayOfWeek)
				.HasColumnName("day_of_week");
			entity.Property(e => e.Time)
				.HasColumnType("TINYINT")
				.HasColumnName("time");
			entity.Property(e => e.EntryMode)
			    .HasColumnName("entry_mode");
			entity.Property(e => e.Amount)
                .HasColumnType("SMALLINT")
                .HasColumnName("amount");
			entity.Property(e => e.TypeOfTransaction)
				.HasColumnName("type_of_transaction");
			entity.Property(e => e.CountryOfTransaction)
				.HasColumnName("country_of_transaction");
			entity.Property(e => e.ShippingAddress)
				.HasColumnName("shipping_address");
			entity.Property(e => e.Bank)
                .HasColumnName("bank");
			entity.Property(e => e.TypeOfCard)
				.HasColumnName("type_of_card");
			entity.Property(e => e.Fraud)
                .HasColumnType("TINYINT")
                .HasColumnName("fraud");
            entity.Property(e => e.PredFraud)
                .HasColumnType("TINYINT")
                .HasColumnName("pred_fraud");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasIndex(e => e.ProductId, "IX_Products_product_ID").IsUnique();

            entity.Property(e => e.ProductId)
                .HasColumnType("TINYINT")
                .HasColumnName("product_ID");
            entity.Property(e => e.Category)
                .HasColumnName("category");
            entity.Property(e => e.PublicCategory)
                .HasColumnName("general_category");
            entity.Property(e => e.Description)
                .HasColumnName("description");
            entity.Property(e => e.ImgLink)
                .HasColumnName("img_link");
            entity.Property(e => e.Name)
                .HasColumnName("name");
            entity.Property(e => e.NumParts)
                .HasColumnType("SMALLINT")
                .HasColumnName("num_parts");
            entity.Property(e => e.Price)
                .HasColumnType("SMALLINT")
                .HasColumnName("price");
            entity.Property(e => e.PrimaryColor)
                .HasColumnName("primary_color");
            entity.Property(e => e.SecondaryColor)
                .HasColumnName("secondary_color");
            entity.Property(e => e.Year)
                .HasColumnType("SMALLINT")
                .HasColumnName("year");
            entity.Property(e => e.Recommendation1)
                .HasColumnType("TINYINT")
                .HasColumnName("recommendation1");
            entity.Property(e => e.Similarity1)
                .HasColumnType("FLOAT")
                .HasColumnName("similarity1");
            entity.Property(e => e.Recommendation2)
                .HasColumnType("TINYINT")
                .HasColumnName("recommendation2");
            entity.Property(e => e.Similarity2)
                .HasColumnType("FLOAT")
                .HasColumnName("similarity2");
            entity.Property(e => e.Recommendation3)
                .HasColumnType("TINYINT")
                .HasColumnName("recommendation3");
            entity.Property(e => e.Similarity3)
                .HasColumnType("FLOAT")
                .HasColumnName("similarity3");
            entity.Property(e => e.Recommendation4)
                .HasColumnType("TINYINT")
                .HasColumnName("recommendation4");
            entity.Property(e => e.Similarity4)
                .HasColumnType("FLOAT")
                .HasColumnName("similarity4");
            entity.Property(e => e.Recommendation5)
                .HasColumnType("TINYINT")
                .HasColumnName("recommendation5");
            entity.Property(e => e.Similarity5)
                .HasColumnType("FLOAT")
                .HasColumnName("similarity5");
            entity.Property(e => e.PopularityRank)
                .HasColumnType("FLOAT")
                .HasColumnName("popularity_rank");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
