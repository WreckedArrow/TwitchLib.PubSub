using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace TwitchLib.PubSub.Common
{
    /// <summary>
    /// Static class of helper functions used around the project.
    /// </summary>
    public static class Helpers
    {
        /// <summary>
        /// Takes date time string received from Twitch API and converts it to DateTime object.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>DateTime.</returns>
        public static DateTime DateTimeStringToObject(string dateTime)
        {
            return dateTime == null ? new DateTime() : Convert.ToDateTime(dateTime);
        }


        /// <summary>
        /// Base64s the encode.
        /// </summary>
        /// <param name="plainText">The plain text.</param>
        /// <returns>System.String.</returns>
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        public static T DeserializeFromJson<T>(string text)
        {
            string encodedObject = text.Replace("\\", "");
            var dataEncoded = Helpers.ParseJson(encodedObject).ToString();
            return JsonConvert.DeserializeObject<T>(dataEncoded);
        }

        public static T DeserializeFromJson<T>(JObject obj)
        {
            var dataEncoded = obj.ToString();
            return JsonConvert.DeserializeObject<T>(dataEncoded);
        }

        public static T ToEnum<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>($"\"{value}\"", new StringEnumConverter());
        }

        public static JObject ParseJson(string json, int maxRetries = 5)
        {
            int attempts = 0;
            while (attempts < maxRetries)
            {
                try
                {
                    var obj = JObject.Parse(json);
                    return obj;
                }
                catch (Newtonsoft.Json.JsonReaderException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    // For some reason (micro optimization?) the Json string from Twitch won't necessarily have enough closing brackets.
                    // We'll try to add them as needed a few times.
                    json += '}';
                    attempts++;
                    if (attempts == maxRetries)
                    {
                        throw ex;
                    }
                }
            }
            // Never hitting this....
            throw new Exception($"Unable to parse json: {json}");
        }
    }
}