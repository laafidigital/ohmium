﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Ohmium.Models;

namespace Ohmium.Migrations.Cache
{
    [DbContext(typeof(CacheContext))]
    partial class CacheContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("Ohmium.Models.EFModels.Address", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("address1")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("address2")
                        .HasColumnType("TEXT");

                    b.Property<string>("address3")
                        .HasColumnType("TEXT");

                    b.Property<string>("city")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("country")
                        .HasColumnType("TEXT");

                    b.Property<string>("postalCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("sid")
                        .HasColumnType("TEXT");

                    b.Property<string>("state")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("sid")
                        .IsUnique();

                    b.ToTable("address");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.ColorConfig", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("colorCode")
                        .HasColumnType("TEXT");

                    b.Property<string>("sensorName")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("colorConfig");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Contact", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("contactName")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .HasColumnType("TEXT");

                    b.Property<string>("phone")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("siteID")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("siteID");

                    b.ToTable("Contact");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.EquipmentConfiguration", b =>
                {
                    b.Property<Guid>("equipmentConfigID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("colorConfig")
                        .HasColumnType("TEXT");

                    b.Property<string>("configName")
                        .HasColumnType("TEXT");

                    b.Property<string>("createdBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("equipmentConfiguration")
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("equipmentConfigID");

                    b.ToTable("equipmentConfiguration");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.LotusModels.DeviceDataLog", b =>
                {
                    b.Property<DateTime>("timeStamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("description")
                        .HasColumnType("TEXT");

                    b.HasKey("timeStamp");

                    b.ToTable("deviceDataLog");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.MTSDeviceData", b =>
                {
                    b.Property<string>("deviceID")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timeStamp")
                        .HasColumnType("TEXT");

                    b.Property<short?>("CommStatus")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("HYS")
                        .HasColumnType("REAL");

                    b.Property<Guid>("configID")
                        .HasColumnType("TEXT");

                    b.Property<float?>("hxiT")
                        .HasColumnType("REAL");

                    b.Property<float?>("hxoT")
                        .HasColumnType("REAL");

                    b.Property<string>("siteID")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ver")
                        .HasColumnType("TEXT");

                    b.Property<string>("verM")
                        .HasColumnType("TEXT");

                    b.Property<float?>("wC")
                        .HasColumnType("REAL");

                    b.Property<float?>("wL")
                        .HasColumnType("REAL");

                    b.Property<float?>("wP")
                        .HasColumnType("REAL");

                    b.Property<float?>("wPp")
                        .HasColumnType("REAL");

                    b.Property<float?>("wT")
                        .HasColumnType("REAL");

                    b.HasKey("deviceID", "timeStamp");

                    b.ToTable("mtsDeviceData");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.MTSStackData", b =>
                {
                    b.Property<string>("deviceID")
                        .HasColumnType("TEXT");

                    b.Property<string>("stackMfgID")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timeStamp")
                        .HasColumnType("TEXT");

                    b.Property<float?>("CD")
                        .HasColumnType("REAL");

                    b.Property<float?>("cM1")
                        .HasColumnType("REAL");

                    b.Property<float?>("cM2")
                        .HasColumnType("REAL");

                    b.Property<float?>("cM3")
                        .HasColumnType("REAL");

                    b.Property<float?>("cM4")
                        .HasColumnType("REAL");

                    b.Property<float?>("cM5")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR1")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR2")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR3")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR4")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR5")
                        .HasColumnType("REAL");

                    b.Property<float?>("cR6")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV1")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV2")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV3")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV4")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV5")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV6")
                        .HasColumnType("REAL");

                    b.Property<float?>("cX1")
                        .HasColumnType("REAL");

                    b.Property<float?>("cX2")
                        .HasColumnType("REAL");

                    b.Property<float?>("cX3")
                        .HasColumnType("REAL");

                    b.Property<float?>("cX4")
                        .HasColumnType("REAL");

                    b.Property<float?>("cX5")
                        .HasColumnType("REAL");

                    b.Property<float?>("cumulativeHours")
                        .HasColumnType("REAL");

                    b.Property<float?>("hP")
                        .HasColumnType("REAL");

                    b.Property<float?>("hT")
                        .HasColumnType("REAL");

                    b.Property<float?>("imA")
                        .HasColumnType("REAL");

                    b.Property<float?>("imF")
                        .HasColumnType("REAL");

                    b.Property<string>("interLock")
                        .HasColumnType("TEXT");

                    b.Property<float?>("isF")
                        .HasColumnType("REAL");

                    b.Property<int?>("loopcnt")
                        .HasColumnType("INTEGER");

                    b.Property<string>("position")
                        .HasColumnType("TEXT");

                    b.Property<float?>("psI")
                        .HasColumnType("REAL");

                    b.Property<float?>("psV")
                        .HasColumnType("REAL");

                    b.Property<float?>("runHours")
                        .HasColumnType("REAL");

                    b.Property<float?>("scriptLoopCnt")
                        .HasColumnType("REAL");

                    b.Property<float?>("scriptStep")
                        .HasColumnType("REAL");

                    b.Property<float?>("seqLoopCnt")
                        .HasColumnType("REAL");

                    b.Property<string>("seqName")
                        .HasColumnType("TEXT");

                    b.Property<float?>("seqStep")
                        .HasColumnType("REAL");

                    b.Property<string>("state")
                        .HasColumnType("TEXT");

                    b.Property<int?>("status")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("stepNumber")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("wF")
                        .HasColumnType("REAL");

                    b.Property<float?>("wP")
                        .HasColumnType("REAL");

                    b.Property<float?>("wT")
                        .HasColumnType("REAL");

                    b.HasKey("deviceID", "stackMfgID", "timeStamp");

                    b.ToTable("mtsStackData");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Org", b =>
                {
                    b.Property<Guid>("OrgID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("createdBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("OrgID");

                    b.ToTable("org");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Region", b =>
                {
                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("desc")
                        .HasColumnType("TEXT");

                    b.HasKey("name");

                    b.ToTable("region");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQDevice", b =>
                {
                    b.Property<string>("EqMfgID")
                        .HasColumnType("TEXT");

                    b.Property<string>("EqDes")
                        .HasColumnType("TEXT");

                    b.Property<float?>("HYS")
                        .HasColumnType("REAL");

                    b.Property<bool>("comStatus")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("configID")
                        .HasColumnType("TEXT");

                    b.Property<string>("createdBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("deviceType")
                        .HasColumnType("TEXT");

                    b.Property<float>("h2Production")
                        .HasColumnType("REAL");

                    b.Property<float?>("hxiT")
                        .HasColumnType("REAL");

                    b.Property<float?>("hxoT")
                        .HasColumnType("REAL");

                    b.Property<string>("isRunning")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("lastDataReceivedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("nStack")
                        .HasColumnType("INTEGER");

                    b.Property<float>("powerConsumption")
                        .HasColumnType("REAL");

                    b.Property<float>("siteEfficiency")
                        .HasColumnType("REAL");

                    b.Property<Guid>("siteID")
                        .HasColumnType("TEXT");

                    b.Property<int?>("status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("updatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ver")
                        .HasColumnType("TEXT");

                    b.Property<float?>("wC")
                        .HasColumnType("REAL");

                    b.Property<float?>("wL")
                        .HasColumnType("REAL");

                    b.Property<float?>("wP")
                        .HasColumnType("REAL");

                    b.Property<float?>("wPp")
                        .HasColumnType("REAL");

                    b.Property<float?>("wT")
                        .HasColumnType("REAL");

                    b.HasKey("EqMfgID");

                    b.HasIndex("configID");

                    b.HasIndex("siteID");

                    b.HasIndex("status");

                    b.ToTable("device");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQSite", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("h2Production")
                        .HasColumnType("REAL");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("orgID")
                        .HasColumnType("TEXT");

                    b.Property<float>("powerConsumption")
                        .HasColumnType("REAL");

                    b.Property<float>("siteEfficiency")
                        .HasColumnType("REAL");

                    b.Property<float>("siteLat")
                        .HasColumnType("REAL");

                    b.Property<float>("siteLng")
                        .HasColumnType("REAL");

                    b.Property<Guid>("sqlId")
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("Region");

                    b.HasIndex("orgID");

                    b.ToTable("site");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQStack", b =>
                {
                    b.Property<string>("stackMfgID")
                        .HasColumnType("TEXT");

                    b.Property<string>("deviceID")
                        .HasColumnType("TEXT");

                    b.Property<float>("meaArea")
                        .HasColumnType("REAL");

                    b.Property<int>("meaNum")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("siteID")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("stackConfig")
                        .HasColumnType("TEXT");

                    b.Property<string>("stackPosition")
                        .HasColumnType("TEXT");

                    b.Property<int>("status")
                        .HasColumnType("INTEGER");

                    b.HasKey("stackMfgID");

                    b.HasIndex("deviceID");

                    b.HasIndex("siteID");

                    b.HasIndex("stackConfig");

                    b.HasIndex("status");

                    b.ToTable("stack");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Segment", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<string>("type")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("segment");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Site", b =>
                {
                    b.Property<Guid>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Region")
                        .HasColumnType("TEXT");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<float>("h2Production")
                        .HasColumnType("REAL");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("orgID")
                        .HasColumnType("TEXT");

                    b.Property<float>("powerConsumption")
                        .HasColumnType("REAL");

                    b.Property<float>("siteEfficiency")
                        .HasColumnType("REAL");

                    b.Property<float>("siteLat")
                        .HasColumnType("REAL");

                    b.Property<float>("siteLng")
                        .HasColumnType("REAL");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.HasIndex("Region");

                    b.HasIndex("orgID");

                    b.ToTable("Site");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.StackConfig", b =>
                {
                    b.Property<Guid>("configID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("colorConfig")
                        .HasColumnType("TEXT");

                    b.Property<string>("configName")
                        .HasColumnType("TEXT");

                    b.Property<string>("configString")
                        .HasColumnType("TEXT");

                    b.HasKey("configID");

                    b.ToTable("sconfig");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.StatusType", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("name")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("statusType");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.TestProfileConfig", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Config")
                        .HasColumnType("TEXT");

                    b.HasKey("id");

                    b.ToTable("testProfileConfig");
                });

            modelBuilder.Entity("Ohmium.Models.OrganizationCache", b =>
                {
                    b.Property<Guid>("OrgID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("OrgName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("createdBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("createdOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("status")
                        .HasColumnType("TEXT");

                    b.Property<string>("updatedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("updatedOn")
                        .HasColumnType("TEXT");

                    b.HasKey("OrgID");

                    b.ToTable("orgCache");
                });

            modelBuilder.Entity("Ohmium.Models.TemplateModels.RunStepLibrary", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<float?>("cI")
                        .HasColumnType("REAL");

                    b.Property<float?>("cV")
                        .HasColumnType("REAL");

                    b.Property<float?>("cVlimit")
                        .HasColumnType("REAL");

                    b.Property<float?>("cVt")
                        .HasColumnType("REAL");

                    b.Property<int>("duration")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("hP")
                        .HasColumnType("REAL");

                    b.Property<float?>("imA")
                        .HasColumnType("REAL");

                    b.Property<float?>("imF")
                        .HasColumnType("REAL");

                    b.Property<float?>("mnF")
                        .HasColumnType("REAL");

                    b.Property<float?>("mxF")
                        .HasColumnType("REAL");

                    b.Property<int?>("seqMasterId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("stepNumber")
                        .HasColumnType("INTEGER");

                    b.Property<float?>("wFt")
                        .HasColumnType("REAL");

                    b.Property<float?>("wP")
                        .HasColumnType("REAL");

                    b.Property<float?>("wTt")
                        .HasColumnType("REAL");

                    b.HasKey("id");

                    b.HasIndex("seqMasterId");

                    b.ToTable("RunStepLibrary");
                });

            modelBuilder.Entity("Ohmium.Models.TemplateModels.SequenceLibrary", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("loopCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("sequenceName")
                        .HasColumnType("TEXT");

                    b.Property<int>("sortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("id");

                    b.ToTable("SequenceLibrary");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Address", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.Site", "site")
                        .WithOne("address")
                        .HasForeignKey("Ohmium.Models.EFModels.Address", "sid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("site");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Contact", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.Site", "site")
                        .WithMany("contact")
                        .HasForeignKey("siteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("site");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQDevice", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.EquipmentConfiguration", "ec")
                        .WithMany()
                        .HasForeignKey("configID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ohmium.Models.EFModels.SQSite", "site")
                        .WithMany()
                        .HasForeignKey("siteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ohmium.Models.EFModels.StatusType", "statustype")
                        .WithMany()
                        .HasForeignKey("status");

                    b.Navigation("ec");

                    b.Navigation("site");

                    b.Navigation("statustype");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQSite", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.Region", "reg")
                        .WithMany()
                        .HasForeignKey("Region");

                    b.HasOne("Ohmium.Models.EFModels.Org", "org")
                        .WithMany()
                        .HasForeignKey("orgID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("org");

                    b.Navigation("reg");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.SQStack", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.SQDevice", "device")
                        .WithMany()
                        .HasForeignKey("deviceID");

                    b.HasOne("Ohmium.Models.EFModels.SQSite", "sid")
                        .WithMany()
                        .HasForeignKey("siteID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ohmium.Models.EFModels.StackConfig", "scon")
                        .WithMany()
                        .HasForeignKey("stackConfig")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Ohmium.Models.EFModels.StatusType", "sStatus")
                        .WithMany()
                        .HasForeignKey("status")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("device");

                    b.Navigation("scon");

                    b.Navigation("sid");

                    b.Navigation("sStatus");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Site", b =>
                {
                    b.HasOne("Ohmium.Models.EFModels.Region", "reg")
                        .WithMany()
                        .HasForeignKey("Region");

                    b.HasOne("Ohmium.Models.EFModels.Org", "org")
                        .WithMany()
                        .HasForeignKey("orgID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("org");

                    b.Navigation("reg");
                });

            modelBuilder.Entity("Ohmium.Models.TemplateModels.RunStepLibrary", b =>
                {
                    b.HasOne("Ohmium.Models.TemplateModels.SequenceLibrary", "seqmaster")
                        .WithMany()
                        .HasForeignKey("seqMasterId");

                    b.Navigation("seqmaster");
                });

            modelBuilder.Entity("Ohmium.Models.EFModels.Site", b =>
                {
                    b.Navigation("address");

                    b.Navigation("contact");
                });
#pragma warning restore 612, 618
        }
    }
}
