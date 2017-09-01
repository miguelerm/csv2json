using System.Collections.Generic;
using System.IO;
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

            var input = Encoding.UTF8.GetBytes("Id,Nombre,Activo\n0,Demo,true");
            using (var streamReader = new StreamReader(new MemoryStream(input)))
            {
                var csv = new CsvReader(streamReader);

                var output = new byte[40];

                using (var writer = new StreamWriter(new MemoryStream(output)))
                using (var json = new JsonWriter(writer))
                {
                    Dictionary<string, object> obj;
                    while ((obj = await csv.ReadAsync()) != null)
                    {
                        await json.WriteAsync(obj);
                    }
                }

                result = Encoding.UTF8.GetString(output);

            }

            Assert.Equal("[{\"id\":0,\"nombre\":\"Demo\",\"activo\":true}]", result);
        }

    }

}
