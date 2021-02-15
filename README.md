# Search.API

A small web-based application to perform search by keywords in "well known" search engine and analises position of the website you are intersted in. The application prompts for a string of keywords to search, and a URL to find in the search results. The input values are then processed to return a string of numbers for where the URL is found in the search engine’s results.
For example, "1, 10, 33" or "0".
It analises URLs appeared in the first 50 results.

Special requirements are:
  * **No 3rd party libraries for scraping the HTML pages** otherwise I would use google/bing api or Html Agility Pack to parse html
  * Ajax calls
  * DI
  * Logging 
  * Unit tests
  * SOLID principles
  * Uses httpClientFactory to instantiate httpclient
