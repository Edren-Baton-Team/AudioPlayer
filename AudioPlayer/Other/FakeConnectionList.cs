﻿using Mirror;
using SCPSLAudioApi.AudioCore;
namespace AudioPlayer.Other;
// This is where all Joker119#0119 contraband is stored xD
public class FakeConnectionList
{
    public NetworkIdentity fakeConnection { get; set; }
    public ReferenceHub hubPlayer { get; set; }
    public AudioPlayerBase audioplayer { get; set; }
    public string BotName { get; set; }
    public int BotID { get; set; }
}
