using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Cachy.Common;

namespace Cachy.Storage
{
    public class BookKeeper : BackgroundService
    {
        private readonly ConcurrentBag<IHandler> _handlers;
        public BookKeeper(ConcurrentBag<IHandler> handlers)
        {
            //  handlers.Add
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (stoppingToken.IsCancellationRequested != true)
            {
                await Task.Delay(100);
            }
        }
    }
}
