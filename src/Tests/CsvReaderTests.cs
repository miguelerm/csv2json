using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CsvToJson.Tests
{
    public class CsvReaderTests
    {
        [Fact]
        public void TakesTheFirstLineAsPropertyNamesAndNextLineAsValues()
        {
            Dictionary<string, object> result;

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes("Id,Nombre,Activo\n0,Demo,true")))
            using(var streamReader = new StreamReader(stream))
            {
                var reader = new CsvReader(streamReader);
                result = reader.ReadAsync().Result;
            }

            Assert.Equal(new []{"Id", "Nombre", "Activo"}, result.Keys.ToArray());
            Assert.Equal(0D, result["Id"]);
            Assert.Equal("Demo", result["Nombre"]);
            Assert.Equal(true, result["Activo"]);
        }

        [Fact]
        public void CanModifySeparator()
        {
            Dictionary<string, object> result;

            using(var stream = new MemoryStream(Encoding.UTF8.GetBytes("Id|Nombre|Activo\n0|Demo|true")))
            using(var streamReader = new StreamReader(stream))
            {
                var reader = new CsvReader(streamReader, "|");
                result = reader.ReadAsync().Result;
            }

            Assert.Equal(new []{"Id", "Nombre", "Activo"}, result.Keys.ToArray());
            Assert.Equal(0D, result["Id"]);
            Assert.Equal("Demo", result["Nombre"]);
            Assert.Equal(true, result["Activo"]);
        }

    }

}
