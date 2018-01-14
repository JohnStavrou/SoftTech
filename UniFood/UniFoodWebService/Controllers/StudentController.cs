using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;
using Microsoft.Azure.Mobile.Server;
using softechwebService.DataObjects;
using softechwebService.Models;

namespace softechwebService.Controllers
{
    public class StudentController : TableController<Student>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            softechwebContext context = new softechwebContext();
            DomainManager = new EntityDomainManager<Student>(context, Request);
        }

        // GET tables/Student
        public IQueryable<Student> GetAllStudents()
        {
            return Query();
        }

        // GET tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Student> GetStudent(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Student> PatchStudent(string id, Delta<Student> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Student
        public async Task<IHttpActionResult> PostStudent(Student item)
        {
            Student current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Student/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteStudent(string id)
        {
            return DeleteAsync(id);
        }
    }
}