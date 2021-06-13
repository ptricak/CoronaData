using System;
using TinyCsvParser.Mapping;

namespace CoronaData.Mappers
{
    public class CsvPcrCoronaDataMapping : CsvMapping<Models.DataModel.PCR_TestData_line>
    {
        public CsvPcrCoronaDataMapping() : base()
        {
            MapProperty(0, p => p.Date);
            MapProperty(1, p => p.District, new StringWithoutQuotesTypeConverter());
            MapProperty(2, p => p.DistrictCode, new StringWithoutQuotesTypeConverter());
            MapProperty(3, p => p.PcrPosCount);
            MapProperty(4, p => p.PcrNegCount);
            MapProperty(5, p => p.PcrTotalCount);
        }
    }
}