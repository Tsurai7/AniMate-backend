using Animate_backend.Models.Entities;

namespace Animate_backend.Repositories;

public class UserRepository
{
    public static List<User> users = new List<User>()
    {
        new User("Ken Kaneki", "test@test.com", "123", "https://i.pinimg.com/originals/e6/94/e9/e694e991d7ec9af6330657eb6ee479f5.jpg",new List<string>() {"kizumonogatari-iii-reiketsu-hen", "anatsu-no-taizai-kamigami-no-gekirin"},
        new List<string>() {"kizumonogatari-iii-reiketsu-hen"})
    };
}