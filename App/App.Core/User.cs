﻿namespace App.Core
{
    public record User(
        int Id,
        int Age,
        string FirstName,
        string LastName,
        string Gender
    );
}