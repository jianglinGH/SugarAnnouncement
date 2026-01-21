using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SugarAnnouncement.Core.Exceptions
{
    public class DomainExceptions: Exception
    {
        public DomainExceptions(string message) : base(message) { 
        }
    }
}
