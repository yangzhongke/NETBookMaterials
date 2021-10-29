using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
namespace webapitest1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public void Test()
        {
            
            
        }

        [HttpGet]
        public string Test2()
        {
            return null;
        }

        [HttpGet("school/{schoolName}/class/{classNo}")]
        public ActionResult<Student[]> GetAll(string schoolName,[FromRoute(Name ="classNo")]string classNum,
            [FromQuery]string pageNum,[FromQuery(Name = "pSize")]int pageSize)
        {
            return null;
        }

        [HttpPost("classId/{classId}")]
        public ActionResult<long> AddNew(long classId,Student s)
        {
            return 6;
        }
    }
}
