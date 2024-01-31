
using Dummy;
using Greet;
using Grpc.Core;

internal class Program
{
	const string TARGET = "127.0.0.1:50051";
	private static async Task Main(string[] args)
	{
		Channel channel = new Channel(TARGET, ChannelCredentials.Insecure);

		await channel.ConnectAsync().ContinueWith((task) =>
		{
			if (task.Status == TaskStatus.RanToCompletion)
				Console.WriteLine("The client connected!");
		});

		// var client = new DummyService.DummyServiceClient(channel);
		var client = new GreetingService.GreetingServiceClient(channel);

		// var greeting = new Greeting { FirstName = "Mohamed", LastName = "Elsayed" };
		// var response = client.Greet(new GreetingRequest() { Greeting = greeting });

		// Console.WriteLine(response.Result);

		var greeting = new Greeting { FirstName = "Mohamed", LastName = "Elsayed" };
		// var response = client.GreeManyTimes(new GreeManyTimesRequest { Greeting = greeting });

		// while (await response.ResponseStream.MoveNext())
		// {
		// 	Console.WriteLine(response.ResponseStream.Current.Result);
		// 	await Task.Delay(1000);
		// }


		// var request = new LongGreeRequest { Greeting = greeting };
		// var stream = client.LongGreat();

		// foreach (int i in Enumerable.Range(1, 5))
		// 	await stream.RequestStream.WriteAsync(request);

		// await stream.RequestStream.CompleteAsync();
		// var response = await stream.ResponseAsync;
		// Console.WriteLine(response.Result);


		var stream = client.GreatEveryone();
		var responseTask = Task.Run(async () =>
		{
			while (await stream.ResponseStream.MoveNext())
			{
				Console.WriteLine(stream.ResponseStream.Current.Result);
				await Task.Delay(1000);
			}
		});

		foreach (var item in Enumerable.Range(1, 5))
		{
			await stream.RequestStream.WriteAsync(new GreetingRequest { Greeting = greeting });
		}

		await stream.RequestStream.CompleteAsync();
		await responseTask;

		channel.ShutdownAsync().Wait();
		Console.ReadKey();
	}
}