
using Average;
using Calculator;
using Grpc.Core;
using Max;
using Prime;

internal class Program
{
	private static async Task Main(string[] args)
	{
		const string TARGET = "127.0.0.1:50051";

		Channel channel = new Channel(TARGET, ChannelCredentials.Insecure);
		await channel.ConnectAsync();

		if (channel.State == ChannelState.Ready)
			Console.WriteLine("Connected!");

		/*
		var client = new CalculatorService.CalculatorServiceClient(channel);

		Console.Write("Please enter first number: ");
		int firstNumber = 0;
		int.TryParse(Console.ReadLine(), out firstNumber);

		Console.Write("Please enter second number: ");
		int secondNumber = 0;
		int.TryParse(Console.ReadLine(), out secondNumber);

		SumResponse sumResponse = client.Sum(new SumRequest() { FirstNum = firstNumber, SecondNum = secondNumber });

		Console.WriteLine($"Result is: {sumResponse.Result}");
		*/

		/*
		var client = new PrimeNumberService.PrimeNumberServiceClient(channel);

		var request = new PrimeNumberRequest() { Number = 120 };

		var response = client.PrimeNumberFactor(request);

		while (await response.ResponseStream.MoveNext())
		{
			Console.WriteLine("Factor: {0}", response.ResponseStream.Current.PrimeFactor);
			await Task.Delay(1000);
		}
		*/

		// var client = new averageService.averageServiceClient(channel);
		// var stream = client.avg();

		// foreach (var item in Enumerable.Range(1, 10))
		// {
		// 	var request = new AverageRequest { Number = Random.Shared.Next(1, 100) };
		// 	await stream.RequestStream.WriteAsync(request);
		// }

		// await stream.RequestStream.CompleteAsync();

		// Console.WriteLine(stream.ResponseAsync.Result.Result);


		var client = new FindMaxService.FindMaxServiceClient(channel);
		var stream = client.FinMax();

		var responseTask = Task.Run(async () =>
		{
			while (await stream.ResponseStream.MoveNext())
			{
				Console.WriteLine("Recieved: " + stream.ResponseStream.Current.Max + Environment.NewLine);
			}
		});

		foreach (var item in Enumerable.Range(1, 10))
		{
			int num = Random.Shared.Next(-1000, 1000);
			Console.WriteLine($"Sending: {num}");
			await stream.RequestStream.WriteAsync(new MaxRequest { Number = num });
			await Task.Delay(1000);
		}

		await stream.RequestStream.CompleteAsync();
		await responseTask;
		channel.ShutdownAsync().Wait();
		Console.ReadKey();
	}
}