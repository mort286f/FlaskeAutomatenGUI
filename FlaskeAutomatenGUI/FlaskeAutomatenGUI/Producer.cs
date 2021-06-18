using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FlaskeAutomatenGUI
{
    public class Producer
    {
        //List that contains the Drink class, which Beer and Soda derives from
        public static Queue<Drink> prodQueue;
        public static bool isBeer;
        public static bool isRunning;
        public static bool hasProduced;
        public static bool isAdded;
        public static int produceSpeed = 250;
        public Producer()
        {
            prodQueue = new Queue<Drink>();
        }

        //Produces either one beer or soda with a name and serial number
        public void Produce()
        {
            while (isRunning)
            {
                Thread.Sleep(produceSpeed);
                lock (prodQueue)
                {

                    try
                    {
                        Random rndm = new Random();
                        int typeSwitch = rndm.Next(1, 3);

                        while (prodQueue.Count == 10)
                        {
                            Monitor.Wait(prodQueue);
                        }

                        if (prodQueue.Count < 10)
                        {
                            if (typeSwitch == 1)
                            {
                                prodQueue.Enqueue(new Beer("Tuborg"));
                                isBeer = true;
                                
                            }
                            if (typeSwitch == 2)
                            {
                                prodQueue.Enqueue(new Soda("Fanta"));
                                isBeer = false;
                            }
                            isAdded = true;
                            hasProduced = true;
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    finally
                    {
                        Monitor.PulseAll(prodQueue);
                    }
                }
            }
        }
    }
}
