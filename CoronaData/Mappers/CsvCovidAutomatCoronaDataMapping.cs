using System;
using TinyCsvParser.Mapping;

namespace CoronaData.Mappers
{
    public class CsvCovidAutomatCoronaDataMapping : CsvMapping<Models.DataModel.CovidAutomat_TestData_line>
    {
        public CsvCovidAutomatCoronaDataMapping() : base()
        {
            MapProperty(0, p => p.Date, new SlovakStringDateToDateTypeConverter());
            MapProperty(1, p => p.DistrictCode);
            MapProperty(2, p => p.District, new StringWithoutQuotesWithDistrictTypeConverter());
            MapProperty(5, p => p.DistrictPopulation, new PopulationStringToIntTypeConverter());
            MapProperty(10, p => p.SevenDaysIncidencyActual, new StringToIntTypeConverter());
            MapProperty(11, p => p.SevenDaysIncidencyBefore, new StringToIntTypeConverter());
            MapProperty(17, p => p.Vaccination_Full);
        }
    }
}
