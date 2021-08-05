using KikClient.Utilities.SimpleJSON;
using System;
using System.IO;

namespace KikClient.Utilities
{
    class Config
    {
        protected string ConfigLocation { get; set; }

        protected JSONNode CurrentConfig { get; set; }

        public Config(string username)
        {
            ConfigLocation = $"{Environment.CurrentDirectory}/AccountData/{username}.json";

            //Load config from the disk, if we can
            if (File.Exists(ConfigLocation))
            {
                CurrentConfig = JSON.Parse(File.ReadAllText(ConfigLocation));
            }
            else
            {
                //Create directory if it doesn't exist
                new FileInfo(ConfigLocation).Directory.Create();

                CurrentConfig = new JSONObject();
            }
        }

        public void SaveString(string name, string value)
        {
            CurrentConfig[name] = value;
            File.WriteAllText(ConfigLocation, JsonHelper.FormatJson(CurrentConfig.ToString()));
        }

        public string GetString(string name)
        {
            return CurrentConfig[name].Value;
        }

        public void SaveBoolean(string name, bool value)
        {
            CurrentConfig[name] = value.ToString();
            File.WriteAllText(ConfigLocation, JsonHelper.FormatJson(CurrentConfig.ToString()));
        }

        public bool GetBoolean(string name)
        {
            return CurrentConfig[name].AsBool;
        }

        public void SaveObject(string name, JSONNode jsonObject)
        {
            CurrentConfig[name] = jsonObject;
            File.WriteAllText(ConfigLocation, JsonHelper.FormatJson(CurrentConfig.ToString()));
        }

        public JSONNode GetObject(string name)
        {
            return CurrentConfig[name].AsObject;
        }
    }
}
