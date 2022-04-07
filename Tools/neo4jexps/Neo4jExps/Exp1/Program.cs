using Neo4j.Driver;
using System;
using System.Linq;

namespace Exp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var greeter = new HelloWorldExample("bolt://localhost:7687/db/Trail1", "neo4j", "Welcome123#"))
            {
                greeter.PrintGreeting("hello, world");
            }
            Console.ReadLine();
        }
    }

    public class HelloWorldExample : IDisposable
    {
        private bool _disposed = false;
        private readonly IDriver _driver;

        ~HelloWorldExample() => Dispose(false);

        public HelloWorldExample(string uri, string username, string pwd)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(username, pwd));
        }

        public void PrintGreeting(string message)
        {
            using (var session = _driver.Session())
            {
                var greeting = session.WriteTransaction(tx =>
                {
                    var result = tx.Run("CREATE (a:Greeting) " +
                                        "SET a.message = $message " +
                                        "RETURN a.message + ', from node ' + id(a)",
                        new { message });
                    return result.Single()[0].As<string>();
                });
                Console.WriteLine(greeting);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _driver?.Dispose();
            }

            _disposed = true;
        }
    }
}
