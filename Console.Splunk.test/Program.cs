using POC.Splunk;

namespace Console.Splunk.test
{
    class Program
    {
        static void Main(string[] args)
        {
            var service = new SplunkService();
            
            service.testService();

            System.Console.ReadLine();
        }
    }
}