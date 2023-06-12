using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WinForms.Model;

namespace WinForms.Controllers
{
    interface IConfigController<T>
    {
        T LoadConfig();
        void SaveConfig(T config);
    }
    public class ConfigController<T> : IConfigController<T>
    {
        private string _configName;

        public ConfigController(string configName)
        {
            _configName = configName;
        }

        public T LoadConfig()
        {
            try
            {
                if (File.Exists(_configName))
                {
                    var json = File.ReadAllText(_configName);
                    var config = JsonConvert.DeserializeObject<T>(json);
                    return config;
                }
                else
                    return default(T);
            }
            catch (SerializationException)
            {
                File.Delete(_configName); // jak spiepszony config to wywalic go :P
                throw;
            }
          
        }

        public void SaveConfig(T config)
        {
            if (config == null)
                throw new ArgumentNullException();

            var json = JsonConvert.SerializeObject(config);
            File.WriteAllText(_configName, json);
        }
    }
}
