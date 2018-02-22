﻿using BeatPulse.Core;
using BeatPulse.SqlServer;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using UnitTests.Base;
using Xunit;

namespace BeatPulse.SqlServer
{
    public class beat_pulse_context_should
    {
        [Fact]
        public void register_sqlserver_health_check()
        {
            var webHostBuilder = new WebHostBuilder()
                .UseBeatPulse()
                .UseStartup<DefaultStartup>()
                .ConfigureServices(svc =>
                {
                    svc.AddBeatPulse(context =>
                    {
                        context.AddSqlServer(options=>
                        {
                            options.UseConnectionSring("the-sql-server-connection-string");
                        });
                    });
                });

            var beatPulseContex = new TestServer(webHostBuilder)
                .Host
                .Services
                .GetService<BeatPulseContext>();

            beatPulseContex.All
                .Where(hc => hc.GetType() == typeof(SqlServerHealthCheck))
                .Should().HaveCount(1);

        }
    }
}