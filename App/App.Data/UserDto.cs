using System.Text.Json.Serialization;

namespace App.Data
{
    internal class UserDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("first")]
        public string FirstName { get; set; } = string.Empty;

        [JsonPropertyName("last")]
        public string LastName { get; set; } = string.Empty;

        [JsonPropertyName("gender")]
        public string Gender { get; set; } = string.Empty;
    }
}

