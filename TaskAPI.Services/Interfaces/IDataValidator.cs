using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI.Services.Interfaces
{
    public interface IDataValidator
    {
        Task Validate(object dto);
        bool Handle(object dto);
    }
}
