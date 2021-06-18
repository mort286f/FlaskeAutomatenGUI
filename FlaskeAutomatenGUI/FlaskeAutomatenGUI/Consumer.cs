using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomatenGUI
{
    public class Consumer
    {

        public string Name { get; set; }
        public static bool isRunning;
        public static Queue<Soda> sodaQueue = new Queue<Soda>();
        public static Queue<Beer> beerQueue = new Queue<Beer>();

        public Consumer(string name)
        {
            this.Name = name;
        }

        public void ConsumeSoda(object data)
        {
            while (isRunning)
            {
                Thread.Sleep(750);
                lock (sodaQueue)
                {

                    while (sodaQueue.Count == 0)
                    {
                        Monitor.Wait(sodaQueue);
                    }
                    try
                    {
                        //sodaQueue.Dequeue();
                        
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {

                        Monitor.PulseAll(sodaQueue);
                    }
                }
                
            }
        }

        public void ConsumeBeer(object data)
        {
            while (true)
            {
                Thread.Sleep(750);
                lock (beerQueue)
                {

                    while (beerQueue.Count == 0)
                    {
                        Monitor.Wait(beerQueue);
                    }
                    try
                    {
                        //beerQueue.Dequeue();
                        
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {

                        Monitor.PulseAll(beerQueue);
                    }
                }
            }
        }
    }
}

