using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;

namespace nh_spikes.SignalR
{
    public class ProductHub : Hub
    {
        public async Task ViewProduct(string productId)
        {
            await Groups.Add(Context.ConnectionId, productId);
            Clients.OthersInGroup(productId).productViewed(Context.ConnectionId);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }
    }
}