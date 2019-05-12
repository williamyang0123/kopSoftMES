using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

namespace KopSoftPrint
{
    public class Speech
    {
        private static volatile Speech instance = null;
        private readonly SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

        public static Speech Instance()
        {
            if (instance == null)
            {
                if (instance == null)
                {
                    instance = new Speech();
                }
            }
            return instance;
        }

        public void Speak(int volume, int rate, string content)
        {
            speechSynthesizer.Volume = volume;
            speechSynthesizer.Rate = rate; //-10到10
            //speechSynthesizer.SpeakAsyncCancelAll();
            speechSynthesizer.SpeakAsync(content);
        }
    }
}