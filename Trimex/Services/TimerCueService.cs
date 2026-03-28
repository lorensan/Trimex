#if ANDROID
using Android.Media;
#elif IOS || MACCATALYST
using AudioToolbox;
#endif
using Microsoft.Maui.Devices;

namespace Trimex.Services;

public static class TimerCueService
{
#if ANDROID
    private static readonly Lazy<ToneGenerator> ToneGeneratorInstance = new(() => new ToneGenerator(Android.Media.Stream.Notification, 100));
#elif IOS || MACCATALYST
    private static readonly SystemSound BeepSound = new(1057);
#endif

    public static Task PlayCountdownWarningAsync(CancellationToken cancellationToken = default) =>
        PlaySingleCueAsync(vibrate: false, cancellationToken);

    public static Task PlayStartSequenceAsync(CancellationToken cancellationToken = default) =>
        PlayTripleCueAsync(cancellationToken);

    public static Task PlayCompletionSequenceAsync(CancellationToken cancellationToken = default) =>
        PlayTripleCueAsync(cancellationToken);

    private static Task PlaySingleCueAsync(bool vibrate, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        PlayBeep();

        if (vibrate)
        {
            VibrateOnce();
        }

        return Task.CompletedTask;
    }

    private static async Task PlayTripleCueAsync(CancellationToken cancellationToken)
    {
        for (var cueIndex = 0; cueIndex < 3; cueIndex++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            PlayBeep();
            VibrateOnce();

            if (cueIndex < 2)
            {
                await Task.Delay(160, cancellationToken);
            }
        }
    }

    private static void PlayBeep()
    {
#if ANDROID
        ToneGeneratorInstance.Value.StartTone(Tone.PropBeep2, 120);
#elif IOS || MACCATALYST
        BeepSound.PlaySystemSound();
#elif WINDOWS
        Console.Beep(880, 120);
#endif
    }

    private static void VibrateOnce()
    {
#if ANDROID || IOS
        Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(120));
#endif
    }
}
