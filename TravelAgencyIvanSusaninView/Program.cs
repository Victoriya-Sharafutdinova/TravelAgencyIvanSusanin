using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninImplementDataBase;
using TravelAgencyIvanSusaninImplementDataBase.Implementations;
using System;
using System.Data.Entity;
using System.Windows.Forms;
using Unity;
using Unity.Lifetime;

namespace TravelAgencyIvanSusaninView
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = BuildUnityContainer();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(container.Resolve<FormTours>());
        }

        public static IUnityContainer BuildUnityContainer()
        {
            var currentContainer = new UnityContainer();
            currentContainer.RegisterType<DbContext, AbstractDbContext>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITourService, TourServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReservationService, ReservationServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IRequestService, RequestServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<ITravelService, TravelServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IReportService, ReportServiceDB>(new HierarchicalLifetimeManager());
            currentContainer.RegisterType<IStatisticService, StatisticServiceDB>(new HierarchicalLifetimeManager());
            return currentContainer;
        }
    }
}
