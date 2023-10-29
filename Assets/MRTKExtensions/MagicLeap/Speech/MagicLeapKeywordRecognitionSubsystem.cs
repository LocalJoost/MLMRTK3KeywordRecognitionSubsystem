using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.Scripting;

namespace MRTKExtensions.MagicLeap.Speech
{
    [Preserve]
    [MRTKSubsystem(
        Name = "MRTKExtensions.MagicLeap.SpeechRecognition",
        DisplayName = "MRTK MagicLeap KeywordRecognition Subsystem",
        Author = "LocalJoost",
        ProviderType = typeof(MagicLeapKeywordRecognitionProvider),
        SubsystemTypeOverride = typeof(MagicLeapKeywordRecognitionSubsystem))]
    public class MagicLeapKeywordRecognitionSubsystem : KeywordRecognitionSubsystem
    {
#if MAGICLEAP
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        static void Register()
        {
            var cinfo = XRSubsystemHelpers.ConstructCinfo<MagicLeapKeywordRecognitionSubsystem,
                KeywordRecognitionSubsystemCinfo>();

            if (!Register(cinfo))
            {
                Debug.LogError($"Failed to register the {cinfo.Name} subsystem.");
            }
        }
#endif
    }
}