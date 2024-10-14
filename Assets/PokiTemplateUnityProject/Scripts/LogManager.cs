using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LogManager : MonoBehaviour
{
    public static LogManager instance;
    public TextMeshProUGUI logText; // Reference to the TextMeshProUGUI component
    private Queue<string> logMessages = new Queue<string>(); // Queue to store log messages
    private int maxLogMessages = 10; // Maximum number of log messages

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else if (instance != null)
        {
            Destroy(gameObject);
        }
    }

    // Method to add a message to the log
    public void AddLogMessage(string message)
    {
        // If there are more than 10 messages, remove the oldest one
        if (logMessages.Count >= maxLogMessages)
        {
            logMessages.Dequeue();
        }

        // Add the new message to the queue
        logMessages.Enqueue(message);

        // Update the log text
        UpdateLogText();
    }

    // Updates the TextMeshProUGUI component with the current messages
    private void UpdateLogText()
    {
        logText.text = string.Join("\n", logMessages.ToArray());
    }
}