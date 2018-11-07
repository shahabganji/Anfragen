using System.Collections.Generic;
using Anfragen.Interfaces;
using System.Linq;
using System;

namespace Anfragen {

    public class Questionnaire {

        public readonly IPrinter printer;

        private IList<IQuestion> Questions { get; }

        private int Count => this.Questions.Count;

        public IEnumerable<IQuestion> PreviousQuestions {
            get {
                var counter = 0;
                while ( counter < this._counter ) {
                    yield return this.Questions[ counter++ ];
                }
            }
        }

        public IQuestion PreviousQuestion => this._counter == 0 ? this.CurrentQuestion : this.Questions[ this._counter - 1 ];
        public IQuestion CurrentQuestion => this.Questions[ this._counter ];
        public IQuestion NextQuestion => this._counter + 1 == this.Questions.Count ? this.CurrentQuestion : this.Questions[ this._counter + 1 ];

        public IEnumerable<IQuestion> NextQuestions {
            get {
                var counter = this._counter + 1;

                while ( ++counter < this.Questions.Count ) {
                    yield return this.Questions[ counter ];
                }

            }
        }


        public Questionnaire( IPrinter printer ) {
            this.Questions = new List<IQuestion>( );
            this.printer = printer;
        }

        public Questionnaire Add( IQuestion question, bool here = false ) {

            if ( here ) {
                this.Questions.Insert( this._counter + 1, question );
            } else {
                this.Questions.Add( question );
            }
            return this;
        }

        private int _counter = 0;
        private bool hasStarted = false;

        public void Start( ) {

            if ( this.hasStarted ) {
                throw new InvalidOperationException( "You cannot start a questionnaire more than once." );
            }

            this.hasStarted = true;

            var question = this.Questions[ _counter ];

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );
        }

        public Questionnaire Next( ) {

            if ( !hasStarted ) {
                throw new InvalidOperationException( "You have not strted the questionnaire yet" );
            }

            if ( this._counter == this.Count - 1 ) {
                throw new InvalidOperationException( "Your at the end of the questionnaire, there is no more questions." );
            }

            this._counter++;

            var question = this.Questions[ _counter ];

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;
        }

        public Questionnaire Previous( ) {

            if ( this._counter == 0 ) {
                throw new InvalidOperationException( "Your at the beginning of the questionnaire, there is no previous question." );
            }

            this._counter--;

            var question = this.Questions[ _counter ];

            // 1. ask the question, simply buy just printing it
            question.Ask( this.printer );

            // 2. wait for the user to give answer to the question
            question.TakeAnswer( );

            return this;
        }

    }
}