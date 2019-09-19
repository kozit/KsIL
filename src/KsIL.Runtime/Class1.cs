using System;
using System.Collections.Generic;
using System.Text;

namespace KsIL.Runtime
{
    public class singleton
    {

        private static singleton me = null;

        public static singleton GetSingleton() {

            if (singleton.me == null)
                singleton.me = new singleton();

            return me;

        }


    }
}
