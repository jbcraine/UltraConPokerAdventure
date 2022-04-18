using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DialogueParser
{
    public DialogueParser(){}

    public void ParseDialogueFile(TextAsset dialogue, ref string[] lines, ref Dictionary<string, int> offsets)
    {
        lines = dialogue.text.Split('\n');

        //Take an account of every tag in the file
        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            if (line.Equals(""))
                continue;
            //All lines beginning with the '#' character are tags
            if (line[0] == '#')
            {
                string tag = line.Substring(1);
                offsets.Add(tag, i);
            }
        }
    }
}
