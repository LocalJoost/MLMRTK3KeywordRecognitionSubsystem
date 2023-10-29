using System.Collections.Generic;
using System.Linq;
using MixedReality.Toolkit.Subsystems;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Scripting;
using UnityEngine.XR.MagicLeap;

namespace MRTKExtensions.MagicLeap.Speech
{
    [Preserve]
    internal class MagicLeapKeywordRecognitionProvider : KeywordRecognitionSubsystem.Provider
    {
        private int commandId = 0;
        private MLVoiceIntentsConfiguration voiceConfiguration;

        public override void Start()
        {
            base.Start();
            if (voiceConfiguration == null)
            {
                voiceConfiguration = ScriptableObject.CreateInstance<MLVoiceIntentsConfiguration>();
                voiceConfiguration.VoiceCommandsToAdd = new List<MLVoiceIntentsConfiguration.CustomVoiceIntents>();
                voiceConfiguration.AllVoiceIntents = new List<MLVoiceIntentsConfiguration.JSONData>();
                voiceConfiguration.SlotsForVoiceCommands = new List<MLVoiceIntentsConfiguration.SlotData>();
            }

            if (!running)
            {
                MLVoice.OnVoiceEvent += OnVoiceEvent;
            }
        }

        public override void Stop()
        {
            base.Stop();
            MLVoice.OnVoiceEvent -= OnVoiceEvent;
        }

        public override void Destroy()
        {
            RemoveAllKeywords();
            Stop();
        }

        public override UnityEvent CreateOrGetEventForKeyword(string keyword)
        {
            if (!keywordDictionary.ContainsKey(keyword))
            {
                keywordDictionary.Add(keyword, new UnityEvent());
                AddIntentForKeyword(keyword);
                MLVoice.SetupVoiceIntents(voiceConfiguration);
            }
            return keywordDictionary[keyword];
        }

        public override void RemoveKeyword(string keyword)
        {
            if(keywordDictionary.TryGetValue(keyword, out var eventToRemove))
            {
                eventToRemove.RemoveAllListeners();
                keywordDictionary.Remove(keyword);
                voiceConfiguration.AllVoiceIntents.Remove(
                    voiceConfiguration.AllVoiceIntents.First(k=> k.value == keyword));
                SetupVoiceIntents();
            }
        }

        public override void RemoveAllKeywords()
        {
            foreach( var eventToRemove in keywordDictionary.Values)
            {
                eventToRemove.RemoveAllListeners();
            }
            keywordDictionary.Clear();
            voiceConfiguration.AllVoiceIntents.Clear();
            SetupVoiceIntents();
        }

        public override IReadOnlyDictionary<string, UnityEvent> GetAllKeywords()
        {
            return keywordDictionary;
        }

        private void OnVoiceEvent(in bool wasSuccessful, in MLVoice.IntentEvent voiceEvent)
        {
            if (wasSuccessful)
            {
                if (keywordDictionary.TryGetValue(voiceEvent.EventName, out var value))
                {
                    value.Invoke();
                }
            }
        }
        
        private void AddIntentForKeyword(string keyword)
        {
            var newIntent = new MLVoiceIntentsConfiguration.CustomVoiceIntents
            {
                Value = keyword,
                Id = (uint)commandId++
            };
            voiceConfiguration.VoiceCommandsToAdd.Add(newIntent);
        }

        private void SetupVoiceIntents()
        {
            if (!voiceConfiguration.AllVoiceIntents.Any())
            {
                AddIntentForKeyword("dummyxyznotempty");
            }
            MLVoice.SetupVoiceIntents(voiceConfiguration);
        }
    }
}