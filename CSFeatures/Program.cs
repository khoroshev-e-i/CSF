using CSFeatures;
using CSFeatures.CS8;


// Default interface implementation
// ILogger logger = new WrappedLogger();
// logger.Log("message");



// Asynchronous streams
await foreach (var number in AsynchronousStreams.GenerateSequence())
{
    Console.WriteLine(number);
}


await using (var s = new ExampleConjunctiveDisposableusing())
{
    
}

// Records
Records.Foo();

// Code generator
//HelloWorldGenerator.HelloWorld.SayHello(); // calls Console.WriteLine("Hello World!") and then prints out syntax trees
