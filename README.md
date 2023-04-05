
# Requirements
This plugin uses as dependency [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi)<br>
This plugin uses as dependency [NVorbis](https://github.com/NVorbis/NVorbis)

# AudioPlayer
This plugin adds the ability to play .ogg audio files in the game.<br>
It also has a convenient API for using it in your plugins.

# Commands

**audio add {Bot ID}** - spawns a dummyplayer<br>
**audio kick {Bot ID}** - destroys a dummyplayer<br>
**audio play {Bot ID} {index}** - Starts playing at the given index<br>
**audio pfp {Bot ID} {Players} {index}** - Plays music to a specific player<br>
**audio stop {Bot ID}** - Stop music<br>
**audio spfp {Bot ID} {Player List}** - Stop a player from music<br>
**audio volume {Bot ID} {volume}** - Sets the volume<br>
**audio channel {Bot ID} {VoiceChatChannel}** - Changes bot VoiceChannel<br>
**audio loop {Bot ID} {false/true}** - Make it cyclic playback?<br>
**audio enqueue {Bot ID} {path} {position}** - Adds audio to the queue<br>
**audio nick {Bot ID} {Name}** - Sets name of bot<br>

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
  # Enable developer mode SCPSLAudioApi?
  s_c_p_s_l_audio_api_debug: true
  # Create a bot automatically when the server starts?
  spawn_bot: true
  # The name of the bot when the file will sound. (Bot with ID 99 is better not to touch, he is responsible for commands on the server)
  bots_list:
  - bot_name: Dedicated Server
    bot_i_d: 99
    # Hide the AudioPlayer bot in the Player List?
    show_player_list: true
    # What will be written in Badge? (Only works if BadgeBots = true) | Set null to turn off
    badge_text: AudioPlayer BOT
    # What color will Badge? (Only works if BadgeBots = true)
    badge_color: orange
  - bot_name: Lift
    bot_i_d: 98
    # Hide the AudioPlayer bot in the Player List?
    show_player_list: false
    # What will be written in Badge? (Only works if BadgeBots = true) | Set null to turn off
    badge_text: AudioPlayer BOT
    # What color will Badge? (Only works if BadgeBots = true)
    badge_color: orange
  # Turn all special events on or off?
  special_events_enable: true
  # Special Events Automatic Music, blank to disable.
  lobby_playlist:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  round_start_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  round_end_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  mtf_spawn_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  chaos_spawn_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  warhead_starting_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  warhead_stopping_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  - path: /home/scp/.config/Exiled/Plugins/audio/test2.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  elevator_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  player_died_target_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  player_died_killer_clip:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
  player_connected_server:
  - path: /home/scp/.config/Exiled/Plugins/audio/test.ogg
    loop: false
    volume: 100
    voice_chat_channel: Intercom
    bot_i_d: 99
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
