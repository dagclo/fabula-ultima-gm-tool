using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FabulaUltimaDataImporter.IO
{
    internal class InputTapeIO : UserIOWrapper
    {
        private readonly Queue<string> _inputQueue;
        

        public InputTapeIO(IEnumerable<string> fileLines)
        {
            _inputQueue = new Queue<string>(fileLines.Where(l => !l.StartsWith('#')).Select(l => l.Trim())); // no spaces before or after
        }

        private bool _switchedOver = false;
        public override string? ReadLine()
        {
            if(_inputQueue.Any())
            {
                var inputString = _inputQueue.Dequeue();
                this.WriteLine($"'{inputString}' FROM INPUT FILE");
                Task.Delay(100).Wait();
                return inputString;
            }

            if (!_switchedOver)
            {
                this.WriteLine("TAKING USER INPUT");
                _switchedOver = true;
            }
            return base.ReadLine();
        }
    }
}
