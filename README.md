# XPathMonitor
### AWS services
Web App to monitor product prices for a specific online shop (history price evolution).

## Components
## Web Front-End App
- ReactJs app 
- docker container deployed to EC2 instance; 

## Back-End API
- C# ASP.NET Core APIs;
- docker container deployed to EC2 instance;

## Database
- AWS RDS (sql);

## AWS lambda:
- NodeJs;
- gets triggered at every 24h by EventBridge, use C# APIs to extract existing product links from the DB, extract their current price and push into into the history of the current product;

![image](https://github.com/GabrielDamian/XPathMonitor/assets/76115929/19101240-eeb8-408b-901a-504e16f4272e)
