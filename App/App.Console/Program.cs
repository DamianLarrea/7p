using App.Console.Extensions;
using App.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;

namespace App.Console
{
    internal class Program
    {
        private readonly UserService _userService;

        public Program(UserService userService)
        {
            _userService = userService;
        }

        private async Task Run()
        {
            try
            {
                await FetchAndLogUserWithId(42);

                await FetchAndLogUsersOfAge(23);

                await FetchAndLogUserCountsByAgeAndGender();
            }
            catch (JsonException e)
            {
                System.Console.WriteLine("Bad JSON received from external API when loading users.\n" + e.Message);
            }
            catch (HttpRequestException e)
            {
                System.Console.WriteLine("Something went wrong with reaching the external API.\n" + e.Message);
            }
            catch (TaskCanceledException e)
            {
                System.Console.WriteLine("Something went wrong with reaching the external API.\n" + e.Message);
            }
        }

        private async Task FetchAndLogUserWithId(int id)
        {
            var user = await _userService.GetUser(id);

            if(user is not null) System.Console.WriteLine($"{user.FirstName} {user.LastName}");
        }

        private async Task FetchAndLogUsersOfAge(int age)
        {
            var users = await _userService.GetUsersForAge(age);

            var userFirstNames = users.Select(u => u.FirstName);

            if (userFirstNames.Any())
                System.Console.WriteLine(string.Join(',', userFirstNames));
        }

        private async Task FetchAndLogUserCountsByAgeAndGender()
        {
            var usersByAge = await _userService.GetUsersByAge();

            var userCountsByAgeAndGender = usersByAge.Select(kvp => {
                var counts = GetCountsByGender(kvp.Value);
                return (Age: kvp.Key, counts.Femalecount, counts.MaleCount);
            });

            foreach(var userCount in userCountsByAgeAndGender)
                System.Console.WriteLine($"Age: {userCount.Age} Female: {userCount.Femalecount} Male: {userCount.MaleCount}");
        }

        private static (int Femalecount, int MaleCount) GetCountsByGender(IEnumerable<User> users)
        {
            return users.Aggregate(
                    (femaleCount: 0, maleCount: 0),
                    (acc, user) =>
                    {
                        if (user.IsFemale)
                            acc.femaleCount++;
                        
                        else if (user.IsMale)
                            acc.maleCount++;

                        return acc;
                    }
                );
        }

        static async Task Main(string[] args)
        {
            var host = Host.CreateApplicationBuilder()
                .ConfigureOptions()
                .RegisterServices()
                .Build();

            using var scope = host.Services.CreateScope();

            var program = scope.ServiceProvider.GetService<Program>();

            await program!.Run();
        }
    }
}