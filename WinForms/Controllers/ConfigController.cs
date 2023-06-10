using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinForms.Model;

namespace WinForms.Controllers
{
    interface IConfigController
    {
        Config LoadConfig();
        void SaveConfig (Config config);
    }
    public class ConfigController : IConfigController
    {
        private string _configName;

        public ConfigController(string configName)
        {
            _configName = configName;
        }

        public Config LoadConfig()
        {
            if (File.Exists(_configName))
            {
                var json = File.ReadAllText(_configName);
                var config = JsonConvert.DeserializeObject<Config>(json);
                return config;
            }
            else
                return null;
        }

        public void SaveConfig(Config config)
        {
            if (config == null)
                throw new ArgumentNullException();

            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(_configName, json);
        }
    }
}
