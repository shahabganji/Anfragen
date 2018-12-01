using System;
using Anfragen.Abstractions;
namespace Anfragen.Test {

    public class MockQuestion : Question {

        public MockQuestion( string question, IQuestionnaire questionnaire = null ) : base( question, questionnaire ) {

        }

        protected override IQuestion TakeAnswer( ) {
            this.Answer = this.Terminal.Scanner.ReadLine( );
			this.Terminal.Printer.Write(this.Answer);  
            return this;
        }
    }
}
