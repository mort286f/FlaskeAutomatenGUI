using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace FlaskeAutomatenGUI
{
    public class Splitter
    {
        public static bool isRunning;
        public void SplitDrinks()
        {
            while (isRunning)
            {
                Thread.Sleep(600);
                lock (Producer.prodQueue)
                {

                    try
                    {
                        while (Producer.prodQueue.Count == 0)
                        {
                            Monitor.Wait(Producer.prodQueue);
                        }
                        lock (Consumer.beerQueue)
                        {
                            if (Producer.prodQueue.Peek() is Beer)
                            {
                                Consumer.beerQueue.Enqueue((Beer)Producer.prodQueue.Dequeue());
                                Producer.isAdded = false;
                            }
                        }

                        lock (Consumer.sodaQueue)
                        {
                            if (Producer.prodQueue.Peek() is Soda)
                            {
                                Consumer.sodaQueue.Enqueue((Soda)Producer.prodQueue.Dequeue());
                                Producer.isAdded = false;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                    finally
                    {
                        Monitor.PulseAll(Producer.prodQueue);
                    }
                }
            }
        }
    }
}
