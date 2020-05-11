﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace appsettingstransformer
{
    public static class Extensions
    {
        public static JObject readJsonFromFile(this string filePath)
        {
            JObject o1 = null;
            String subjectString = string.Empty;
            try
            {
                o1 = JObject.Parse(File.ReadAllText(filePath));                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return o1;
        }

        public static string readFile(this string filePath)
        {
            String subjectString = string.Empty;
            try
            {   
                using (StreamReader sr = new StreamReader(filePath))
                {
                    // Read the stream to a string, and write the string to the console.
                    subjectString = sr.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                //DO NOTHING
            }
            return subjectString;
        }

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(this string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public static string getJName(this JToken jtoken)
        {
            return ((Newtonsoft.Json.Linq.JProperty)jtoken).Name.ToLower();
        }

        public static string getJValue(this JToken jtoken)
        {
            return ((Newtonsoft.Json.Linq.JProperty)jtoken).Value.ToString();
        }


    }
}
