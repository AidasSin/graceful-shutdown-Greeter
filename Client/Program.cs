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

using Greet;
using Grpc.Net.Client;
using System;
using System.Threading;

using var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new Greeter.GreeterClient(channel);

    try
    {
        var call = client.SayHello(new HelloRequest { Name = "GreeterClient" });

        while (await call.ResponseStream.MoveNext(CancellationToken.None))
        {
            Console.WriteLine("Greeting: " + call.ResponseStream.Current.Message);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
