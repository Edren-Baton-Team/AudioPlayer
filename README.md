[![Downloads](https://img.shields.io/github/downloads/Edren-Baton-Team/AudioPlayer/total?color=brown&label=Downloads&style=for-the-badge)](https://github.com/Edren-Baton-Team/AudioPlayer/releases)
![Lines](https://img.shields.io/tokei/lines/github/Edren-Baton-Team/AudioPlayer?style=for-the-badge)
# AudioPlayer
This plugin adds the ability to play .ogg audio files in the game.<br>
It also has a convenient API for using it in your plugins.

# Installation
You will need **latest** [EXILED Version](https://github.com/Exiled-Team/EXILED/releases/latest) installed on your server.

Put your [`AudioPlayer.dll`](https://github.com/Edren-Baton-Team/AudioPlayer/releases/latest) file in `EXILED/Plugins` path.<br>
Put your [`dependencies`](https://github.com/Edren-Baton-Team/AudioPlayer/releases/latest) file in `EXILED/Plugins/dependencies` path

# Requirements
This plugin uses as dependency [SCPSLAudioApi](https://github.com/CedModV2/SCPSLAudioApi)<br>
This plugin uses as dependency [NVorbis](https://github.com/NVorbis/NVorbis)<br>
↑<br>
(All of this is in the release)

**Important**
* Sound files must be in .ogg format.
* The sound file must be mono channel
* The sound frequency should be 48000 Hz

 I recommend using the website [online-convert.com](https://audio.online-convert.com/convert/mp3-to-ogg)<br>
![228310162-4188d665-0a3b-40e1-8e9a-e32cfde1ea22](https://github.com/Edren-Baton-Team/AudioPlayer/assets/72207886/51fd2727-e922-4308-86c1-9ae9a9e0f68b)<br>
or using the website [convertio.co](https://convertio.co/mp3-ogg/)<br>
![Screenshot_5](https://github.com/Edren-Baton-Team/AudioPlayer/assets/72207886/cbdb9673-9043-4bd7-aeb5-9102bc54edb9)<br>
Upload a file there<br>
![Screenshot_7](https://github.com/Edren-Baton-Team/AudioPlayer/assets/72207886/f85518cf-e569-4bb7-b95f-f0741e6f2577)<br>
And select the following settings:<br>
![Screenshot_8](https://github.com/Edren-Baton-Team/AudioPlayer/assets/72207886/cf33373b-dcd8-49d2-9fcd-00ff819ce1b9)<br>
# VoiceChannel
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
# Credits
- Plugin is supported by [Rysik5318](https://github.com/Rysik5318)
- Creator of the first version of AudioPlayer on SCP SL 12.0 by [Mariki](https://github.com/Marikider)
- Creator of SCPSLAudioApi by [ced777ric](https://github.com/ced777ric)
