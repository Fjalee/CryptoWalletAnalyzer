# CryptoWalletAnalyzer
Software to filter crypto wallets which are obtianed by web scraping block explorers' dex tables. This scraper can currently scrape:
* etherscan.com

## How to use
### Start/Exit
* Start the scraper by starting the executable file in a terminal.
* Exit the scraper by closing the terminal.

### Output
* Output folder: Crypto-Analyzer-Output.
* Output file: name example SUSHI_2021_06_10_1306.xlsx.
    * File name contains token name, date and time of the beginning of the scrape.
    * Output files should be opened with google sheets. When opened with excel some functions inside the files will not work.
### OS
* Version for 64-bit Windows: windows_x64_executable/CryptoWalletAnalyzer.exe
* Version for 64-bit Linux: linux_x64_executable/CryptoWalletAnalyzer
   
Some softwares that opens xlsx files may lock output's file from being changed by the scraper. If this occurs while scraping the scraper will continue scraping, however the new data will only be added to the file only after it is closed.

### Error handling
If something unexpected happens, the scraper will stop and produce a .log file that will log the html page that the scraper stopped on.

### appsettings.json