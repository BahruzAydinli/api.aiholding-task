using AIHolding_task.Data;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIHolding_task.Repos
{
    public class FileRepository : IFileRepository
    {
        private readonly DataContext _context;
        public FileRepository(DataContext context)
        {
            _context = context;
        }

        public void WriteFile(IFormFile file, string folder)
        {

        }

        public void DeleteFile(int id)
        {

        }
    }
}
