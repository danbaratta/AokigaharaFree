*** Introloop v1.0 ***
/* 
/// Copyright (c) 2015 Sirawat Pitaksarit, Exceed7 Experiments LP 
/// http://www.exceed7.com/introloop
*/

=== What did it install ===
Assets/Introloop
Assets/Resources/Introloop

=== How to use Introloop ===
With internet connection you shoud visit : http://www.exceed7.com/introloop

Without internet connection, here are brief instructions!

1. Right click your AudioClip asset you want to play Introloop style, select Introloop > Create IntroloopAudio
2. Set appropriate boundaries. When playhead reaches Looping Boundary, it will return to Intro Boundary.
3. Uncheck "Preload Audio Data" on your original AudioClip recommended.
4. Create something like "public IntroloopAudio myIntroloopAudio;" in your script.
5. Drag your newly created IntroloopAudio asset file to the inspector slot.
6. In the script call IntroloopPlayer.Instance.Play(myIntroloopAudio) to play. There are Stop, Pause and Resume also.
7. (Extra) Setup mixer routing and other settings in a "template prefab" file located in Assets/Resources/Introloop/

Questions/Problems/Suggestions : 5argon@exceed7.com
