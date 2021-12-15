using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teamy.Shared.Common
{
    public enum QuizElementType
    {
        SingleSelectQuestion,
        MultiSelectQuestion,
        GradeQuestion,
        FreeTextQuestion,
        
        InformationOnly,
        Picture,
        PageBreak
    }
}
