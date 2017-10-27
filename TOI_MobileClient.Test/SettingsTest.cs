using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOI_MobileClient.Models;

namespace TOI_MobileClient.Test
{
    [TestClass]
    public class SettingsTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An unlisted option was selected in a Settings list.")]
        public void RadioSetting_SetSelected_ThrowArgumentException()
        {
            var radioSetting = new RadioSetting("radio settings", "Radio Setting", CreateOptionList());

            radioSetting.SetSelected(555);
        }

        [TestMethod]
        public void RadioSetting_SetSelected_ValidOptionSelected()
        {
            var radioSetting = new RadioSetting("radio settings", "Radio Setting", CreateOptionList());

            radioSetting.SetSelected(1);
            Assert.AreEqual("option2", radioSetting.SelectedValue);
        }

        [TestMethod]
        public void RadioSetting_Constructor_ValidNoOptionSelected()
        {
            var radioSetting = new RadioSetting("radio settings", "Radio Setting", CreateOptionList());

            Assert.AreEqual("option1", radioSetting.SelectedValue);
        }

        private List<string> CreateOptionList()
        {
            return new List<string> { "option1", "option2", "option3" };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An empty options list was successfully passed to a Radio Setting.")]
        public void RadioSetting_Constructor_EmptyListException() => new RadioSetting("radio settings", "Radio Setting", new List<string>());
    }


}