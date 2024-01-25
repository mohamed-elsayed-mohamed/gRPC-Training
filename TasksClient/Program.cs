
using Calculator;
using Grpc.Core;

internal class Program
{
	private static async Task Main(string[] args)
	{
		const string TARGET = "127.0.0.1:50051";

		Channel channel = new Channel(TARGET, ChannelCredentials.Insecure);
		await channel.ConnectAsync();

		if (channel.State == ChannelState.Ready)
			Console.WriteLine("Connected!");

		var client = new CalculatorService.CalculatorServiceClient(channel);

		Console.Write("Please enter first number: ");
		int firstNumber = 0;
		int.TryParse(Console.ReadLine(), out firstNumber);

		Console.Write("Please enter second number: ");
		int secondNumber = 0;
		int.TryParse(Console.ReadLine(), out secondNumber);

		SumResponse sumResponse = client.Sum(new SumRequest() { FirstNum = firstNumber, SecondNum = secondNumber });

		Console.WriteLine($"Result is: {sumResponse.Result}");

		channel.ShutdownAsync().Wait();
		Console.ReadKey();
	}
}