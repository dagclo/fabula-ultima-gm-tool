using FabulaUltimaDatabase;
using FabulaUltimaDataImporter.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaDataImporter.Processor
{
    public class EndWorkflow : IWorkflow
    {
        private readonly UserIOWrapper _userIOWrapper;

        public EndWorkflow(UserIOWrapper userIOWrapper)
        {   
            _userIOWrapper = userIOWrapper;            
        }

        public string GetName => throw new NotImplementedException();

        public WorkFlowKind Kind => WorkFlowKind.END;

        public IEnumerable<IWorkflow> Run()
        {
            //todo: generate a report of what was created... somehow
            _userIOWrapper.WriteLines("done!");

            yield break;
        }
    }
}
