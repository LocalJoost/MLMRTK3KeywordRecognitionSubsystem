using System;
using JetBrains.Annotations;
using MixedReality.Toolkit;
using MixedReality.Toolkit.Subsystems;
using TMPro;
using UnityEngine;

public class DemoMenuController : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro recognizedCommand;
    
    [SerializeField]
    private TextMeshPro availableCommands;

    private IKeywordRecognitionSubsystem keywordRecognitionSubsystem;

    private void Start()
    {
        keywordRecognitionSubsystem = XRSubsystemHelpers.KeywordRecognitionSubsystem;
        InitStandardPhrases();
    }

    public void InitStandardPhrases()
    {
        RemoveAll();
        keywordRecognitionSubsystem.CreateOrGetEventForKeyword("Good morning").
            AddListener(() => ShowRecognizedCommand("Good morning"));
        keywordRecognitionSubsystem.CreateOrGetEventForKeyword("Nice weather").
            AddListener(() => ShowRecognizedCommand("Nice weather"));
        keywordRecognitionSubsystem.CreateOrGetEventForKeyword("Mixed Reality is cool").
            AddListener(() => ShowRecognizedCommand("Mixed Reality is cool"));
        UpdateRecognizedCommands();
    }
    
    private void ShowRecognizedCommand(string command)
    {
        recognizedCommand.text = $"Recognized: {command}";
    }

    public void AddHello()
    {
        keywordRecognitionSubsystem.CreateOrGetEventForKeyword("Hello there").
            AddListener(() => ShowRecognizedCommand("Hello there"));
        UpdateRecognizedCommands();
    }

    public void RemoveHello()
    {
        keywordRecognitionSubsystem.RemoveKeyword("Hello there");
        UpdateRecognizedCommands();
    }
    
    public void RemoveAll()
    {
        keywordRecognitionSubsystem.RemoveAllKeywords();
        UpdateRecognizedCommands();
    }
    
    private void UpdateRecognizedCommands()
    {
        var commands = keywordRecognitionSubsystem.GetAllKeywords();
        availableCommands.text = $"Available commands: {string.Join(Environment.NewLine, commands.Keys)}";
    }
    
    public void ToggleKeywordRecognition()
    {
        if (keywordRecognitionSubsystem.running)
        {
            keywordRecognitionSubsystem.Stop();
        }
        else
        {
            keywordRecognitionSubsystem.Start();
        }
    }
}
