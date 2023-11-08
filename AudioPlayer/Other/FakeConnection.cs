using AudioPlayer.API;
using Exiled.API.Features;
using Mirror;
using System;
using System.Linq;

namespace AudioPlayer.Other;

public class FakeConnection : NetworkConnectionToClient
{
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