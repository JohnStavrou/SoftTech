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
    public class ShiftController : TableController<Shift>
    {
        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            softechwebContext context = new softechwebContext();
            DomainManager = new EntityDomainManager<Shift>(context, Request);
        }

        // GET tables/Shift
        public IQueryable<Shift> GetAllShifts()
        {
            return Query();
        }

        // GET tables/Shift/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Shift> GetShift(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Shift/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Shift> PatchShift(string id, Delta<Shift> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/Shift
        public async Task<IHttpActionResult> PostShift(Shift item)
        {
            Shift current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Shift/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteShift(string id)
        {
            return DeleteAsync(id);
        }
    }
}