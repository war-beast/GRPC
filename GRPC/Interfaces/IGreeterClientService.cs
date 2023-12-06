﻿using Grpc.Core;

namespace GRPC.Client.Interfaces;

public interface IGreeterClientService
{
	Task<string> CallGreeterMessage(string name, string jwt, CancellationToken token);
}