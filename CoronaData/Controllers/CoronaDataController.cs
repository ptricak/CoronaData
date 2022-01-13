using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using CoronaData.Mappers;
using CoronaData.Models;
using CoronaData.Models.DataModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TinyCsvParser;

namespace CoronaData.Controllers
{
    public class CoronaDataController : Controller
    {
        private readonly ILogger<CoronaDataController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Remote data sources
        private const string AG_openData_source = "https://raw.githubusercontent.com/Institut-Zdravotnych-Analyz/covid19-data/main/AG_Tests/OpenData_Slovakia_Covid_AgTests_District.csv";
        private const string PCR_openData_source = "https://raw.githubusercontent.com/Institut-Zdravotnych-Analyz/covid19-data/main/PCR_Tests/OpenData_Slovakia_Covid_PCRTests_District.csv";
        private const string CovidAutomat_openData_source = "https://raw.githubusercontent.com/Institut-Zdravotnych-Analyz/covid19-data/main/OpenData_Slovakia_CovidAutomat.csv";

        //Helper constats       
        private const string ML_district = "Okres Medzilaborce";
        private const string SN_district = "Okres Spišská Nová Ves";
        private const string SP_district = "Okres Stropkov";
        private const string SK_district = "Okres Svidník";
        private const string HE_district = "Okres Humenné";
        private const string SV_district = "Okres Snina";

        private const int ML_district_Code = 63;
        private const int SN_district_Code = 21;
        private const int SP_district_Code = 38;
        private const int SK_district_Code = 53;
        private const int HE_district_Code = 9;
        private const int SV_district_Code = 50;

        private const string default_district = ML_district;
        private const int default_district_Code = ML_district_Code;

        public CoronaDataController(ILogger<CoronaDataController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult Index()
        {
            return View(GetDataForSelectedDistrict(default_district, default_district_Code));
        }

        public IActionResult Medzilaborce()
        {
            return View("Index", GetDataForSelectedDistrict(ML_district, ML_district_Code));
        }

        public IActionResult SpisskaNovaVes()
        {
            return View("Index", GetDataForSelectedDistrict(SN_district, SN_district_Code));
        }

        public IActionResult Stropkov()
        {
            return View("Index", GetDataForSelectedDistrict(SP_district, SP_district_Code));
        }

        public IActionResult Svidnik()
        {
            return View("Index", GetDataForSelectedDistrict(SK_district, SK_district_Code));
        }

        public IActionResult Humenne()
        {
            return View("Index", GetDataForSelectedDistrict(HE_district, HE_district_Code));
        }

        public IActionResult Snina()
        {
            return View("Index", GetDataForSelectedDistrict(SV_district, SV_district_Code));
        }

        #region Helpers
        private DataVisualisationModel GetDataForSelectedDistrict(string selectedDistrict, int selectedDistrictCode)
        {
            #region Download AG and PCR data files if necessary
            CheckAndPrepareDataFiles();
            #endregion Download AG and PCR data files if necessary

            IEnumerable<AG_TestData_line> selectedDistrictOnlyAgData = null;
            IEnumerable<PCR_TestData_line> selectedDistrictOnlyPcrData = null;
            CovidAutomat_TestData_line selectedDistrictOnlyCovidAutomatData = new CovidAutomat_TestData_line { Date = DateTime.Now, Vaccination_Full = "0,00%", SevenDaysIncidencyActual = 0, SevenDaysIncidencyBefore = 0, DistrictPopulation = 0 };

            //initialize CSV parser
            CsvParserOptions csvParserOptions = new CsvParserOptions(true, ';');

            //fetch data for AG report
            #region AG data processing
            var agCsvParser = new CsvParser<AG_TestData_line>(csvParserOptions, new CsvAgCoronaDataMapping());
            var allAgRecords = agCsvParser.ReadFromFile(GetAgLocalFilePath(), Encoding.UTF8).ToList();
            #endregion AG data processing

            //fetch data for PCR report
            #region PCR data processing
            var pcrCsvParser = new CsvParser<PCR_TestData_line>(csvParserOptions, new CsvPcrCoronaDataMapping());
            var allPcrRecords = pcrCsvParser.ReadFromFile(GetPcrLocalFilePath(), Encoding.UTF8).ToList();
            #endregion PCR data processing

            //fetch data for CovidAutomat report
            #region CovidAutomat data processing
            var covidAutomatCsvParser = new CsvParser<CovidAutomat_TestData_line>(new CsvParserOptions(true, ','), new CsvCovidAutomatCoronaDataMapping());
            var allCovidAutomatRecords = covidAutomatCsvParser.ReadFromFile(GetCovidAutomatLocalFilePath(), Encoding.UTF8).ToList();
            #endregion CovidAutomat data processing

            #region Validation and filtration
            if (allAgRecords != null && allAgRecords.Count() > 0 && allPcrRecords != null && allPcrRecords.Count() > 0)
            {
                selectedDistrictOnlyAgData = allAgRecords.Where(d => d.Result.District == selectedDistrict).Select(d => d.Result).OrderByDescending(d => d.Date).Take(80).ToList();
                selectedDistrictOnlyPcrData = allPcrRecords.Where(d => d.Result.District == selectedDistrict).Select(d => d.Result).OrderByDescending(d => d.Date).Take(80).ToList();
            }
            else return new DataVisualisationModel();

            var last14DaysAGData = selectedDistrictOnlyAgData.Take(14);
            var last14DaysPCRData = selectedDistrictOnlyPcrData.Take(14);
            #endregion Validation and filtration

            #region CovidAutomat data validation and filtration
            if (allCovidAutomatRecords != null && allCovidAutomatRecords.Count() > 0)
            {
                selectedDistrictOnlyCovidAutomatData = allCovidAutomatRecords.Where(d => d.Result.DistrictCode == selectedDistrictCode).Select(d => d.Result).OrderByDescending(d => d.Date).FirstOrDefault();
            }
            #endregion CovidAutomat data validation and filtration

            var last14DaysAG_ordered = last14DaysAGData.OrderBy(d => d.Date);
            var last14DaysPCR_ordered = last14DaysPCRData.OrderBy(d => d.Date);

            return new DataVisualisationModel
            {
                Last14DaysAG = last14DaysAG_ordered,
                Last14DaysPCR = last14DaysPCR_ordered,
                Last14DaysPCRAG = PrepareCombinedPcrAndAgLast14DaysData(last14DaysAG_ordered, last14DaysPCR_ordered),
                LastDayAGData = PrepareLastDayAgData(selectedDistrictOnlyAgData.First()),
                LastDayPCRData = PrepareLastDayPcrData(selectedDistrictOnlyPcrData.First()),
                AllDataAG = selectedDistrictOnlyAgData,
                AllDataPCR = selectedDistrictOnlyPcrData,
                SelectedDistrict = selectedDistrict,
                LastCovidAutomatData = selectedDistrictOnlyCovidAutomatData != null ? selectedDistrictOnlyCovidAutomatData : new CovidAutomat_TestData_line { Date = DateTime.Now, Vaccination_Full = "0,00%", SevenDaysIncidencyActual = 0, SevenDaysIncidencyBefore = 0, DistrictPopulation = 0 }
            };
        }

        private void CheckAndPrepareDataFiles()
        {
            //local file sources
            FileInfo agSourceFile = new FileInfo(GetAgLocalFilePath());
            FileInfo pcrSourceFile = new FileInfo(GetPcrLocalFilePath());
            FileInfo covidAutomatSourceFile = new FileInfo(GetCovidAutomatLocalFilePath());

            #region AG data
            //if data exists, check how old they are
            if (agSourceFile.Exists)
            {
                var agDataRequest = (HttpWebRequest)WebRequest.Create(AG_openData_source);
                agDataRequest.Method = "HEAD";
                var agDataResponse = (HttpWebResponse)agDataRequest.GetResponse();

                //only if data are old, download new one
                if (agDataResponse.ContentLength != agSourceFile.Length)
                {
                    // create another request to download the whole file
                    DownloadAGData();
                }
            }
            else
            {
                //download data because they not exists
                DownloadAGData();
            }
            #endregion AG data

            #region PCR data
            //if data exists, check how old they are
            if (pcrSourceFile.Exists)
            {
                var pcrDataRequest = (HttpWebRequest)WebRequest.Create(PCR_openData_source);
                pcrDataRequest.Method = "HEAD";
                var pcrDataResponse = (HttpWebResponse)pcrDataRequest.GetResponse();

                //only if data are old, download new one
                if (pcrDataResponse.ContentLength != pcrSourceFile.Length)
                {
                    // create another request to download the whole file
                    DownloadPCRData();
                }
            }
            else
            {
                //download data because they not exists
                DownloadPCRData();
            }
            #endregion PCR data

            #region CovidAutomat data
            //if data exists, check how old they are
            if (covidAutomatSourceFile.Exists)
            {
                //if data not from today, check it
                if (covidAutomatSourceFile.LastWriteTime.Date != DateTime.Now.Date)
                {
                    var covidAutomatDataRequest = (HttpWebRequest)WebRequest.Create(CovidAutomat_openData_source);
                    covidAutomatDataRequest.Method = "HEAD";
                    var covidAutomatDataResponse = (HttpWebResponse)covidAutomatDataRequest.GetResponse();

                    //only if data are old, download new one
                    if (covidAutomatDataResponse.ContentLength != covidAutomatSourceFile.Length)
                    {
                        // create another request to download the whole file
                        DownloadCovidAutomatData();
                    }
                }
            }
            else
            {
                //download data because they not exists
                DownloadCovidAutomatData();
            }
            #endregion CovidAutomat data
        }

        private void DownloadAGData()
        {
            using var client = new WebClient();
            client.DownloadFile(AG_openData_source, GetAgLocalFilePath());
        }

        private void DownloadPCRData()
        {
            using var client = new WebClient();
            client.DownloadFile(PCR_openData_source, GetPcrLocalFilePath());
        }

        private void DownloadCovidAutomatData()
        {
            using var client = new WebClient();
            client.DownloadFile(CovidAutomat_openData_source, GetCovidAutomatLocalFilePath());
        }

        private string GetAgLocalFilePath()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            return Path.Combine(webRootPath, "DataFiles/agTestData.csv");
        }

        private string GetPcrLocalFilePath()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            return Path.Combine(webRootPath, "DataFiles/pcrTestData.csv");
        }

        private string GetCovidAutomatLocalFilePath()
        {
            string webRootPath = _webHostEnvironment.WebRootPath;

            return Path.Combine(webRootPath, "DataFiles/covidAutomatTestData.csv");
        }

        private IEnumerable<PCRAG_TestData_line> PrepareCombinedPcrAndAgLast14DaysData(IEnumerable<AG_TestData_line> agData, IEnumerable<PCR_TestData_line> pcrData)
        {
            var listToReturn = new List<PCRAG_TestData_line>();

            foreach (var agDataLine in agData)
            {
                var pcrDataLine = pcrData.FirstOrDefault(d => d.DateString == agDataLine.DateString);

                //add element with aggregated data to list
                listToReturn.Add(new PCRAG_TestData_line
                {
                    Date = agDataLine.Date,
                    PcrAgPosCount = pcrDataLine != null ? pcrDataLine.PcrPosCount + agDataLine.AgPosCount : agDataLine.AgPosCount,
                    PcrAgTotalCount = pcrDataLine != null ? pcrDataLine.PcrTotalCount + agDataLine.AgTotalCount : agDataLine.AgTotalCount
                });
            }

            return listToReturn;
        }

        private LastDayData PrepareLastDayAgData(AG_TestData_line data)
        {
            return new LastDayData
            {
                Date = data.Date,
                NegCount = data.AgNegCount,
                PosCount = data.AgPosCount,
                TotalCount = data.AgTotalCount
            };
        }

        private LastDayData PrepareLastDayPcrData(PCR_TestData_line data)
        {
            return new LastDayData
            {
                Date = data.Date,
                NegCount = data.PcrNegCount,
                PosCount = data.PcrPosCount,
                TotalCount = data.PcrTotalCount
            };
        }
        #endregion Helpers
    }
}
