using EngenhariaReversaDb.Services;

namespace EngenhariaReversaDb
{
    class Program
    {
        static void Main(string[] args)
        {
            var connstr = @"Data source=C:\Users\phdua\source\repos\phduarte\Agenda\Gadz.Agenda.Web\agenda.db";
            var service = new GenerateModelService(Domain.Provider.SQLite);

            var database = service.GetDatabase(connstr);


        }
    }
}
