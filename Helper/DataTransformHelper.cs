using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TSBXG.Helper
{
    public class DataTransHelper
    {
        public static void CopyProperties(object target, object source, params string[] properties)
        {
            foreach (var field in properties)
            {
                var sourceProp = source.GetType().GetProperty(field);
                var targetProp = target.GetType().GetProperty(field);
                targetProp.SetValue(target, sourceProp.GetValue(source));
            }
        }
    }
}