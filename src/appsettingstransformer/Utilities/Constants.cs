using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;


namespace appsettingstransformer
{
    public static class Options
    {
        public const string createlocalaccount = "CLA";
        public const string transformconfig = "TC";
        public const string apppoolsetting = "AS";
        public const string enablegzip = "GZIP";
        public static bool isValidOption(string option)
        {
            return (option.Equals(createlocalaccount) || option.Equals(transformconfig) || option.Equals(apppoolsetting) || option.Equals(enablegzip));            
        }
    }

    public static class Constants
    {        
        public const string scrlocaladminuser = "scr_local_admin_user";
        public const string scrlocaladminpassword = "scr_local_admin_pwd";
        public const string base64 = ".base64";        
    }

    public static class fileextensions
    {
        public const string config = ".config";
        public const string xml = ".xml";
        public const string json = ".json";
    }

    public static class xnames
    {
        public static XName environments = "environments";
        public static XName secrets = "secrets";        
    }
}
