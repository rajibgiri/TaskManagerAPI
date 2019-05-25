using TaskManagerAPI;
using TaskManagerAPI.Controllers;
using NUnit.Framework;
using System.Collections.Generic;
using Moq;
using TaskManagerDAC;
using System.Net.Http;
using System.Web.Http;

namespace TaskManagerAPI.Tests.Controllers
{
    [TestFixture]
    public class TaskControllerTest
    {
        [Test]
        public void Get()
        {
            // var mockTaskTab = new Mock<TaskManagerDAC.TaskTab>();
            Mock<TaskManagerDBEntities> mockTask = new Mock<TaskManagerDBEntities>();
            mockTask.Setup(x => x.TaskTabs.Add(It.IsAny<TaskManagerDAC.TaskTab>())).Returns((TaskManagerDAC.TaskTab task) => task);
            TaskController TaskCtrl = new TaskController();
            var tsk  = TaskCtrl.Get();
            List<TaskClass> lst = (List<TaskClass>) tsk;
            Assert.IsTrue(lst.Count > 1, "This List contains more than 1 record");
        }

        [Test]
        public void Post()
        {
            Mock<TaskClass> mockTask = new Mock<TaskClass>();
            mockTask.Object.ParentTask = "TestParent";
            mockTask.Object.Task = "TestTask";
            mockTask.Object.StartDate = new System.DateTime(2019, 05, 01);
            mockTask.Object.EndDate = new System.DateTime(2019,05,30);
            mockTask.Object.Priority = 15;
            TaskClass tskCls = mockTask.Object;
            TaskController TaskCtrl = new TaskController();
            TaskCtrl.Request = new HttpRequestMessage();
            TaskCtrl.Configuration = new HttpConfiguration();
            var tsk = TaskCtrl.Post(tskCls);
            Assert.AreEqual(tsk.ReasonPhrase, "Created");
        }

        [Test]
        public void Put()
        {
            Mock<TaskClass> mockTask = new Mock<TaskClass>();
            mockTask.Object.ParentTask = "ModifiedTestParent";
            mockTask.Object.TaskID = 21;
            mockTask.Object.Task = "ModifiedTestTask";
            mockTask.Object.StartDate = new System.DateTime(2019, 05, 06);
            mockTask.Object.EndDate = new System.DateTime(2019, 05, 31);
            mockTask.Object.Priority = 16;
            TaskClass tskCls = mockTask.Object;
            TaskController TaskCtrl = new TaskController();
            TaskCtrl.Request = new HttpRequestMessage();
            TaskCtrl.Configuration = new HttpConfiguration();
            var tsk = TaskCtrl.Put(tskCls);
            Assert.AreEqual(tsk.ReasonPhrase, "Created");
        }

        [Test]
        public void Delete()
        {
            TaskController TaskCtrl = new TaskController();
            TaskCtrl.Request = new HttpRequestMessage();
            TaskCtrl.Configuration = new HttpConfiguration();
            var tsk = TaskCtrl.Delete(21);
            Assert.AreEqual(tsk.ReasonPhrase, "OK");
        }
    }
}
