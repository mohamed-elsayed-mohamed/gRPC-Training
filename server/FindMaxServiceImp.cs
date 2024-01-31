using Grpc.Core;
using Max;
using static Max.FindMaxService;

namespace server;

public class FindMaxServiceImp : FindMaxServiceBase
{
	public override async Task FinMax(IAsyncStreamReader<MaxRequest> requestStream, IServerStreamWriter<MaxResponse> responseStream, ServerCallContext context)
	{
		int mx = int.MinValue;

		while (await requestStream.MoveNext())
		{
			mx = Math.Max(mx, requestStream.Current.Number);
			await responseStream.WriteAsync(new MaxResponse() { Max = mx });
		}
	}
}
