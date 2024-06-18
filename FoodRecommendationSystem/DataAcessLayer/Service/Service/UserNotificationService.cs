using DataAcessLayer.Service.IService;

namespace DataAcessLayer.Service.Service
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IRepository<UserNotification> _userNotificationRepository;

        public UserNotificationService(IRepository<UserNotification> userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
        }

        public void AddUserNotification(UserNotificationDTO userNotificationDTO)
        {
            try
            {
                UserNotification userNotification = (UserNotification)userNotificationDTO;
                _userNotificationRepository.Insert(userNotification);
                _userNotificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error inserting user notification", ex);
            }
        }

        public List<UserNotificationDTO> GetAllUserNotifications()
        {
            try
            {
                var userNotifications = _userNotificationRepository.GetAll();
                return userNotifications.Select(userNotification => (UserNotificationDTO)userNotification).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all user notifications", ex);
            }
        }

        public List<UserNotificationDTO> GetUsersUnreadNotification(int userId)
        {
            try
            {
                var userNotifications = _userNotificationRepository.GetAll().Where(x => x.UserId == userId && !x.IsRead);
                return userNotifications.Select(userNotification => (UserNotificationDTO)userNotification).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting all user notifications", ex);
            }
        }

        public void MarkNotificationAsRead(int userId)
        {
            try
            {
                var userNotifications = _userNotificationRepository.GetAll().Where(x => x.UserId == userId && !x.IsRead);

                foreach (var userNotification in userNotifications)
                {
                    userNotification.IsRead = true;

                    _userNotificationRepository.Update(userNotification);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Not able to  mark the notification as read");
            }
        }

        public UserNotificationDTO GetUserNotification(int id)
        {
            try
            {
                var userNotification = _userNotificationRepository.GetById(id);
                return (UserNotificationDTO)userNotification;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting the user notification {id}", ex);
            }
        }

        public void UpdateUserNotification(UserNotificationDTO userNotificationDTO)
        {
            try
            {
                var userNotification = (UserNotification)userNotificationDTO;
                _userNotificationRepository.Update(userNotification);
                _userNotificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception("Error updating the user notification", ex);
            }
        }

        public void DeleteUserNotification(int id)
        {
            try
            {
                _userNotificationRepository.Delete(id);
                _userNotificationRepository.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting the summary rating {id}", ex);
            }
        }
    }
}