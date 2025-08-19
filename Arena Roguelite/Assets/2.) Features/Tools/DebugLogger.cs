using UnityEngine;

public static class DebugLogger 
{
    public enum LogStyle
    {
        Normal,
        Bold,
        Italic
    }

    public static void Log(string message, LogStyle style = LogStyle.Normal, Color? color = null)
    {
        string formattedMessage = FormatMessage(message, style, color);
        Debug.Log(formattedMessage);
    }
    
    private static string FormatMessage(string message, LogStyle style, Color? color)
    {
        string formattedMessage = message;

        // Apply style
        switch (style)
        {
            case LogStyle.Bold:
                formattedMessage = $"<b>{formattedMessage}</b>";
                break;
            case LogStyle.Italic:
                formattedMessage = $"<i>{formattedMessage}</i>";
                break;
            case LogStyle.Bold | LogStyle.Italic:
                formattedMessage = $"<b><i>{formattedMessage}</i></b>";
                break;
        }

        // Apply color
        if (color.HasValue)
        {
            Color col = color.Value;
            string colorCode = ColorUtility.ToHtmlStringRGB(col);
            formattedMessage = $"<color=#{colorCode}>{formattedMessage}</color>";
        }

        return formattedMessage;
    }
}
