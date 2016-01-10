using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using DataAccessPostgreSqlProvider;

namespace DataAccessPostgreSqlProvider.Migrations
{
    [DbContext(typeof(DomainModelPostgreSqlContext))]
    partial class DomainModelPostgreSqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("DomainModel.Model.DataEventRecord", b =>
                {
                    b.Property<long>("DataEventRecordId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<int>("SourceInfoId");

                    b.Property<long?>("SourceInfoSourceInfoId");

                    b.Property<DateTime>("Timestamp");

                    b.Property<DateTime>("UpdatedTimestamp");

                    b.HasKey("DataEventRecordId");
                });

            modelBuilder.Entity("DomainModel.Model.SourceInfo", b =>
                {
                    b.Property<long>("SourceInfoId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<string>("Name");

                    b.Property<DateTime>("Timestamp");

                    b.Property<DateTime>("UpdatedTimestamp");

                    b.HasKey("SourceInfoId");
                });

            modelBuilder.Entity("DomainModel.Model.DataEventRecord", b =>
                {
                    b.HasOne("DomainModel.Model.SourceInfo")
                        .WithMany()
                        .HasForeignKey("SourceInfoSourceInfoId");
                });
        }
    }
}
