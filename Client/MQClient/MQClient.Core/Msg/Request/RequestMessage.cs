using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MQClient.Core.Msg.Request
{
    public class RequestMessage
    {
        private Em_Mode _Mode = Em_Mode.CreateSampleData;

        private string _Id = string.Empty;

        private string _OutputFolderPath = string.Empty;

        public RequestMessage(Em_Mode mode, string id, string outputFolderPath)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException("id");
            if (string.IsNullOrEmpty(outputFolderPath)) throw new ArgumentNullException("outputFolderPath");

            _Mode = mode;
            _Id = id;
            _OutputFolderPath = outputFolderPath;
        }

        public virtual XElement CreateRequestMsg()
        {
            var modeName = Enum.GetName(typeof(Em_Mode), _Mode);
            var root = new XElement("Root");
            root.Add(new XElement("Mode", modeName));
            root.Add(new XElement("Id", _Id));
            root.Add(new XElement("OutputFolderPath", _OutputFolderPath));

            return root;
        }
    }
}
