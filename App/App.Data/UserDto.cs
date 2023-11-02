namespace App.Data
{
    internal class UserDto
    {
        public int Id { get; set; }

        public int Age { get; set; }

        public string FirstName { get; set; } = string.Empty;
        
        public string LastName { get; set; } = string.Empty;
        
        public string Gender { get; set; } = string.Empty;
    }
}

