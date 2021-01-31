using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Repos
{
    public interface IFileRepository
    {
        void WriteFile(IFormFile file, string folder);
        void DeleteFile(int id);
    }
}
