using SCPSLAudioApi.AudioCore;
namespace AudioPlayer;

public class FakeConnectionList
{
    public FakeConnection fakeConnection { get; set; }
    public ReferenceHub hubPlayer { get; set; }
    public AudioPlayerBase audioplayer { get; set; }
}
