using System;
namespace CoronaData.Models
{
    public class LastDayData
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
        /// Count of negative tests in District.
        /// </summary>
        public int NegCount { get; set; }

        /// <summary>
        /// Count of positive tests in District.
        /// </summary>
        public int PosCount { get; set; }

        /// <summary>
        /// Count of total tests in District.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Positive test percentage.
        /// </summary>
        public int PositivePercentage { get { return TotalCount > 0 ? (int)Math.Round((double)(100 * PosCount) / TotalCount) : 0; } }

        /// <summary>
        /// Positive test percentage as string.
        /// </summary>
        public string PositivePercentageString { get { return (TotalCount > 0 ? (int)Math.Round((double)(100 * PosCount) / TotalCount) : 0) + "%"; } }
    }
}
