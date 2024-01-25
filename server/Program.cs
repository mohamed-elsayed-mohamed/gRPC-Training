using Greet;
using Grpc.Core;
using server;

internal class Program
{
	const int PORT = 50051;
	private static void Main(string[] args)
	{
		Server? server = null;
		try
		{
			server = new Server()
			{
				Services = { GreetingService.BindService(new GreetingServiceImpl()) },
				Ports = { new ServerPort("localhost", PORT, ServerCredentials.Insecure) }
			};

			server.Start();
			Console.WriteLine($"The server is listening on port: {PORT}");
			Console.ReadKey();
		}
		catch (IOException ex)
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"The server failed to start: {ex.Message}");
			Console.ForegroundColor = ConsoleColor.Gray;
		}
		finally
		{
			if (server != null)
				server.ShutdownAsync().Wait();
		}
	}
}