using EazyQuiz.Models.DTO;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file;
            file = File.Open(Application.persistentDataPath + "/Saveuser.save", FileMode.OpenOrCreate);
            binaryFormatter.Serialize(file, userResponse);
            file.Close();
        }

        public UserResponse LoadUser()
        {
            if (File.Exists(Application.persistentDataPath + "/Saveuser.save"))
            {
                Debug.Log("Загрузка пользователя");
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/Saveuser.save", FileMode.Open);
                UserResponse userResponse = (UserResponse)binaryFormatter.Deserialize(file);
                file.Close();
                return userResponse;
            }
            return null;
        }

        public void DeleteUser()
        {
            File.Delete(Application.persistentDataPath + "/Saveuser.save");
        }
    }
}