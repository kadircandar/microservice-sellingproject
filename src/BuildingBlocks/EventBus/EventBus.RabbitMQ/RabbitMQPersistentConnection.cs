using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.RabbitMQ
{
    public class RabbitMQPersistentConnection : IDisposable
    {
        private readonly IConnectionFactory connectionFactory;
        private readonly int retryCount;
        private IConnection connection;
        private object lock_object = new object();
        private bool _disposed;


        public RabbitMQPersistentConnection(IConnectionFactory connectionFactory, int retryCount = 5)
        {
            this.connectionFactory = connectionFactory;
            this.retryCount = retryCount;
        }

        public bool IsConnected => connection != null && connection.IsOpen;


        public IChannel CreateChannel()
        {
            return connection.CreateChannelAsync().Result;
        }

        public void Dispose()
        {
            _disposed = true;
            connection.Dispose();
        }


        public bool TryConnect()
        {
            lock (lock_object)
            {
                var policy = Policy.Handle<SocketException>()
                    .Or<BrokerUnreachableException>()
                    .WaitAndRetry(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                    {
                    }
                );

                policy.Execute(() =>
                {
                    connection = connectionFactory.CreateConnectionAsync().Result;
                });

                if (IsConnected)
                {
                    connection.ConnectionShutdownAsync += Connection_ConnectionShutdown;
                    connection.CallbackExceptionAsync += Connection_CallbackException;
                    connection.ConnectionBlockedAsync += Connection_ConnectionBlocked;
                    // log

                    return true;
                }

                return false;
            }
        }

        private Task Connection_ConnectionBlocked(object sender, global::RabbitMQ.Client.Events.ConnectionBlockedEventArgs e)
        {
            if (_disposed)
                return Task.CompletedTask;
            TryConnect();
            return Task.CompletedTask;
        }

        private Task Connection_CallbackException(object sender, global::RabbitMQ.Client.Events.CallbackExceptionEventArgs e)
        {
            if (_disposed)
                return Task.CompletedTask;
            TryConnect();
            return Task.CompletedTask;
        }

        private Task Connection_ConnectionShutdown(object sender, global::RabbitMQ.Client.Events.ShutdownEventArgs e)
        {
            // log Connection_ConnectionShutdown

            if (_disposed)
                return Task.CompletedTask;
            TryConnect();
            return Task.CompletedTask;
        }
    }
}
