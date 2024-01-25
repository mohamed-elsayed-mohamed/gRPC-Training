﻿using Calculator;
using Grpc.Core;
using static Calculator.CalculatorService;

namespace server;

public class CalculatorServiceImp : CalculatorServiceBase
{
	public override Task<SumResponse> Sum(SumRequest request, ServerCallContext context)
	{
		return Task.FromResult(new SumResponse() { Result = request.FirstNum + request.SecondNum });
	}

}
