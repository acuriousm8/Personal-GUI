using System;
using System.Collections.Generic;
using System.Text;

namespace Personal_GUI.Stores
{
    public class AutoTyperStore
    {
        //stores all the autotyper data
        private string _Text_to_type;
        private int _Times_type;
        private int _Type_delay;
        private int _Start_delay;
        private bool _Enter_press;



        public string Text_to_type
        {
            get => _Text_to_type;
            set
            {
                _Text_to_type = value;
            }
        }

        public int Times_type
        {
            get => _Times_type;
            set
            {
                _Times_type = value;
            }
        }

        public int Type_delay
        {
            get => _Type_delay;
            set
            {
                _Type_delay = value;
            }
        }

        public int Start_delay
        {
            get => _Start_delay;
            set
            {
                _Start_delay = value;
            }
        }

        public bool Enter_press
        {
            get => _Enter_press;
            set
            {
                _Enter_press = value;
            }
        }

    }
}
