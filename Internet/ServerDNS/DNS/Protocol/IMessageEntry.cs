namespace DNS.Protocol
{
	public interface IMessageEntry
	{
		Domain Name { get; }
		RecordType Type { get; set; }
		RecordClass Class { get; }

		int Size { get; }
		byte[] ToArray();
	}
}
