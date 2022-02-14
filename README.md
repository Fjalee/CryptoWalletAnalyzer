# CryptoWalletAnalyzer
Software to filter crypto wallets which are obtained by web scraping block explorers' dex tables. This scraper can currently scrape:
* etherscan.com

## Setup
### API Key
* Before starting you have to go to local-appsettings.json file and add ApiKeys to block explorers in the "ApiKey": "" line.
    example: "ApiKey": "2ZQJHDLKWS4AUM8G8HH5EH737HCK4W5HIO"
* To get this key for:
    * Etherscan:
        * go to https://etherscan.io/login to log in / sign up.
        * go to https://etherscan.io/myapikey and add new API key (or use already existing one).
        * api key example: "2ZQJHDLKWS4AUM8G8HH5EH737HCK4W5HIO"

## How to use
### Select tokens to scrape
* Etherscan:
    * go to token detail page (example https://etherscan.io/token/0x6b3595068778dd592e39a122f4f5a5cf09c90fe2)
    * copy the token hash (example 0x6b3595068778dd592e39a122f4f5a5cf09c90fe2)
    * go to local-appsettings.json file and add new object in TokensToScrape array (there are few examples already in the file)
        example:
            {
                "Hash": "0xB8c77482e45F1F44dE1745F52C74426C631bDD52",
                "RowsAmount": "300"
            }

### Start/Exit
* Start the scraper by starting the executable file in a terminal. (winKey + R, copy-paste WalletAnalyzer.exe).
* Exit the scraper by closing the terminal.

### Output
* Output folder: Crypto-Analyzer-Output.
* Output file: name example SUSHI_2021_06_10_1306.xlsx.
    * File name contains token name, date and time of the beginning of the scrape.
    * Output files should be opened with google sheets. When opened with excel some functions inside the files will not work.

### FIY
Some softwares that opens xlsx files may lock output's file from being changed by the scraper. If this occurs while scraping the scraper will continue scraping, however the new data will only be added to the file only after it is closed.

### Error handling
If something unexpected happens, the scraper will stop and produce a .log file that will log the html page that the scraper stopped on.

### OS
* Version for 64-bit Windows: windows_x64_executable/CryptoWalletAnalyzer.exe