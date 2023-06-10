//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using StructureMap;
//using System.IO;
//using VeriSahibi.Listings;
//using VeriSahibi.Listings.Repositories;
//using VeriSahibi.Mongo.Data;

//namespace Moduler.Helpers.Ioc
//{
//    public class IocInvoker
//    {
//        public StructureMap.IContainer container;
//        public IocInvoker()
//        {
//            container = GetContainer();
//        }
//        public static StructureMap.IContainer GetContainer()
//        {
//            AppSetting.config.Initialize();
//            IContainer container = IoC.Initialize();
//            return container;
//        }
//    }
//    public static class IoC
//    {
//        public static IConfigurationRoot GetConfiguration()
//        {
//            var builder = new ConfigurationBuilder()
//                  .SetBasePath(Directory.GetCurrentDirectory())
//                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

//            return builder.Build();
//        }
//        public static IContainer Initialize()
//        {
//            var container = new Container();
//            container.Configure(x =>
//            {
//                x.Scan(scan =>
//                {
//                    scan.TheCallingAssembly();
//                    scan.WithDefaultConventions();

//                    scan.Assembly("VeriSahibi.Listings");
//                });

//                //x.AddRegistry(new AutomationRegistry());
//                x.For<IConfigurationRoot>().Use(GetConfiguration());
//                //x.For<AppBaseSettings>().Use(AppBaseSettings.GetConfig());

//                //x.For<AppMongoDbSettings>().Use(GetConfiguration().GetSection("ListingDatabase"));
//                //x.b.Services.Configure<AppMongoDbSettings>(
//                //    builder.Configuration.GetSection("ListingDatabase"));


//                var optionsBuilder = new DbContextOptionsBuilder<AppLogDbContext>();
//                optionsBuilder.UseNpgsql("Server=" + AppSetting.config.SourceHost + ";Port=5432;Database=" + AppSetting.config.SourceDb +
//                    ";Integrated Security=true;Pooling=true;User Id=myvs;Password=MyPass987.")
//                    //.EnableSensitiveDataLogging()
//                    ;
//                x.For<AppLogDbContext>().Use(() => new AppLogDbContext(optionsBuilder.Options));

//                //var optionsBuilder2 = new DbContextOptionsBuilder<VSListingsDbContext>();
//                //optionsBuilder2.UseNpgsql("Server=" + host + ";Port=5432;Database=" + target +
//                //    ";Integrated Security=true;Pooling=true;User Id=myvs;Password=MyPass987.")
//                //    //.EnableSensitiveDataLogging()
//                //    ;
//                //x.For<VSListingsDbContext>().Use(() => new VSListingsDbContext(optionsBuilder2.Options));

//                var optionsBuilder3 = new DbContextOptionsBuilder<AppDbContext>();
//                optionsBuilder3.UseNpgsql("Server=" + AppSetting.config.TargetHost + ";Port=5432;Database=" + AppSetting.config.TargetDb +
//                    ";Integrated Security=true;Pooling=true;User Id=myvs;Password=MyPass987.")
//                    .EnableSensitiveDataLogging()
//                    ;
//                //optionsBuilder3.UseTriggers(triggerOptions =>
//                //{
//                //    triggerOptions.AddTrigger<CreateHistoryOnUpdate>();
//                //});
//                x.For<AppDbContext>().Transient().Use(() => new AppDbContext(optionsBuilder3.Options));
//            });
//            return container;
//        }
//    }
//}