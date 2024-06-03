using Animate_backend.Models.Entities;

namespace Animate_backend.Repositories;

public class UserRepository
{
    public static List<User> users = new List<User>()
    {
        new User("Ken Kaneki", "test@test.com", "123", "https://i.pinimg.com/originals/e6/94/e9/e694e991d7ec9af6330657eb6ee479f5.jpg",new List<string>() {"kizumonogatari-iii-reiketsu-hen", "anatsu-no-taizai-kamigami-no-gekirin"},
        new List<string>() {"kizumonogatari-iii-reiketsu-hen"})
    };

    public List<User> GetAllUsers()
        {
            return users;
        }

        public void AddUser(User user)
        {
            users.Add(user);
        }

        public void RemoveUser(long id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
            }
        }

        public void UpdateUser(long id, User updatedUser)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                user.Username = updatedUser.Username;
                user.Email = updatedUser.Email;
                user.ProfileImage = updatedUser.ProfileImage;
                user.PasswordHash = updatedUser.PasswordHash;
                user.WatchedTitles = updatedUser.WatchedTitles;
                user.LikedTitles = updatedUser.LikedTitles;
            }
        }

        
}