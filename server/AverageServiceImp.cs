using Average;
using Grpc.Core;
using static Average.averageService;

namespace server;

public class AverageServiceImp : averageServiceBase
{
	public override async Task<AverageResponse> avg(IAsyncStreamReader<AverageRequest> requestStream, ServerCallContext context)
	{
		int count = 0;
		double total = 0.0;

		while (await requestStream.MoveNext())
		{
			total += requestStream.Current.Number;
			count++;
		}

		return new AverageResponse() { Result = total / count };
	}

}
