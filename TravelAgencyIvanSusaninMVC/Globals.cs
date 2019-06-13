using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TravelAgencyIvanSusaninDAL.Interfaces;
using TravelAgencyIvanSusaninDAL.ViewModel;
using TravelAgencyIvanSusaninImplementDataBase;
using TravelAgencyIvanSusaninImplementDataBase.Implementations;

namespace TravelAgencyIvanSusaninMVC
{
    public static class Globals
    {
        public static AbstractDbContext DbContext { get; } = new AbstractDbContext();

        public static IClientService ClientService { get; } = new ClientServiceDB(DbContext);

        public static ITourService TourService { get; } = new TourServiceDB(DbContext);

        public static ITravelService TravelService { get; } = new TravelServiceDB(DbContext);

        public static IReportService ReportService { get; } = new ReportServiceDB(DbContext);

        public static ClientViewModel AuthClient { get; set; } = null;

        public static IStatisticService StatisticService { get; } = new StatisticServiceDB(DbContext);

    }
}