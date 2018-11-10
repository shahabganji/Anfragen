using System;
using System.Collections.Generic;
using System.Text;

namespace Anfragen.Interfaces
{
	public interface IBranch
	{
		string Name { get; } // the name of the branch
		IEnumerable<Question> Questions { get; }
		IBranch Add(Question question, int? position = null); // adds a question ton the end of the list 
	}

}
