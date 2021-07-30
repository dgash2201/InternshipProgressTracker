using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Models.Common
{
    /// <summary>
    /// DTO for patch requests
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class PatchRequestDto<TModel> where TModel : class
    { 
        public int Id { get; set; }

        public JsonPatchDocument<TModel> PatchDocument { get; set; }
    }
}
