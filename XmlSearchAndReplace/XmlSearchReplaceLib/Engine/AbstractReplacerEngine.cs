using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceLib.Engine
{
	public abstract class AbstractReplacerEngine : IReplacerEngine
	{

		public static IReplacerEngine CreateEngine(ReplacerEngineType type, SearchReplaceOperationOptions options)
		{
			switch (type)
			{
				case ReplacerEngineType.StringEngine:
					return new StringReplacerEngine(options);
				default:
					throw new ArgumentException(String.Format("Invalid type '{0}' specified when creating search replace engine.", type));
			}
		}
		
		protected ReplacementOptionValidatorList _Validators;

		public AbstractReplacerEngine ()
		{
			_Validators = new ReplacementOptionValidatorList();

		}

		public string Replace(string actualString, string searchString, string replaceString)
		{
			if (_Validators.IsValidForReplacement(actualString, searchString))
				return ReplaceString(actualString, searchString, replaceString);
			else
				return actualString;
		}

		protected abstract string ReplaceString(string actualString, string searchString, string replaceString);

        public bool IsValidForReplacement(string actualString, string searchString)
        {
            return _Validators.IsValidForReplacement(actualString, searchString);
        }
	}
}
