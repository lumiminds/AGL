using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace AGL.Services.HelperClasses
{
    public static class AppSettingsHelper
    {
        public static object RetrieveConfig(IConfiguration configuration, string sectionName)
        {
            try
            {
                if (configuration.GetSection(sectionName).Exists())
                {
                    return configuration.GetSection(sectionName).Value;
                }
                else
                    throw new Exception("Provided config section not found, please contact your administrator.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static T RetrieveSection<T>(IConfiguration configuration, string sectionName)
        {
            T type = configuration.GetSection(sectionName).Get<T>();

            return type;
        }

        public static Dictionary<string, string> RetrieveSectionInfo(IConfiguration configuration, string sectionName)
        {
            //Get API Information
            Dictionary<string, string> apiConfig = new Dictionary<string, string>();

            try
            {
                if (configuration.GetSection(sectionName).Exists())
                {
                    //Retrieve api information from the config file.
                    foreach (var item in configuration.GetSection(sectionName).GetChildren())
                        apiConfig.Add(item.Key, item.Value);
                }
                else
                    throw new Exception("Provided config section not found, please contact your administrator.");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return apiConfig;
        }
    }
}
