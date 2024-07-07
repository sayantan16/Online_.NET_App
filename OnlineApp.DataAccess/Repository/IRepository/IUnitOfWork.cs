﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineApp.DataAccess.Repository.IRepository
{
    // unit of work - represents handling of all DB changes as a single uit/batch
    public interface IUnitOfWork
    {
        // all repos of Category Model
        ICategoryRepository Category { get; }

        // saving all repo changes as a single unit/batch
        void Save();
    }
}