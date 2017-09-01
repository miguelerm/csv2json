using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CsvToJson.Tests
{
    public class JsonWriterTests
    {
        [Fact]
        public void GivenADictionaryItCreatesAJsonObject()
        {
            var testObj = new Dictionary<string, object>() {
                { "Id", 0 },
                { "Name", "Test" }
            };

            string result;
            var output = new byte[24];

            using (var writer = new StreamWriter(new MemoryStream(output)))
            using (var formatter = new JsonWriter(writer))
            {
                formatter.WriteAsync(testObj).Wait();
            }

            result = Encoding.UTF8.GetString(output);


            Assert.Equal("[{\"id\":0,\"name\":\"Test\"}]", result);
        }

        [Fact]
        public void SequentialCallsWriteJsonSeparatedByComa() {
            var testObj = new Dictionary<string, object>() {
                { "Id", 0 },
                { "Name", "Test" }
            };

            string result;

            var output = new byte[47];
            using (var writer = new StreamWriter(new MemoryStream(output)))
            using (var formatter = new JsonWriter(writer))
            {
                formatter.WriteAsync(testObj).Wait();
                formatter.WriteAsync(testObj).Wait();
            }

            result = Encoding.UTF8.GetString(output);

            Assert.Equal("[{\"id\":0,\"name\":\"Test\"},{\"id\":0,\"name\":\"Test\"}]", result);
        }
    }

}
