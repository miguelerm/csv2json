using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CsvToJson.Tests
{
    public class CsvToJsonTests
    {
        [Fact]
        public async Task IntegrationTest()
        {

            string result;

            using(var input = new MemoryStream(Encoding.UTF8.GetBytes("Id,Nombre,Activo\n0,Demo,true")))
            using(var streamReader = new StreamReader(input))
            {
                var csv = new CsvReader(streamReader);

                using(var output = new MemoryStream())
                {
                    using (var writer = new StreamWriter(output))
                    using (var json = new JsonWriter(writer))
                    {
                        Dictionary<string, object> obj;
                        while((obj = await csv.ReadAsync()) != null) {
                            await json.WriteAsync(obj);
                        }
                    }

                    result = Encoding.UTF8.GetString(output.ToArray());
                }

            }

            Assert.Equal("[{\"id\":0,\"nombre\":\"Demo\",\"activo\":true}]", result);
        }

    }

}
