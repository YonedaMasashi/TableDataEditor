using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MQClient.Core.Msg.Request
{
    public class CalculateRequestMessage : RequestMessage
    {
        private List<string> _FormulaList = new List<string>();

        public CalculateRequestMessage(string id, string outputFolderPath, List<string> formulaList)
            : base(Em_Mode.Calculate, id, outputFolderPath)
        {
            if (formulaList == null || formulaList.Count == 0) throw new ArgumentException("formulaList");
            _FormulaList = formulaList;
        }

        public override XElement CreateRequestMsg()
        {
            var resultNode = base.CreateRequestMsg();
            
            var formulaList = new XElement("FormulaList");
            foreach (var elem in _FormulaList)
            {
                formulaList.Add(new XElement("Formula", elem));
            }

            resultNode.Add(formulaList);

            return resultNode;
        }
    }
}
