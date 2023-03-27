
# Requirements
This plugin uses as dependency [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi)

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

```yml
AudioPlayer:
  # Plugin Enabled?
  is_enabled: true
  # Enable developer mode?
  debug: true
  # The name of the bot when the file will sound
  bot_name: INTERCOM

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
