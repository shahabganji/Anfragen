using System;
using System.Collections.Generic;
namespace Anfragen.Interfaces {

    public interface IConfirm : IQuestion {

        IList<string> PossibleAnswers { get; }
    }
}
