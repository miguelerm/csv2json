using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvToJson
{
    public class CsvReader
    {
        private string[] headers;
        private StreamReader input;
        private readonly string separator;

        public CsvReader(StreamReader input, string separator = ",")
        {
            this.input = input;
            this.separator = separator ?? ",";
        }

        public async Task<Dictionary<string, object>> ReadAsync()
        {
            if (input.EndOfStream) return null;

            var line = await input.ReadLineAsync();
            var values = line?.Split(separator.ToCharArray()) ?? new string[]{};

            if (headers == null) {
                headers = values;

                if (input.EndOfStream) return null;

                line = await input.ReadLineAsync();
                values = line?.Split(separator.ToCharArray()) ?? new string[]{};
            }

            var obj = new Dictionary<string, object>();

            for (int i = 0; i < headers.Length; i++)
            {
                if (values.Length >= i) {
                    var val = values[i];
                    if (double.TryParse(val, out var numVal))
                    {
                        obj.Add(headers[i], numVal);
                    } else if (bool.TryParse(val, out var boolVal)) {
                        obj.Add(headers[i], boolVal);
                    } else {
                        obj.Add(headers[i], val);
                    }
                } else {
                    obj.Add(headers[i], null);
                }
            }

            return obj;

        }
    }
}
