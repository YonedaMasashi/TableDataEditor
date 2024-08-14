using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MQClient.Core.Msg.Response
{
    public class ResponseMessage
    {
        private string _Status;
        public string Status
        {
            get { return _Status; }
        }

        private long _Index;
        public long Index
        {
            get { return _Index; }
        }

        private string _OutputFilePath;
        public string OutputFilePath
        {
            get { return _OutputFilePath; }
        }

        public ResponseMessage(string responseMessageString)
        {
            var responseNode = XElement.Parse(responseMessageString);

            // Status
            var statusNode = responseNode.Element("Status");
            if (statusNode == null) { throw new ArgumentNullException("StatusNode"); }
            _Status = statusNode.Value;

            // Index
            var indexNode = responseNode.Element("Index");
            if (indexNode == null) { throw new ArgumentNullException("Index"); }
            long.TryParse(indexNode.Value, out _Index);

            // OutputFilePath
            var outputFilePathNode = responseNode.Element("OutputFilePath");
            if (outputFilePathNode == null) { throw new ArgumentNullException("OutputFilePath"); }
            _OutputFilePath = outputFilePathNode.Value;
        }
    }
}
