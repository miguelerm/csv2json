using System;
using System.Runtime.Serialization;

namespace CsvToJson
{
    [Serializable]
    public class CsvReaderException : Exception
    {
        public CsvReaderException()
        {
        }

        public CsvReaderException(string message) : base(message)
        {
        }

        public CsvReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CsvReaderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}