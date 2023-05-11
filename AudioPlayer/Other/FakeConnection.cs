using Mirror;
using System;

public class FakeConnection : NetworkConnectionToClient
{
    //Code from ced777ric#8321
    public FakeConnection(int connectionId) : base(connectionId)
    {
    }

    public override string address
    {
        get
        {
            return "localhost";
        }
    }

    public override void Send(ArraySegment<byte> segment, int channelId = 0)
    {
    }
    public override void Disconnect()
    {
    }
}