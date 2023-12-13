using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymEquipment.Backend.Exceptions
{
    internal class InvalidSearchingExecption : Exception
    {
        public InvalidSearchingExecption()
        {

        }

        public InvalidSearchingExecption(string message) : base(message) { }
    }
}
