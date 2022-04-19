using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TaxCalc.Models
{
    public class TestCaseMismatch
    {
        public string PropertyName { get; set; }
        public string ExpectedValue { get; set; }
        public string ReceivedValue { get; set; }
        public string GetDisplayColor
        {
            get
            {
                return ExpectedValue == ReceivedValue ? "#2FF353" : "#F3412F";
            }
        }

        public string GetDisplayBorderColor
        {
            get
            {
                return ExpectedValue == ReceivedValue ? "#198C2E" : "#732018";
            }
        }
    }
}
