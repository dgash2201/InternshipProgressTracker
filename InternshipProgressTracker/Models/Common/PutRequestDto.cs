using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Common
{
    /// <summary>
    /// DTO for put requests
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class PutRequestDto<TModel>
    {
        public int Id { get; set; }

        public TModel Model { get; set; }
    }
}
