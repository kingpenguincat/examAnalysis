using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace examAnalysis
{
    class JsonHelper
    {
        public static  String GetJson(String filePath,String key) {
            try
            {
                StreamReader file = System.IO.File.OpenText(filePath);
                JsonTextReader jsonTextReader = new JsonTextReader(file);
                JObject jObject = (JObject)JToken.ReadFrom(jsonTextReader);
                String result = jObject[key].ToString();
                return result;
            }
            catch (Exception ex) {
                return "ERROR";
            }
            

        }


    }
}
