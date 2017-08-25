using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CsvToJson
{
    public class JsonWriter : IDisposable
    {
        private string objectPrefix;
        private StreamWriter output;

        public JsonWriter(StreamWriter output)
        {
            this.output = output;
            objectPrefix = "[";
        }

        public async Task WriteAsync(Dictionary<string, object> testObj)
        {
            var sb = new StringBuilder();

            sb.Append(objectPrefix + "{");
            string prefix = "";
            foreach (var item in testObj)
            {
                var propertyName = GetPropertyName(item.Key);
                var propertyValue = item.Value is string ? $"\"{item.Value}\"" : item.Value.ToString().ToLower();
                sb.Append($"{prefix}\"{propertyName}\":{propertyValue}");
                if (prefix == "") { 
                    prefix = ","; 
                }
            }
            sb.Append("}");

            await output.WriteAsync(sb.ToString());
            await output.FlushAsync();

            if (objectPrefix == "[") {
                objectPrefix = ",";
            }
        }

        private string GetPropertyName(string key)
        {
            return key.Substring(0, 1).ToLower() + key.Substring(1);
        }

        public void Dispose()
        {
            output.Write("]");
            output.Flush();
        }

    }
}
