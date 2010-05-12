using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XmlSearchReplaceConsoleLib
{
    public class ApplicationParameterValidator
    {
        
        
        public static ApplicationParameterCollection GetMissingMandatoryParams(ApplicationParameterCollection mandatoryParams, ApplicationParameterWithValueCollection currentAppParams)
        {
            ApplicationParameterCollection missingRequiredParams = new ApplicationParameterCollection();
            foreach (ApplicationParameter param in mandatoryParams)
            {
                if (param.IsMandatory && currentAppParams.Find(p => String.Compare(p.GetName(), param.GetName(), true) == 0) == null)
                {
                    missingRequiredParams.Add(param);
                }
            }

            return missingRequiredParams;
        }
    }
}
