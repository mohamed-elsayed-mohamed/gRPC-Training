using Grpc.Core;
using Prime;
using static Prime.PrimeNumberService;

namespace server;

public class PrimeNumberServiceImp : PrimeNumberServiceBase
{
	public override async Task PrimeNumberFactor(PrimeNumberRequest request, IServerStreamWriter<PrimeNumberRespose> responseStream, ServerCallContext context)
	{
		int number = request.Number;
		Console.WriteLine($"Server recieved the request for number: {number}");
		int dividor = 2;

		while (number > 1)
		{
			if (number % dividor == 0)
			{
				number /= dividor;
				await responseStream.WriteAsync(new PrimeNumberRespose() { PrimeFactor = dividor });
			}
			else
				dividor++;
		}
	}

}
