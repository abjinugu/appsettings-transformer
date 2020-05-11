using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml;
using System.Linq;
using System.Reflection;

namespace appsettingstransformer
{
    public static class TransformConfigs
    {
        public static void transform(string sourcefile, string dockerconfig)
        {
            try
            {
                var extension = Path.GetExtension(sourcefile);
                switch (extension)
                {
                    case fileextensions.config:
                        processxmlconfigs(sourcefile, dockerconfig);
                        break;
                    case fileextensions.xml:
                        processxmlconfigs(sourcefile, dockerconfig);
                        break;
                    case fileextensions.json:
                        processjsonfile(sourcefile, dockerconfig);
                        break;
                    default:
                        processxmlconfigs(sourcefile, dockerconfig);
                        break;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void processxmlconfigs(string sourcefile, string dockerconfig)
        {
            try
            {                
                //find all placeholders that start with '#{' and end with '}'
                string pattern = @"#{.*?\}";
                XDocument xo1 = null;
                string appconfig = sourcefile.readFile();
                var o1 = dockerconfig.readJsonFromFile();
                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(o1.ToString());
                xo1 = XDocument.Parse(doc.OuterXml);

                MatchCollection matches = Regex.Matches(appconfig, pattern);
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        try
                        {
                            string strkey = capture.Value.TrimStart("#{".ToCharArray()).TrimEnd('}').ToLower();
                            string strvalue = string.Empty;

                            var element = xo1.Element(xnames.environments).Element(xnames.secrets).Elements().Where(x => x.Name.ToString().ToLower() == strkey).FirstOrDefault();

                            if (element.IsEmpty)
                            {
                                Console.WriteLine("No config found for {0}", capture.Value);
                            }
                            else
                            {
                                Console.WriteLine("Transforming = {0}", capture.Value);                                
                                appconfig = appconfig.Replace(capture.Value, strkey.Contains(Constants.base64.ToLower()) ? element.Value.Base64Decode() : element.Value);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("No config found for {0}", capture.Value);
                            Console.WriteLine(ex.Message);
                            continue;
                        }

                    }
                }
                using (StreamWriter writer = new StreamWriter(sourcefile, false, Encoding.UTF8))
                {
                    writer.WriteLine(appconfig);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void processjsonfile(string sourcefile, string dockerconfig)
        {
            try
            {
                XName envirs = "environments";
                //find all placeholders that start with '#{' and end with '}'
                string pattern = @"#{.*?\}";

                string appconfig = sourcefile.readFile();
                var o1 = dockerconfig.readJsonFromFile();
                var co1 = (Newtonsoft.Json.Linq.JObject)JsonConvert.DeserializeObject(o1["environments"][0]["secrets"].ToString());                                
                MatchCollection matches = Regex.Matches(appconfig, pattern);
                foreach (Match match in matches)
                {
                    foreach (Capture capture in match.Captures)
                    {
                        try
                        {
                            string strvalue = string.Empty;
                            Console.WriteLine("Transforming = {0}", capture.Value);
                            string strkey = capture.Value.TrimStart("#{".ToCharArray()).TrimEnd('}').ToLower();
                            strvalue = co1.Children().Where(jt => jt.getJName() == strkey).FirstOrDefault().getJValue();

                            if (strkey.Contains(Constants.base64))
                            {                                
                                appconfig = appconfig.Replace("\""+capture.Value+"\"", strvalue.Base64Decode());
                            }
                            else
                            {                                
                                appconfig = appconfig.Replace(capture.Value, (JsonConvert.SerializeObject(strvalue)).Trim('"'));
                            }
                            
                            
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("No config found for {0}", capture.Value);
                            continue;
                        }
                    }
                }
                using (StreamWriter writer = new StreamWriter(sourcefile, false, Encoding.UTF8))
                {
                    writer.WriteLine(appconfig);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void getdockersecret(string dockerconfig, out string username, out string password)
        {
            username = string.Empty;
            password = string.Empty;
            try
            {
                var o1 = dockerconfig.readJsonFromFile();
                username = o1["environments"][0]["secrets"][Constants.scrlocaladminuser].Value<string>();
                password = o1["environments"][0]["secrets"][Constants.scrlocaladminpassword].Value<string>();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
