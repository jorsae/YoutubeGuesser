using System.Linq;
using YoutubeGuesserApi.Model;

namespace YoutubeGuesserApi.DataAccess
{
    public static class DatabaseInitializer
    {
        public static void Initialize(DatabaseContext context)
        {
            context.Database.EnsureCreated();
            bool hasChanged = false;

            if (!context.YoutubeVideos.Any())
            {
                context.YoutubeVideos.Add(new YoutubeVideo("astISOttCQ0", "The Gummy Bear Song - Long English Version", "icanrockyourworld", 1774287431));
                context.YoutubeVideos.Add(new YoutubeVideo("e1t-OOyePuw", "Will Mr. Bean be back? - BBC", "BBC", 5731165));
                context.YoutubeVideos.Add(new YoutubeVideo("DnpO_RTSNmQ", "Donald Trump: Last Week Tonight with John Oliver (HBO)", "LastWeekTonight", 35867220));
                hasChanged = true;
            }

            if (!context.Guesses.Any())
            {
                context.Guesses.Add(new Guesses());
                hasChanged = true;
            }

            if (!context.UserData.Any())
            {
                context.UserData.Add(new UserData("test", "test", "test", "test", "test", "test", false));
                hasChanged = true;
            }

            if (!context.Profanities.Any())
            {
                context.Profanities.Add(new Profanity("fuck"));
                hasChanged = true;
            }

            if(hasChanged)
                context.SaveChanges();
        }
    }
}