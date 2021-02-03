using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    /// <summary>
    /// This class can used for all types of Entity Media, User, URL as well as hashTag
    /// </summary>
    class TextCounter
    {
        /// <summary>
        /// This property would take the text for media, username, url and actual value of Hashtag without #
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// This property will maintain the number of times each text shows in list
        /// </summary>
        public int Frequency { get; set; }


        /// <summary>
        /// Just a constructor for the class, Later can be used for Unit Test case
        /// </summary>
        /// <param name="text"></param>
        /// <param name="frequency"></param>
        public TextCounter(string text, int frequency)
        {
            this.Text = text;
            this.Frequency = frequency;
        }
    }
}
