using System.Net;
using System.Net.Sockets;
using DNS.Protocol;

namespace DNS.Client.RequestResolver
{
	public class UdpRequestResolver : IRequestResolver
	{
		private IRequestResolver fallback;

		public UdpRequestResolver(IRequestResolver fallback)
		{
			this.fallback = fallback;
		}

		public UdpRequestResolver()
		{
			this.fallback = new NullRequestResolver();
		}

		public ClientResponse Request(ClientRequest request)
		{
			UdpClient udp = new UdpClient();
			IPEndPoint dns = request.Dns;

			try
			{
				udp.Connect(dns);
				var bytes = request.ToArray();
				udp.Send(bytes, bytes.Length);

				byte[] buffer = udp.Receive(ref dns);
				Response response = Response.FromArray(buffer); //null;

				if (response.Truncated)
				{
					return fallback.Request(request);
				}

				return new ClientResponse(request, response, buffer);
			}
			finally
			{
				udp.Close();
			}
		}
	}
}
