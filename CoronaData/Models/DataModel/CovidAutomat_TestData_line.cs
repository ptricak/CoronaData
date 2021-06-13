using System;
namespace CoronaData.Models.DataModel
{
    /// <summary>
    /// Class representing single line of CovidAutomat data.
    /// </summary>
    public class CovidAutomat_TestData_line
    {
        /// <summary>
        /// DateOfTest - time part not important.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Date of test in string format dd.mm.yyyy
        /// </summary>
        public string DateString { get { return Date.ToString("dd.MM.yyyy"); } }

        /// <summary>
        /// District.
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// District code.
        /// </summary>
        public int DistrictCode { get; set; }

        /// <summary>
        /// District population.
        /// </summary>
        public int DistrictPopulation { get; set; }

        /// <summary>
        /// Actual 7 days incidency.
        /// </summary>
        public int SevenDaysIncidencyActual { get; set; }

        /// <summary>
        /// Last 7 days incidency.
        /// </summary>
        public int SevenDaysIncidencyBefore { get; set; }

        /// <summary>
        /// Number of hospitalised people count.
        /// </summary>
        public int Hospitalisation { get; set; }
    }
}
