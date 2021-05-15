using Foundatio.Queues;
using System;
using System.Threading.Tasks;

namespace DemoQueue
{
    public class MyQueue<T> : InMemoryQueue<T> where T : class
    {
        public override Task<IQueueEntry<T>> DequeueAsync(TimeSpan? timeout = null)
        {
            Console.WriteLine("next one now. ");
            return base.DequeueAsync(timeout);
        }
    }

    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    class Program
    {
        public static double CalculateDistance(Location point1, Location point2)
        {
            var d1 = point1.Latitude * (Math.PI / 180.0);
            var num1 = point1.Longitude * (Math.PI / 180.0);
            var d2 = point2.Latitude * (Math.PI / 180.0);
            var num2 = point2.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) +
                     Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);
            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));

        }
        

        static async Task Main(string[] args)
        {
            var strLong1 = "10.8461474";
            var strLa1 = "101.8461474";

            var strLong2 = "10.8461474";
            var strLa2 = "109.8461474";

            var location1 = new Location()
            {
                Longitude = double.Parse(strLong1, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo),
                Latitude = double.Parse(strLa1, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)
            };

            var location2 = new Location()
            {
                Longitude = double.Parse(strLong2, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo),
                Latitude = double.Parse(strLa2, System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.NumberFormatInfo.InvariantInfo)
            };
            Console.WriteLine(  CalculateDistance(location1, location2)); 
            //IQueue<Model> queue = new MyQueue<Model>();

            //await queue.EnqueueAsync(new Model
            //{
            //    Age = 1,
            //    Name = "AAA"
            //});



            //await queue.EnqueueAsync(new Model
            //{
            //    Age = 2,
            //    Name = "AAA"
            //});


            //await queue.EnqueueAsync(new Model
            //{
            //    Age = 3,
            //    Name = "AAA"
            //});


            //IQueueEntry<Model> workItem = await queue.DequeueAsync();
            //Console.WriteLine(workItem.Value.Age);

            //IQueueEntry<Model> workItem2 = await queue.DequeueAsync();
            //Console.WriteLine(workItem2.Value.Age);

            //IQueueEntry<Model> workItem3 = await queue.DequeueAsync();
            //Console.WriteLine(workItem3.Value.Age);
        }
    }
}
