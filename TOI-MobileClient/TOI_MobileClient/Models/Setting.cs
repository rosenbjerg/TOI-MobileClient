using System;
using System.Collections.Generic;
using TOI_MobileClient.Managers;

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
        private readonly string _id;

        public bool Toggle
        {
            get => SettingsManager.AppSettings.GetValueOrDefault(_id, true);
            set => SettingsManager.AppSettings.AddOrUpdateValue(_id, value);
        }

        public Type Capability { get; set; }

        public BooleanSetting(string id, string title)
        {
            _id = id;
            Title = title;
            Type = SettingType.Boolean;
        }
    }

    public class RadioSetting : Setting
    {
        private readonly string _id;
        private readonly int _default;
        public List<string> Options { get; }

        public int Selected
        {
            get => SettingsManager.AppSettings.GetValueOrDefault(_id, _default);
            set => SettingsManager.AppSettings.AddOrUpdateValue(_id, value);
        }

        public string SelectedValue => Options[Selected];

        public RadioSetting(string id, string title, List<string> options) : this(id, title, options, 1)
        {
        }

        public RadioSetting(string id, string title, List<string> options, int selectedIndex)
        {
            _id = id;
            _default = selectedIndex;
            if (options.Count == 0) throw new ArgumentException("Option list must have some content.");
            if (selectedIndex > options.Count)
                throw new ArgumentException("Selected index is greater than the amount of options.");
            Title = title;
            Options = options;
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