using System;
namespace CoronaData.Models.DataModel
{
    /// <summary>
    /// Class representing single line of PCR data report.
    /// </summary>
    public class PCR_TestData_line
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
        /// District.
        /// </summary>
        public string DistrictCode { get; set; }

        /// <summary>
        /// Count of negative PCR tests in District.
        /// </summary>
        public int PcrNegCount { get; set; }

        /// <summary>
        /// Count of positive PCR tests in District.
        /// </summary>
        public int PcrPosCount { get; set; }

        /// <summary>
        /// Count of total PCR tests in District.
        /// </summary>
        public int PcrTotalCount { get; set; }
    }
}
