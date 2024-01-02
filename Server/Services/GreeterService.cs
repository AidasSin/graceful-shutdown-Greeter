#region Copyright notice and license

// Copyright 2019 The gRPC Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Threading.Tasks;
using Greet;
using Grpc.Core;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Server
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public GreeterService(ILoggerFactory loggerFactory, IHostApplicationLifetime applicationLifetime)
        {
            _logger = loggerFactory.CreateLogger<GreeterService>();
            _applicationLifetime = applicationLifetime;
        }


        public override async Task SayHello(HelloRequest request, IServerStreamWriter<HelloReply> responseStream, ServerCallContext context)
        {
            try
            {
                while (!_applicationLifetime.ApplicationStopping.IsCancellationRequested)
                {
                    await responseStream.WriteAsync(new HelloReply { Message = "Hello " + request.Name }, _applicationLifetime.ApplicationStopping);

                    await Task.Delay(1000, _applicationLifetime.ApplicationStopping);
                }
            }

            catch (RpcException ex) when (ex.InnerException is OperationCanceledException)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, "Shutting down..."));
            }

            catch (OperationCanceledException)
            {
                throw new RpcException(new Status(StatusCode.Unavailable, "Shutting down..."));
            }
        }
    }
}
