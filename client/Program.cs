
using Dummy;
using Greet;
using Grpc.Core;

internal class Program
{
	const string TARGET = "127.0.0.1:50051";
	private static void Main(string[] args)
	{
		Channel channel = new Channel(TARGET, ChannelCredentials.Insecure);

		channel.ConnectAsync().ContinueWith((task) =>
		{
			if (task.Status == TaskStatus.RanToCompletion)
				Console.WriteLine("The client connected!");
		});

		// var client = new DummyService.DummyServiceClient(channel);
		var client = new GreetingService.GreetingServiceClient(channel);

		var greeting = new Greeting { FirstName = "Mohamed", LastName = "Elsayed" };
		var response = client.Greet(new GreetingRequest() { Greeting = greeting });

		Console.WriteLine(response.Result);

		channel.ShutdownAsync().Wait();
		Console.ReadKey();
	}
}