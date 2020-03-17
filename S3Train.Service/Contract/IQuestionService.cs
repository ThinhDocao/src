﻿using S3Train.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace S3Train.Contract
{
    public interface IQuestionService : IGenenicServiceBase<Question>
    {
        List<Question> ListAll();
        bool? ChangeStatus(Guid id);
        bool Create(Question question);
        bool Delete(Guid id);
    }
}
