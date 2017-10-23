using System;
using System.Collections.Generic;

namespace TOI_MobileClient.Models
{
    public abstract class Setting
    {
        public enum SettingType
        {
            Boolean,
            Radio
        }

        public string Title { get; protected set; }
        public SettingType Type { get; protected set; }
    }

    public class BooleanSetting : Setting
    {
        public bool Toggle { get; set; }

        public BooleanSetting(string title)
        {
            Title = title;
            Type = SettingType.Boolean;
        }
    }

    public class RadioSetting : Setting
    {
        public List<string> Options { get; }
        public int Selected { get; private set; }
        public string SelectedValue => Options[Selected];

        public RadioSetting(string title, List<string> options) : this(title, options, 0)
        {
        }

        public RadioSetting(string title, List<string> options, int selectedIndex)
        {
            if (options.Count == 0) throw new ArgumentException("Option list must have some content.");
            if (selectedIndex > options.Count)
                throw new ArgumentException("Selected index is greater than the amount of options.");
            Title = title;
            Options = options;
            SetSelected(selectedIndex);
            Type = SettingType.Radio;
        }

        public void SetSelected(int selected)
        {
            if (selected >= 0 && selected < Options.Count)
            {
                Selected = selected;
            }
            else
            {
                throw new ArgumentException("Selected item must exist in the list of Options.");
            }
        }
    }
}