﻿using System;
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
        public TaskTab Get(string id)
        {
            using (TaskManagerDBEntities TskEntity = new TaskManagerDBEntities())
            {
                return TskEntity.TaskTabs.FirstOrDefault(x => x.Task == id);
            }
        }

        // POST: api/Task
        public HttpResponseMessage Post([FromBody]TaskClass task)
        {
            try
            {
                using (TaskManagerDBEntities TskEntity = new TaskManagerDBEntities())
                {
                    if (task.ParentTask.Trim() == "" || task.ParentTask == null)
                    {
                        TskEntity.TaskTabs.Add(new TaskTab
                        {
                            Task = task.Task,
                            Priority = task.Priority,
                            Start_Date = task.StartDate,
                            End_Date = task.EndDate,
                            IsClosed = task.IsClosed,
                        });
                        TskEntity.SaveChanges();
                    }
                    else
                    {
                        ParentTaskTab parentTsk = new ParentTaskTab
                        {
                            Parent_Task = task.ParentTask
                        };
                    
                        TskEntity.ParentTaskTabs.Add(parentTsk);
                        TskEntity.SaveChanges();
                        TskEntity.TaskTabs.Add(new TaskTab()
                        {
                            Task = task.Task,
                            Parent_ID = parentTsk.Parent_ID,
                            Priority = task.Priority,
                            Start_Date = task.StartDate,
                            End_Date = task.EndDate,
                            IsClosed = task.IsClosed,
                        });
                        TskEntity.SaveChanges();
                    }
                    var message = Request.CreateResponse(HttpStatusCode.Created, task);
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
          
        }

        // PUT: api/Task/5
        public HttpResponseMessage Put(int id, [FromBody]TaskClass task)
        {
            try
            {
                using (TaskManagerDBEntities tskEntity = new TaskManagerDBEntities ())
                {
                    //Checking if we are try to update an existing Task
                    var entity = tskEntity.TaskTabs.FirstOrDefault(x => x.Task_ID == id);
                    if (entity ==null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Task Not Found");
                    }
                    else
                    {
                        //In case parent Task Name modified 
                        if (task.ParentTask != null || task.ParentTask.Trim()!="")
                        {
                            //If the modiefied Parent task already exist in ParentTask Table
                            var parentEntity = tskEntity.ParentTaskTabs.FirstOrDefault(x => x.Parent_Task == task.ParentTask);
                            if (parentEntity != null)
                            {
                                entity.Task = task.Task;
                                entity.Parent_ID = parentEntity.Parent_ID;
                                entity.Priority = task.Priority;
                                entity.Start_Date = task.StartDate;
                                entity.End_Date = task.EndDate;
                                entity.IsClosed = task.IsClosed;
                                tskEntity.SaveChanges();
                                var message = Request.CreateResponse(HttpStatusCode.Created, task);
                                return message;
                            }
                            else
                            {
                                //If the modified parent task not exist in the ParentTask table
                                ParentTaskTab parentTsk = new ParentTaskTab
                                {
                                    Parent_Task = task.ParentTask
                                };
                                tskEntity.ParentTaskTabs.Add(parentTsk);
                                tskEntity.SaveChanges();


                                entity.Task = task.Task;
                                entity.Parent_ID = parentTsk.Parent_ID;
                                entity.Priority = task.Priority;
                                entity.Start_Date = task.StartDate;
                                entity.End_Date = task.EndDate;
                                entity.IsClosed = task.IsClosed;
                                tskEntity.SaveChanges();
                                var message = Request.CreateResponse(HttpStatusCode.Created, task);
                                return message;
                            }
                        }
                        else
                        {
                            //If user removed the parent task
                            entity.Task = task.Task;
                            entity.Parent_ID = null;
                            entity.Priority = task.Priority;
                            entity.Start_Date = task.StartDate;
                            entity.End_Date = task.EndDate;
                            entity.IsClosed = task.IsClosed;
                            tskEntity.SaveChanges();
                            var message = Request.CreateResponse(HttpStatusCode.Created, task);
                            return message;
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

        }

        // DELETE: api/Task/5
        public void Delete(int id)
        {
        }
    }

    public class TaskClass {
        public int TaskID { get; set; }
        public string Task { get; set; }
        public int ParentID { get; set; }
        public string ParentTask { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Priority { get; set; }
        public Nullable<bool> IsClosed { get; set; }
    }
}