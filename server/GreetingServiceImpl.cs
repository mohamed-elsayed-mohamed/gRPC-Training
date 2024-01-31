using Greet;
using Grpc.Core;
using static Greet.GreetingService;

namespace server;

public class GreetingServiceImpl : GreetingServiceBase
{
	public override Task<GreetingResponse> Greet(GreetingRequest request, ServerCallContext context)
	{
		string result = string.Format("Hello {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);

		return Task.FromResult(new GreetingResponse() { Result = result });
	}

	public override async Task GreeManyTimes(GreeManyTimesRequest request, IServerStreamWriter<GreetManyTimesResponse> responseStream, ServerCallContext context)
	{
		Console.WriteLine("The server recieved the request from {0} {1}", request.Greeting.FirstName, request.Greeting.LastName);

		foreach (int i in Enumerable.Range(1, 10))
		{
			await responseStream.WriteAsync(new GreetManyTimesResponse { Result = i.ToString() });
		}
	}

	public override async Task<LongGreetResponse> LongGreat(IAsyncStreamReader<LongGreeRequest> requestStream, ServerCallContext context)
	{
		string result = "";

		while (await requestStream.MoveNext())
		{
			result += string.Format("Hello {0} {1} {2}", requestStream.Current.Greeting.FirstName, requestStream.Current.Greeting.LastName, Environment.NewLine);
		}

		return new LongGreetResponse { Result = result };
	}

	public override async Task GreatEveryone(IAsyncStreamReader<GreetingRequest> requestStream, IServerStreamWriter<GreetingResponse> responseStream, ServerCallContext context)
	{
		while (await requestStream.MoveNext())
		{
			var res = string.Format("{0} {1}", requestStream.Current.Greeting.FirstName, requestStream.Current.Greeting.LastName);
			Console.WriteLine("Recieved from: " + res);

			await responseStream.WriteAsync(new GreetingResponse { Result = "Hi " + res });
		}
	}


}
