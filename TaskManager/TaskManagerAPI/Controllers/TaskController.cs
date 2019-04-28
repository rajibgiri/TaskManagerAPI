using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaskManagerDAC;

namespace TaskManagerAPI.Controllers
{
    public class TaskController : ApiController
    {
        // GET: api/Task
        public IEnumerable<TaskTab> Get()
        {
            using (TaskManagerDBEntities TskEntity = new TaskManagerDBEntities())
            {
                return TskEntity.TaskTabs.ToList();
            } 
        }

        // GET: api/Task/5
        public TaskTab Get(string taskName)
        {
            using (TaskManagerDBEntities TskEntity = new TaskManagerDBEntities())
            {
                return TskEntity.TaskTabs.FirstOrDefault(x => x.Task == taskName);
            }
        }

        // POST: api/Task
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Task/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Task/5
        public void Delete(int id)
        {
        }
    }
}
