using Autofac;
using SudokuApp.Bl.ServiceInterfaces;
using SudokuApp.Bl.Services;
using SudokuApp.Common.ServiceInterfaces;
using SudokuApp.Common.Services;
using SudokuApp.Data.Config;
using SudokuApp.DataAccess.Repository;
using SudokuApp.DataAccess.RepositoryInterface;

namespace SudokuApp.Api
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ResponseUtilityService>()
                .As<IResponseUtilityService>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<Repository<SudokuAppDbContext>>()
                .As<IRepository>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ReadOnlyRepository<SudokuAppDbContext>>()
                .As<IReadOnlyRepository>()
                .InstancePerLifetimeScope();
            
            builder.RegisterType<BoardService>()
                .As<IBoardService>()
                .InstancePerLifetimeScope();
        }
    }
}