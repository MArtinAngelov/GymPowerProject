using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GymPower.Helpers
{
    public static class SessionExtensions
    {
        // Save any object into session as JSON
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            session.SetString(key, json);
        }

        // Retrieve object from session
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default : JsonConvert.DeserializeObject<T>(json);
        }
    }
}