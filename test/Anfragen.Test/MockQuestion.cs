using System;
using Anfragen.Interfaces;
namespace Anfragen.Test {

    public class MockQuestion : Question {

        public MockQuestion( string question, IQuestionnaire questionnaire = null ) : base( question, questionnaire ) {

        }

        protected override Question TakeAnswer( ) {
            this.Answer = this.Terminal.Scanner.ReadLine( );
            return this;
        }
    }
}
