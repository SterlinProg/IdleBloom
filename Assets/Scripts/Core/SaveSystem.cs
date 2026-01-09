using UnityEngine;

namespace Core
{
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    
    public static class SaveSystem
    {
        public static void SavePlayer()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            string path = Application.persistentDataPath + $"{Path.DirectorySeparatorChar}player.sav";
            FileStream stream = new FileStream(path, FileMode.Create);

            PlayerData data = new PlayerData();
            
            formatter.Serialize(stream,data);
            stream.Close();
        }

        public static PlayerData LoadPlayer()
        {
            string path =  Application.persistentDataPath + $"{Path.DirectorySeparatorChar}player.sav";
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);
                
                PlayerData data = formatter.Deserialize(stream) as PlayerData;
                
                stream.Close();
                return data;
            }
            else
            {
                return null;
            }
        }

        public static void ClearData()
        {
            string path =  Application.persistentDataPath + $"{Path.DirectorySeparatorChar}player.sav";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}