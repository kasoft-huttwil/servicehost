﻿using System;
using Mono.Unix;
using Mono.Unix.Native;

namespace servicehost
{
    public class Server {
        public static void Run(Uri endpointUri) {
            using (var servicehost = new ServiceHost()) {
                servicehost.Start(endpointUri);
                Console.WriteLine("Service host running on {0}...", endpointUri.ToString());

                if (Is_running_on_Mono) {
                    Console.WriteLine("Ctrl-C to stop service host");
                    UnixSignal.WaitAny(UnixTerminationSignals);
                }
                else {
                    Console.WriteLine("ENTER to stop service host");
                    Console.ReadLine();
                }

                Console.WriteLine("Stopping service host");
                servicehost.Stop();
            }
        }
	    
        private static bool Is_running_on_Mono => Type.GetType("Mono.Runtime") != null;

        private static UnixSignal[] UnixTerminationSignals =>  new[] {
            new UnixSignal(Signum.SIGINT),
            new UnixSignal(Signum.SIGTERM),
            new UnixSignal(Signum.SIGQUIT),
            new UnixSignal(Signum.SIGHUP)
        };
    }
}