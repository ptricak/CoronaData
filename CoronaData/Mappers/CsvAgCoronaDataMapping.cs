using System;
using TinyCsvParser.Mapping;

namespace CoronaData.Mappers
{
    public class CsvAgCoronaDataMapping : CsvMapping<Models.DataModel.AG_TestData_line>
    {
        public CsvAgCoronaDataMapping() : base()
        {
            MapProperty(0, p => p.Date);
            MapProperty(1, p => p.District, new StringWithoutQuotesTypeConverter());
            MapProperty(2, p => p.AgNegCount);
            MapProperty(3, p => p.AgPosCount);
            MapProperty(4, p => p.AgTotalCount);
        }
    }
}
