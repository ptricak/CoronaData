using System;
using System.Collections.Generic;
using CoronaData.Models.DataModel;

namespace CoronaData.Models
{
    public class DataVisualisationModel
    {
        /// <summary>
        /// Collection of AG test data from last 14 days.
        /// </summary>
        public IEnumerable<AG_TestData_line> Last14DaysAG { get; set; }

        /// <summary>
        /// Collection of PCR test data from last 14 days.
        /// </summary>
        public IEnumerable<PCR_TestData_line> Last14DaysPCR { get; set; }

        /// <summary>
        /// Collection of PCR + AG test data from last 14 days.
        /// </summary>
        public IEnumerable<PCRAG_TestData_line> Last14DaysPCRAG { get; set; }

        /// <summary>
        /// CovidAutomat latest test data.
        /// </summary>
        public CovidAutomat_TestData_line LastCovidAutomatData { get; set; }

        /// <summary>
        /// Last day AG data.
        /// </summary>
        public LastDayData LastDayAGData { get; set; }

        /// <summary>
        /// Last day PCR data.
        /// </summary>
        public LastDayData LastDayPCRData { get; set; }

        /// <summary>
        /// Collection of all AG test data.
        /// </summary>
        public IEnumerable<AG_TestData_line> AllDataAG { get; set; }

        /// <summary>
        /// Collection of all PCR test data.
        /// </summary>
        public IEnumerable<PCR_TestData_line> AllDataPCR { get; set; }

        /// <summary>
        /// Selected district for data.
        /// </summary>
        public string SelectedDistrict { get; set; }
    }
}
