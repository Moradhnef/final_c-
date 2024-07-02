using Prism.Events;
using System;

namespace EasyTestMaker.Event
{
    public class QuestionEventArgs 
    { 
        public object Arg;

        public QuestionEventArgs(object arg)
        {
            Arg = arg;
        }
    }
}