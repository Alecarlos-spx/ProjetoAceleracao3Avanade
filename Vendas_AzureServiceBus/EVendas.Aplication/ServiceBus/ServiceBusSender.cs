using EVendas.Aplication.Interfaces;
using EVendas.Aplication.Model;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVendas.Aplication.ServiceBus
{
    public class ServiceBusSender : IServiceBusSender
    {
        private QueueClient _queueClient;
        private readonly IConfiguration _configuration;

        public ServiceBusSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task SendCreateProdutoMessage(ProdutoCriadoModel request)
        {
            _queueClient = CreateQueueClient("produtocriado");
            return SendMessage(request);
        }

        public Task SendProdutoVendidoMessage(string codigoProduto, ProdutoVendidoModel request)
        {
            _queueClient = CreateQueueClient("produtovendido");
            request.CodigoProduto = codigoProduto;
            return SendMessage(request);
        }

        public Task SendUpdateProdutoMessage(string codigoProduto, ProdutoEditadoModel request)
        {
            _queueClient = CreateQueueClient("produtoeditado");
            request.CodigoProduto = codigoProduto;
            return SendMessage(request);
        }


        private QueueClient CreateQueueClient(string queue)
        {
            return new QueueClient(_configuration.GetConnectionString("ServiceBusConnectionString"), queue);
        }


        public async Task SendMessage(object produto)
        {
            string data = JsonConvert.SerializeObject(produto);
            Message message = new Message(Encoding.UTF8.GetBytes(data));

            //var message = new Message(produto.ToJsonBytes());

            message.ContentType = "application/json";
            message.UserProperties.Add("CorrelationId", Guid.NewGuid().ToString());


            await _queueClient.SendAsync(message);
        }


    }
}
