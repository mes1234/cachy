using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using Cachy.Events;
using CachyClient.Common;
using Grpc.Core;

namespace CachyClient
{
    public class Cachy : ICachy
    {
        private readonly string _server;
        private readonly Channel _channel;
        private readonly InsertItem.InsertItemClient _insertItemClient;
        private readonly GetItem.GetItemClient _getItemClient;
        private readonly RemoveItem.RemoveItemClient _removeItemClient;
        public Cachy(string Server = "127.0.0.1:5001")
        {
            _server = Server;
            _channel = new Channel(_server, ChannelCredentials.Insecure);
            _insertItemClient = new InsertItem.InsertItemClient(_channel);
            _getItemClient = new GetItem.GetItemClient(_channel);
            _removeItemClient = new RemoveItem.RemoveItemClient(_channel);
        }


        public async Task Add(string name, byte[] paylod, int ttl = 3600)
        {
            var ack = await _insertItemClient.InsertItemAsync(
                      new Item
                      {
                          Data = Google.Protobuf.ByteString.CopyFrom(paylod),
                          Name = name,
                          Ttl = new TimeToLive { Seconds = ttl }
                      }
                  );
        }

        public async Task<byte[]> Get(string name)
        {
            var result = await _getItemClient.GetAsync(
                  new ItemToRetrieve
                  {
                      Name = name,
                      // There is always first revision assigned 
                      // if set to zero it means use latest from snapshot
                      Revision = 0
                  }
              );
            if (result.Item.Data.Length == 0) throw new KeyNotFoundException();

            return result.Item.Data.ToByteArray();
        }

        public async Task<byte[]> Get(string name, int revision)
        {
            var result = await _getItemClient.GetAsync(
                  new ItemToRetrieve
                  {
                      Name = name,
                      Revision = revision
                  }
              );
            if (result.Item.Data.Length == 0) throw new KeyNotFoundException();

            return result.Item.Data.ToByteArray();
        }

        public async Task Remove(string name)
        {
            await _removeItemClient.RemoveAsync(
               new ItemToRemove
               { Name = name }
           );
        }
    }
}
