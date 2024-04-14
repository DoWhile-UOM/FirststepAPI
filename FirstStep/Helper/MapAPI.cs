using Newtonsoft.Json;
using System.Net;

namespace FirstStep.Helper
{
    public struct Coordinate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public static class MapAPI
    {
        private static HttpClient? client = null;

        public static async Task<float> GetDistance(string city1, string city2)
        {
            // Get the coordinates of the two cities
            var city1Coordinates = await GetCoordinates(city1.ToLower());
            var city2Coordinates = await GetCoordinates(city2.ToLower());

            // Calculate the distance between the two cities using the Haversine formula
            double distance = CalculateDistance(city1Coordinates, city2Coordinates);

            return (float)distance;
        }

        public static async Task<float> GetDistance(string city, Coordinate reqCityCoordinate)
        {
            var cityCoordinates = await GetCoordinates(city.ToLower());

            double distance = CalculateDistance(cityCoordinates, reqCityCoordinate);

            return (float)distance;
        }

        public static async Task<Coordinate> GetCoordinates(string cityName)
        {
            // URL for the Nominatim API
            string url = $"https://nominatim.openstreetmap.org/search?q={Uri.EscapeDataString(cityName)}&format=json&limit=1";

            // Create a new instance of HttpClient if not already initialized
            if (client == null)
            {
                HttpClientHandler handler = new HttpClientHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                };
                client = new HttpClient(handler);

                // Set appropriate headers (User-Agent is important for Nominatim API)
                client.DefaultRequestHeaders.Add("User-Agent", "FirstStep");
            }

            // Send a request to the API
            HttpResponseMessage response = await client.GetAsync(url);

            // Check for errors in the response status code
            if (!response.IsSuccessStatusCode)
            {
                // Handle different error status codes
                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    throw new Exception("Forbidden: You may have exceeded the rate limit or the request was blocked.");
                }

                // Throw an exception for other non-success status codes
                throw new Exception($"API request failed with status code {response.StatusCode}");
            }

            // Parse the response
            string jsonResponse = await response.Content.ReadAsStringAsync();
            dynamic? data = JsonConvert.DeserializeObject(jsonResponse);

            if (data == null)
            {
                throw new Exception("City not found or invalid response from API.");
            }

            // Ensure the data is not null and contains at least one result
            if (data.Count == 0)
            {
                throw new Exception("City not found or invalid response from API.");
            }

            return new Coordinate
            { 
                Latitude = data[0].lat, 
                Longitude = data[0].lon 
            };
        }

        // Method to calculate distance between two coordinates using the Haversine formula
        private static double CalculateDistance(Coordinate coord1, Coordinate coord2)
        {
            double R = 6378.137; // Radius of the Earth in km
            double dLat = ToRadians(coord2.Latitude - coord1.Latitude);
            double dLon = ToRadians(coord2.Longitude - coord1.Longitude);
            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(coord1.Latitude)) * Math.Cos(ToRadians(coord2.Latitude)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = R * c;

            // convert distance to nearest 2nd decimal place
            distance = Math.Round(distance, 2);

            return distance;
        }

        // Method to convert degrees to radians
        private static double ToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}
