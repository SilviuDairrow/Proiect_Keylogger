using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Interfață pentru implementarea design pattern-ului Observer.
namespace process_monitor
{
    public interface Observer
    {
        void Update(string newActiveProcess);
    }
}
