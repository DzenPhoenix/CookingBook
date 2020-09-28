using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookingBook.Service
{
    public class Filter
    {
        public List<string> Categories { get; set; }
        public List<string> Kitchens { get; set; }
        public List<string> Ingridients { get; set; }
    }
}
