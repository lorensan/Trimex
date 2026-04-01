#if ANDROID
using Android.Media;
#elif IOS || MACCATALYST
using AVFoundation;
using Foundation;
#elif WINDOWS
using Windows.Media.Core;
using Windows.Media.Playback;
#endif
using Microsoft.Maui.Devices;
using Microsoft.Maui.Storage;

namespace Trimex.Services;

public static class TimerCueService
{
    private const string WarningSoundFileName = "beep1.mp3";
    private const string EndSoundFileName = "beepEnd.mp3";
    private const string StartSoundFileName = "beepStart.mp3";

    private static readonly SemaphoreSlim CacheLock = new(1, 1);
    private static readonly Dictionary<string, string> CachedSoundPaths = new(StringComparer.OrdinalIgnoreCase);

#if ANDROID
    private static MediaPlayer? _androidPlayer;
#elif IOS || MACCATALYST
    private static AVAudioPlayer? _applePlayer;
#elif WINDOWS
    private static readonly MediaPlayer WindowsPlayer = new();
#endif

    public static Task PlaySoundAsync(string soundName, CancellationToken cancellationToken = default) =>
        PlaySingleCueAsync(NormalizeSoundFileName(soundName), vibrate: false, cancellationToken);

    public static Task PlayCountdownWarningAsync(CancellationToken cancellationToken = default) =>
        PlaySingleCueAsync(WarningSoundFileName, vibrate: false, cancellationToken);

    public static Task PlayStartSequenceAsync(CancellationToken cancellationToken = default) =>
        PlaySingleCueAsync(StartSoundFileName, vibrate: true, cancellationToken);

    public static Task PlayCompletionSequenceAsync(CancellationToken cancellationToken = default) =>
        PlaySingleCueAsync(EndSoundFileName, vibrate: true, cancellationToken);

    private static async Task PlaySingleCueAsync(string soundFileName, bool vibrate, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var filePath = await GetSoundFilePathAsync(soundFileName, cancellationToken);
        PlaySoundFile(filePath);

        if (vibrate)
        {
            VibrateOnce();
        }
    }

    private static async Task<string> GetSoundFilePathAsync(string soundFileName, CancellationToken cancellationToken)
    {
        await CacheLock.WaitAsync(cancellationToken);

        try
        {
            if (CachedSoundPaths.TryGetValue(soundFileName, out var cachedPath) && File.Exists(cachedPath))
            {
                return cachedPath;
            }

            var cachePath = Path.Combine(FileSystem.CacheDirectory, soundFileName);
            Directory.CreateDirectory(Path.GetDirectoryName(cachePath)!);

            await using var packageStream = await FileSystem.OpenAppPackageFileAsync(soundFileName);
            await using var cacheStream = File.Create(cachePath);
            await packageStream.CopyToAsync(cacheStream, cancellationToken);

            CachedSoundPaths[soundFileName] = cachePath;
            return cachePath;
        }
        finally
        {
            CacheLock.Release();
        }
    }

    private static string NormalizeSoundFileName(string soundName)
    {
        var normalized = soundName.Trim();
        return Path.HasExtension(normalized) ? normalized : $"{normalized}.mp3";
    }

    private static void PlaySoundFile(string filePath)
    {
#if ANDROID
        _androidPlayer?.Stop();
        _androidPlayer?.Release();
        _androidPlayer?.Dispose();
        _androidPlayer = new MediaPlayer();
        _androidPlayer.SetDataSource(filePath);
        _androidPlayer.Prepare();
        _androidPlayer.Start();
#elif IOS || MACCATALYST
        _applePlayer?.Stop();
        _applePlayer?.Dispose();
        _applePlayer = AVAudioPlayer.FromUrl(NSUrl.FromFilename(filePath));
        _applePlayer?.PrepareToPlay();
        _applePlayer?.Play();
#elif WINDOWS
        WindowsPlayer.Pause();
        WindowsPlayer.Source = MediaSource.CreateFromUri(new Uri(filePath, UriKind.Absolute));
        WindowsPlayer.Play();
#endif
    }

    private static void VibrateOnce()
    {
#if ANDROID || IOS
        Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(120));
#endif
    }
}
