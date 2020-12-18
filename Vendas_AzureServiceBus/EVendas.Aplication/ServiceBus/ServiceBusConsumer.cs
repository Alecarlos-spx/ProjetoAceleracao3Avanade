using EVendas.Aplication.Interfaces;
using EVendas.Aplication.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EVendas.Aplication.ServiceBus
{
    public class ServiceBusConsumer : IServiceBusConsumer
    {
        private QueueClient _queueClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IProdutoService _produtoService;

        public ServiceBusConsumer(IConfiguration configuration, IProdutoService produtoService)
        {
            _configuration = configuration;
            _produtoService = produtoService;
        }


        public void RegisterOnMessageHandler_ProdutoCriado()
        {
            _queueClient = new QueueClient(_configuration.GetConnectionString("ServiceBusConnectionString"), "produtocriado");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProdutoCriado_MessagesAsync, messageHandlerOptions);
        }

        public void RegisterOnMessageHandler_ProdutoEditado()
        {
            _queueClient = new QueueClient(_configuration.GetConnectionString("ServiceBusConnectionString"), "produtoeditado");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProdutoEditado_MessagesAsync, messageHandlerOptions);
        }

        public void RegisterOnMessageHandler_ProdutoVendido()
        {
            _queueClient = new QueueClient(_configuration.GetConnectionString("ServiceBusConnectionString"), "produtovendido");

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProdutoVendido_MessagesAsync, messageHandlerOptions);
        }


        private async Task ProdutoCriado_MessagesAsync(Message message, CancellationToken token)
        {
            var produto = JsonConvert.DeserializeObject<ProdutoCriadoModel>(Encoding.UTF8.GetString(message.Body));
            await _produtoService.Create(produto);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private async Task ProdutoEditado_MessagesAsync(Message message, CancellationToken token)
        {
            var produto = JsonConvert.DeserializeObject<ProdutoEditadoModel>(Encoding.UTF8.GetString(message.Body));
            await _produtoService.Update(produto.CodigoProduto, produto);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private async Task ProdutoVendido_MessagesAsync(Message message, CancellationToken token)
        {
            var produto = JsonConvert.DeserializeObject<ProdutoVendidoModel>(Encoding.UTF8.GetString(message.Body));
            await _produtoService.VenderProduto(produto.CodigoProduto, produto.Quantidade);
            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }



        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            return Task.CompletedTask;
            //_logger.LogError(exceptionReceivedEventArgs.Exception, "Message handler encountered an exception");
            //var context = exceptionReceivedEventArgs.ExceptionReceivedContext;

            //_logger.LogDebug($"- Endpoint: {context.Endpoint}");
            //_logger.LogDebug($"- Entity Path: {context.EntityPath}");
            //_logger.LogDebug($"- Executing Action: {context.Action}");

            //return Task.CompletedTask;
        }



        public async Task CloseQueueAsync()
        {
            await _queueClient.CloseAsync();
        }
    }
}
