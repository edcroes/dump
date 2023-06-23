using System.Net.Http.Headers;

using var httpClient = new HttpClient(new HttpClientHandler { UseDefaultCredentials = true });

httpClient.BaseAddress = new Uri("http://localhost:5034");
httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Negotiate");

try
{
    var response = await httpClient.GetAsync("/WeatherForecast");
    response.EnsureSuccessStatusCode();

    var forecast = await response.Content.ReadAsStringAsync();
    Console.WriteLine(forecast);
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}