using System.IO;
using ConfigGenerator.Core;

namespace ConfigGenerator.Services.ConfigurationGenerators
{
    public abstract class ConfigurationGenerator : IConfigurationGenerator
    {
        public ConfigurationType ConfigurationType { get; set; }

        public virtual void Generate(ConfigModel configModel)
        {
            if (!IsValidConfiguration())
            {
                return;
            }

            configModel.ConfigFolder = GetConfigurationFolder();

            var path = string.Format(@"{0}\{1}\{2}", 
                Utility.GetConfigurationFolder(), 
                configModel.SystemName,
                configModel.ConfigFolder);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var configName = path + string.Format(@"\{0}.{1}.{2}.{3}",
                GetConfigurationName(), 
                configModel.Licensee,
                configModel.EnvironmentName, 
                GetExtenstionName());

            var result = GetTransformResult(configModel);
            
            File.WriteAllText(configName, result);
        }

        protected virtual string GetTransformResult(ConfigModel configModel)
        {
            return "Output here";
        }

        protected abstract bool IsValidConfiguration();

        protected abstract string GetConfigurationFolder();
        
        protected abstract string GetConfigurationName();

        protected virtual string GetExtenstionName()
        {
            return "config";
        }
    }

    
}