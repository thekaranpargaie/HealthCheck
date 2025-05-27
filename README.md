# HealthCheck

A Blazor-based Health Dashboard for monitoring the status of multiple services in real time. This dashboard visualizes health check results, provides historical status pills, and auto-refreshes data at regular intervals.

---

## Overview

**HealthCheck.Ui** is a Blazor (.NET 8) web application that displays the health status of various backend services. It fetches health check data from a REST API and presents it in a user-friendly dashboard with real-time updates and historical context.

### Key Features

- **Live Health Status:** View the current health of all registered services.
- **History Pills:** Visualize recent health history for each service.
- **Auto-Refresh:** Data is refreshed automatically at a configurable interval.
- **Error Handling:** User-friendly error messages if the API is unreachable.

---

## Project Structure

- `HealthCheck.Ui/Components/Pages/Home.razor`  
  Main dashboard page and logic.
- `HealthCheck.Main.Models/HealthCheckPingResult.cs`  
  Model for health check results.
- `appsettings.json`  
  (If present) Used for configuration such as API endpoints.

---

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) or VS Code
- A running HealthCheck API (default: `http://healthcheck-api:5001/api/healthchecks`)

---

## Setup Instructions

1. **Clone the Repository**
git clone <your-repo-url> cd HealthCheck.Ui


2. **Configure the API Endpoint**
By default, the API endpoint is set in `Home.razor`:
var http = new HttpClient { BaseAddress = new Uri("http://healthcheck-api:5001/") };

If you want to make this configurable:
   - Add an entry in `appsettings.json` (if not present, create one in the project root):
     ```json
     {
       "HealthCheckApi": {
         "BaseUrl": "http://healthcheck-api:5001/"
       }
     }
     ```
   - Inject `IConfiguration` in your component and use it to read the value.

3. **Restore Dependencies**
dotnet restore

4. **Run the Application**
dotnet run --project HealthCheck.Ui

Or open the solution in Visual Studio and press `F5`.

5. **Access the Dashboard**
   - Open your browser and navigate to `http://localhost:5000` (or the port shown in the console).

---

## How to Use

- The dashboard will automatically load and display the health status of all services.
- Each card shows the latest status, last checked time, and a short history.
- The dashboard refreshes every 5 minutes by default.
- If the API is unreachable, an error message will be displayed.

---

## Customization

### Change API Endpoint

- Update the `BaseAddress` in `Home.razor` or use `appsettings.json` as described above.

### Adjust Refresh Interval

- In `Home.razor`, modify the timer interval: timer = new System.Threading.Timer(..., TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(5));


### UI Styling

- Modify the CSS in `Home.razor` for custom look and feel.

---

## Example `appsettings.json`
{ "HealthCheckApi": { "BaseUrl": "http://healthcheck-api:5001/" } }


---

## Troubleshooting

- **API Not Reachable:** Ensure the HealthCheck API is running and accessible from the Blazor app.
- **CORS Issues:** If running the API and UI on different hosts/ports, ensure CORS is enabled on the API.

---

## License

This project is licensed under the MIT License.
