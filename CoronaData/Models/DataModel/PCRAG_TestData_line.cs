using System;
namespace CoronaData.Models.DataModel
{
    public class PCRAG_TestData_line
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
        /// Count of positive PCR + AG tests in District.
        /// </summary>
        public int PcrAgPosCount { get; set; }

        /// <summary>
        /// Count of total PCR + AG tests in District.
        /// </summary>
        public int PcrAgTotalCount { get; set; }
    }
}
