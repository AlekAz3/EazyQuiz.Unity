using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using EazyQuiz.Models.DTO;
using UnityEngine;

namespace EazyQuiz.Unity.Services
{
    /// <summary>
    /// Сохранение пользователя в залогиненым
    /// </summary>
    public class SaveUserService
    {
       public void SaveUser(UserResponse userResponse)
        {
            Debug.Log("Сохранение пользователя");
            var binaryFormatter = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/Saveuser.save", FileMode.OpenOrCreate);
            binaryFormatter.Serialize(file, userResponse);
            file.Close();
        }

        public UserResponse LoadUser()
        {
            if (!File.Exists(Application.persistentDataPath + "/Saveuser.save")) return null;
            
            Debug.Log("Загрузка пользователя");
            var binaryFormatter = new BinaryFormatter();
            var file = File.Open(Application.persistentDataPath + "/Saveuser.save", FileMode.Open);
            var userResponse = (UserResponse)binaryFormatter.Deserialize(file);
            file.Close();
            return userResponse;
        }

        public void DeleteUser()
        {
            File.Delete(Application.persistentDataPath + "/Saveuser.save");
        }
    }
}