using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ohmium.Models.EFModels;
using Ohmium.Models.EFModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ohmium.Models;
using System.IO;
using Ohmium.Models.EFModels.LotusModels;
using Ohmium.Models.TemplateModels;
using System.Runtime.CompilerServices;

namespace Ohmium.Repository
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    public class SensorContext : DbContext
    {
        public SensorContext(DbContextOptions<SensorContext> options) : base(options)
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
            builder.Entity<StackSync>().HasKey(table => new
            {
                table.stackID,
                table.scriptID
            });
            builder.Entity<StacksThatRan>().HasKey(table => new {
                table.stackID,
                table.deviceID
            });
            builder.Entity<MTSDeviceDataNew>().HasKey(table => new {
                table.deviceID,
                table.timeStamp
            });
            builder.Entity<MTSStackDataNew>().HasKey(table => new
            {
                table.deviceID,
                table.stackMfgID,
                table.timeStamp
            });
            builder.Entity<MTSDeviceDataNew2>().HasKey(table => new {
                table.deviceID,
                table.timeStamp
            });
            builder.Entity<MTSStackDataNew2>().HasKey(table => new
            {
                table.deviceID,
                table.stackMfgID,
                table.timeStamp
            });

        }

        public DbSet<DeviceData> deviceData { get; set; }
        public DbSet<TSStackData> tsStackData { get; set; }
        public DbSet<Org> org { get; set; }
        public DbSet<EquipmentConfiguration> equipmentConfiguration { get; set; }
        public DbSet<Site> site { get; set; }
        public DbSet<Segment> segment { get; set; }
        public DbSet<Device> device { get; set; }
        public DbSet<Address> address { get; set; }
        public DbSet<Stack> stack { get; set; }
        public DbSet<StackConfig> sconfig { get; set; }
        public DbSet<Region> region { get; set; }
        public DbSet<RunProfile> runProfile { get; set; }
        public DbSet<StatusType> statusType { get; set; }
        public DbSet<TestProfileConfig> testProfileConfig { get; set; }
        public DbSet<StackRunProfile> stkRunProfile { get; set; }
        public DbSet<RunStep> stkStep { get; set; }
        public DbSet<ColorConfig> colorConfig { get; set; }
        public DbSet<RunProfileTemplate> runProfileTemplate { get; set; }
        public DbSet<StackRunProfileTemplate> stackRunProfileTemplate { get; set; }
        public DbSet<RunStepTemplate> runStepTemplate { get; set; }
        public DbSet<DeviceTemplateAllocation> deviceTemplateAllocation { get; set; }
        public DbSet<Feedback> feedback { get; set; }
        public DbSet<LotusDeviceData> lotusDeviceData { get; set; }
        //public DbSet<LHC> LHCData { get; set; }
        //public DbSet<LPC> LPCData { get; set; }
        //public DbSet<LCC> LCCData { get; set; }
        //public DbSet<LWC> LWCData { get; set; }
        //public DbSet<LTC> LTCData { get; set; }
        public DbSet<SYS> sysMaster { get; set; }
      //  public DbSet<SYSVM> sysVMData { get; set; }
        public DbSet<TestState> testStates { get; set; }
        public DbSet<RunStepTemplateGroup> runStepTemplateGroup { get; set; }
        public DbSet<StackPhaseSetting> stkPhaseSetting { get; set; }

        public DbSet<LotusTemp> lotusTempdata { get; set; }
        public DbSet<MTSStackData> mtsStackData { get; set; }
        public DbSet<MTSDeviceData> mtsDeviceData { get; set; }
        public DbSet<StackTestRunHours> stackTestRunHours { get; set; }
        public DbSet<MinMaxParams> mmp { get; set; }
        public DbSet<StackPhaseSettingNew> stackPhaseSettingNew { get; set; }
        public DbSet<UserLogin> userLogin { get; set; }
        public DbSet<RunStepLibrary> runstepLibrary { get; set; }
        public DbSet<SequenceLibrary> sequencyLibrary { get; set; }
        public DbSet<ScriptLibrary> scriptLibrary { get; set; }
        public DbSet<ScriptList> scriptlists { get; set; }
        public DbSet<StackSync> stackSyncData { get; set; }
        public DbSet<ThresholdConfig> thresholdconfigs { get; set; }
        //[DbFunction("spsinglestack")]
        //public virtual DbSet<List<MTSStackData>> spsinglestack(string stackid, DateTime sdate, DateTime edate)
        //{
        //    throw new NotImplementedException();
        //}
        public DbSet<StacksThatRan> StacksThatRan {get; set; }
        public DbSet<MTSDeviceDataNew> mtsDeviceDataNew { get; set; }
        public DbSet<MTSStackDataNew> mtsStackDataNew { get; set; }

        public DbSet<MTSDeviceDataNew2> mtsDeviceDataNew2 { get; set; }
        public DbSet<MTSStackDataNew2> mtsStackDataNew2 { get; set; }
    }


    public interface IRMCCRepository<T> where T:class
    {
       Task<IEnumerable<T>> GetAll();
       Task<T> GetByID(int? id);
        Task<bool> Create(T Obj);
       Task<T> Delete(T obj);
       Task< bool> Update(T obj);

    }


    public class RMCCRepository<T> : IRMCCRepository<T> where T : class
    {
        private SensorContext _Context = null;
        private DbSet<T> table = null;
        public RMCCRepository(SensorContext _context)
        {
            this._Context = _context;
            table = _context.Set<T>();
        }
        public RMCCRepository(SensorContext _context, bool check)
        {
            this._Context = _context;
            table = _context.Set<T>();
        }
        //public RMCCRepository()
        //{
        //    this._Context = new SensorContext();
        //    table = _Context.Set<T>();
        //}

        public async Task<IEnumerable<T>> GetAll()
        {
          return  await table.ToListAsync();
        }

    public async Task<T> GetByID(int? id)
        {
            return await table.FindAsync(id);
        }

       public async Task<bool> Create(T Obj)
        {
            try
            {
              await  _Context.AddAsync(Obj);
            await    _Context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Update(T obj)
        {
            try
            {
               _Context.Entry(obj).State = EntityState.Modified;
               await _Context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<T> Delete(T obj)
        {
            _Context.Remove(obj);
           await  _Context.SaveChangesAsync();
            return obj;
        }

    }
}
