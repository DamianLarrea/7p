namespace App.Core
{
    public record User(
        int Id,
        int Age,
        string FirstName,
        string LastName,
        string Gender
    )
    {
        public bool IsMale => Gender.ToUpper() == "M";

        public bool IsFemale => Gender.ToUpper() == "F";
    };
}
