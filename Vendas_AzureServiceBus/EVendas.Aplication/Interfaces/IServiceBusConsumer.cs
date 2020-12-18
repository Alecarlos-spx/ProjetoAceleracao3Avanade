using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EVendas.Aplication.Interfaces
{
    public interface IServiceBusConsumer
    {
        void RegisterOnMessageHandler_ProdutoCriado();
        void RegisterOnMessageHandler_ProdutoEditado();
        void RegisterOnMessageHandler_ProdutoVendido();
        Task CloseQueueAsync();
    }
}
