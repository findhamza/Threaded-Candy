using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CandyTeacher
{
    class CandyBowl
    {
        private int? prof;
        private int? candy;
        private int? multi;
        private bool wakeUp;
        private bool keepRunning;

        public CandyBowl(int? prof, int? candy, int? multi)
        {
            this.prof = prof;
            this.candy = candy;
            this.multi = multi;
            wakeUp = false;
            keepRunning = true;
        }

        internal void GetCandy(TextBlock outText)
        {
            Thread t = new Thread(() => Assistant(outText));
            t.Start();

            var profItems = Enumerable.Range(1, (int)prof);
            var parallelProfItems = profItems.AsParallel().WithDegreeOfParallelism((int)prof);

            parallelProfItems.ForAll((q)=>Professors(q,outText));
        }

        private void Assistant(TextBlock outText)
        {
            int initCandy = (int)candy;

            while(keepRunning)
            {
                if (wakeUp == false)
                    Thread.Sleep(100/(int)multi);
                else
                {
                    Console.WriteLine("\n\nTeaching Assistant Woke Up & Added Candy\n\n");
                    outText.Dispatcher.BeginInvoke((Action)(() => outText.Text = ("\n\nTeaching Assistant Woke Up & Added Candy\n\n" +
                                                                            "Teaching Assistant Went Back to Sleep\n\n") + outText.Text));
                    lock (this)
                        candy = (int?)initCandy;
                    wakeUp = false;
                    Console.WriteLine("\n\nTeaching Assistant Went Back to Sleep\n\n");
                }
            }
        }

        internal void StopCandy()
        {
            keepRunning = false;
        }

        private void Professors(int value, TextBlock outText)
        {
            //outText.Text = ($"Worker {Thread.CurrentThread.ManagedThreadId} is processing {value}");
            outText.Dispatcher.BeginInvoke((Action)(() => outText.Text = ($"Professor {value} entered the room.\n")+outText.Text));
            Console.WriteLine($"Worker {Thread.CurrentThread.ManagedThreadId} is processing {value}");
            //Thread.Sleep(1000); // Simulate work.
            while (keepRunning)
            {
                lock (this)
                {
                    if (candy > 0)
                    {
                        Console.WriteLine($"Professor {value} is eating candy.\n");
                        outText.Dispatcher.BeginInvoke((Action)(() => outText.Text = ($"Professor {value} is eating candy\n") + outText.Text));
                        candy--;
                        Console.WriteLine($"Candy left is {candy}\n");
                        outText.Dispatcher.BeginInvoke((Action)(() => outText.Text = ($"Candy left is {candy}\n") + outText.Text));
                        Thread.Sleep(1000/(int)multi);
                    }
                    else
                    {
                        wakeUp = true;
                        Thread.Sleep(1000/(int)multi);
                    }
                }
            }
        }
    }
}
