
using Calculator;
using Grpc.Core;
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

		var client = new PrimeNumberService.PrimeNumberServiceClient(channel);

		var request = new PrimeNumberRequest() { Number = 120 };

		var response = client.PrimeNumberFactor(request);

		while (await response.ResponseStream.MoveNext())
		{
			Console.WriteLine("Factor: {0}", response.ResponseStream.Current.PrimeFactor);
			await Task.Delay(1000);
		}


		channel.ShutdownAsync().Wait();
		Console.ReadKey();
	}
}