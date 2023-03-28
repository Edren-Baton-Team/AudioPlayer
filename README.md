
# Requirements
This plugin uses as dependency [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi)
This plugin uses as dependency [NVorbis](https://github.com/NVorbis/NVorbis)

# AudioPlayer
This plugin adds the ability to play .ogg audio files in the game.<br>
It also has a convenient API for using it in your plugins.

# Commands
**audio play {index}** - starts playing at the given index<br>
**audio stop** - Stop music<br>
**audio volume {volume}** - sets the volume<br>
**audio channel {VoiceChatChannel}** - Changes bot VoiceChannel<br>
**audio loop false/true** - Make it cyclic playback?<br>
**audio nick {Name}** - Sets name of bot<br>

# Instalation
Put AudioPlayer.dll into Exiled Plugins Folder.<br>
Put SCPSLAudioApi into Exiled dependencies folder.

**Important**
* Sound files must be in .ogg format.
* The sound file must be mono channel
* The sound frequency should be 48000 Hz

 I recommend using the website https://audio.online-convert.com/convert/mp3-to-ogg
![Screenshot_12](https://user-images.githubusercontent.com/72207886/228310162-4188d665-0a3b-40e1-8e9a-e32cfde1ea22.png)
# Default Config
```yml

AudioPlayer:
  # Plugin Enabled?
  is_enabled: true
  # Enable developer mode?
  debug: true
  # Create a bot automatically when the server starts?
  spawn_bot: true
  # The name of the bot when the file will sound
  bot_name: Dedicated Server
  # Special Events Automatic Music, blank to disable.
  lobby_playlist:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  round_start_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  round_end_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  mtf_spawn_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  chaos_spawn_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  warhead_starting_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
  warhead_stopping_clip:
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test.ogg
    volume: 100
  - path: /home/rp/.config/ExiledTestServer/Plugins/audio/test2.ogg
    volume: 100
```
**VoiceChannel**

```md
- None
- Proximity
- Radio
- ScpChat
- Spectator
- RoundSummary
- Intercom
- Mimicry
- Scp1576
```
