using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CsvToJson
{
    public class CsvReader
    {
        private int lineNumber;
        private string[] headers;
        private StreamReader input;
        private readonly string separator;
        private string lastLine;

        public CsvReader(StreamReader input, string separator = ",")
        {
            this.input = input;
            this.separator = separator ?? ",";

            lineNumber = 0;
        }

        public async Task<Dictionary<string, object>> ReadAsync()
        {
            try
            {
                return await InternalReadAsync();
            }
            catch (Exception ex)
            {
                throw new CsvReaderException($"Error on line {lineNumber}, last line: {lastLine}", ex);
            }
        }

        private async Task<Dictionary<string, object>> InternalReadAsync()
        {
            if (input.EndOfStream) return null;

            lastLine = await ReadLine();
            var values = lastLine?.Split(separator.ToCharArray()) ?? new string[] { };

            if (headers == null)
            {
                headers = values;

                if (input.EndOfStream) return null;

                lastLine = await ReadLine();
                values = lastLine?.Split(separator.ToCharArray()) ?? new string[] { };
            }

            var obj = new Dictionary<string, object>();

            for (int i = 0; i < headers.Length; i++)
            {
                if (values.Length >= i)
                {
                    var val = values[i];
                    if (double.TryParse(val, out var numVal))
                    {
                        obj.Add(headers[i], numVal);
                    }
                    else if (bool.TryParse(val, out var boolVal))
                    {
                        obj.Add(headers[i], boolVal);
                    }
                    else
                    {
                        obj.Add(headers[i], val);
                    }
                }
                else
                {
                    obj.Add(headers[i], null);
                }
            }

            return obj;
        }

        private Task<string> ReadLine()
        {
            lineNumber++;
            return input.ReadLineAsync();
        }
    }
}
