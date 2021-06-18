using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlaskeAutomatenGUI
{
    public class Drink
    {
        public string Name { get; set; }
        public int SerialNumber { get; set; }
        public static object drinkLock = new object();
        public Drink(string name)
        {
            this.Name = name;
            //Sets a random serial number every time a new instance of Drink is made.
            Random rndm = new Random();
            this.SerialNumber = rndm.Next(1000000, 2000000);
        }
    }
}
