using AutoMapper;
using CryptoAnalyzer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using WebScraper.WebScrapers;

namespace WalletAnalyzer
{
    public class DexCollector : IDexCollector
    {
        private readonly IDexScraper _scraper;
        private readonly IDexOutput _dexOutput;
        private readonly IMapper _mapper;
        private List<DexRow> _allNewRows = new List<DexRow>();
        private List<DexRow> _allOutputHistory = new List<DexRow>();
        private readonly Stopwatch _stopwatch = new Stopwatch();

        private int _totalRowsScraped = 0;
        private long _msWorthOfDataOutputed = 0;
        private bool _isNeededSaveAsap = false;
        private string _scrapeStartDate;


        public DexCollector(IDexScraper scraper, IDexOutput dexOutput, IMapper mapper)
        {
            _scraper = scraper;
            _dexOutput = dexOutput;
            _mapper = mapper;
        }

        public async Task Start(string url, int sleepTimeMs, int appendPeriodInMs)
        {
            _scrapeStartDate = DateTime.Now.ToString("yyyy_MM_dd_HHmm");
            //Trace.Listeners.Add(new TextWriterTraceListener(ConfigurationManager.AppSettings.Get("LOG_PATH")));
            //Trace.AutoFlush = true;

            _stopwatch.Start();
            while (true)
            {
                var pageRows = await _scraper.ScrapeTable(url);
                _allNewRows.AddRange(pageRows);
                _totalRowsScraped += pageRows.Count;

                Thread.Sleep(sleepTimeMs);

                if (_msWorthOfDataOutputed + appendPeriodInMs < _stopwatch.ElapsedMilliseconds
                    || _isNeededSaveAsap)
                {
                    TryOutput();
                }
            }
        }

        private void TryOutput()
        {
            _allOutputHistory.AddRange(_allNewRows);
            _allNewRows.Clear();

            var ts = _stopwatch.Elapsed;
            var timeOutput = string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

            try
            {
                var output = _mapper.Map<List<DexOutputDto>>(_allOutputHistory);
                _dexOutput.DoOutput(_scrapeStartDate, output, timeOutput, _totalRowsScraped);
                //new CsvOutput().WriteFile(ConfigurationManager.AppSettings.Get("OUTPUT_PATH"), State.ScrapeDate, output, timeOutput, totalRowsScraped);
                _msWorthOfDataOutputed = _stopwatch.ElapsedMilliseconds;

                if (_isNeededSaveAsap)
                {
                    _isNeededSaveAsap = false;
                    Console.WriteLine("Output file closed. Scraped data was added SUCCESSFULLY...");
                }

                Console.WriteLine("Appended data scraped in " + timeOutput);
            }
            catch
            {
                _isNeededSaveAsap = true;
                Console.WriteLine("Scraped data could not be added. Please close the output file...");
            }
        }
    }
}
