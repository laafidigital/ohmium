using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels.LotusModels;
using Ohmium.Models.EFModels;
using Ohmium.Models.TemplateModels;

namespace Ohmium.Models
{
    public class OrganizationCache
    { 
        [Key]
        public Guid OrgID { get; set; }
        [Required]
        [Display(Name = "ORGANIZATION")]
        public string OrgName { get; set; }
        [Display(Name = "STATUS")]
        //active, inactive, deleted, suspended
        public string status { get; set; }
        [Display(Name = "CREATED ON")]
        public DateTime createdOn { get; set; }
        [Display(Name = "UPDATED ON")]
        public DateTime updatedOn { get; set; }
        [Display(Name = "CREATED BY")]
        public string createdBy { get; set; }
        [Display(Name = "UPDATED BY")]
        public string updatedBy { get; set; }
    }

    public class CacheContext: DbContext
    {
        public CacheContext(DbContextOptions<CacheContext> options): base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<MTSDeviceData>().HasKey(table => new {
                table.deviceID,
                table.timeStamp
            });
            builder.Entity<MTSStackData>().HasKey(table => new
            {
                table.deviceID,
                table.stackMfgID,
                table.timeStamp
            });
            //builder.Entity<StackSync>().HasKey(table => new
            //{
            //    table.stackID,
            //    table.scriptID
            //});
        }
        public DbSet<OrganizationCache> orgCache { get; set; }
        public DbSet<Org> org { get; set; }
        public DbSet<EquipmentConfiguration> equipmentConfiguration { get; set; }
        public DbSet<SQSite> site { get; set; }
        public DbSet<Segment> segment { get; set; }
        public DbSet<SQDevice> device { get; set; }
        public DbSet<Address> address { get; set; }
        public DbSet<SQStack> stack { get; set; }
        public DbSet<StackConfig> sconfig { get; set; }
        public DbSet<Region> region { get; set; }
        public DbSet<StatusType> statusType { get; set; }
        public DbSet<TestProfileConfig> testProfileConfig { get; set; }
        public DbSet<ColorConfig> colorConfig { get; set; }
        public DbSet<Ohmium.Models.TemplateModels.SequenceLibrary> SequenceLibrary { get; set; }
        public DbSet<Ohmium.Models.TemplateModels.RunStepLibrary> RunStepLibrary { get; set; }
        public DbSet<MTSStackData> mtsStackData { get; set; }
        public DbSet<MTSDeviceData> mtsDeviceData { get; set; }
        public DbSet<DeviceDataLog> deviceDataLog { get; set; }

    }
}
