using System;
namespace CoronaData.Models.DataModel
{
    /// <summary>
    /// Class representing single line of AG data report.
    /// </summary>
    public class AG_TestData_line
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
        /// Count of negative AG tests in District.
        /// </summary>
        public int AgNegCount { get; set; }

        /// <summary>
        /// Count of positive AG tests in District.
        /// </summary>
        public int AgPosCount { get; set; }

        /// <summary>
        /// Count of total AG tests in District.
        /// </summary>
        public int AgTotalCount { get; set; }
    }
}
