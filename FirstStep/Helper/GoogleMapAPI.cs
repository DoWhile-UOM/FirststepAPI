using Newtonsoft.Json.Linq;

namespace FirstStep.Helper
{
    public class GoogleMapAPI
    {
        public int GetDistance(string origin, string destination)
        {
            // Construct the API request URL
            string url = "http://maps.googleapis.com/maps/api/directions/json?origin=" + origin + "&destination=" + destination + "&sensor=false";
            string content = FileGetContents(url);

            // Parse the JSON response
            JObject o = JObject.Parse(content);
            try
            {
                int distance = (int) o.SelectToken("routes[0].legs[0].distance.value")!;
                return distance; // Distance in meters
            }
            catch
            {
                return 0; // Error handling
            }
        }

        private string FileGetContents(string fileName)
        {
            // Implement your own method to retrieve content from the URL or file
            // For simplicity, I'm using a placeholder here
            return "Sample content from API response";
        }

    }
}
