using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.Backend
{
    internal class InvalidAddingException : Exception
    {
        public InvalidAddingException() 
        {

        }

        public InvalidAddingException(string message) : base(message) { }
    }
}
